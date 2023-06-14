using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input.Layouts
{
    public interface ILayout
    {

        /// <summary>
        /// Keyboard Localizated Friendly Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Keyboard English Friendly Name
        /// </summary>
        string EnglishName { get; }


        /// <summary>
        /// Keyboard Layout Codename
        /// </summary>
        string LayoutCode { get; }

        /// <summary>
        /// Orbis Language ID get with ORBIS_SYSTEM_SERVICE_PARAM_ID_LANG
        /// </summary>
        int LanguageID { get; }


        /// <summary>
        /// Parse the Key state to his character if possible.
        /// </summary>
        /// <param name="Key">The key state</param>
        /// <returns>The key character</returns>
        char? GetKeyChar(IMEKeyModifier Key);
    }
}
