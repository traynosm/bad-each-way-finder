namespace bad_each_way_finder_domain.DomainModel
{
    public class RunnerInfo
    {
        //exchange properties
        public long RunnerSelectionId { get; set; }
        public string RunnerName { get; set; }
        public double ExchangeWinPrice { get; set; }
        public double ExchangePlacePrice { get; set; }

        //sportbook properties
        public double WinRunnerOddsDecimal { get; set; }
        public double WinExpectedValue { get; set; }
        public double PlaceExpectedValue { get; set; }
        public double EachWayExpectedValue { get; set; }

        public RunnerInfo() { }

        public RunnerInfo(RunnerInfo runner)
        {
            RunnerSelectionId = runner.RunnerSelectionId;
            RunnerName = runner.RunnerName;
            ExchangeWinPrice = runner.ExchangeWinPrice;
            ExchangePlacePrice = runner.ExchangePlacePrice;
            WinRunnerOddsDecimal = runner.WinRunnerOddsDecimal;
            WinExpectedValue = runner.WinExpectedValue;
            PlaceExpectedValue = runner.PlaceExpectedValue;
            EachWayExpectedValue = runner.EachWayExpectedValue;
        }


    }
}
