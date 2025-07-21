using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (SByte)")]
    public class STSByte : NodeBase
    {
        public STSByte(sbyte val)
        {
            Type = NodeType.SByte;
            Value = val;
        }

        public sbyte Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
