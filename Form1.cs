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
        private Color selectedColor = Color.BlueViolet;
        private GameControl gameController;
        private IEnumerable<Button> betButtons;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gameController = new GameControl();

            IEnumerable<Button> buttons = this.Controls.OfType<Button>();
            betButtons = buttons.Where(btn => btn.Name.StartsWith("button"));

            foreach (var btn in betButtons)
            {
                btn.Click += betBtn_Click;
            }

            initialState();
        }

        void betBtn_Click(object sender, EventArgs e)
        {
            Button betBtn = (Button)sender;
            string betStr = betBtn.Text;

            if (gameController.BetIsChoiced(betStr))
                gameController.RemoveBet(betStr);
            else
                gameController.AddBet(betStr);

            reloadColors();
        }

        private void resetGame()
        {
            gameController.Reset();
            reloadColors();
        }

        private void reloadColors()
        {
            foreach (var betBtn in betButtons)
            {
                if (gameController.BetIsChoiced(betBtn.Text))
                    betBtn.BackColor = selectedColor;
                else
                    betBtn.BackColor = default(Color);
            }

            labCost.Text = "Valor: " + gameController.gamePrice().ToString("C");
        }

        private void initialState()
        {
            btnNewBet.Enabled = true;
            btnMakeBet.Enabled = false;
            btnCheckBet.Enabled = true;
            labCost.Visible = false;
            txtBetProt.Enabled = true;
            txtBetProt.Clear();

            foreach (var betBtn in betButtons) { betBtn.Enabled = false; }
            resetGame();
        }

        private void btnNewBet_Click(object sender, EventArgs e)
        {
            // New Game State
            btnNewBet.Enabled = false;
            btnMakeBet.Enabled = true;
            btnCheckBet.Enabled = false;
            labCost.Visible = true;
            txtBetProt.Enabled = false;
            txtBetProt.Clear();

            foreach (var betBtn in betButtons) { betBtn.Enabled = true; }
            resetGame();
        }

        private void btnMakeBet_Click(object sender, EventArgs e)
        {
            if (gameController.storeGame())
            {
                showReceipt();
                initialState();
            }
            else
            {
                var errorsStr = gameController.gameErrors().Aggregate((src, acc) => src + "\n" + acc);
                MessageBox.Show(errorsStr, "Jogo não registrado");
            }
            
        }

        private void btnCheckBet_Click(object sender, EventArgs e)
        {
            try 
            {
                gameController.LoadGame(txtBetProt.Text);
                labCost.Visible = true;
                reloadColors();
                showReceipt();
            } 
            catch (GameControl.GameNotFound)
            {
                MessageBox.Show("Verifique o número do protocolo.", "Jogo não registrado.");
            }

            initialState();
        }


        private void showReceipt()
        {
            Game game = gameController.game;
            long protocol = game.protocol;

            string receipt = "JOGO MEGA TIME\n";
            receipt += identate("PROTOCOLO:") + protocol.ToString() + "\n\n";

            receipt += identate("DEZENAS:");
            receipt += aggregateBets(game.bets);
                           
            receipt += "\n\n";

            receipt += game.betedTeams
                            // Sort teams by name
                           .OrderBy(group => group.Key.abbr)
                           .Select(group => identate(group.Key.abbr + ":") + 
                                            "(" + group.Count().ToString() + ") " +
                                            aggregateBets(group.ToList()))
                           .Aggregate((src, acc) => src + "\n" + acc);

            receipt += "\n\n";
            receipt += identate("VALOR:") + game.Price().ToString("C");

            (new ReceiptForm(receipt)).ShowDialog();
        }

        private string aggregateBets(IEnumerable<BetNumber> bets)
        {
            // Force 00 as last
            return bets.OrderBy(bet => (bet.number.Equals("00")) ? "999" : bet.number)
                .Select(bet => bet.number)
                .Aggregate((src, acc) => src + ", " + acc);
        }

        private string identate(string str)
        {
            if (str.Length > 13)
                return str;

            return str.Length < 7 ? str + "\t\t" : str + "\t";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            initialState();
        }
    }
}
