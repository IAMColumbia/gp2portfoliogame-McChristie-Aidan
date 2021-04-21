using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using TwinStickShooter.Weapons;

namespace TwinStickShooter.Enemies
{
    class RangedEnemy : Enemy
    {
        Weapon weapon;
        

        public RangedEnemy(Game game, Weapon gun) : base(game)
        {
            this.weapon = gun;
        }
    }
}
