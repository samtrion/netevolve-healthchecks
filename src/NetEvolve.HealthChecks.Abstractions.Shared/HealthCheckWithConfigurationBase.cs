#if USE_HEALTHCHECK_WITH_CONFIG
namespace NetEvolve.HealthChecks.Abstractions;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
internal abstract class HealthCheckWithConfigurationBase<TConfiguration> : IHealthCheck
{
  private readonly IOptionsMonitor<TConfiguration> _optionsMonitor;

  public HealthCheckWithConfigurationBase(IOptionsMonitor<TConfiguration> optionsMonitor) => _optionsMonitor = optionsMonitor;

  [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Required at this time. May be removed in the future if refactored.")]
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    try
    {
      var configurationName = context.Registration.Name;
      var options = _optionsMonitor.Get(configurationName);
      if (options is null)
      {
        return new HealthCheckResult(context.Registration.FailureStatus, description: $"Missing configuration for '{configurationName}'");
      }

      return await ExecuteHealthCheckAsync(configurationName, options, context, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
    }
  }

  protected abstract ValueTask<HealthCheckResult> ExecuteHealthCheckAsync(string name, TConfiguration options, HealthCheckContext context, CancellationToken cancellationToken);
}
#endif
