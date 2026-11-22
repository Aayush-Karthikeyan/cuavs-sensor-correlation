var csvPath = args.Length > 0 ? args[0] : "data/SensorData1.csv";
var jsonPath = args.Length > 1 ? args[1] : "data/SensorData2.json";

var sensor1 = SensorDataLoader.LoadCsv(csvPath);
var sensor2 = SensorDataLoader.LoadJson(jsonPath);

Console.WriteLine($"Sensor 1: {sensor1.Count} readings");
Console.WriteLine($"Sensor 2: {sensor2.Count} readings");
