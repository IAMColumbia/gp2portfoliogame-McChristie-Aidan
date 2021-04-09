using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;

namespace TwinStickShooter.ObjectPool
{
    public interface IPool
    {
        DrawableSprite Obj { get; }
    }

    public class Pool
    {      
        protected DrawableSprite obj;

        protected DrawableSprite Obj
        {
            get { return obj; }
            set { obj = value; }
        }
    }
}
