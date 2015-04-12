using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_PI
{
    public class BetNumber
    {
        public Team team { get; private set; }
        public string number { get; private set; }

        public BetNumber(Team team, string number)
        {
            this.team = team;
            this.number = number;
        }
    }
}
