using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Projeto_PI
{
    
    public class Team
    {
        public string abbr { get; private set; }

        public Team(string abbr)
        {
            this.abbr = abbr;
        }

    }
}
