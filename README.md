# cuavs-sensor-correlation

Correlates anomaly readings from two sensors (CSV + JSON) using geodesic distance matching – Canadian UAVs intern coding challenge

## Matching readings

`SensorMatcher.Match(sensor1, sensor2, thresholdMeters: 200)` finds every pair
within the distance threshold using the haversine formula. The default is 200 m:
each sensor is within 100 m of the true anomaly location, so two readings of the
same anomaly can be up to 100 + 100 = 200 m apart. Pairs exactly at the threshold
are included.

Candidates are sorted by distance, then sensor 1 ID, then sensor 2 ID. The real
data includes a reading at exactly the same distance from two other readings,
so the ID tie-breakers make the result deterministic regardless of input order.

The matcher takes pairs in that order, skipping any pair whose sensor 1 ID or
sensor 2 ID has already been used. Each ID can appear at most once per sensor.
The selected pairs are returned in the order they were chosen.
