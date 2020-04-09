using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourierApplication.DAL
{
    public static class GeoCoordinatesExtensions
    {
        private const double PIx = Math.PI;
        private static double Radians(double x)
        {
            return x * PIx / 180;
        }

        public static double GetDistanceTo(this GeoCoordinate g1, GeoCoordinate g2)
        {
            double R = 6371; // km
            
            double sLat1 = Math.Sin(Radians(g1.Latitude));
            double sLat2 = Math.Sin(Radians(g2.Latitude));
            double cLat1 = Math.Cos(Radians(g1.Latitude));
            double cLat2 = Math.Cos(Radians(g2.Latitude));
            double cLon = Math.Cos(Radians(g1.Longitude) - Radians(g2.Longitude));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }
    }
}
