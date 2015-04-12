using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_PI
{
    public partial class Form1 : Form
    {
        private GroupedBet bet;
        private GroupedBetRepository repo;
        private IEnumerable<Button> betButtons;

        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            IEnumerable<Button> buttons = this.Controls.OfType<Button>();

            betButtons = buttons.Where(btn => btn.Name.StartsWith("button"));
            bet = new GroupedBet(betButtons);
            repo = new GroupedBetRepository();
            resetForm();
        }

        private void btnMakeBet_Click(object sender, EventArgs e)
        {
            if (bet.validate())
            {
                Decimal value = bet.betPrice();

                string message = "O valor da sua aposta é " + value.ToString("C") + ".\nConfirmar?";
                bool accepted = MessageBox.Show(message, "Confirmar Aposta", MessageBoxButtons.YesNo) == DialogResult.Yes;

                if (accepted)
                {
                    long protocol = repo.storeGroupedBet(bet);
                    showReceipt(bet, protocol);
                    resetForm();
                }
            }
        }

        private void btnNewBet_Click(object sender, EventArgs e)
        {

            txtBetProt.Clear();
            txtBetProt.Enabled = false;

            bet.unblock();
            
            btnMakeBet.Enabled = true;
            btnCheckBet.Enabled = false;
            btnNewBet.Enabled = false;

        }

        private void btnCheckBet_Click(object sender, EventArgs e)
        {
            long protocol;
            bet.reset();

            if (long.TryParse(txtBetProt.Text, out protocol))
            {
                try
                {
                    repo.loadGroupedBet(bet, protocol);
                    showReceipt(bet, protocol);
                }
                catch (GroupedBetRepository.BetNotFound)
                {
                    MessageBox.Show("Protocolo Inexistente no Órgão Regulador", "Protocolo Inválido");
                }
            }
            else
            {
                MessageBox.Show("Número do protocolo mal formado.", "Protocolo Inválido");
            }

        }

        private void resetForm()
        {

            bet.reset();
            btnMakeBet.Enabled = false;
            btnCheckBet.Enabled = true;
            btnNewBet.Enabled = true;
            txtBetProt.Clear();
            txtBetProt.Enabled = true;
        }

        private void txtBetProt_TextChanged(object sender, EventArgs e)
        {
            bet.reset();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            resetForm();
            
        }

        private void showReceipt(GroupedBet bet, long protocol)
        {
            string receipt = "JOGO MEGA TIME\n";
            receipt += identate("Protocolo:") + protocol.ToString() + "\n\n";

            receipt += identate("Dezenas:");
            receipt += bet.betNumbers()
                           .Select(num => String.Format("{0:00}", num))
                           .Aggregate((src, acc) => src + ", " + acc);
            receipt += "\n\n";

            receipt += bet.teams.Where(team => team.isBeated())
                           .Select(team => identate(team.name() + ":") + team.countBets())
                           .Aggregate((src, acc) => src + "\n" + acc);

            receipt += "\n\n";
            receipt += identate("Valor:") + bet.betPrice().ToString("C");

            MessageBox.Show(receipt, "Recibo de Aposta");
            resetForm();
        }

        private string identate(string str)
        {
            return str.Length < 11 ? str + "\t\t" : str + "\t";
        }
    }
}
