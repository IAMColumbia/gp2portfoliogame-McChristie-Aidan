﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickShooter.EntitiyTraits
{
    interface IDamagable
    {
        float Health { get; set; } 
        void TakeDamage(float damageAmmount);
    }
}
