using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CoCaNgua
{
    public partial class ChessBoard : Form
    {
        // Danh sách quản lý các quân cờ
        List<QuanCo> pieces = new List<QuanCo>();

        NetworkHelper network;

        TeamColor myTeam;
        TeamColor currentTurn;
        int currentDiceValue = 0;
        bool hasRolled = false;

        public ChessBoard(NetworkHelper existingNetwork)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

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

            // --- KHỞI TẠO QUÂN CỜ VÀ GÁN PICTUREBOX TƯƠNG ỨNG ---

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

            // ✅ GÁN SỰ KIỆN CLICK CHO TỪNG QUÂN CỜ (CHỈ 1 LẦN)
            foreach (var p in pieces)
            {
                var piece = p; // Capture variable trong lambda
                p.UiControl.Click += (sender, e) => Piece_Click(piece);
            }

            // ✅ GÁN SỰ KIỆN CHO NÚT DICE
            btnDice.Click += btnDice_Click_1;

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

        // --- XỬ LÝ MẠNG (NHẬN TIN TỪ SERVER) ---

        private void HandleNetworkMessage(string msg)
        {
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
                            AddToChat($"=== Hệ thống: Bạn tham gia đội {myTeam} ===");
                            break;

                        case "TURN":
                            currentTurn = (TeamColor)Enum.Parse(typeof(TeamColor), parts[1]);
                            hasRolled = false;

                            if (currentTurn == myTeam)
                            {
                                AddToChat(">>> ĐẾN LƯỢT BẠN! Hãy tung xúc xắc. <<<");
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

                        // ✅ THÊM VÀO TRONG case "DICE" của HandleNetworkMessage

                        case "DICE":
                            currentDiceValue = int.Parse(parts[1]);

                            // ✅ HIỂN THỊ HÌNH XÚC XẮC
                            ShowDiceImage(currentDiceValue);

                            AddToChat($"Xúc xắc: {currentTurn} tung được [{currentDiceValue}] điểm.");

                            if (currentTurn == myTeam)
                            {
                                hasRolled = true;
                                btnDice.Enabled = false;
                                bool anyMovable = false;

                                foreach (var pc in pieces)
                                {
                                    if (pc.Team != myTeam) continue;

                                    int np;
                                    PieceState ns;
                                    QuanCo enemy;

                                    if (TryComputeMove(pc, currentDiceValue,
                                                       out np, out ns, out enemy,
                                                       allowMessage: false))
                                    {
                                        anyMovable = true;
                                        break;
                                    }
                                }

                                if (!anyMovable)
                                {
                                    AddToChat("⚠️ Không có quân nào đi được! Bỏ lượt.");
                                    hasRolled = false;

                                    // ✅ ĐỢI 1 CHÚT RỒI TỰ ĐỘNG CHUYỂN LƯỢT
                                    System.Threading.Tasks.Task.Delay(1500).ContinueWith(_ =>
                                    {
                                        if (this.InvokeRequired)
                                            this.Invoke(new Action(() => network.Send("END_TURN")));
                                        else
                                            network.Send("END_TURN");
                                    });
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
                                else if (newState == PieceState.Finished && p.State != PieceState.Finished)
                                    AddToChat($"Quân {p.Team} đã VỀ ĐÍCH thành công!");

                                p.CurrentPosition = newPos;
                                p.State = newState;
                                UpdatePieceUI(p);

                                int finishedCount = pieces.FindAll(x => x.Team == p.Team && x.State == PieceState.Finished).Count;
                                if (finishedCount == 4 && p.Team == myTeam)
                                {
                                    AddToChat($"🎉 BẠN ĐÃ CHIẾN THẮNG! Đang xác nhận...");
                                    network.Send("DONE");
                                    btnDice.Enabled = false;
                                }
                            }
                            break;

                        case "RANK":
                            AddToChat($"KẾT QUẢ: Đội {parts[1]} về đích - Hạng {parts[2]}!");
                            break;

                        case "GAME_OVER":
                            MessageBox.Show("Ván chơi đã kết thúc!", "Thông báo");
                            ResetGameVisuals(); // Reset để chơi ván mới
                            break;

                        case "CHAT":
                            if (parts.Length >= 3)
                                AddToChat($"{parts[1]}: {parts[2]}");
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
            // 1. Kiểm tra điều kiện
            if (currentTurn != myTeam) return;
            if (piece.Team != myTeam) return;
            if (!hasRolled)
            {
                AddToChat("Bạn phải tung xúc xắc trước!");
                return;
            }

            int nextPos = -1;
            PieceState nextState = piece.State;
            bool canMove = false;
            QuanCo enemyPiece = null;

            // --- TRONG CHUỒNG ---
            if (piece.State == PieceState.InHome)
            {
                if (currentDiceValue == 6)
                {
                    int startPos = GetStartPosition(myTeam);
                    var blocker = GetPieceAtPosition(startPos, PieceState.OnTrack);

                    if (blocker != null)
                    {
                        if (blocker.Team == myTeam)
                        {
                            AddToChat("Cửa chuồng đang bị quân mình chặn.");
                            return;
                        }
                        enemyPiece = blocker; // Xác định đá
                    }

                    nextPos = startPos;
                    nextState = PieceState.OnTrack;
                    canMove = true;
                }
                else
                {
                    AddToChat("Cần 6 điểm để ra quân.");
                    return;
                }
            }
            // --- TRÊN ĐƯỜNG ĐUA (Logic 52 ô) ---
            else if (piece.State == PieceState.OnTrack)
            {
                int entryPos = GetFinishEntryPosition(myTeam);
                int currentPos = piece.CurrentPosition;

                // Tính khoảng cách đến cửa chuồng
                int distanceToEntry = (entryPos - currentPos + 52) % 52;

                // Vào Chuồng Đích
                if (currentDiceValue > distanceToEntry && distanceToEntry < 12)
                {
                    int stepsInFinish = currentDiceValue - distanceToEntry;
                    if (stepsInFinish >= 1 && stepsInFinish <= 6)
                    {
                        var blocker = GetPieceAtPosition(stepsInFinish, PieceState.InFinish);
                        if (blocker != null)
                        {
                            AddToChat("Ô đích đã có quân.");
                            return;
                        }
                        nextPos = stepsInFinish;
                        nextState = PieceState.InFinish;
                        canMove = true;
                    }
                    else
                    {
                        AddToChat("Dư bước, không thể vào đích.");
                        return;
                    }
                }
                else
                {
                    // Công thức đi vòng quanh bàn cờ 52 ô
                    nextPos = (currentPos + currentDiceValue) % 52;

                    var blocker = GetPieceAtPosition(nextPos, PieceState.OnTrack);
                    if (blocker != null)
                    {
                        if (blocker.Team == myTeam)
                        {
                            AddToChat("Bị quân mình chặn đường.");
                            return;
                        }
                        enemyPiece = blocker; // Đá quân
                    }

                    nextState = PieceState.OnTrack;
                    canMove = true;
                }
            }
            // --- TRONG ĐÍCH ---
            else if (piece.State == PieceState.InFinish)
            {
                int potentialNext = piece.CurrentPosition + currentDiceValue;
                if (potentialNext <= 6)
                {
                    var blocker = GetPieceAtPosition(potentialNext, PieceState.InFinish);
                    if (blocker != null)
                    {
                        AddToChat("Ô đích bị chặn.");
                        return;
                    }

                    nextPos = potentialNext;
                    nextState = (nextPos == 6) ? PieceState.Finished : PieceState.InFinish;
                    canMove = true;
                }
                else
                {
                    AddToChat("Không thể đi quá ô số 6.");
                    return;
                }
            }

            // --- GỬI LỆNH ĐI ---
            if (canMove)
            {
                btnDice.Enabled = false; // Khóa nút

                if (enemyPiece != null)
                {
                    network.Send($"MOVE|{enemyPiece.Id}|-1|InHome");
                    AddToChat($"⚔️ Bạn đã đá quân đội {enemyPiece.Team}!");
                }

                network.Send($"MOVE|{piece.Id}|{nextPos}|{nextState}");

                if (nextState == PieceState.Finished)
                {
                    int currentFinishedCount = pieces.FindAll(p => p.Team == myTeam && p.State == PieceState.Finished).Count;
                    if (currentFinishedCount + 1 == 4)
                    {
                        network.Send("DONE");
                        hasRolled = false;
                        XuLyKetThucGame();
                        return;
                    }
                }

                if (currentDiceValue == 6)
                {
                    AddToChat("Bạn được đi tiếp do tung được 6!");
                    hasRolled = false;
                    btnDice.Enabled = true;
                }
                else
                {
                    // Delay 200ms để không treo máy
                    await Task.Delay(200);
                    network.Send("END_TURN");
                }
            }
        }

        // --- CÁC HÀM HỖ TRỢ HIỂN THỊ ---

        private void UpdatePieceUI(QuanCo piece)
        {
            Point centerPoint = GetPixelCoordinates(piece.Id, piece.CurrentPosition, piece.Team, piece.State);

            // ✅ THÊM LOG ĐỂ DEBUG
            System.Diagnostics.Debug.WriteLine($"UpdatePieceUI: Piece {piece.Id} ({piece.Team}) State={piece.State} Pos={piece.CurrentPosition} -> Pixel({centerPoint.X},{centerPoint.Y})");

            int newX = centerPoint.X - (piece.UiControl.Width / 2);
            int newY = centerPoint.Y - (piece.UiControl.Height / 2);

            if (piece.UiControl.InvokeRequired)
                piece.UiControl.Invoke(new Action(() => piece.UiControl.Location = new Point(newX, newY)));
            else
                piece.UiControl.Location = new Point(newX, newY);

            if (piece.UiControl.Parent != panel1)
            {
                piece.UiControl.Parent = panel1;
                piece.UiControl.BackColor = Color.Transparent;
            }
            piece.UiControl.BringToFront();
        }

        private void AddToChat(string content)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            rtbChatLog.AppendText($"[{time}] {content}{Environment.NewLine}");
            rtbChatLog.ScrollToCaret();
        }

        private void btnDice_Click(object sender, EventArgs e)
        {
            network.Send("ROLL");
        }

        // --- LOGIC TÍNH TOÁN VỊ TRÍ  ---

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
            return (start - 1 + 52) % 52;
        }

        private QuanCo GetPieceAtPosition(int pos, PieceState state)
        {
            return pieces.Find(p =>
                p.CurrentPosition == pos &&
                p.State == state &&
                (state != PieceState.InFinish || p.Team == myTeam)
            );
        }
        /// Thử tính nước đi cho một quân với giá trị xúc xắc cho trước.
        /// Trả về true nếu đi được, kèm nextPos, nextState, enemyPiece.
        /// allowMessage = false thì không AddToChat (dùng để kiểm tra bỏ lượt).
        private bool TryComputeMove(
            QuanCo piece,
            int diceValue,
            out int nextPos,
            out PieceState nextState,
            out QuanCo enemyPiece,
            bool allowMessage = true)
        {
            nextPos = -1;
            nextState = piece.State;
            enemyPiece = null;

            // --- TRONG CHUỒNG ---
            if (piece.State == PieceState.InHome)
            {
                if (diceValue == 6)
                {
                    int startPos = GetStartPosition(piece.Team);
                    var blocker = GetPieceAtPosition(startPos, PieceState.OnTrack);

                    if (blocker != null && blocker.Team == piece.Team)
                    {
                        if (allowMessage)
                            AddToChat("Cửa chuồng đang bị quân mình chặn.");
                        return false;
                    }

                    if (blocker != null && blocker.Team != piece.Team)
                        enemyPiece = blocker; // chuẩn bị đá

                    nextPos = startPos;
                    nextState = PieceState.OnTrack;
                    return true;
                }
                else
                {
                    if (allowMessage)
                        AddToChat("Cần 6 điểm để ra quân.");
                    return false;
                }
            }
            // --- TRÊN ĐƯỜNG ĐUA ---
            else if (piece.State == PieceState.OnTrack)
            {
                int entryPos = GetFinishEntryPosition(piece.Team);
                int currentPos = piece.CurrentPosition;

                int distanceToEntry = (entryPos - currentPos + 52) % 52;

                // Vào đoạn đích
                if (diceValue > distanceToEntry && distanceToEntry < 12)
                {
                    int stepsInFinish = diceValue - distanceToEntry;
                    if (stepsInFinish >= 1 && stepsInFinish <= 6)
                    {
                        var blocker = GetPieceAtPosition(stepsInFinish, PieceState.InFinish);
                        if (blocker != null)
                        {
                            if (allowMessage)
                                AddToChat("Ô đích đã có quân.");
                            return false;
                        }

                        nextPos = stepsInFinish;
                        nextState = PieceState.InFinish;
                        return true;
                    }
                    else
                    {
                        if (allowMessage)
                            AddToChat("Dư bước, không thể vào đích.");
                        return false;
                    }
                }
                else
                {
                    int tmpPos = (currentPos + diceValue) % 52;

                    var blocker = GetPieceAtPosition(tmpPos, PieceState.OnTrack);
                    if (blocker != null && blocker.Team == piece.Team)
                    {
                        if (allowMessage)
                            AddToChat("Bị quân mình chặn đường.");
                        return false;
                    }

                    if (blocker != null && blocker.Team != piece.Team)
                        enemyPiece = blocker; // sẽ bị đá

                    nextPos = tmpPos;
                    nextState = PieceState.OnTrack;
                    return true;
                }
            }
            // --- TRONG ĐOẠN ĐÍCH ---
            else if (piece.State == PieceState.InFinish)
            {
                int potentialNext = piece.CurrentPosition + diceValue;
                if (potentialNext <= 6)
                {
                    var blocker = GetPieceAtPosition(potentialNext, PieceState.InFinish);
                    if (blocker != null)
                    {
                        if (allowMessage)
                            AddToChat("Ô đích bị chặn.");
                        return false;
                    }

                    nextPos = potentialNext;
                    nextState = (nextPos == 6) ? PieceState.Finished : PieceState.InFinish;
                    return true;
                }
                else
                {
                    if (allowMessage)
                        AddToChat("Không thể đi quá ô số 6.");
                    return false;
                }
            }

            return false;
        }


        // --- MẢNG TỌA ĐỘ 52 Ô ---
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

        // --- TỌA ĐỘ CHUỒNG và ĐÍCH ---

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
                network.Send($"CHAT|{txtMessage.Text}");
                txtMessage.Clear();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
        private void btnDice_Click_1(object sender, EventArgs e)
        {
            if (network != null)
            {
                network.Send("ROLL");
                btnDice.Enabled = false;
            }
        }
        private void XuLyKetThucGame()
        {
            List<Player> ketQua = new List<Player>();

            foreach (TeamColor mauDoi in Enum.GetValues(typeof(TeamColor)))
            {
                int soQuan = pieces.Count(p => p.Team == mauDoi && p.State == PieceState.Finished);
                string tenHienThi = "";

                if (mauDoi == myTeam)
                {
                    tenHienThi = GameSession.CurrentUser_Name;
                    soQuan = 4;
                }
                else
                {
                    tenHienThi = "Team " + mauDoi.ToString();
                }
                ketQua.Add(new Player(tenHienThi, mauDoi, soQuan));
            }

            ketQua = ketQua.OrderByDescending(x => x.SoQuanVeDich).ToList();

            for (int i = 0; i < ketQua.Count; i++)
            {
                ketQua[i].ThuHang = i + 1;
            }
            if (network != null) network.Send("DONE");

            this.Invoke((MethodInvoker)delegate
            {
                RankingBoard frm = new RankingBoard();
                frm.HienThiKetQua(ketQua);
                frm.ShowDialog();

                this.Close();
            });
        }

        private void green3_Click(object sender, EventArgs e)
        {

        }
    }
}
