using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Linq;

namespace CoCaNgua
{
    public partial class ChessBoard : Form
    {
        List<QuanCo> pieces = new List<QuanCo>();
        NetworkHelper network;

        TeamColor myTeam;
        TeamColor currentTurn;
        int currentDiceValue = 0;
        bool hasRolled = false;

        public ChessBoard(NetworkHelper existingNetwork)
        {
            InitializeComponent();
            this.network = existingNetwork;
            if (this.network != null)
            {
                this.network.OnMessageReceived += HandleNetworkMessage;
            }

            InitializeGame();
        }

        private void InitializeGame()
        {
            if (pieces == null) pieces = new List<QuanCo>();
            pieces.Clear();

            // Khởi tạo quân cờ
            pieces.Add(new QuanCo(0, TeamColor.Red, red1));
            pieces.Add(new QuanCo(1, TeamColor.Red, red2));
            pieces.Add(new QuanCo(2, TeamColor.Red, red3));
            pieces.Add(new QuanCo(3, TeamColor.Red, red4));

            pieces.Add(new QuanCo(4, TeamColor.Green, green1));
            pieces.Add(new QuanCo(5, TeamColor.Green, green2));
            pieces.Add(new QuanCo(6, TeamColor.Green, green3));
            pieces.Add(new QuanCo(7, TeamColor.Green, green4));

            pieces.Add(new QuanCo(8, TeamColor.Yellow, yellow1));
            pieces.Add(new QuanCo(9, TeamColor.Yellow, yellow2));
            pieces.Add(new QuanCo(10, TeamColor.Yellow, yellow3));
            pieces.Add(new QuanCo(11, TeamColor.Yellow, yellow4));

            pieces.Add(new QuanCo(12, TeamColor.Blue, blue1));
            pieces.Add(new QuanCo(13, TeamColor.Blue, blue2));
            pieces.Add(new QuanCo(14, TeamColor.Blue, blue3));
            pieces.Add(new QuanCo(15, TeamColor.Blue, blue4));

            foreach (var p in pieces)
            {
                var piece = p;
                p.UiControl.Click += (sender, e) => Piece_Click(piece);
            }

            ResetGameVisuals();
        }

        private void ResetGameVisuals()
        {
            foreach (var p in pieces)
            {
                p.UiControl.Parent = panel1;
                p.State = PieceState.InHome;
                p.CurrentStep = 0;
                p.CurrentPosition = -1;
                UpdatePieceUI(p);
            }
            hasRolled = false;
            currentDiceValue = 0;
            AddToChat("--- SẴN SÀNG VÁN MỚI ---");
        }

        private void HandleNetworkMessage(string msg)
        {
            if (this.IsDisposed) return; 

            this.Invoke((MethodInvoker)delegate
            {
                try
                {
                    string[] parts = msg.Split('|');
                    string command = parts[0];

                    switch (command)
                    {
                        case "ASSIGN":
                            myTeam = (TeamColor)Enum.Parse(typeof(TeamColor), parts[1]);
                            this.Text = $"Cờ Cá Ngựa - Bạn là đội: {myTeam}";
                            AddToChat($"[Hệ thống] Bạn tham gia đội {myTeam}");
                            break;

                        case "TURN":
                            currentTurn = (TeamColor)Enum.Parse(typeof(TeamColor), parts[1]);
                            hasRolled = false;

                            if (currentTurn == myTeam)
                            {
                                AddToChat("--- ĐẾN LƯỢT BẠN! Hãy tung xúc xắc. ---");
                                btnDice.Enabled = true;
                                this.BackColor = Color.LightYellow;
                            }
                            else
                            {
                                AddToChat($"--- Đến lượt của đội {currentTurn} ---");
                                btnDice.Enabled = false;
                                this.BackColor = Color.WhiteSmoke;
                            }
                            break;

                        case "DICE":
                            currentDiceValue = int.Parse(parts[1]);
                            ShowDiceImage(currentDiceValue);
                            AddToChat($"[XÚC XẮC] {currentTurn} tung được [{currentDiceValue}] điểm.");

                            if (currentTurn == myTeam)
                            {
                                hasRolled = true;
                                btnDice.Enabled = false;
                                bool anyMovable = false;

                                foreach (var pc in pieces)
                                {
                                    if (pc.Team != myTeam) continue;
                                    // Kiểm tra xem quân này có đi được không
                                    if (TryComputeMove(pc, currentDiceValue, out _, out _, out _, allowMessage: false))
                                    {
                                        anyMovable = true;
                                        break;
                                    }
                                }

                                if (!anyMovable)
                                {
                                    //  Xử lý logic tung được 6 nhưng không đi được ---
                                    if (currentDiceValue == 6)
                                    {
                                        AddToChat("⚠️ Không có quân đi được. Nhưng tung được 6 BẠN ĐƯỢC TUNG TIẾP!");
                                        hasRolled = false;
                                        btnDice.Enabled = true;
                                    }
                                    else
                                    {
                                        AddToChat("⚠️ Không có quân nào đi được! Bỏ lượt.");
                                        hasRolled = false;

                                        // Tự động chuyển lượt sau 1.5s
                                        Task.Delay(1500).ContinueWith(_ =>
                                        {
                                            if (this.IsHandleCreated && !this.IsDisposed)
                                                this.Invoke(new Action(() => network.Send("END_TURN")));
                                        });
                                    }
                                }
                            }
                            break;

                        case "MOVE":
                            int pId = int.Parse(parts[1]);
                            int newPos = int.Parse(parts[2]);
                            PieceState newState = (PieceState)Enum.Parse(typeof(PieceState), parts[3]);

                            var p = pieces.Find(x => x.Id == pId);

                            if (p != null)
                            {
                                if (newState == PieceState.InHome && p.State != PieceState.InHome)
                                    AddToChat($"Quân {p.Team} đã bị ĐÁ về chuồng!");

                                p.CurrentPosition = newPos;
                                p.State = newState;
                                UpdatePieceUI(p);
                            }
                            break;

                        case "RANK":
                            AddToChat($"KẾT QUẢ: Đội {parts[1]} về đích - Hạng {parts[2]}!");
                            break;

                        case "GAME_OVER":
                            string winnerTeam = "";
                            if (parts.Length > 1) winnerTeam = parts[1];

                            if (myTeam.ToString() == winnerTeam)
                            {
                                AddToChat("🎉 CHÚC MỪNG! BẠN ĐÃ CHIẾN THẮNG!");
                                MessageBox.Show("Chúc mừng! Bạn đã chiến thắng!", "Kết quả");
                            }
                            else
                            {
                                AddToChat($"🏁 Đội {winnerTeam} đã giành chiến thắng!");
                                MessageBox.Show($"Đội {winnerTeam} đã thắng ván này!", "Kết quả");
                            }
                            XuLyKetThucGame();
                            break;

                        case "CHAT":
                            if (parts.Length >= 3)
                            {
                                string senderName = parts[1];
                                string content = string.Join("|", parts.Skip(2));
                                AddToChat($"[CHAT] {senderName}: {content}");
                            }
                            break;

                        case "ERROR":
                            AddToChat($"Lỗi: {parts[1]}");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    AddToChat($"[Client Error]: {ex.Message}");
                }
            });
        }

        private async void Piece_Click(QuanCo piece)
        {
            if (currentTurn != myTeam) return;
            if (piece.Team != myTeam) return;
            if (!hasRolled) { AddToChat("Bạn phải tung xúc xắc trước!"); return; }

            int nextPos;
            PieceState nextState;
            QuanCo enemyPiece;

            // Kiểm tra nước đi
            if (TryComputeMove(piece, currentDiceValue, out nextPos, out nextState, out enemyPiece, allowMessage: true))
            {
                btnDice.Enabled = false;

                // Xử lý đá quân 
                if (enemyPiece != null)
                {
                    network.Send($"MOVE|{enemyPiece.Id}|-1|InHome");
                    AddToChat($"⚔️ Bạn đã đá quân đội {enemyPiece.Team}!");

                    // Cập nhật giao diện quân bị đá ngay lập tức
                    enemyPiece.CurrentPosition = -1;
                    enemyPiece.State = PieceState.InHome;
                    UpdatePieceUI(enemyPiece);

                    await Task.Delay(100);
                }

                // Gửi lệnh di chuyển quân mình
                network.Send($"MOVE|{piece.Id}|{nextPos}|{nextState}");

                // Cập nhật giao diện quân mình
                piece.CurrentPosition = nextPos;
                piece.State = nextState;
                UpdatePieceUI(piece);

                // --- Kiểm tra điều kiện thắng ---
                if (CheckWin(myTeam))
                {
                    network.Send($"GAME_OVER|{myTeam}");
                    hasRolled = false;
                    return;
                }

                if (currentDiceValue == 6)
                {
                    AddToChat("Bạn được đi tiếp do tung được 6!");
                    hasRolled = false;
                    btnDice.Enabled = true;
                }
                else
                {
                    await Task.Delay(200);
                    network.Send("END_TURN");
                }
            }
        }

        // --- Logic kiểm tra thắng chính xác hơn ---
        private bool CheckWin(TeamColor team)
        {
            // Lấy tất cả quân đang ở trong thang về đích
            var myPieces = pieces.Where(p => p.Team == team && p.State == PieceState.InFinish).ToList();

            // Kiểm tra xem đã lấp đầy các ô cao nhất chưa
            bool has6 = myPieces.Any(p => p.CurrentPosition == 6);
            bool has5 = myPieces.Any(p => p.CurrentPosition == 5);
            bool has4 = myPieces.Any(p => p.CurrentPosition == 4);
            bool has3 = myPieces.Any(p => p.CurrentPosition == 3);

            return has6 && has5 && has4 && has3;
        }

        private bool TryComputeMove(QuanCo piece, int diceValue, out int nextPos, out PieceState nextState, out QuanCo enemyPiece, bool allowMessage = true)
        {
            nextPos = -1;
            nextState = piece.State;
            enemyPiece = null;

            // 1. RA QUÂN
            if (piece.State == PieceState.InHome)
            {
                if (diceValue == 6)
                {
                    int startPos = GetStartPosition(piece.Team);
                    var blocker = GetPieceAtPosition(startPos, PieceState.OnTrack);

                    if (blocker != null && blocker.Team == piece.Team)
                    {
                        if (allowMessage) AddToChat("Cửa ra quân đang bị quân mình chặn.");
                        return false;
                    }
                    if (blocker != null && blocker.Team != piece.Team) enemyPiece = blocker;

                    nextPos = startPos;
                    nextState = PieceState.OnTrack;
                    return true;
                }
                if (allowMessage) AddToChat("Cần tung được 6 để ra quân.");
                return false;
            }

            // 2. DI CHUYỂN TRÊN ĐƯỜNG ĐUA
            else if (piece.State == PieceState.OnTrack)
            {
                int entryPos = GetFinishEntryPosition(piece.Team);
                int currentPos = piece.CurrentPosition;

                // Nếu ĐANG ĐỨNG TẠI CỬA CHUỒNG -> Vào chuồng
                if (currentPos == entryPos)
                {
                    int targetStep = diceValue; // Tung 2 vào ô 2
                    // Kiểm tra xem ô đích đó có quân chưa
                    var blockerInFinish = GetPieceAtPosition(targetStep, PieceState.InFinish);

                    if (blockerInFinish != null)
                    {
                        if (allowMessage) AddToChat($"Ô số {targetStep} trong chuồng đã có quân.");
                        return false;
                    }
                    nextPos = targetStep;
                    nextState = PieceState.InFinish;
                    return true;
                }

                // Tính khoảng cách tới cửa chuồng
                int distanceToEntry = (entryPos - currentPos + 52) % 52;

                // Kiểm tra vật cản trên đường đi
                for (int i = 1; i <= diceValue; i++)
                {
                    int checkPos = (currentPos + i) % 52;
                    var obstacle = GetPieceAtPosition(checkPos, PieceState.OnTrack);

                    if (obstacle != null)
                    {
                        // Nếu là quân mình -> Chặn
                        if (obstacle.Team == piece.Team)
                        {
                            if (allowMessage) AddToChat("Bị quân mình chặn.");
                            return false;
                        }
                        // Nếu là quân địch
                        else
                        {
                            // Nếu địch nằm ở đúng điểm đến -> Đá được
                            if (i == diceValue)
                            {
                                enemyPiece = obstacle;
                            }
                            // Nếu địch nằm giữa đường -> Không nhảy qua được
                            else
                            {
                                if (allowMessage) AddToChat("Không thể nhảy qua đầu quân địch.");
                                return false;
                            }
                        }
                    }
                }

                // Kiểm tra có đi lố cửa chuồng không
                if (diceValue == distanceToEntry)
                {
                    nextPos = entryPos;
                    nextState = PieceState.OnTrack;
                    return true;
                }
                else if (diceValue < distanceToEntry)
                {
                    nextPos = (currentPos + diceValue) % 52;
                    nextState = PieceState.OnTrack;
                    return true;
                }
                else
                {
                    if (allowMessage) AddToChat("Dư bước! Cần tung đúng số để đến cửa chuồng.");
                    return false;
                }
            }

            // 3. LEO THANG TRONG CHUỒNG
            else if (piece.State == PieceState.InFinish)
            {
                // Nếu tung được số lớn hơn vị trí đang đứng -> Tính toán di chuyển
                if (diceValue > piece.CurrentPosition)
                {
                    int targetStep = diceValue;

                    // Kiểm tra ô đích có quân chưa
                    var blocker = GetPieceAtPosition(targetStep, PieceState.InFinish);
                    if (blocker != null)
                    {
                        // Nếu ô đích bị chặn, ta kiểm tra xem có phải do quân mình đã lấp đầy hết phía trên không
                        if (allowMessage) AddToChat($"Ô số {targetStep} đã có quân.");
                        return false;
                    }

                    nextPos = targetStep;
                    nextState = PieceState.InFinish;
                    return true;
                }
                else
                {
                    if (allowMessage)
                    {
                        bool isFullAbove = true;
                        // Kiểm tra tất cả các ô phía trên vị trí hiện tại cho đến ô 6
                        for (int i = piece.CurrentPosition + 1; i <= 6; i++)
                        {
                            // Nếu tìm thấy một ô trống phía trên -> Nghĩa là chưa về đích xong, vẫn còn cơ hội leo lên
                            if (GetPieceAtPosition(i, PieceState.InFinish) == null)
                            {
                                isFullAbove = false;
                                break;
                            }
                        }

                        if (isFullAbove)
                        {
                            // Nếu phía trên đã kín hết quân -> Coi như đã xong
                            AddToChat("Quân này đã về đích!");
                        }
                        else
                        {
                            // Nếu phía trên còn trống -> Báo cần tung cao hơn
                            AddToChat($"Cần tung lớn hơn {piece.CurrentPosition} để lên bước tiếp theo!");
                        }
                    }
                    return false;
                }
            }

            return false;
        }

        private void UpdatePieceUI(QuanCo piece)
        {
            if (piece.UiControl.IsDisposed) return;

            Point centerPoint = GetPixelCoordinates(piece.Id, piece.CurrentPosition, piece.Team, piece.State);
            int newX = centerPoint.X - (piece.UiControl.Width / 2);
            int newY = centerPoint.Y - (piece.UiControl.Height / 2);
            Point newLoc = new Point(newX, newY);

            if (piece.UiControl.InvokeRequired)
            {
                piece.UiControl.Invoke(new Action(() =>
                {
                    piece.UiControl.Location = newLoc;
                    if (piece.UiControl.Parent != panel1)
                    {
                        piece.UiControl.Parent = panel1;
                        piece.UiControl.BackColor = Color.Transparent;
                    }
                    piece.UiControl.BringToFront();
                }));
            }
            else
            {
                piece.UiControl.Location = newLoc;
                if (piece.UiControl.Parent != panel1)
                {
                    piece.UiControl.Parent = panel1;
                    piece.UiControl.BackColor = Color.Transparent;
                }
                piece.UiControl.BringToFront();
            }
        }

        private void AddToChat(string content)
        {
            if (rtbChatLog.IsDisposed) return;

            if (rtbChatLog.InvokeRequired)
            {
                rtbChatLog.Invoke(new Action(() => AddToChat(content)));
                return;
            }

            string time = DateTime.Now.ToString("HH:mm:ss");
            rtbChatLog.AppendText($"[{time}] {content}{Environment.NewLine}");
            rtbChatLog.ScrollToCaret();
        }

        private int GetStartPosition(TeamColor team)
        {
            switch (team)
            {
                case TeamColor.Red: return 0;
                case TeamColor.Green: return 13;
                case TeamColor.Yellow: return 26;
                case TeamColor.Blue: return 39;
                default: return 0;
            }
        }

        private int GetFinishEntryPosition(TeamColor team)
        {
            int start = GetStartPosition(team);
            return (start - 2 + 52) % 52;
        }

        private QuanCo GetPieceAtPosition(int pos, PieceState state)
        {
            return pieces.Find(p =>
                p.CurrentPosition == pos &&
                p.State == state &&
                (state == PieceState.InFinish ? p.Team == myTeam : true)
            );
        }

        // --- TỌA ĐỘ BÀN CỜ  ---
        private readonly Point[] trackPoints = new Point[52]
        {
            // Red: Index 0-12
            new Point(71, 283), new Point(117, 282), new Point(158, 284), new Point(211, 280),
            new Point(249, 280), new Point(298, 238), new Point(291, 197), new Point(294, 154),
            new Point(291, 110), new Point(294, 68), new Point(293, 25), new Point(335, 22),
            new Point(380, 22),

            // Green: Index 13-25
            new Point(382, 71), new Point(378, 114), new Point(379, 155), new Point(378, 197),
            new Point(380, 235), new Point(432, 280), new Point(467, 280), new Point(514, 279),
            new Point(566, 280), new Point(594, 276), new Point(647, 275), new Point(642, 321),
            new Point(651, 359),

            // Yellow: Index 26-38
            new Point(599, 365), new Point(559, 366), new Point(514, 362), new Point(463, 363),
            new Point(422, 365), new Point(376, 407), new Point(378, 445), new Point(378, 489),
            new Point(381, 530), new Point(377, 577), new Point(379, 613), new Point(328, 617),
            new Point(292, 619),

            // Blue: Index 39-51
            new Point(285, 576), new Point(290, 532), new Point(290, 498), new Point(281, 445),
            new Point(290, 410), new Point(240, 361), new Point(202, 357), new Point(154, 363),
            new Point(110, 363), new Point(58, 367),  new Point(16, 365),  new Point(16, 322),
            new Point(21, 285)
        };

        private readonly Point[] redHomePoints = new Point[] {
            new Point(87, 85), new Point(187, 89), new Point(91, 171), new Point(182, 176)
        };
        private readonly Point[] greenHomePoints = new Point[] {
            new Point(487, 87), new Point(581, 85), new Point(491, 174), new Point(585, 173)
        };
        private readonly Point[] yellowHomePoints = new Point[] {
            new Point(491, 469), new Point(582, 471), new Point(494, 555), new Point(583, 559)
        };
        private readonly Point[] blueHomePoints = new Point[] {
            new Point(92, 466), new Point(181, 473), new Point(89, 556), new Point(177, 553)
        };

        private readonly Point[] redFinishPoints = new Point[] {
            new Point(65, 321), new Point(110, 321), new Point(152, 318),
            new Point(199, 317), new Point(242, 317), new Point(292, 317)
        };
        private readonly Point[] greenFinishPoints = new Point[] {
            new Point(337, 63), new Point(334, 108), new Point(337, 152),
            new Point(334, 189), new Point(335, 233), new Point(337, 273)
        };
        private readonly Point[] yellowFinishPoints = new Point[] {
            new Point(604, 317), new Point(556, 317), new Point(513, 319),
            new Point(465, 318), new Point(424, 319), new Point(378, 319)
        };
        private readonly Point[] blueFinishPoints = new Point[] {
            new Point(334, 573), new Point(335, 533), new Point(341, 485),
            new Point(338, 447), new Point(334, 407), new Point(336, 365)
        };

        private Point GetPixelCoordinates(int pieceId, int logicPosition, TeamColor team, PieceState state)
        {
            if (state == PieceState.InHome)
            {
                int homeIndex = pieceId % 4;
                switch (team)
                {
                    case TeamColor.Red: return redHomePoints[homeIndex];
                    case TeamColor.Green: return greenHomePoints[homeIndex];
                    case TeamColor.Yellow: return yellowHomePoints[homeIndex];
                    case TeamColor.Blue: return blueHomePoints[homeIndex];
                }
            }
            if (state == PieceState.OnTrack)
            {
                if (logicPosition >= 0 && logicPosition < trackPoints.Length)
                {
                    return trackPoints[logicPosition];
                }
            }
            if (state == PieceState.InFinish || state == PieceState.Finished)
            {
                int index = logicPosition - 1;
                if (index < 0) index = 0;
                if (index > 5) index = 5;

                switch (team)
                {
                    case TeamColor.Red: return redFinishPoints[index];
                    case TeamColor.Green: return greenFinishPoints[index];
                    case TeamColor.Yellow: return yellowFinishPoints[index];
                    case TeamColor.Blue: return blueFinishPoints[index];
                }
            }
            return new Point(0, 0);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                string cleanMsg = txtMessage.Text;
                network.Send($"CHAT|Team {myTeam}|{cleanMsg}");
                txtMessage.Clear();
            }
        }

        private void ShowDiceImage(int value)
        {
            Image img = null;
            switch (value)
            {
                case 1: img = Properties.Resources.dice1; break;
                case 2: img = Properties.Resources.dice2; break;
                case 3: img = Properties.Resources.dice3; break;
                case 4: img = Properties.Resources.dice4; break;
                case 5: img = Properties.Resources.dice5; break;
                case 6: img = Properties.Resources.dice6; break;
            }

            if (pictureBox1.InvokeRequired)
            {
                pictureBox1.Invoke(new Action(() =>
                {
                    pictureBox1.Image = img;
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                }));
            }
            else
            {
                pictureBox1.Image = img;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void XuLyKetThucGame()
        {
            List<Player> ketQua = new List<Player>();

            foreach (TeamColor mauDoi in Enum.GetValues(typeof(TeamColor)))
            {
                var quanTrongChuong = pieces.Where(p => p.Team == mauDoi && p.State == PieceState.InFinish).ToList();
                int soQuan = quanTrongChuong.Count;
                int tongDiemViTri = quanTrongChuong.Sum(p => p.CurrentPosition);

                string tenHienThi = (mauDoi == myTeam) ? "Bạn" : "Team " + mauDoi.ToString();

                var p = new Player(tenHienThi, mauDoi, soQuan);
                ketQua.Add(p);
            }

            ketQua = ketQua.OrderByDescending(x => x.SoQuanVeDich) 
                           .ToList();

            for (int i = 0; i < ketQua.Count; i++)
            {
                ketQua[i].ThuHang = i + 1;
            }

            this.Invoke((MethodInvoker)delegate
            {
                RankingBoard frm = new RankingBoard();
                frm.HienThiKetQua(ketQua);
                frm.ShowDialog();
                this.Close();
            });
        }

        private void btnDice_Click(object sender, EventArgs e)
        {
            if (network != null)
            {
                network.Send("ROLL");
                btnDice.Enabled = false;
            }
        }
    }
}