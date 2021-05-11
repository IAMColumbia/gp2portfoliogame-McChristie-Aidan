using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickShooter.HUD
{
    interface IScoreManager
    { }

    class ScoreManager : IScoreManager
    {
        public int waveNumber;

        public int score;
    }
}
