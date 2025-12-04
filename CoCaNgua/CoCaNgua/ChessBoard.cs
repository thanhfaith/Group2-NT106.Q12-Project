using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CoCaNgua
{
    public partial class ChessBoard : Form 
    {
        List<QuanCo> pieces = new List<QuanCo>();
        Random random = new Random();

        public ChessBoard()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            if (pieces == null) pieces = new List<QuanCo>();
            pieces.Clear();

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
                p.UiControl.Parent = panel1;
                p.UiControl.BackColor = Color.Transparent;
                p.State = PieceState.InHome;
                p.CurrentStep = 0;
                p.CurrentPosition = -1;

                UpdatePieceUI(p);
            }
        }

        private void UpdatePieceUI(QuanCo piece)
        {
            Point centerPoint = GetPixelCoordinates(piece.Id, piece.CurrentPosition, piece.Team, piece.State);
            int newX = centerPoint.X - (piece.UiControl.Width / 2);
            int newY = centerPoint.Y - (piece.UiControl.Height / 2);
            piece.UiControl.Location = new Point(newX, newY);
            if (piece.UiControl.Parent != panel1) 
            {
                piece.UiControl.Parent = panel1;
                piece.UiControl.BackColor = Color.Transparent;
            }

            piece.UiControl.BringToFront();
        }

        private readonly Point[] trackPoints = new Point[]
        {
            new Point(71, 283), new Point(117, 282), new Point(158, 284), new Point(211, 280),
            new Point(249, 280), new Point(298, 238), new Point(291, 197), new Point(294, 154),
            new Point(291, 110), new Point(294, 68), new Point(293, 25), new Point(335, 22),
            new Point(380, 22), new Point(382, 71), new Point(378, 114), new Point(379, 155),
            new Point(378, 197), new Point(380, 235), new Point(432, 280), new Point(467, 280),
            new Point(514, 279), new Point(566, 280), new Point(594, 276), new Point(647, 275),
            new Point(642, 321), new Point(651, 359), new Point(599, 365), new Point(559, 366),
            new Point(514, 362), new Point(463, 363), new Point(422, 365), new Point(376, 407),
            new Point(378, 445), new Point(378, 489), new Point(381, 530), new Point(377, 577),
            new Point(379, 613), new Point(328, 617), new Point(292, 619), new Point(285, 576),
            new Point(290, 532), new Point(290, 498), new Point(281, 445), new Point(290, 410),
            new Point(240, 361), new Point(202, 357), new Point(154, 363), new Point(110, 363),
            new Point(58, 367),  new Point(16, 365),  new Point(16, 322),  new Point(21, 285)
        };

        private readonly Point[] redHomePoints = new Point[]
        {
            new Point(87, 85), new Point(187, 89), new Point(91, 171), new Point(182, 176)
        };

        private readonly Point[] greenHomePoints = new Point[]
        {
            new Point(487, 87), new Point(581, 85), new Point(491, 174), new Point(585, 173)
        };

        private readonly Point[] yellowHomePoints = new Point[]
        {
            new Point(491, 469), new Point(582, 471), new Point(494, 555), new Point(583, 559)
        };

        private readonly Point[] blueHomePoints = new Point[]
        {
            new Point(92, 466), new Point(181, 473), new Point(89, 556), new Point(177, 553)
        };

        private readonly Point[] redFinishPoints = new Point[]
        {
            new Point(65, 321), new Point(110, 321), new Point(152, 318),
            new Point(199, 317), new Point(242, 317), new Point(292, 317)
        };

        private readonly Point[] greenFinishPoints = new Point[]
        {
            new Point(337, 63), new Point(334, 108), new Point(337, 152),
            new Point(334, 189), new Point(335, 233), new Point(337, 273)
        };

        private readonly Point[] yellowFinishPoints = new Point[]
        {
            new Point(604, 317), new Point(556, 317), new Point(513, 319),
            new Point(465, 318), new Point(424, 319), new Point(378, 319)
        };

        private readonly Point[] blueFinishPoints = new Point[]
        {
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

        private void Vaogame_Load(object sender, EventArgs e)
        {

        }
    }
}