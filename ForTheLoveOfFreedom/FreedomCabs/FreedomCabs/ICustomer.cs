using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreedomCabs
{
    public interface ICustomer
    {
        bool HasRequestedRide { get; }

        void RequestRide();
    }
}
