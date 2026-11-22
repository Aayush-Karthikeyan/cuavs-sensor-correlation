public static class SensorMatcher
{
    // Each sensor is within 100 m of the true spot, so matching readings can be 200 m apart.
    public static List<(int Sensor1Id, int Sensor2Id)> Match(
        List<SensorReading> sensor1, List<SensorReading> sensor2, double thresholdMeters = 200)
    {
        var candidates = new List<(int Sensor1Id, int Sensor2Id, double Distance)>();

        foreach (var reading1 in sensor1)
        {
            foreach (var reading2 in sensor2)
            {
                var distance = Geo.DistanceMeters(
                    reading1.Latitude, reading1.Longitude,
                    reading2.Latitude, reading2.Longitude);

                if (distance <= thresholdMeters)
                {
                    candidates.Add((reading1.Id, reading2.Id, distance));
                }
            }
        }

        var sortedCandidates = candidates
            .OrderBy(pair => pair.Distance)
            .ThenBy(pair => pair.Sensor1Id)
            .ThenBy(pair => pair.Sensor2Id);

        var usedSensor1Ids = new HashSet<int>();
        var usedSensor2Ids = new HashSet<int>();
        var matches = new List<(int Sensor1Id, int Sensor2Id)>();

        foreach (var pair in sortedCandidates)
        {
            if (usedSensor1Ids.Contains(pair.Sensor1Id) || usedSensor2Ids.Contains(pair.Sensor2Id))
            {
                continue;
            }

            matches.Add((pair.Sensor1Id, pair.Sensor2Id));
            usedSensor1Ids.Add(pair.Sensor1Id);
            usedSensor2Ids.Add(pair.Sensor2Id);
        }

        return matches;
    }
}
