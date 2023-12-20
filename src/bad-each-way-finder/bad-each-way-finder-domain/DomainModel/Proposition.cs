namespace bad_each_way_finder_domain.DomainModel
{
    public class Proposition
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
        public double Rule4Deduction { get; set; }
        public double WinBsp { get; set; }
        public double PlaceBsp { get; set; }

        //result field Win/Place/Lose


        //runner info
        public long RunnerSelectionId { get; set; }
        public string RunnerName { get; set; }
        public int RunnerOrder { get; set; }
        public double ExchangeWinPrice { get; set; }
        public double ExchangeWinSize { get; set; }
        public double ExchangePlacePrice { get; set; }
        public double ExchangePlaceSize { get; set; }

        //sportbook properties
        public double WinRunnerOddsDecimal { get; set; }
        public int WinRunnerOddsNumerator { get; set; }
        public int WinRunnerOddsDenominator { get; set; }
        public double EachWayPlacePart { get; set; }
        public double WinExpectedValue { get; set; }
        public double PlaceExpectedValue { get; set; }
        public double EachWayExpectedValue { get; set; }
        public string RunnerStatus { get; set; }

        public double LatestWinPrice { get; set; }
        public double LatestPlacePrice { get; set; }
        public double LatestWinExpectedValue { get; set; }
        public double LatestEachWayExpectedValue { get; set; }

        public Proposition()
        {

        }
    }
}
