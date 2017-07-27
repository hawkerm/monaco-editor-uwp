using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco
{
    public interface IPosition: IJsonable
    {
        uint Column { get; }
        uint LineNumber { get; }
    }
}
