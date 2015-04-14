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
    public class ReceiptForm : Form
    {
        public ReceiptForm(string message)
        {
            InitializeComponent();
            rtxReceipt.Text = message;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
