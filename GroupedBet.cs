using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_PI
{
    class GroupedBet
    {
        public Team[] teams;

        public GroupedBet(IEnumerable<Button> buttons)
        {
            teams = new Team[25];
            for (int i = 0; i < 25; i++)
            {
                teams[i] = new Team(i);
            }

            foreach (Button btn in buttons)
            {
                int btnNum = int.Parse(btn.Text);

                int btnCalcNum = btnNum == 0 ? 100 : btnNum;
                btnCalcNum--;

                int teamNumber = btnCalcNum / 4;
                int btnIndex =   btnCalcNum % 4;

                Team team = teams[teamNumber];

                btn.Text = String.Format("{0:00}", btnNum);
                btn.Click += (s,e) => {
                    numberButtonClick(team, btnIndex, btn);
                };

                team.addButton(btn, btnIndex);
            }
        }

        public void reset()
        {
            foreach (Team team in teams) team.reset();
        }

        public void unblock()
        {
            foreach (Team team in teams) team.unblock();
        }

        private void numberButtonClick(Team team, int buttonIndex, Button btn) {
            team.toggleBet(buttonIndex);
        }

        public int totalBets()
        {
            return teams.Select(team => team.countBets()).Sum();
        }

        public int totalTeamsBet()
        {
            return teams.Where(team => team.isBeated()).Count();
        }

        public IEnumerable<int> betNumbers()
        {
            return teams.SelectMany(team => team.betNumbers());
        }

        public void toggleBet(int num)
        {
            num = num == 0 ? 100 : num;
            num--;
            int teamNumber = num / 4;
            int btnIndx = num % 4;

            teams[teamNumber].toggleBet(btnIndx);
        }

        public bool validate()
        {
            string errors = "";
            bool valid = true;

            // Validates quantity of bets
            int betCount = totalBets();

            if (betCount < 10)
            {
                errors += "Sua aposta deve conter pelo menos 10 números.\n";
                valid = false;
            }

            if (betCount > 20)
            {
                errors += "Sua aposta deve conter até 20 números.\n";
                valid = false;
            }

            int teamsBet = totalTeamsBet();
            if (teamsBet < 5)
            {
                errors += "Você deve apostar em pelo menos 5 times.\n";
                valid = false;
            }

            if (!valid)
            {
                MessageBox.Show(errors, "Verifique sua aposta");
            }
            return valid;
        }

        public Decimal betPrice()
        {
            Decimal price = Decimal.Parse("5");
            Decimal secondPrice = Decimal.Parse("0,75");
            Decimal lastPrice = Decimal.Parse("2,25");
            Decimal fullPrice = Decimal.Parse("7,0");
            Decimal teamAdd = Decimal.Parse("1,25");

            int pendingToCalc = totalBets() - 10;

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

            int totalTeam = totalTeamsBet() - 5;
            if (totalTeam > 0)
            {
                price = Decimal.Add(price, Decimal.Multiply(teamAdd, totalTeam));
            }
            return price;
        }

    }
}
