using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (Int)")]
    public class STInt : NodeBase
    {
        public STInt(int val)
        {
            Type = NodeType.Int;
            Value = val;
        }

        public int Value { get; set; }
        public NodeBase KeyConfigNode { get; set; }
        public override string ToString()
            => Value.ToString();
    }
}
