using System.Text.Json;

var csvPath = args.Length > 0 ? args[0] : "data/SensorData1.csv";
var jsonPath = args.Length > 1 ? args[1] : "data/SensorData2.json";
var outputPath = args.Length > 2 ? args[2] : "output.json";

var sensor1 = SensorDataLoader.LoadCsv(csvPath);
var sensor2 = SensorDataLoader.LoadJson(jsonPath);

Console.WriteLine($"Sensor 1: {sensor1.Count} readings");
Console.WriteLine($"Sensor 2: {sensor2.Count} readings");

const double thresholdMeters = 200;
var matches = SensorMatcher.Match(sensor1, sensor2, thresholdMeters);

Console.WriteLine($"Matched pairs: {matches.Count}");
foreach (var match in matches)
{
    Console.WriteLine($"{match.Sensor1Id} -> {match.Sensor2Id}");
}

var dict = matches.ToDictionary(match => match.Sensor1Id.ToString(), match => match.Sensor2Id);
var json = JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true });
File.WriteAllText(outputPath, json);
Console.WriteLine($"Wrote {dict.Count} pairs to {outputPath}");
