using System.Net.Http.Json;
using FluentAssertions;
using ImdbData;
using ImdbData.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace ImdbApiTests;

[TestClass]
public class TitleBasicControllerTests
{
    private static ImdbContext _context = null!;
    private static IServiceScope _scope = null!;
    private static SeedData _seed = null!;
    private static Uri _root = null!;

    [ClassInitialize]
    public static void SetupClass(TestContext context)
    {
        _scope   = AssemblyFixture.ImdbServer.ServiceProvider.CreateScope();
        _context = _scope.ServiceProvider.GetRequiredService<ImdbContext>();
        _seed    = new SeedData(_context);
        _root    = new Uri(AssemblyFixture.ImdbServer.Urls.First());
    }
    
    [TestInitialize]
    public async Task Setup()
    {
        await _seed.Seed();
    }
    
    [TestCleanup]
    public async Task Teardown()
    {
        await _seed.Clear();
    }
    
    [ClassCleanup]
    public static void TeardownClass()
    {
        _scope.Dispose();
    }
    
    [TestMethod]
    public async Task Create_Works()
    {
        var client = new HttpClient();
        var creation = new List<DbTitleBasic>
        {
            new()
            {
                PrimaryTitle   = "My little movie",
                RuntimeMinutes = 62,
                StartYear      = 2024
            }
        };

        var uri = new Uri(_root, "imdb/title-basic/create");
        var response = await client.PostAsJsonAsync(uri, creation);
        response.Should().NotBeNull();
        
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.StatusCode + " " + content);
        
        response.IsSuccessStatusCode.Should().BeTrue();
    }
    
    [TestMethod]
    public async Task List_Works()
    {
        var client = new HttpClient();

        var uri = new Uri(_root, "imdb/title-basic/list");
        var response = await client.GetAsync(uri);
        response.Should().NotBeNull();
        
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(response.StatusCode + " " + content);
        
        response.IsSuccessStatusCode.Should().BeTrue();
    }
}