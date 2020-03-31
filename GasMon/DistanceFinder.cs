using System;

namespace GasMon
{
    interface IDistanceFinder
    {
        double VerticalDistance(Location firstLocation, Location secondLocation);
        double HorizontalDistance(Location firstLocation, Location secondLocation);
        double DiagonalDistance(Location firstLocation, Location secondLocation);
    }

    public class DistanceFinder : IDistanceFinder
    {
        public double VerticalDistance(Location firstLocation, Location secondLocation)
        {
            return Math.Abs(firstLocation.x - secondLocation.x);
        }

        public double HorizontalDistance(Location firstLocation, Location secondLocation)
        {
            return Math.Abs(firstLocation.y - secondLocation.y);
        }

        public double DiagonalDistance(Location firstLocation, Location secondLocation)
        {
            return
                Math.Sqrt(
                    Math.Pow(VerticalDistance(firstLocation, secondLocation), 2) +
                    Math.Pow(HorizontalDistance(firstLocation, secondLocation), 2));
        }
    }
}