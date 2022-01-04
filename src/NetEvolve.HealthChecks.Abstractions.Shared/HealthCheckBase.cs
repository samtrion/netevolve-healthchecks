#if USE_HEALTHCHECK
namespace NetEvolve.HealthChecks.Abstractions;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
internal abstract class HealthCheckBase : IHealthCheck
{
  public HealthCheckBase() { }

  [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Required at this time. May be removed in the future if refactored.")]
  public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
  {
    try
    {
      return await ExecuteHealthCheckAsync(context, cancellationToken).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
      return HealthCheckResult.Unhealthy(exception: ex);
    }
  }

  protected abstract ValueTask<HealthCheckResult> ExecuteHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken);
}
#endif
