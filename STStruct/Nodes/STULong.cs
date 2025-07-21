using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (ULong)")]
    public class STULong : NodeBase
    {
        public STULong(ulong val)
        {
            Type = NodeType.Double;
            Value = val;
        }

        public ulong Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
