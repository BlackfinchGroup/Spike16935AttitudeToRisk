using AttitudeQuestions.AppHost.MongoDBAtlas;

var builder = DistributedApplication.CreateBuilder(args);

var mongodb = builder.AddMongoDBAtlas("question-demo")
             .WithMongoExpress()
             .WithLifetime(ContainerLifetime.Persistent)
             .WithDataVolume()
             .WithConfigVolume()
             .WithSearchVolume();

var questionDb = mongodb.AddDatabase("questions");

builder.AddProject<Projects.AttitudeQuestions_API>("attitudequestions-api")
    .WithReference(questionDb)
    .WaitFor(questionDb);

builder.Build().Run();
