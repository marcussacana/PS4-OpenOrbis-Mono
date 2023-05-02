// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Advanced
{
    /// <summary>
    /// Defines the contract for an action that operates on a row.
    /// </summary>
    public interface IRowOperation
    {
        /// <summary>
        /// Invokes the method passing the row y coordinate.
        /// </summary>
        /// <param name="y">The row y coordinate.</param>
        void Invoke(int y);
    }
}
