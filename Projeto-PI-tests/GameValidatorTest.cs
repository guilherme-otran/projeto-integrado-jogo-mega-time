using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projeto_PI;
using System.Linq;

namespace Projeto_PI_tests
{
    [TestClass]
    public class GameValidatorTest
    {
        [TestMethod]
        public void GameValidator_Valid_WhenGameIsValid()
        {
            var game = buildGame("01", "02",
                                 "05", "06",
                                 "09", "10",
                                 "13", "14",
                                 "17", "18",
                                 "19");
            var validator = new GameValidator(game);
            Assert.AreEqual(true, validator.valid());
        }

        [TestMethod]
        public void GameValidator_Errors_WhenGameIsValid()
        {
            var game = buildGame("01", "02",
                                 "05", "06",
                                 "09", "10",
                                 "13", "14",
                                 "17", "18",
                                 "19");

            var validator = new GameValidator(game);
            validator.valid();

            Assert.AreEqual(0, validator.errors.Count());
        }

        [TestMethod]
        public void GameValidator_Valid_WhenTeamLess_WhenNumberLess()
        {
            var game = buildGame("01", "02", "03");
            var validator = new GameValidator(game);
            Assert.AreEqual(false, validator.valid());
        }

        [TestMethod]
        public void GameValidator_Errors_WhenTeamLess_WhenNumberLess()
        {
            var game = buildGame("01", "02", "03");
            var validator = new GameValidator(game);
            validator.valid();

            Assert.AreEqual(2, validator.errors.Count());
            CollectionAssert.Contains(validator.errors.ToList(), "Sua aposta deve conter pelo menos 10 números.");
            CollectionAssert.Contains(validator.errors.ToList(), "Você deve apostar em pelo menos 5 times.");
        }

        [TestMethod]
        public void GameValidator_Valid_WhenNumberLess()
        {
            var game = buildGame("01", "05", "09", "13", "17");
            var validator = new GameValidator(game);
            Assert.AreEqual(false, validator.valid());
        }

        [TestMethod]
        public void GameValidator_Errors_WhenNumberLess()
        {
            var game = buildGame("01", "05", "09", "13", "17");
            var validator = new GameValidator(game);
            validator.valid();

            Assert.AreEqual(1, validator.errors.Count());
            CollectionAssert.Contains(validator.errors.ToList(), "Sua aposta deve conter pelo menos 10 números.");
        }

        [TestMethod]
        public void GameValidator_Valid_WhenNumberMax()
        {
            var game = buildGame("01", "05", "09", "13", "17", 
                                 "21", "25", "29", "33", "37", 
                                 "41", "45", "49", "53", "57", 
                                 "61", "65", "69", "73", "77",
                                 "81");
            var validator = new GameValidator(game);
            Assert.AreEqual(false, validator.valid());
        }

        [TestMethod]
        public void GameValidator_Errors_WhenNumberMax()
        {
            var game = buildGame("01", "05", "09", "13", "17",
                                 "21", "25", "29", "33", "37",
                                 "41", "45", "49", "53", "57",
                                 "61", "65", "69", "73", "77",
                                 "81");
            var validator = new GameValidator(game);
            validator.valid();

            Assert.AreEqual(1, validator.errors.Count());
            CollectionAssert.Contains(validator.errors.ToList(), "Sua aposta deve conter até 20 números.");
        }

        [TestMethod]
        public void GameValidator_Valid_WhenTeamLess()
        {
            var game = buildGame("01", "02", "03", "04",
                                 "05", "06", "07", "08",
                                 "09", "10");

            var validator = new GameValidator(game);
            Assert.AreEqual(false, validator.valid());
        }

        [TestMethod]
        public void GameValidator_Errors_WhenTeamLess()
        {
            var game = buildGame("01", "02", "03", "04",
                                 "05", "06", "07", "08",
                                 "09", "10");
            var validator = new GameValidator(game);
            validator.valid();

            Assert.AreEqual(1, validator.errors.Count());
            CollectionAssert.Contains(validator.errors.ToList(), "Você deve apostar em pelo menos 5 times.");
        }

        private Game buildGame(params string[] bets)
        {
            var repo = new BetNumberRepository();
            Game g = new Game();

            bets.ToList().ForEach(bet => g.AddBetNumber(repo.find(bet)));

            return g;
        }
    }
}
