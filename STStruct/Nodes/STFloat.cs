using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Value} (Float)")]
    public class STFloat : NodeBase
    {
        public STFloat(float val)
        {
            Type = NodeType.Float;
            Value = val;
        }

        public float Value { get; set; }

        public override string ToString()
            => Value.ToString();
    }
}
