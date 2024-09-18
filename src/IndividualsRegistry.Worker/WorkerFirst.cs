﻿
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Credo.JCS.Extension.Services;
namespace Credo.JCS.Worker;

public class WorkerFirst : BackgroundService
{

    private readonly ILogger<WorkerFirst> _logger;
    private readonly IJobService _jobService;

    public WorkerFirst(
        ILogger<WorkerFirst> logger, IJobService jobService)
    {
        _logger = logger;
        _jobService=jobService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _jobService.HandleJob(async () =>
        {
            _logger.LogInformation($"Service RUN 1 ${DateTime.Now}");
            await Task.Delay(TimeSpan.FromSeconds(10));
        }, "efc81045-bab7-473f-bd3f-be0d90b2aeca", stoppingToken);
    }
}