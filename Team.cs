using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Projeto_PI
{
    
    class Team
    {
        private string[] NAMES = { "CAM", "CAP", "BAH", "BOT", "CEA", 
                                   "COR", "CFC", "CRU", "FLA", "FLU",
                                   "FOR", "GOI", "GRE", "GUA", "INT",
                                   "NAU", "PAL", "PRC", "APP", "SCR",
                                   "SAN", "SAO", "SPO", "VAS", "VIT" };

        private Color SELECTED_COLOR = Color.LightCoral;
        public Button[] buttons;
        public int id;

        public Team(int id)
        {
            this.id = id;
            buttons = new Button[4];
        }

        public void addButton(Button btn, int number) {
            buttons[number] = btn;
        }

        public int countBets()
        {
            return buttons.Count(btn => btn.BackColor == SELECTED_COLOR);
        }

        public bool isBeated()
        {
            return countBets() > 0;
        }

        public void toggleBet(int idx)
        {
            Button betButton = buttons[idx];
            if (betButton.BackColor == SELECTED_COLOR)
            {
                betButton.BackColor = default(Color);
            }
            else
            {
                betButton.BackColor = SELECTED_COLOR;
            }
            
        }

        public IEnumerable<int> betNumbers()
        {
            return buttons.Where(btn => btn.BackColor == SELECTED_COLOR).Select(btn => int.Parse(btn.Text));
        }

        public void reset()
        {
            foreach (Button btn in buttons)
            {
                btn.Enabled = false;
                btn.BackColor = default(Color);
            }

        }

        public void unblock()
        {
            foreach (Button btn in buttons) btn.Enabled = true;
        }

        public String name()
        {
            return NAMES[id];
        }
    }
}
