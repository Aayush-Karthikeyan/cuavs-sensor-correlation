namespace SensorCorrelation.Tests;

public class GeoTests
{
    [Theory]
    [InlineData(51, -114, 51, -114, 0)]
    [InlineData(0, 0, 0, 1, 111194.92664455874)]
    [InlineData(0, 0, 90, 0, 10007543.398010286)]
    [InlineData(0, 0, 0, 180, 20015086.79602057)]
    [InlineData(0, 179, 0, -179, 222389.85328911748)]
    public void DistanceMeters_MatchesKnownDistances(
        double lat1, double lon1, double lat2, double lon2, double expected)
    {
        var actual = Geo.DistanceMeters(lat1, lon1, lat2, lon2);

        Assert.InRange(Math.Abs(actual - expected), 0, 0.000001);
    }
}
