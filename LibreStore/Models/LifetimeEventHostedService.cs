internal class LifetimeEventsHostedService : IHostedService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _appLifetime;

    // 2. Inject `IHostApplicationLifetime` through dependency injection in the constructor.
    public LifetimeEventsHostedService(
        ILogger<LifetimeEventsHostedService> logger, 
        IHostApplicationLifetime appLifetime)
    {
        _logger = logger;
        _appLifetime = appLifetime;
    }

    // 3. Implemented by `IHostedService`, setup here your event registration. 
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _appLifetime.ApplicationStarted.Register(OnStarted);

        return Task.CompletedTask;
    }

    // 4. Implemented by `IHostedService`, setup here your shutdown registration.
    //    If you have nothing to stop, then just return `Task.CompletedTask`
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
        _logger.LogInformation("OnStarted has been called.");
        // TODO: Add database TABLE creation scripts here
        // so they are only called upon start up of service.
    }
}