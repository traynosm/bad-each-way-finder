namespace bad_each_way_finder_domain.DomainModel
{
    public class Proposition : RunnerInfo
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime RecordedAt { get; set; }

        //exchange poperties
        public string ExchangeWinMarketId { get; set; }
        public string ExchangePlaceMarketId { get; set; }

        //sportbook properties
        public string SportsbookWinMarketId { get; set; }
        public bool SportsbookEachwayAvailable { get; set; }
        public int SportsbookNumberOfPlaces { get; set; }
        public int SportsbookPlaceFractionDenominator { get; set; }


        public Proposition()
        {

        }
        public Proposition(Race race, RunnerInfo runner) : base(runner)
        {
            EventId = race.EventId;
            EventName = race.EventName;
            EventDateTime = race.EventDateTime;
            RecordedAt = DateTime.UtcNow;
            ExchangeWinMarketId = race.ExchangeWinMarketId;
            ExchangePlaceMarketId = race.ExchangePlaceMarketId;
            SportsbookWinMarketId = race.SportsbookWinMarketId;
            SportsbookEachwayAvailable = race.SportsbookEachwayAvailable;
            SportsbookNumberOfPlaces = race.SportsbookNumberOfPlaces;
            SportsbookPlaceFractionDenominator = race.SportsbookPlaceFractionDenominator;
        }
    }
}
