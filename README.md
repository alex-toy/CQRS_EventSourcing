# CQRS and EventSourcing

**CQRS** and **Event Sourcing** enable to create microservices that are super decoupled and extremely scalable. In this project, we will create .NET microservices that comply with the **CQRS** and **Event Sourcing** patterns : 

- Handle commands and raise events.
- Use the **Mediator** pattern to implement command and query dispatchers.
- Create and change the state of an aggregate with event messages.
- Implement an event store / write database in MongoDB.
- Create a read database in MS SQL.
- Apply event versioning.
- Implement optimistic concurrency control.
- Produce events to Apache Kafka.
- Consume events from Apache Kafka to populate and alter records in the read database.
- Replay the event store and recreate the state of the aggregate.
- Separate read and write concerns.
- Structure the code using **Domain-Driven-Design** best practices.
- Replay the event store to recreate the entire read database.
- Replay the event store to recreate the entire read database into a different database type - PostgreSQL.

## Prerequisites

1. Network :
```
docker network create --attachable -d bridge mydockernetwork
```

2. Kafka :
```
docker exec dotnetmicroservicescqrseventsourcingwithkafka_kafka_1 kafka-topics.sh  --bootstrap-server localhost:9092  --create --replication-factor 1 --partitions 1 --topic SocialMediaPostEvents
```

3. Mongodb :
```
docker run -it -d --name mongo-container -p 27017:27017 --network mydockernetwork --restart always -v mongodb_data_container:/data/db mongo:latest
```

4. Microsoft SQL Server
```
docker run -d --name sql-container --network mydockernetwork --restart always -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=$tr0ngS@P@ssw0rd02' -e 'MSSQL_PID=Express' -p 1433:1433 mcr.microsoft.com/mssql/server:2017-latest-ubuntu 
```

5. APIs
```
docker-compose up -d
```

<img src="/pictures/docker.png" title="docker images"  width="900"> 

Connect to the server using the following credentials :
- Server name : localhost
- no database name
- user name : sa
- Password : from docker command
- No profile name

5. Nuget Packages

- in *CQRS.Core*
```
MongoDB.Driver
```

- in *Post.Cmd.Infrastructure*
```
Confluent.Kafka 
Microsoft.Extensions.Options 6.0.0
MongoDB.Driver 
```

- in *Post.Query.Api*
```
Microsoft.EntityFrameworkCore.SqlServer
Npgsql.EntityFrameworkCore.PostgreSQL
```

Swashbuckle.AspNetCore" Version="6.2.3

- in *Post.Query.Infrastructure*
```
Microsoft.EntityFrameworkCore.Proxies
Npgsql.EntityFrameworkCore.PostgreSQL
```

## Test API

### Create Post
<img src="/pictures/post_cmd_api.png" title="post cmd api"  width="900"> 




