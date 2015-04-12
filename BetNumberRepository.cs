using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class BetNumberRepository
    {
        private TeamRepository teamRepo;
        private const int BET_BY_TEAM = 4;
        private IEnumerable<BetNumber> betNumbers;

        public BetNumberRepository(TeamRepository teamRepo = null)
        {
            if (teamRepo != null)
            {
                this.teamRepo = teamRepo;
            }
            else
            {
                this.teamRepo = new TeamRepository();
            }
        }

        public IEnumerable<BetNumber> availableBetNumbers()
        {
            // Don't create new instances in every call to this method
            // 1. Prevent Equal problems.
            // 2. Prevent large memory usage.
            //
            // If you want new ones, Create a new repository instance.
            //
            if (this.betNumbers != null)
                return this.betNumbers;

            var teams = teamRepo.availableTeams();

            List<BetNumber> betNumbers = new List<BetNumber>();
            int currentBetNumber = 1;

            foreach(Team team in teams)
            {
                for (int i = 1; i <= BET_BY_TEAM; i++)
                {
                    // Makes the last bet number become 00
                    if (currentBetNumber == 100)
                        currentBetNumber = 0;

                    string number = String.Format("{0:00}", currentBetNumber);
                    betNumbers.Add(new BetNumber(team, number));
                    currentBetNumber++;
                }
            }

            this.betNumbers = betNumbers;
            return betNumbers;
        }

        public BetNumber find(string betNumberStr)
        {
            return availableBetNumbers().First(bet => bet.number.Equals(betNumberStr));
        }
    }
}
