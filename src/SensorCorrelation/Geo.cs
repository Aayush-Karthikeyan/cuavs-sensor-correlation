public static class Geo
{
    public static double DistanceMeters(double lat1, double lon1, double lat2, double lon2)
    {
        const double earthRadiusMeters = 6_371_000;
        const double degreesToRadians = Math.PI / 180;

        var dLat = (lat2 - lat1) * degreesToRadians;
        var dLon = (lon2 - lon1) * degreesToRadians;
        lat1 *= degreesToRadians;
        lat2 *= degreesToRadians;

        var sinHalfLat = Math.Sin(dLat / 2);
        var sinHalfLon = Math.Sin(dLon / 2);
        var a = sinHalfLat * sinHalfLat
            + Math.Cos(lat1) * Math.Cos(lat2) * sinHalfLon * sinHalfLon;
        a = Math.Clamp(a, 0, 1);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusMeters * c;
    }
}
