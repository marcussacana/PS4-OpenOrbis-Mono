﻿// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Processing.Processors.Filters
{
    /// <summary>
    /// Converts the colors of the image recreating Achromatopsia (Monochrome) color blindness.
    /// </summary>
    public sealed class AchromatopsiaProcessor : FilterProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AchromatopsiaProcessor"/> class.
        /// </summary>
        public AchromatopsiaProcessor()
            : base(KnownFilterMatrices.AchromatopsiaFilter)
        {
        }
    }
}
