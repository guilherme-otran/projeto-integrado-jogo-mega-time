using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projeto_PI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Projeto_PI_tests
{
    [TestClass]
    public class TeamRepositoryTest
    {
        [TestMethod]
        public void TeamRepository_FindTeamByName()
        {
            var tr = new TeamRepository();
            var team = tr.findTeamByName("VASCO DA GAMA");
            var expectedTeam = tr.availableTeams().ElementAt(23);

            Assert.AreSame(expectedTeam, team);
        }
    }
}
