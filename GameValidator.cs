using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class GameValidator
    {
        private Game game;
        public IEnumerable<String> errors { get; private set; }

        public GameValidator(Game game)
        {
            this.game = game;
            this.errors = new List<String>();
        }

        public bool valid()
        {
            List<String> errors = (List<String>)this.errors;

            errors.Clear();

            // Validates quantity of bets
            int betCount = game.totalBets;

            if (betCount < 10)
                errors.Add("Sua aposta deve conter pelo menos 10 números.");

            if (betCount > 20)
                errors.Add("Sua aposta deve conter até 20 números.");


            int teamsBet = game.totalTeamsBet;
            if (teamsBet < 5)
            {
                errors.Add("Você deve apostar em pelo menos 5 times.");
            }

            return errors.Count == 0;
        }
    }
}
