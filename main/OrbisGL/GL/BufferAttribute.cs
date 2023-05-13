namespace OrbisGL.GL
{
    public struct BufferAttribute
    {
        public BufferAttribute(string Name, AttributeType Type, AttributeSize Size) {
            this.Type = Type;
            this.Size = Size;
            this.Name = Name;

            AttributeIndex = 0;
            AttributeSize = 0;
            AttributeType = 0;
            AttributeOffset = 0;
        }
        public AttributeType Type;
        public AttributeSize Size;
        public string Name;

        internal int AttributeIndex;
        internal int AttributeSize;
        internal int AttributeType;
        internal int AttributeOffset;
    }
}