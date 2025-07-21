using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("Object")]
    public class STObject : NodeBase
    {
        public STObject()
        {
            Type = NodeType.Object;
        }
        
        public NodeBase Child;
    }
}
