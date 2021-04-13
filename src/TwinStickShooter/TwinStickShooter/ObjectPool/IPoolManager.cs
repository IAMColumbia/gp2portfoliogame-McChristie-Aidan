using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickShooter.ObjectPool
{
    interface IPoolManager
    {
        Dictionary<string, Pool> PoolDictionary { get; }
    }
}
