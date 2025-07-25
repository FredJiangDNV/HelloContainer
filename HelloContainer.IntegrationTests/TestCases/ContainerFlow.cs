using FluentAssertions;
using HelloContainer.DTOs;
using HelloContainer.IntegrationTests.Fixtures;
using HelloContainer.IntegrationTests.Services;
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
    public async Task A010_CreateContainer()
    {
        // Arrange
        var name = Utilities.RandomName();

        // Act
        var response = await _fixture.CreateContainer(name, 10);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        _fixture.C_Id = result!.Id;

        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.Capacity.Should().Be(10.0);
    }

    [Fact]
    public async Task A020_CreateContainer2()
    {
        // Arrange
        var name = Utilities.RandomName();

        // Act
        var response = await _fixture.CreateContainer(name, 10);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        _fixture.C2_Id = result!.Id;
        result!.Name.Should().Be(name);
    }

    [Fact]
    public async Task A030_Connect_Containers()
    {
        // Arrange
        var connectDto = new ConnectContainersDto(_fixture.C_Id, _fixture.C2_Id);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync("/api/containers/connections", connectDto);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task A040_AddWater_To_Container1()
    {
        // Arrange
        var addWaterDto = new AddWaterDto(4);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/containers/{_fixture.C_Id}/water", addWaterDto);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        result!.Amount.Should().Be(2);
    }


    [Fact]
    public async Task A050_GetWater_Container2()
    {
        // Act
        var response = await _fixture.Client.GetAsync($"/api/containers/{_fixture.C2_Id}");

        // Assert
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        result!.Amount.Should().Be(2);
    }

    [Fact]
    public async Task A060_AddWater_TriggerAnAlert()
    {
        // Arrange
        var addWaterDto = new AddWaterDto(12);

        // Act
        var response = await _fixture.Client.PostAsJsonAsync($"/api/containers/{_fixture.C_Id}/water", addWaterDto);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<ContainerReadDto>();
        result!.Amount.Should().Be(8);
    }

    [Fact]
    public async Task A070_GetAlert()
    {
        // Act
        var response = await _fixture.Client.GetAsync($"/api/alerts/{_fixture.C_Id}");

        // Assert
        var alerts = await response.Content.ReadFromJsonAsync<IEnumerable<AlertReadDto>>();
        var alert = Assert.Single(alerts);

        alert.Should().NotBeNull();
        alert!.ContainerId.Should().Be(_fixture.C_Id);
        alert.Message.Should().Be($"Container {_fixture.C_Id} has overflowed with water.");
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
