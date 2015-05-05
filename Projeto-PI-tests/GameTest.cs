using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections;
using Projeto_PI;
using System.Collections.Generic;

namespace Projeto_PI_tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void Game_AddBetNumber()
        {
            var testTeam1 = new Team("A");
            var bet1 = new BetNumber(testTeam1, "00");

            var game = new Game();
            game.AddBetNumber(bet1);

            Assert.AreEqual(bet1, game.bets.First());
            Assert.AreEqual(1, game.bets.Count());
        }

        [TestMethod]
        public void Game_teams_Property()
        {
            var testTeam1 = new Team("A");
            var testTeam2 = new Team("B");

            var bet1 = new BetNumber(testTeam1, "00");
            var bet2 = new BetNumber(testTeam1, "01");
            var bet3 = new BetNumber(testTeam2, "02");

            var game = new Game();
            game.AddBetNumber(bet1);
            game.AddBetNumber(bet2);
            game.AddBetNumber(bet3);

            Assert.AreEqual(2, game.teams.Count());
            CollectionAssert.Contains(game.teams.ToList(), testTeam1);
            CollectionAssert.Contains(game.teams.ToList(), testTeam2);
        }

        [TestMethod]
        public void Game_totalTeamsBet_Property()
        {
            var testTeam1 = new Team("A");
            var testTeam2 = new Team("B");

            var bet1 = new BetNumber(testTeam1, "00");
            var bet2 = new BetNumber(testTeam1, "01");
            var bet3 = new BetNumber(testTeam2, "02");

            var game = new Game();
            game.AddBetNumber(bet1);
            game.AddBetNumber(bet2);
            game.AddBetNumber(bet3);

            Assert.AreEqual(2, game.totalTeamsBet);
        }

        [TestMethod]
        public void Game_totalBets_Property()
        {
            var testTeam1 = new Team("A");
            var testTeam2 = new Team("B");

            var bet1 = new BetNumber(testTeam1, "00");
            var bet2 = new BetNumber(testTeam1, "01");
            var bet3 = new BetNumber(testTeam2, "02");

            var game = new Game();
            game.AddBetNumber(bet1);
            game.AddBetNumber(bet2);
            game.AddBetNumber(bet3);

            Assert.AreEqual(3, game.totalBets);
        }

        [TestMethod]
        public void Game_betedTeams_Property()
        {
            var testTeam1 = new Team("A");
            var testTeam2 = new Team("B");

            var bet1 = new BetNumber(testTeam1, "00");
            var bet2 = new BetNumber(testTeam1, "01");
            var bet3 = new BetNumber(testTeam2, "02");

            var game = new Game();
            game.AddBetNumber(bet1);
            game.AddBetNumber(bet2);
            game.AddBetNumber(bet3);

            // The keys are the beted teams
            Assert.AreEqual(2, game.betedTeams.Count());
            Assert.AreEqual(testTeam1, game.betedTeams.First().Key);
            Assert.AreEqual(testTeam2, game.betedTeams.Last().Key);

            // The values are the bets from team key
            var firstTeamBets = game.betedTeams.First().ToList();

            // For the first team, we should have bet1 and bet2
            CollectionAssert.Contains(firstTeamBets, bet1);
            CollectionAssert.Contains(firstTeamBets, bet2);
            CollectionAssert.DoesNotContain(firstTeamBets, bet3);

            // Now For the team 2
            var secondTeamBets = game.betedTeams.Last().ToList();
            CollectionAssert.DoesNotContain(secondTeamBets, bet1);
            CollectionAssert.DoesNotContain(secondTeamBets, bet2);
            CollectionAssert.Contains(secondTeamBets, bet3);
        }

        [TestMethod]
        public void Game_Price_When_10_Bet_5_Team()
        {
            var game = buildGame("01", "02", "05", "06", "09", "10", "13", "14", "17", "18");
            var price = Decimal.Parse("5");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_11_Bet_5_Team()
        {
            var game = buildGame("01", "02", "05", "06", "09", "10", "13", "14", "17", "18", "19");
            var price = Decimal.Parse("5,75");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_12_Bet_5_Team()
        {
            var game = buildGame("01", "02", "05", "06", "09", "10", "13", "14", "17", "18", "19", "20");
            var price = Decimal.Parse("6,50");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_19_Bet_5_Team()
        {
            var game = buildGame("01", "02", "03", "04",
                                 "05", "06", "07", "08",
                                 "09", "10", "11", "12",
                                 "13", "14", "15", "16",
                                 "17", "18", "19");

            var price = Decimal.Parse("20,75");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_20_Bet_5_Team()
        {
            var game = buildGame("01", "02", "03", "04",
                                 "05", "06", "07", "08",
                                 "09", "10", "11", "12",
                                 "13", "14", "15", "16",
                                 "17", "18", "19", "20");

            var price = Decimal.Parse("27,75");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_10_Bet_7_Team()
        {
            var game = buildGame("01", "02", 
                                 "05", "06",
                                 "14", "15",
                                 "18", 
                                 "24",
                                 "27",
                                 "30");

            var price = Decimal.Parse("7,50");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Price_When_20_Bet_20_Team()
        {
            var game = buildGame("01", "06", "10", "16", "17", "22", "25", "29", "33", "37", 
                                 "41", "45", "49", "53", "58", "61", "65", "69", "73", "77");

            var price = Decimal.Parse("46,5");

            Assert.AreEqual(price, game.Price());
        }

        [TestMethod]
        public void Game_Reset()
        {
            var game = buildGame("00", "01");
            game.Reset();
            Assert.AreEqual(0, game.bets.Count());
        }

        [TestMethod]
        public void Game_RemoveBetNumber()
        {
            var game = buildGame("00", "02");
            var remove = game.bets.First();
            var persist = game.bets.Last();

            game.RemoveBetNumber(remove);

            CollectionAssert.DoesNotContain(game.bets.ToList(), remove);
            CollectionAssert.Contains(game.bets.ToList(), persist);
        }

        private Game buildGame(params string[] bets)
        {
            var repo = new BetNumberRepository(new TeamRepository());
            Game g = new Game();

            bets.ToList().ForEach(bet => g.AddBetNumber(repo.find(bet)));
            
            return g;
        }
    }
}
