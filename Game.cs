using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_PI
{
    public class Game
    {
        public WinnerResult winnerResult;
        public IEnumerable<BetNumber> bets { get; private set; }
        public IEnumerable<Team> teams
        {
            get
            {
                return betedTeams.Select(group => group.Key);
            }
        }

        public IEnumerable<IGrouping<Team, BetNumber>> betedTeams
        {
            get
            {
                return bets.GroupBy(bet => bet.team, bet => bet);
            }
        }

        public bool resultAvailable { get { return winnerResult != null; } }

        public IEnumerable<BetNumber> winnedNumbersOnRaffle
        {
            get
            {
                return bets.Intersect(winnerResult.winnerNumbers);
            }
        }

        public IEnumerable<BetNumber> winnedNumbersOnTeam
        {
            get
            {
                var winnerTeam = winnerResult.winnerTeam;
                var teamNumbersGroup = betedTeams.FirstOrDefault(group => group.Key == winnerTeam);

                if (teamNumbersGroup == null)
                    return new List<BetNumber>();

                return teamNumbersGroup.ToList();
            }
        }

        public bool winned
        {
            get
            {
                return winnedNumbersOnRaffle.Count() > 2 || winnedNumbersOnTeam.Count() > 0;
            }
        }

        public long protocol;

        public Game()
        {
            bets = new List<BetNumber>();
        }

        public int totalBets
        {
            get { return bets.Count(); }
        }

        public int totalTeamsBet
        {
            get { return teams.Count(); }
        }

        public Decimal Price()
        {
            Decimal price = Decimal.Parse("5");
            Decimal secondPrice = Decimal.Parse("0,75");
            Decimal lastPrice = Decimal.Parse("2,25");
            Decimal fullPrice = Decimal.Parse("4,0");
            Decimal teamAdd = Decimal.Parse("1,25");

            int pendingToCalc = totalBets - 10;

            if (pendingToCalc > 0)
            {
                price = Decimal.Add(price, Decimal.Multiply(secondPrice, pendingToCalc));
                pendingToCalc -= 5;
            }

            if (pendingToCalc > 0)
            {
                price = Decimal.Add(price, Decimal.Multiply(lastPrice, pendingToCalc));
            }

            if (pendingToCalc == 5)
            {
                price = Decimal.Add(price, fullPrice);
            }

            int totalTeam = totalTeamsBet - 5;
            if (totalTeam > 0)
            {
                price = Decimal.Add(price, Decimal.Multiply(teamAdd, totalTeam));
            }
            return price;
        }

        public Decimal RafflePrize()
        {   
            Decimal rafflePrize;

            var raffleCount = winnedNumbersOnRaffle.Count();
            if (winnerResult.awardValues.TryGetValue(raffleCount, out rafflePrize))
            {
                return raffleCount;
            }

            return Decimal.Zero;
        }

        public Decimal TeamPrize()
        {
            Decimal prizeByTeam = Convert.ToDecimal(5.0);
            var teamCount = winnedNumbersOnTeam.Count();

            return Decimal.Multiply(prizeByTeam, teamCount);
        }

        public Decimal Prize()
        {
            return Decimal.Add(RafflePrize(), TeamPrize());
        }

        public void Reset()
        {
            var bets = (List<BetNumber>)this.bets;
            bets.Clear();
        }

        public void AddBetNumber(BetNumber bet)
        {
            var bets = (List<BetNumber>)this.bets;
            bets.Add(bet);
        }

        public void RemoveBetNumber(BetNumber bet)
        {
            var bets = (List<BetNumber>)this.bets;
            bets.Remove(bet);
        }
    }
}
