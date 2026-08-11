using System.Reflection.Metadata;

namespace NuCaps.Core.Tests;

/// <summary>
/// Proves the test harness runs, and that this project's central bet holds: that
/// <see cref="MetadataReader"/> resolves with no package reference at all, because
/// System.Reflection.Metadata ships in the net10.0 shared framework. If that ever stops being
/// true, NuCaps.Core acquires a dependency, and this test is where it shows up first.
/// </summary>
public class HarnessTests
{
    [Test]
    public async Task Srm_resolves_from_the_shared_framework()
    {
        string? assemblyName = typeof(MetadataReader).Assembly.GetName().Name;

        await Assert.That(assemblyName).IsEqualTo("System.Reflection.Metadata");
    }
}
