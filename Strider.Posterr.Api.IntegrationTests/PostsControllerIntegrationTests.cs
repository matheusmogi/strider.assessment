using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using Strider.Posterr.Api.ApiModel;

namespace Strider.Posterr.Api.IntegrationTests;

[TestFixture]
public class PostsControllerIntegrationTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        var testConfigurationSettings = new Dictionary<string, string>
        {
            { "IsTesting", "true" },
        };

        var testConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(testConfigurationSettings)
            .Build();

        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => { builder.UseConfiguration(testConfiguration); });

        _client = factory.CreateClient();
    }

    [Test]
    public async Task UserProfile_CreateAsync_ShouldCreateNewPost()
    {
        //act
        var response = await _client.PostAsync("/UserProfile/CreatePost", JsonContent.Create(new PostRequestApiModel { Content = "integration-test" }));

        //assert
        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
    }
    
    [Test]
    public async Task Home_CreateAsync_ShouldCreateNewPost()
    {
        //act
        var response = await _client.PostAsync("/Home/CreatePost", JsonContent.Create(new PostRequestApiModel { Content = "integration-test" }));

        //assert
        response.EnsureSuccessStatusCode();
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Accepted));
    }
 
}