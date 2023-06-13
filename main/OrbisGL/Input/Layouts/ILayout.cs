using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input.Layouts
{
    public interface ILayout
    {
        string Name { get; }
        string EnglishName { get; }

        string LayoutCode { get; }

        char? GetKeyChar(IMEKeyModifier Key);
    }
}
