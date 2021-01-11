# RS3 Bot
Play RS3 On Discord! A work in progress still.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
* [PostgresSQL](https://www.postgresql.org/download/)
* [Net5.0](https://dotnet.microsoft.com/download/dotnet/5.0)


### Installing
1. Go to [appsettings.json](https://github.com/JayArrowz/RS3Bot/blob/main/RS3Bot.Cli/appsettings.json) and ensure the ConnectionString to your database is correct
2. Ensure your Discord bot API key is correct
3. Go to your Terminal (Make sure its current directory is matching the root of this repo) or VS Console then type, this will scaffold the DB on postgres:
```
dotnet tool install -g dotnet-ef
dotnet build
dotnet ef database update --project RS3Bot.Cli
```

To Run in Terminal: 
```
dotnet run --project RS3Bot.Cli
```

## Images
![alt text](https://i.imgur.com/JDvYw0z.png)
![alt text](https://i.imgur.com/sSi7MXE.png)
![alt text](https://i.imgur.com/AAHRsJx.png)
![alt text](https://i.imgur.com/zLiAtHX.png)

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/JayArrowz/RS3Bot/tags). 

## Authors

* **JayArrowz** - [JayArrowz](https://github.com/JayArrowz)

See also the list of [contributors](https://github.com/JayArrowz/RS3Bot/contributors) who participated in this project.

## Acknowledgments
* JayArrowz
* Graham
