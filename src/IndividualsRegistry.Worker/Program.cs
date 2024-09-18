using System.Data;
using System.Data.SqlClient;
using Credo.JCS.Extension.Services;
using Credo.JCS.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;

        services.AddHostedService<WorkerFirst>();
        services.AddHostedService<WorkerSecond>();
        services.AddJobService(hostContext.Configuration);
        services.AddTransient<IJobService, JobService>();

    })
    .Build();

await host.RunAsync();