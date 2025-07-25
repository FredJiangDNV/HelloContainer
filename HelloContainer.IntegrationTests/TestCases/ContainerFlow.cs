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
        var name = Utilities.RandomName();

        var result = await _fixture.CreateContainer(name, 10);

        _fixture.C_Id = result!.Id;

        result.Should().NotBeNull();
        result!.Name.Should().Be(name);
        result.Capacity.Should().Be(10.0);
    }

    [Fact]
    public async Task A020_CreateContainer2()
    {
        var name = Utilities.RandomName();

        var result = await _fixture.CreateContainer(name, 10);

        _fixture.C2_Id = result!.Id;
        result!.Name.Should().Be(name);
    }

    [Fact]
    public async Task A030_Connect_Containers()
    {
        var result = await _fixture.ConnectContainers(_fixture.C_Id, _fixture.C2_Id);
        result!.Id.Should().Be(_fixture.C_Id);
    }

    [Fact]
    public async Task A040_AddWater_To_Container1()
    {
        var response = await _fixture.AddWater(_fixture.C_Id, 4);
        response!.Amount.Should().Be(2);
    }

    [Fact]
    public async Task A050_GetWater_Container2()
    {
        var result = await _fixture.GetContainer(_fixture.C2_Id);
        result!.Amount.Should().Be(2);
    }

    [Fact]
    public async Task A060_AddWater_TriggerAnAlert()
    {
        var response = await _fixture.AddWater(_fixture.C_Id, 12);
        response!.Amount.Should().Be(8);
    }

    [Fact]
    public async Task A070_GetAlert()
    {
        var alerts = await _fixture.GetAlerts(_fixture.C_Id);

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
