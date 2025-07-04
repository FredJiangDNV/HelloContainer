using FluentAssertions;
using HelloContainer.Application.DTOs;
using HelloContainer.IntegrationTests.Fixtures;
using System.Net.Http.Json;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace HelloContainer.IntegrationTests.TestCases;

[TestCaseOrderer(
    "HelloContainer.IntegrationTests.TestCases.AlphabeticalOrderer",
    "HelloContainer.IntegrationTests"
)]
public class ContainerFlow : IClassFixture<HelloContainerFixture>
{
    private readonly HelloContainerFixture _fixture;

    public ContainerFlow(HelloContainerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task A010_CreateContainer_A()
    {
        // Arrange
        var createDto = new CreateContainerDto("A", 100);

        // Act
        var response = await this._fixture.Client.PostAsJsonAsync("/api/containers", createDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        _fixture.ContainerAId = result!.Id;

        result.Should().NotBeNull();
        result!.Name.Should().Be("A");
        result.Capacity.Should().Be(100.0);
    }

    [Fact]
    public async Task A020_CreateContainer_B()
    {
        // Arrange
        var createDto = new CreateContainerDto("B", 100);

        // Act
        var response = await this._fixture.Client.PostAsJsonAsync("/api/containers", createDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        _fixture.ContainerBId = result!.Id;
        result!.Name.Should().Be("B");
    }

    [Fact]
    public async Task A030_Connect_A_And_B()
    {
        // Arrange
        var connectDto = new ConnectContainersDto(_fixture.ContainerAId, _fixture.ContainerBId);

        // Act
        var response = await this._fixture.Client.PostAsJsonAsync("/api/containers/connections", connectDto);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        result!.Name.Should().Be("A");
    }
}

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        var list = testCases.OrderBy(x => x.TestMethod.Method.Name, StringComparer.OrdinalIgnoreCase);
        list.ToList().ForEach(x => Console.WriteLine(x.DisplayName));
        return list;
    }
}
