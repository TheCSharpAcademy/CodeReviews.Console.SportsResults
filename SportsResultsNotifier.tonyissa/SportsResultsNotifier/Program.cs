using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);



var app = builder.Build();

app.Run();