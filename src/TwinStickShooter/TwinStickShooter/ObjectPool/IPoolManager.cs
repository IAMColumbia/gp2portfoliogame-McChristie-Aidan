using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinStickShooter.ObjectPool
{
    //mainly used to mark the poolmanager as a service as in a way
    interface IPoolManager
    {
        Dictionary<string, Pool> PoolDictionary { get; }
    }
}
