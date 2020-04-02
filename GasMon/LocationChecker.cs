using System.Collections.Generic;
using System.Linq;

namespace GasMon
{
    public class LocationChecker
    {
        private readonly IEnumerable<string> _validLocationIds;

        public LocationChecker(IEnumerable<Location> validLocations)
        {
            _validLocationIds = validLocations.Select(location => location.Id);
        }

        public bool ValidLocationCheck(ReadingMessage readingMessage)
        {
            return _validLocationIds.Contains(readingMessage.Reading.LocationId);
        }
    }
}