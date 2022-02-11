# CoffeeChallenge

## Getting Started

Provide passwords used to setup the MariaDB container as UTF-8 text files in ./.secrets

- hims-mariadb-pw
- hims-mariadb-rootpw

Build images and run containers for application and backend:

```
docker-compose --profile backend --profile coffee-services up -d
```

SwaggerUI provides an interactive, documented interface for CoffeeStore's REST API and can be reached under [http://localhost:5000/swagger](http://localhost:5000/swagger), when the service is up on the local machine.

## Development Environment

To set up the dev environment for CoffeeStore set the configured MariaDB user password in the .NET secret storage
```
dotnet user-secrets set "CoffeeStoreDatabase:Password" "<hims-mariadb-pw>"
```
