namespace GasMon
{
    public class Reading
    {
        public string EventId { get; set; }
        public string LocationId { get; set; }
        public double Value { get; set; }
        public long Timestamp { get; set; }

        public override string ToString()
        {
            return $"EventID: {EventId}, LocationID: {LocationId}, Value:{Value}, Timestamp: {Timestamp}";
        }
    }
}