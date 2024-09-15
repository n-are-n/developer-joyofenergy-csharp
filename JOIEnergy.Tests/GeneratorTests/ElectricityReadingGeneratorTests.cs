using Xunit;
using JOIEnergy.Generator;
using System.Collections.Generic;
using JOIEnergy.Domain;
namespace JOIEnergy.Tests.GeneratorTests;
public class ElectricityReadingGeneratorTests
{
    private readonly ElectricityReadingGenerator generator;
    private readonly int numberOfReadings;
    private readonly List<ElectricityReading> electricityReadings;
    public ElectricityReadingGeneratorTests()
    {
        generator = new ElectricityReadingGenerator();
        numberOfReadings = 5;
        electricityReadings = generator.Generate(numberOfReadings);
    }
    [Fact]
    public void Generate_ReturnsCorrectNumberOfReadings()
    {
        Assert.Equal(numberOfReadings, electricityReadings.Count);
    }
    [Fact]
    public void Generate_ReadingsAreWithinExpectedRange()
    {
        foreach (var reading in electricityReadings) Assert.InRange(reading.Reading, 0m, 1m);
    }
    [Fact]
    public void Generate_ReadingsAreSortedByTime()
    {
        for (int i = 0; i < electricityReadings.Count - 1; i++) Assert.True(electricityReadings[i].Time <= electricityReadings[i + 1].Time);
    }
}