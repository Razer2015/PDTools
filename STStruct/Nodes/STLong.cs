using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (Long)")]
    public class STLong : NodeBase
    {
        public STLong(long val)
        {
            Type = NodeType.Long;
            Value = val;
        }

        public long Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
