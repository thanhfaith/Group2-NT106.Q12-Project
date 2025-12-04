using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoCaNgua
{
    public enum PieceState
    {
        InHome,
        OnTrack,
        InFinish,
        Finished
    }

    public enum TeamColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }
    internal class QuanCo
    {
        public int Id { get; set; }
        public TeamColor Team { get; set; }
        public PieceState State { get; set; }
        public int CurrentStep { get; set; }
        public int CurrentPosition { get; set; }
        public PictureBox UiControl { get; set; }

        private const int TotalTrackSteps = 56;
        private const int StepsToReachFinish = 56;

        public QuanCo(int id, TeamColor team, PictureBox uiControl)
        {
            Id = id;
            Team = team;
            State = PieceState.InHome;
            CurrentStep = 0;
            CurrentPosition = -1;
            UiControl = uiControl;
        }

        public bool Move(int diceValue)
        {
            switch (State)
            {
                case PieceState.InHome:
                    return HandleInHome(diceValue);

                case PieceState.OnTrack:
                    return HandleOnTrack(diceValue);

                case PieceState.InFinish:
                    return HandleInFinish(diceValue);

                case PieceState.Finished:
                    return false;

                default:
                    return false;
            }
        }

        private bool HandleInHome(int dice)
        {
            if (dice == 6)
            {
                State = PieceState.OnTrack;
                CurrentStep = 1;

                CurrentPosition = GetStartPosition(Team);
                return true;
            }
            return false;
        }

        private bool HandleOnTrack(int dice)
        {
            if (CurrentStep + dice > StepsToReachFinish)
            {
                int stepsExcess = (CurrentStep + dice) - StepsToReachFinish;

                if (stepsExcess <= 6)
                {
                    State = PieceState.InFinish;
                    CurrentStep += dice;
                    CurrentPosition = stepsExcess;
                    return true;
                }
                return false;
            }
            else
            {
                CurrentStep += dice;
                CurrentPosition = (CurrentPosition + dice) % TotalTrackSteps;
                return true;
            }
        }

        private bool HandleInFinish(int dice)
        {
            int currentLevel = CurrentPosition;
            int nextLevel = currentLevel + dice;

            if (nextLevel == 6)
            {
                State = PieceState.Finished;
                CurrentPosition = 6;
                return true;
            }
            else if (nextLevel < 6)
            {
                CurrentPosition = nextLevel;
                return true;
            }

            return false;
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
    }
}
