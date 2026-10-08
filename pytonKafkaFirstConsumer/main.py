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

connection = pika.BlockingConnection(pika.ConnectionParameters("localhost", 5672))
channel = connection.channel()

client = Elasticsearch(
    "http://localhost:9200")

conf = {'bootstrap.servers': 'localhost:9092',
        'group.id': 'validData2',
        'auto.offset.reset': 'earliest'}

consumer = Consumer(conf)

db_redis = redis.Redis(host='localhost', port=6379, decode_responses=True)

servers = Servers()

#create a invalidGeodata queue in RabbitMQ 
channel.queue_declare(
    queue="invalidGeodata",
    durable=True,
    arguments={'x-queue-type': 'quorum'}
)

'''start read a message from kafka 
    chek with validation and redis, 
    after send it to rabbit by geolokation''' 
try:
    consumer.subscribe(["first-topic"])

    while True:
        msg = consumer.poll(timeout=1.0)
        if msg is None:
            continue

        if msg.error():
            raise KafkaException(msg.error())

        data = json.loads(msg.value().decode("utf-8"))

        if not servers.Check_Validation(data):
            # warnning log
            client.index(
                    index="logs",
                    document={
                        "Level": "warning",
                        "message":"not valid data send to redis invalid geodata"
                    })

            #send a message
            channel.basic_publish(
                exchange="",
                routing_key='invalidGeodata',
                body=msg.value()
            )
            continue

        geo_key = servers.get_region_with_geopandas(
            "./regions.geojson",
            float(data["lon"]),
            float(data["lat"])
        )

        try:
            # the key is a alert_id
            if db_redis.exists(data["alert_id"]):
                continue

            db_redis.set(data["alert_id"], json.dumps(data))

            #create a geo_key queue in RabbitMQ 
            #send a message
            channel.basic_publish(
                exchange="",
                routing_key=geo_key,
                body=msg.value()
            )

            # add a log
            client.index(
                    index="logs",
                    document={
                        "Level": "info",
                        "message":f"add a file to rabbit: {geo_key}"
                    })

        except Exception as e:
            # add a log
            print(e)
            client.index(
                    index="logs",
                    document={
                        "Level": "warning",
                        "message":f"{e}"
                    })

finally:
    # Close down consumer to commit final offsets.
    consumer.close()