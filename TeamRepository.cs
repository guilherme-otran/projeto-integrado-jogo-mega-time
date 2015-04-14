using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class TeamRepository
    {
        private string[] ABBRS = { "ATLETICO MG", "ATLETICO PR", "BAHIA", "BOTAFOGO", "CEARA", 
                                   "CORINTHIANS", "CORITIBA", "CRUZEIRO", "FLAMENGO", "FLUMINENSE",
                                   "FORTALEZA", "GOIAS", "GREMIO", "GUARANI", "INTERNACIONAL",
                                   "NAUTICO", "PALMEIRAS", "PARANA CLUBE", "PONTE PRETA", "SANTA CRUZ",
                                   "SANTOS", "SAO PAULO", "SPORT", "VASCO DA GAMA", "VITORIA" };

        public IEnumerable<Team> availableTeams()
        {
            return ABBRS.Select(abbr => new Team(abbr));
        }
    }
}
