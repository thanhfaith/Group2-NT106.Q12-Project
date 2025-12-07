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
            listBox1.Font = new Font("Arial", 14, FontStyle.Bold);

            foreach (Player p in danhSachKetQua)
            {
                string danhHieu = "";
                switch (p.ThuHang)
                {
                    case 1: danhHieu = "VÔ ĐỊCH 🏆"; break;
                    case 2: danhHieu = "Á QUÂN 🥈"; break;
                    case 3: danhHieu = "HẠNG BA 🥉"; break;
                    case 4: danhHieu = "HẠNG BÉT 🐢"; break;
                }

                string dongHienThi = $"{danhHieu.PadRight(12)} | {p.Ten} ({p.Doi}) | {p.SoQuanVeDich} quân";

                listBox1.Items.Add(dongHienThi);
            }
        }
    }
}
