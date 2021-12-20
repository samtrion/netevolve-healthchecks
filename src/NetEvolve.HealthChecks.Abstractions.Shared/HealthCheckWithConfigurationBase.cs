namespace NetEvolve.HealthChecks.Abstractions;

#if USE_HEALTHCHECK_WITH_CONFIG
internal abstract class HealthCheckWithConfigurationBase<TConfiguration> : IHealthCheck
{
  private readonly IOptionsMonitor<TConfiguration> _optionsMonitor;

  public HealthCheckWithConfigurationBase(IOptionsMonitor<TConfiguration> optionsMonitor) => _optionsMonitor = optionsMonitor;

  [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Required at this time. May be removed in the future if refactored.")]
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    var configurationName = context.Registration.Name;

    try
    {
      var options = _optionsMonitor.Get(configurationName);
      if (options is null)
      {
        return HealthCheckResult.Unhealthy(description: $"Missing configuration for '{configurationName}'");
      }

      return await ExecuteHealthCheckAsync(options, context, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      return HealthCheckResult.Unhealthy(exception: ex);
    }
  }

  protected abstract ValueTask<HealthCheckResult> ExecuteHealthCheckAsync(TConfiguration options, HealthCheckContext context, CancellationToken cancellationToken);
}
#endif
