using ImdbApi;

namespace ImdbApiTests;

[TestClass]
public class AssemblyFixture
{
    public static ImdbServer ImdbServer = null!;
    
    [AssemblyInitialize]
    public static async Task SetupAssembly(TestContext context)
    {
        ImdbServer = new ImdbServer(true, []);
        await ImdbServer.Start();
    }

    [AssemblyCleanup]
    public static async Task TeardownAssembly()
    {
        await ImdbServer.Stop();
    }

    [TestMethod]
    public void ItWorks()
    {
        Assert.IsNotNull(ImdbServer);
    }
}