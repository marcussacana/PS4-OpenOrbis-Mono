// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

namespace SixLabors.ImageSharp.Metadata.Profiles.Exif
{
    internal sealed class ExifLongArray : ExifArrayValue<uint>
    {
        public ExifLongArray(ExifTag<uint[]> tag)
            : base(tag)
        {
        }

        public ExifLongArray(ExifTagValue tag)
            : base(tag)
        {
        }

        private ExifLongArray(ExifLongArray value)
            : base(value)
        {
        }

        public override ExifDataType DataType => ExifDataType.Long;

        public override IExifValue DeepClone() => new ExifLongArray(this);
    }
}
