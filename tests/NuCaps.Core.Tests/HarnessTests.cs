using System.Reflection.Metadata;

namespace NuCaps.Core.Tests;

/// <summary>
/// Proves that the test harness runs, and that <see cref="MetadataReader"/> resolves without a
/// package reference, because System.Reflection.Metadata ships inside the net10.0 shared
/// framework. If this test ever fails, reading metadata has started to need a package.
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
