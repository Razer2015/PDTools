using System.Collections.Generic;
using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("Map: {Elements.Count} Elements")]
    public class STMap : NodeBase
    {
        public STMap()
        {
            Type = NodeType.Map;
        }
        
        public Dictionary<string, NodeBase> Elements { get; set; } = new Dictionary<string, NodeBase>();
    }
}
