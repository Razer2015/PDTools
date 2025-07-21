using System;
using System.Diagnostics;

namespace PDTools.STStruct.Nodes
{
    [DebuggerDisplay("byte[{Data.Length}]")]
    public class MBlob : NodeBase
    {
        public MBlob(Memory<byte> data)
        {
            Type = NodeType.MBlob;
            Data = data;
        }
        public Memory<byte> Data { get; set; }
    }
}
