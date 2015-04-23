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
        private TeamRepository teamRepo;

        public class GameNotFound : System.NullReferenceException { };
        public class GameNotStored : System.NullReferenceException { };
        public class GameResultNotFound : System.NullReferenceException { };

        public GameRepository(TeamRepository teamRepo, BetNumberRepository betRepo)
        {
            this.teamRepo = teamRepo;
            this.betRepo = betRepo;
            this.endpoint = new EndPoint();
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

        public WinnerResult getWinnerResult()
        {
            var winnerResult = new WinnerResult();
            var winnersString = endpoint.ObterTodosNumerosSorteados();
            var winnerTeam = endpoint.obterNomeTimeSorteado();

            if (winnersString == null || winnerTeam == null)
                throw new GameResultNotFound();

            var winners = winnersString.Split(',');
            winnerResult.winnerNumbers = winners.Select(winner => betRepo.find(winner));
            winnerResult.winnerTeam = teamRepo.findTeamByName(winnerTeam);

            return winnerResult;
        }
    }
}
