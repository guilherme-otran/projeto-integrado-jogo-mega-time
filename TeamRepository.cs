using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class TeamRepository
    {
        private IEnumerable<Team> teams;
        private string[] ABBRS = { "ATLETICO MG", "ATLETICO PR", "BAHIA", "BOTAFOGO", "CEARA", 
                                   "CORINTHIANS", "CORITIBA", "CRUZEIRO", "FLAMENGO", "FLUMINENSE",
                                   "FORTALEZA", "GOIAS", "GREMIO", "GUARANI", "INTERNACIONAL",
                                   "NAUTICO", "PALMEIRAS", "PARANA CLUBE", "PONTE PRETA", "SANTA CRUZ",
                                   "SANTOS", "SAO PAULO", "SPORT", "VASCO DA GAMA", "VITORIA" };
        public TeamRepository()
        {
            teams = ABBRS.Select(abbr => new Team(abbr)); 
        }

        public IEnumerable<Team> availableTeams()
        {
            return teams;
        }

        public Team findTeamByName(string name)
        {
            return availableTeams().First(team => name.ToUpper().StartsWith(team.abbr));
        }
    }
}
