using System.Diagnostics;
using System.Text.Json.Nodes;

namespace SensorCorrelation.Tests;

public class OutputTests
{
    [Fact]
    public async Task Program_WritesSampleOutputMatchingProvidedJson()
    {
        var sampleDirectory = Path.Combine(AppContext.BaseDirectory, "data", "Samples");
        var outputPath = Path.Combine(Path.GetTempPath(), $"sensor-output-{Guid.NewGuid():N}.json");

        try
        {
            var startInfo = new ProcessStartInfo("dotnet")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            startInfo.ArgumentList.Add(typeof(SensorReading).Assembly.Location);
            startInfo.ArgumentList.Add(Path.Combine(sampleDirectory, "SampleData1.csv"));
            startInfo.ArgumentList.Add(Path.Combine(sampleDirectory, "SampleData2.json"));
            startInfo.ArgumentList.Add(outputPath);

            using var process = Process.Start(startInfo)!;
            var stdoutTask = process.StandardOutput.ReadToEndAsync();
            var stderrTask = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            var stdout = await stdoutTask;
            var stderr = await stderrTask;

            Assert.True(process.ExitCode == 0, $"Program failed:\n{stdout}\n{stderr}");
            Assert.Contains($"Wrote 2 pairs to {outputPath}", stdout);

            var expected = JsonNode.Parse(File.ReadAllText(
                Path.Combine(sampleDirectory, "SampleOutput.json")));
            var actual = JsonNode.Parse(File.ReadAllText(outputPath));

            // Compare all JSON keys and values, ignoring indentation and line endings.
            Assert.True(JsonNode.DeepEquals(expected, actual),
                $"Expected:\n{expected}\nActual:\n{actual}");
        }
        finally
        {
            File.Delete(outputPath);
        }
    }
}
