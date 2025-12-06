using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void btnRule_Click(object sender, EventArgs e)
        {
            this.Hide();
            Rules f = new Rules();
            f.Show();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            StartGame start = new StartGame();
            start.Show();
            this.Hide();
        }
    }
}
