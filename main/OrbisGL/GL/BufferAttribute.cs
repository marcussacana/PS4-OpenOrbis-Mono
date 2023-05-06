namespace OrbisGL.GL
{
    public struct BufferAttribute
    {
        public AttributeType Type;
        public AttributeSize Size;
        public string Name;

        internal int AttributeIndex;
        internal int AttributeSize;
        internal int AttributeType;
        internal int AttributeOffset;
    }
}