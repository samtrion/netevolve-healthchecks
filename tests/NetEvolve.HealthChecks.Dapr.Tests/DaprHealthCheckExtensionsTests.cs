namespace NetEvolve.HealthChecks.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using NetEvolve.HealthChecks.Dapr;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

[ExcludeFromCodeCoverage]
public class DaprHealthCheckExtensionsTests
{
  [Fact]
  public void Add_Dapr_Expected()
  {
    var services = new ServiceCollection();

    _ = services
      .AddHealthChecks()
      .AddDapr();
    var serviceProvider = services.BuildServiceProvider();

    var healthChecksOptions = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>()!;

    Assert.Equal(1, healthChecksOptions.Value.Registrations.Count);

    var healthCheck = healthChecksOptions.Value.Registrations.First()!;

    Assert.Equal(HealthStatus.Unhealthy, healthCheck.FailureStatus);
    Assert.Equal(0, healthCheck.Tags.Count);
  }

  [Fact]
  public void Add_Dapr_MultipleTimes_Expected()
  {
    var services = new ServiceCollection();

    _ = services
      .AddHealthChecks()
      .AddDapr()
      .AddDapr();
    var serviceProvider = services.BuildServiceProvider();

    var healthChecksOptions = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>()!;

    Assert.Equal(1, healthChecksOptions.Value.Registrations.Count);
  }
}
