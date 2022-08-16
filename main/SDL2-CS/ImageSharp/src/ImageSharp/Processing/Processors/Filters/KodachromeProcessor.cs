﻿// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Processing.Processors.Filters
{
    /// <summary>
    /// Applies a filter matrix recreating an old Kodachrome camera effect matrix to the image
    /// </summary>
    public sealed class KodachromeProcessor : FilterProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KodachromeProcessor"/> class.
        /// </summary>
        public KodachromeProcessor()
            : base(KnownFilterMatrices.KodachromeFilter)
        {
        }
    }
}
