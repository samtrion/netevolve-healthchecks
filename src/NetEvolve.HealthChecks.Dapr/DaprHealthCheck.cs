namespace NetEvolve.HealthChecks.Dapr;

using global::Dapr.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NetEvolve.HealthChecks.Abstractions;
using System.Threading;
using System.Threading.Tasks;

internal sealed class DaprHealthCheck : HealthCheckBase
{
  private readonly DaprClient _client;

  public DaprHealthCheck(DaprClient client) => _client = client;

  protected override sealed async ValueTask<HealthCheckResult> ExecuteHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken)
  {
    var healthy = await _client.CheckHealthAsync(cancellationToken).ConfigureAwait(false);

    if (healthy)
    {
      return HealthCheckResult.Healthy("Dapr Sidecar is healthy.");
    }

    return new HealthCheckResult(context.Registration.FailureStatus, "Dapr Sidecar is unhealthy.");
  }
}
