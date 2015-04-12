using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_OrgaoRegulador;

namespace Projeto_PI
{
    class GameRepository
    {
        private EndPoint endpoint;
        private BetNumberRepository betRepo;

        public class GameNotFound : System.NullReferenceException { };
        public class GameNotStored : System.NullReferenceException { };

        public GameRepository(BetNumberRepository betRepo = null)
        {
            if (betRepo == null)
            {
                 betRepo = new BetNumberRepository();
            }

            this.endpoint = new EndPoint();
            this.betRepo = betRepo;
        }

        public void storeGame(Game game)
        {
            string toApi = game.bets.Select(bet => bet.number).Aggregate((src, acc) => src + "," + acc);
            long result = endpoint.gravarAposta(toApi);

            if (result == 0)
            {
                throw new GameNotStored();
            }

            game.protocol = result;
        }

        public void loadToGame(Game game, long protocol)
        {
            string betsString = endpoint.obterTodasDezenasApostadas(protocol);

            if (betsString == null)
            {
                throw new GameNotFound();
            }

            game.protocol = protocol;

            var bets = betsString.Split(',').Select(betString => betRepo.find(betString));

            foreach (var bet in bets)
                game.AddBetNumber(bet);
        }
    }
}
