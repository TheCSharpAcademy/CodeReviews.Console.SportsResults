using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsResults.BrozDa
{
    internal class Game
    {
        public Team Winner { get; set; } = null!;
        public Team Looser { get; set; } = null!;
        public Stat Pts { get; set; } = null!;
        public Stat Trb { get; set; } = null!;
    }
}
