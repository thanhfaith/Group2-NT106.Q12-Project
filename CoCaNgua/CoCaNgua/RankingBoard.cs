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
    public partial class RankingBoard : Form
    {
        public RankingBoard()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void HienThiKetQua(List<Player> danhSachKetQua)
        {
            listBox1.Items.Clear();
            listBox1.Font = new Font("Arial", 12, FontStyle.Bold);

            listBox1.Items.Add(string.Format("{0,-12} | {1,-20} | {2,-10} ", "HẠNG", "TÊN", "QUÂN"));
            listBox1.Items.Add("-------------------------------------------------------------------");

            foreach (Player p in danhSachKetQua)
            {
                string danhHieu = "";
                switch (p.ThuHang)
                {
                    case 1: danhHieu = "VÔ ĐỊCH 🏆"; break;
                    case 2: danhHieu = "Á QUÂN 🥈"; break;
                    case 3: danhHieu = "HẠNG 3 🥉"; break;
                    case 4: danhHieu = "HẠNG BÉT 🐢"; break;
                }
                string dongHienThi = string.Format("{0,-12} | {1,-20} | {2,-10}",
                    danhHieu,
                    $"{p.Ten} ({p.Doi})",
                    $"{p.SoQuanVeDich} về");

                listBox1.Items.Add(dongHienThi);
            }
        }

        private void llblQuit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MenuForm f = new MenuForm();
            f.Show();
            this.Close();
        }
    }
}
