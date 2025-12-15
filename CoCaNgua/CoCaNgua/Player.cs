using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCaNgua
{
    public class Player
    {
        public string Ten { get; set; }   
        public TeamColor Doi { get; set; }  
        public int SoQuanVeDich { get; set; }
        public int ThuHang { get; set; }
        public Player(string ten, TeamColor doi, int soQuan)
        {
            Ten = ten;
            Doi = doi;
            SoQuanVeDich = soQuan;
            ThuHang = 0; 
        }
    }
}
