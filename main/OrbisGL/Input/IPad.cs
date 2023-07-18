using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbisGL.Input
{
    public interface IPad : IDisposable
    {
        /// <summary>
        /// Gets the gamepad current data struct
        /// </summary>
        object Data { get; }

        /// <summary>
        /// Open a gamepad of the given User ID
        /// </summary>
        /// <param name="UserID">The User ID</param>
        void Open(int UserID);

        /// <summary>
        /// Close this gamepad
        /// </summary>
        void Close();

        /// <summary>
        /// Refresh the gamepad current data
        /// </summary>
        void Refresh();
    }
}
