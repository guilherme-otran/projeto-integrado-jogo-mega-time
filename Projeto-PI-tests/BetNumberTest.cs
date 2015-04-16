using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projeto_PI;

namespace Projeto_PI_tests
{
    [TestClass]
    public class BetNumberTest
    {
        [TestMethod]
        public void BetNumber_Team_Property()
        {
            Team team = new Team("TES");
            BetNumber bet = new BetNumber(team, "00");
            Assert.AreEqual(team, bet.team);
        }

        [TestMethod]
        public void BetNumber_Number_Property()
        {
            Team team = new Team("Teste");
            string testNum = "10";

            BetNumber bet = new BetNumber(team, testNum);
            Assert.AreEqual(testNum, bet.number);
        }
    }
}
