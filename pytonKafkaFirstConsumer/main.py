from confluent_kafka import Consumer, KafkaException
from Serves import Servers
import redis
import json
import os
from elasticsearch import Elasticsearch
import pika



'''conection elastic from logs
    Consumer from Read a data 
        redis to check 
        and rabbit to send'''

connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()

client = Elasticsearch(
    "https://localhost:9200")

conf = {'bootstrap.servers': 'host1:9092,host2:9092',
        'group.id': 'validData',
        'auto.offset.reset': 'earliest'}

consumer = Consumer(conf)


db_redis = redis.Redis(host='localhost', port=6379, decode_responses=True)


servers = Servers()

#create a invalidGeodata queue in RabbitMQ
channel.queue_declare(queue="invalidGeodata", durable=True, arguments={'x-queue-type': 'quorum'})


'''start read a message from kafka
    chek with validation and redis,
    after send it to rabbit by geolokation'''
try:
    consumer.subscribe("first-topic")

    while True:
        msg = consumer.poll(timeout=1.0)
        if msg is None: continue

        data = json.loads(msg)

        if not servers.Check_Validation(data):
              # warnning log
                client.index(
                        index="logs",
                        document={
                            "Level": "warning",
                            "message":"not valid data send to redis invalid geodata",
                        })

                

                #send a message
                channel.basic_publish(exchange='',
                        routing_key='invalidGeodata',
                        body=data)
                

        geo_key = servers.get_region_with_geopandas("./regions.geojson", data["lon"], data["lat"])

        
        try:    
            # the key is a alert_id
            db_redis.set(data["alert_id"], data)

            #create a geo_key queue in RabbitMQ
            channel.queue_declare(queue=geo_key, durable=True, arguments={'x-queue-type': 'quorum'})


            #send a message
            channel.basic_publish(exchange='',
                    routing_key=f"{geo_key}",
                    body=data)
                    
            # add a log
            client.index(
                    index="logs",
                    document={
                        "Level": "info",
                        "message":f"add a file to rabbit: {geo_key}",
                    })

        except (Exception) as e:
             # add a log
            client.index(
                    index="logs",
                    document={
                        "Level": "warning",
                        "message":f"{e}",
                    })

        
        

        if msg.error():
            raise KafkaException(msg.error())
        else:
            print("Error")
finally:
    # Close down consumer to commit final offsets.
    consumer.close()












