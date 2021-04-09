using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinStickShooter.Projectiles;
using Microsoft.Xna.Framework;
using MonoGameLibrary.GameComponents;
using MonoGameLibrary.Util;
using TwinStickShooter.Projectiles;

namespace TwinStickShooter.ObjectPool
{
    public class ShotPool : Pool
    {
        public ShotPool()
        {
            this.Obj = Shot;
        }

    }
}
