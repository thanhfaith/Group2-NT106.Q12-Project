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

            Font fontDam = new Font(richTextBox1.Font, FontStyle.Bold);
            Font fontThuong = new Font(richTextBox1.Font, FontStyle.Regular);

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("1. Số lượng người chơi\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "  • Người chơi: Từ 2-4 người (4 đội: Đỏ, Xanh Lá, Vàng, Xanh Dương).\n" +
                "  • Số quân: Mỗi đội có 4 quân cờ.\n" +
                "  • Khởi đầu: Tất cả quân cờ nằm trong \"Chuồng\".\n\n");

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("2. Quy tắc Tung Xúc Xắc\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "  • Đến lượt ai người đó tung.\n" +
                "  • Luật đặc biệt: Nếu bạn tung được 6 điểm, bạn sẽ được tung tiếp một lượt nữa (trừ khi không còn quân nào đi được).\n\n");

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("3. Luật Ra Quân (Xuất chuồng)\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "  • Điều kiện: Bạn bắt buộc phải tung được 6 mới được đưa 1 quân từ nhà ra ô khởi hành.\n" +
                "  • Cản trở:\n" +
                "    - Nếu ô khởi hành đang bị quân cùng màu chặn: Bạn không được ra quân.\n" +
                "    - Nếu ô khởi hành đang có quân địch: Bạn được ra quân và đá quân địch về chuồng.\n\n");

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("4. Luật Di Chuyển trên đường đua\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "Quân cờ đi theo chiều kim đồng hồ dựa trên số điểm xúc xắc.\n" +
                "  • Cản:\n" +
                "    - Không được đi xuyên qua hoặc nhảy qua đầu quân cờ khác (dù là quân mình hay quân địch) nếu điểm xúc xắc lớn hơn khoảng cách đến quân đó.\n" +
                "    - Không được dừng lại tại ô đang có quân cùng màu (Bị quân mình chặn).\n" +
                "  • Đá:\n" +
                "    - Nếu điểm đến trùng khít với vị trí một quân địch đang đứng, bạn sẽ đá quân địch về chuồng và chiếm vị trí đó.\n" +
                "  • Vào cửa chuồng:\n" +
                "    - Để vào được cửa chuồng, quân cờ phải đi vừa đủ số bước tới cửa.\n" +
                "    - Nếu đi lố qua cửa chuồng nước đi bị coi là không hợp lệ.\n\n");

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("5. Luật Leo Thang\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "Khi quân đã vào trong chuồng (các ô dọc để về đích):\n" +
                "  • Cơ chế: Bạn di chuyển đến ô số mấy thì cần tung xúc xắc đúng số đó (hoặc lớn hơn vị trí hiện tại).\n" +
                "    - Ví dụ: nếu bạn đang ở ô 2, bạn tung được 3, 4, 5, hoặc 6 thì quân sẽ nhảy thẳng lên ô tương ứng với số xúc xắc đó.\n" +
                "  • Điều kiện: Ô đích (theo số xúc xắc) phải đang trống.\n" +
                "  • Logic chặn: Nếu ô tương ứng với xúc xắc đã có quân của mình, bạn không thể lên nước đó.\n\n");

            richTextBox1.SelectionFont = fontDam;
            richTextBox1.AppendText("6. Điều kiện Chiến Thắng\n");

            richTextBox1.SelectionFont = fontThuong;
            richTextBox1.AppendText(
                "  • Người chiến thắng là người đầu tiên xếp đủ 4 quân của mình vào các vị trí 6, 5, 4 và 3 trong thang về đích.\n" +
                "  • Lưu ý: Ngay khi một đội thỏa mãn điều kiện này, game sẽ kết thúc và bảng xếp hạng sẽ hiện ra.\n");

            richTextBox1.ReadOnly = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            MenuForm f = new MenuForm();
            f.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
