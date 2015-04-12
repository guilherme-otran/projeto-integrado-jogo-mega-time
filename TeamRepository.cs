using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class TeamRepository
    {
        private string[] ABBRS = { "CAM", "CAP", "BAH", "BOT", "CEA", 
                                   "COR", "CFC", "CRU", "FLA", "FLU",
                                   "FOR", "GOI", "GRE", "GUA", "INT",
                                   "NAU", "PAL", "PRC", "APP", "SCR",
                                   "SAN", "SAO", "SPO", "VAS", "VIT" };

        public IEnumerable<Team> availableTeams()
        {
            return ABBRS.Select(abbr => new Team(abbr));
        }
    }
}
