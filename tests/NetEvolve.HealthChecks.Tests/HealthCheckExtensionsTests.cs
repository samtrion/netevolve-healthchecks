namespace NetEvolve.HealthChecks.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

[ExcludeFromCodeCoverage]
public class HealthCheckExtensionsTests
{
  [Fact]
  public void Add_SelfCheck_Expected()
  {
    var services = new ServiceCollection();

    _ = services
      .AddHealthChecks()
      .AddSelf();
    var serviceProvider = services.BuildServiceProvider();

    var healthChecksOptions = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>()!;

    Assert.Equal(1, healthChecksOptions.Value.Registrations.Count);

    var healthCheck = healthChecksOptions.Value.Registrations.First()!;

    Assert.Equal(2, healthCheck.Tags.Count);
  }

  [Fact]
  public void Add_SelfCheckWithTags_Expected()
  {
    var services = new ServiceCollection();

    _ = services
      .AddHealthChecks()
      .AddSelf("no-bang", "self");
    var serviceProvider = services.BuildServiceProvider();

    var healthChecksOptions = serviceProvider.GetService<IOptions<HealthCheckServiceOptions>>()!;

    Assert.Equal(1, healthChecksOptions.Value.Registrations.Count);

    var healthCheck = healthChecksOptions.Value.Registrations.First()!;

    Assert.Equal(3, healthCheck.Tags.Count);
  }
}
