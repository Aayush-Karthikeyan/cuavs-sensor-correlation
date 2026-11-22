var csvPath = args.Length > 0 ? args[0] : "data/SensorData1.csv";
var jsonPath = args.Length > 1 ? args[1] : "data/SensorData2.json";

var sensor1 = SensorDataLoader.LoadCsv(csvPath);
var sensor2 = SensorDataLoader.LoadJson(jsonPath);

Console.WriteLine($"Sensor 1: {sensor1.Count} readings");
Console.WriteLine($"Sensor 2: {sensor2.Count} readings");

var csvReading = sensor1.FirstOrDefault(reading => reading.Id == 56);
var jsonReading = sensor2.FirstOrDefault(reading => reading.Id == 46);

if (csvReading is null || jsonReading is null)
{
    Console.WriteLine("Sample distance unavailable: CSV reading 56 or JSON reading 46 is missing.");
}
else
{
    var distance = Geo.DistanceMeters(
        csvReading.Latitude, csvReading.Longitude,
        jsonReading.Latitude, jsonReading.Longitude);

    Console.WriteLine($"Distance (CSV 56 to JSON 46): {distance:F2} metres");
}
