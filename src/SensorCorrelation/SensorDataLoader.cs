using System.Globalization;
using System.Text.Json;

public static class SensorDataLoader
{
    public static List<SensorReading> LoadCsv(string path)
    {
        return File.ReadAllLines(path)
            .Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line =>
            {
                var parts = line.Split(',');
                return new SensorReading(
                    int.Parse(parts[0]),
                    double.Parse(parts[1], CultureInfo.InvariantCulture),
                    double.Parse(parts[2], CultureInfo.InvariantCulture));
            })
            .ToList();
    }

    public static List<SensorReading> LoadJson(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<SensorReading>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? new List<SensorReading>();
    }
}
