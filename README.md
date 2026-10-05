# Repo : KolAman
## About
The project reads information about various situations from different sources, \
aggregates it into a single database, and—following a cleaning and verification \
process—distributes it to the appropriate destinations.

# First Producer (C#)
This producer finds new messages, reads them from a file, and sends them via Kafka. \
It runs as a continuous stream, constantly waiting for new messages.

# First Consumer (Python)
This consumer reads a message from Kafka and \
writes it to Redis without duplication.
Since the logic isn't overly complex and doesn't involve many steps, hard-coding was used.

