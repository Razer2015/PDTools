using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (Byte)")]
    public class STByte : NodeBase
    {
        public STByte(byte val)
        {
            Type = NodeType.UByte;
            Value = val;
        }

        public byte Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
