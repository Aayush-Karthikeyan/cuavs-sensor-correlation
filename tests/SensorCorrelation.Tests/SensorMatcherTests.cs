namespace SensorCorrelation.Tests;

public class SensorMatcherTests
{
    [Fact]
    public void Match_SelectsClosestPairsWithoutReusingEitherId()
    {
        var sensor1 = new List<SensorReading>
        {
            new(1, 0, 0),
            new(2, 0, 0.001)
        };
        var sensor2 = new List<SensorReading>
        {
            new(10, 0, 0.0009),
            new(20, 0, -0.001)
        };

        var matches = SensorMatcher.Match(sensor1, sensor2);

        Assert.Equal(new[] { (2, 10), (1, 20) }, matches);
    }

    [Fact]
    public void Match_BreaksEqualDistanceTiesByBothIdsRegardlessOfInputOrder()
    {
        var sensor1 = new List<SensorReading> { new(2, 0, 0), new(1, 0, 0) };
        var sensor2 = new List<SensorReading> { new(20, 0, 0), new(10, 0, 0) };
        var expected = new[] { (1, 10), (2, 20) };

        Assert.Equal(expected, SensorMatcher.Match(sensor1, sensor2));

        sensor1.Reverse();
        sensor2.Reverse();

        Assert.Equal(expected, SensorMatcher.Match(sensor1, sensor2));
    }

    [Fact]
    public void Match_IncludesDistanceAtThresholdAndExcludesDistanceAboveIt()
    {
        var sensor1 = new List<SensorReading> { new(1, 0, 0) };
        var sensor2 = new List<SensorReading> { new(2, 0, 0.001) };
        var distance = Geo.DistanceMeters(0, 0, 0, 0.001);

        Assert.Equal(new[] { (1, 2) }, SensorMatcher.Match(sensor1, sensor2, distance));
        Assert.Empty(SensorMatcher.Match(sensor1, sensor2, Math.BitDecrement(distance)));
    }

    [Theory]
    [InlineData(100, false)]
    [InlineData(200, true)]
    public void Match_UsesSuppliedThreshold(double thresholdMeters, bool shouldMatch)
    {
        var sensor1 = new List<SensorReading> { new(1, 0, 0) };
        var sensor2 = new List<SensorReading> { new(2, 0, 0.001) };

        var matches = SensorMatcher.Match(sensor1, sensor2, thresholdMeters);

        Assert.Equal(shouldMatch ? 1 : 0, matches.Count);
    }

    [Fact]
    public void Match_ReturnsNoPairsWhenEitherSensorIsEmpty()
    {
        var readings = new List<SensorReading> { new(1, 0, 0) };
        var empty = new List<SensorReading>();

        Assert.Empty(SensorMatcher.Match(readings, empty));
        Assert.Empty(SensorMatcher.Match(empty, readings));
    }
}
