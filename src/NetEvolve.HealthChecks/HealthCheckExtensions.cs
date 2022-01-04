namespace NetEvolve.HealthChecks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public static class HealthCheckExtensions
{
  /// <summary>
  /// Add a health check, which represents the readiness of this service.
  /// </summary>
  /// <param name="builder">Instance of <see cref="IHealthChecksBuilder"/>.</param>
  /// <param name="tags">A list of tags that can be used to filter health checks.</param>
  /// <returns>Instance of <see cref="IHealthChecksBuilder"/>.</returns>
  public static IHealthChecksBuilder AddSelf([NotNull] this IHealthChecksBuilder builder, params string[] tags)
  {
    if (builder.Services.Any(x => x.ServiceType == typeof(SelfHealthCheckMarker)))
    {
      return builder;
    }

    _ = builder.Services.AddSingleton<SelfHealthCheckMarker>();
    return builder.AddCheck("self", Self, new[] { "self", "readiness" }.Union(tags));

    static HealthCheckResult Self() => HealthCheckResult.Healthy();
  }
}
