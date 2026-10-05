from confluent_kafka import Consumer, KafkaException
import redis
import json

conf = {'bootstrap.servers': 'host1:9092,host2:9092',
        'group.id': 'validData',
        'auto.offset.reset': 'earliest'}

consumer = Consumer(conf)


db_redis = redis.Redis(host='localhost', port=6379, decode_responses=True)



try:
    consumer.subscribe("first-topic")

    while True:
        msg = consumer.poll(timeout=1.0)
        if msg is None: continue

        data = json.loads(msg)

        if not db_redis.get(data["alert_id"]):
            # the key is a alert_id
                    db_redis.set(data["alert_id"], data)

        else:
             print("not add to redis is a doblicait")

        
        

        if msg.error():
            raise KafkaException(msg.error())
        else:
            print("Error")
finally:
    # Close down consumer to commit final offsets.
    consumer.close()














