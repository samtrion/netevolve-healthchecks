namespace NetEvolve.HealthChecks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;

public static class HealthCheckExtensions
{
  /// <summary>
  /// Add a health check, which represents the readiness of this service.
  /// </summary>
  /// <param name="builder">Instance of <see cref="IHealthChecksBuilder"/>.</param>
  /// <param name="tags">A list of tags that can be used to filter health checks.</param>
  /// <returns>Instance of <see cref="IHealthChecksBuilder"/>.</returns>
  public static IHealthChecksBuilder AddSelf(this IHealthChecksBuilder builder, params string[] tags)
    => builder.AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "self", "readiness" }.Union(tags));
}
