using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliCliBoy.model
{
    public interface IDebugOutput
    {
        void Put(String s);
        void Flush();
        void Clear();
        String Prefix { get; set; }
    }
}
