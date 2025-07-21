using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("{Name} (String)")]
    public class STString : NodeBase
    {
        public STString()
        {
            Type = NodeType.String;
        }
        
        public string Name { get; set; }
    }
}
