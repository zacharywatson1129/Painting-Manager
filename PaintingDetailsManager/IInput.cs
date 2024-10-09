using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager
{
    interface IInput<T1>
    {
        T1 CurrentItem { get; set; }
        bool validate();
    }
}
