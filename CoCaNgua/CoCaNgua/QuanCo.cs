using System;
using System.Windows.Forms;

namespace CoCaNgua
{
    // Class này dùng để giữ thông tin quân cờ
    internal class QuanCo
    {
        public int Id { get; set; }
        public TeamColor Team { get; set; }
        public PieceState State { get; set; }

       
        public int CurrentPosition { get; set; }

        public PictureBox UiControl { get; set; }
        public int CurrentStep { get; internal set; }

        public QuanCo(int id, TeamColor team, PictureBox uiControl)
        {
            Id = id;
            Team = team;
            UiControl = uiControl;
            State = PieceState.InHome;
            CurrentPosition = -1;
        }
    }
}