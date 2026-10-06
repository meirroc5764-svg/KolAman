# Repo : KolAman
## About
The project reads information about various situations from different sources, \
aggregates it into a single database, and—following a cleaning and verification \
process—distributes it to the appropriate destinations.

# First Producer (C#)
This producer finds new messages, reads them from a file, and sends them via Kafka. \
It runs as a continuous stream, constantly waiting for new messages.

# First Consumer (Python)
The consumer reads messages from Kafka, performs minimal processing hes check a geodata if not valid send a log and data send to , \
notvalid rabbit, if is valid continue a process, \
and checks Redis to ensure the ID is not a duplicate; if a duplicate is found, \
it logs a warning and sends the message to a separate RabbitMQ queue. \
Validated messages are distributed across RabbitMQ based on geolocation.

# Save To Mongo db
This component receives data from RabbitMQ based on \ geolocation and saves it to the database; I chose MongoDB \ because the system is dynamic, and it makes sense to make  it as flexible as possible to accommodate various needs and formats.

