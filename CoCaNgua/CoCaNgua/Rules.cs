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
    public partial class Rules : Form
    {
        public Rules()
        {
            InitializeComponent();
            HienThiLuatChoi();
        }
        private void HienThiLuatChoi()
        {
            richTextBox1.Clear();

            richTextBox1.Text = @"1. Khởi tạo, lượt chơi:
    Người đi trước: Chọn ngẫu nhiên.
    Vòng lặp lượt: Cố định theo chiều: Đỏ, xanh lá, vàng, xanh dương.

2. Điều kiện Ra quân :
    Chỉ khi tung xúc xắc được 6 mới được đưa ngựa ra bàn cờ.

3. Cơ chế Di chuyển:
    Bước đi: Di chuyển đúng số bước bằng kết quả xúc xắc.
    Va chạm (Đá quân): Nếu đi đến ô đang có quân khác màu, quân đến sau sẽ đá quân đang đứng (quân bị đá về lại trạng thái chờ ra quân).

4. Về đích, chiến thắng:
    Vào chuồng: Đi hết 1 vòng bàn cờ sẽ về cửa chuồng, tiến vào chuồng.
    Trạng thái an toàn: Khi đã nằm trong chuồng, ngựa an toàn tuyệt đối (không bị đá).
    Điều kiện thắng: Người đầu tiên đưa đủ 4 quân vào chuồng là nhất.";

            richTextBox1.ReadOnly = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MenuForm f = new MenuForm();
            f.Show();
        }
    }
}
