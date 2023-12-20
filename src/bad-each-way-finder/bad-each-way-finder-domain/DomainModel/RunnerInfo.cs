using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace bad_each_way_finder_domain.DomainModel
{
        public class RunnerInfo
        {
            public string Id { get; set; }
            public long RunnerSelectionId { get; set; }
            public string RunnerName { get; set; }
            public int RunnerOrder { get; set; }

            //exchange properties
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


            public RunnerInfo() { }
        }
}

