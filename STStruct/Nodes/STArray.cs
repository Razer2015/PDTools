using System.Collections.Generic;
using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("Array[{Elements.Count}]")]
    public class STArray : NodeBase
    {
        public STArray()
        {
            Type = NodeType.Array;
        }
        
        public List<NodeBase> Elements { get; set; }
    }
}
