using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (UShort)")]
    public class STUShort : NodeBase
    {
        public STUShort(ushort val)
        {
            Type = NodeType.UShort;
            Value = val;
        }

        public ushort Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
