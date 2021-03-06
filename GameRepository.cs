﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_OrgaoRegulador;
using System.IO;

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

            if (!Directory.Exists("C:\\temp"))
            {
                Directory.CreateDirectory("C:\\temp");
            }

            if (!File.Exists("C:\\temp\\PREMIOS.TXT"))
            {
                StreamWriter outStream = File.CreateText("C:\\temp\\PREMIOS.TXT");
                outStream.Write(Properties.Resources.PREMIOS);
                outStream.Close();
            }

            if (!File.Exists("C:\\temp\\RESULTADOS.TXT"))
            {
                StreamWriter outStream = File.CreateText("C:\\temp\\RESULTADOS.TXT");
                outStream.Write(Properties.Resources.RESULTADOS);
                outStream.Close();
            }

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

            try
            {
                var winResult = getWinnerResult();
                game.winnerResult = winResult;
            } 
            catch (GameResultNotFound)
            {
                game.winnerResult = null;
            }
        }

        public WinnerResult getWinnerResult()
        {
            var winnerResult = new WinnerResult();
            var winnersString = endpoint.ObterTodosNumerosSorteados();
            var winnerTeam = endpoint.obterNomeTimeSorteado();

            if (winnersString == null || winnerTeam == null)
                throw new GameResultNotFound();

            for (int i = 3; i <= 8; i++)
            {
                var award = Convert.ToDecimal(endpoint.obterPremioPorAcertos(i));
                if (award.CompareTo(Decimal.Zero) <= 0)
                    throw new GameResultNotFound();

                winnerResult.awardValues.Add(i, award);
            }

            var winners = winnersString.Split(',');
            winnerResult.winnerNumbers = winners.Select(winner => betRepo.find(winner));
            winnerResult.winnerTeam = teamRepo.findTeamByName(winnerTeam);

            return winnerResult;
        }
    }
}
