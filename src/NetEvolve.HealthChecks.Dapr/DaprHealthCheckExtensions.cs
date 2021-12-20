namespace NetEvolve.HealthChecks.Dapr;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public static class DaprHealthCheckExtensions
{
  /// <summary>
  /// Add a health check, which determines the health state of the dapr sidecar.
  /// </summary>
  /// <param name="builder">Instance of <see cref="IHealthChecksBuilder"/>.</param>
  /// <param name="failureStatus">Failure status - <see langword="default"/> <see cref="HealthStatus.Unhealthy"/>.</param>
  /// <param name="tags">A list of tags that can be used to filter health checks.</param>
  /// <returns>Instance of <see cref="IHealthChecksBuilder"/>.</returns>
  /// <exception cref="ArgumentNullException">When <paramref name="builder"/> is <see langword="null"/>.</exception>
  public static IHealthChecksBuilder AddDapr([NotNull] this IHealthChecksBuilder builder, HealthStatus failureStatus = HealthStatus.Unhealthy, params string[] tags)
  {
    if (builder.Services.Any(x => x.ServiceType == typeof(DaprHealthCheckMarker)))
    {
      return builder;
    }

    _ = builder.Services.AddSingleton<DaprHealthCheckMarker>();
    return builder.AddCheck<DaprHealthCheck>("dapr-sidecar", failureStatus, tags);
  }
}
