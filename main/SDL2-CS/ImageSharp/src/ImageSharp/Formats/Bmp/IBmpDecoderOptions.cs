﻿// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Formats.Bmp
{
    /// <summary>
    /// Image decoder options for decoding Windows bitmap streams.
    /// </summary>
    internal interface IBmpDecoderOptions
    {
        /// <summary>
        /// Gets the value indicating how to deal with skipped pixels, which can occur during decoding run length encoded bitmaps.
        /// </summary>
        RleSkippedPixelHandling RleSkippedPixelHandling { get; }
    }
}
