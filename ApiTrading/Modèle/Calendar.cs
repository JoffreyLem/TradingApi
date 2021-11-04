namespace Modele
{
    public class Calendar
    {
        public Calendar(dynamic calendarResponse)
        {
            Country = calendarResponse.Country;
            Current = calendarResponse.Current;
            Forecast = calendarResponse.Forecast;
            Impact = calendarResponse.Impact;
            Period = calendarResponse.Period;
            Previous = calendarResponse.Previous;
            Time = calendarResponse.Time;
            Title = calendarResponse.Title;
        }

        public string Country { get; }
        public string Current { get; }
        public string Forecast { get; }
        public string Impact { get; }
        public string Period { get; }
        public string Previous { get; }
        public long? Time { get; }
        public string Title { get; }
    }
}