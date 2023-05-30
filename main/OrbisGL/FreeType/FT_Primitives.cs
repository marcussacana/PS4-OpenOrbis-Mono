using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.FreeType
{
#if ORBIS
    public enum FT_Long : long {}
    public enum FT_Pos : long { }
    public enum FT_Fixed : long { }
#else
    public enum FT_Long : int { }
    public enum FT_Pos : int { }
    public enum FT_Fixed : int { }
#endif
}
