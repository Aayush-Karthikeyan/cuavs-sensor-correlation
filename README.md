# cuavs-sensor-correlation

Correlates anomaly readings from two sensors (CSV + JSON) using geodesic distance matching – Canadian UAVs intern coding challenge.

## Requirements
- .NET 10 SDK

## Run
```bash
dotnet run --project src/SensorCorrelation
```
Reads `data/SensorData1.csv` and `data/SensorData2.json` and writes `output.json`.
Custom paths: `dotnet run --project src/SensorCorrelation -- <sensor1.csv> <sensor2.json> <output.json>`

## Test
```bash
dotnet test
```
Includes an end-to-end test confirming the sample data produces exactly `Samples/SampleOutput.json`.

## Result
11 matched pairs (from 12 candidate pairs within 200 m) – see `output.json`.

## Approach
1. **Distance:** haversine formula on WGS 84 decimal-degree coordinates (Earth radius 6,371 km).
2. **Threshold:** each sensor is accurate to 100 m, so two readings of the same anomaly can be up to 100 + 100 = 200 m apart. Pairs exactly at the threshold are included.
3. **Matching:** all candidate pairs within the threshold are sorted by distance, then sensor 1 ID, then sensor 2 ID. Pairs are taken greedily, skipping any pair whose ID is already used, so each reading is matched at most once.
4. **Ties:** the real data has a reading exactly the same distance from two others; the ID tie-breakers make the result deterministic regardless of input order.

## Assumptions and limitations
- Input files are well-formed (CSV header `id,latitude,longitude`; JSON array of `{id, latitude, longitude}`).
- IDs are unique within each sensor but may repeat across sensors, so output keys are sensor 1 IDs and values are sensor 2 IDs.
- Greedy closest-first matching is simple and deterministic but not guaranteed to be globally optimal when many readings cluster together; an optimal assignment (e.g. the Hungarian algorithm) could be used instead.
- Comparing every pair is O(n × m), which is fine here. For very large datasets, a spatial index (grid or k-d tree) would limit comparisons to nearby readings.

## Project structure
```
src/SensorCorrelation/        SensorReading, SensorDataLoader, Geo, SensorMatcher, Program
tests/SensorCorrelation.Tests/  Geo, matcher and end-to-end output tests
data/                         Provided sensor data and samples
```
