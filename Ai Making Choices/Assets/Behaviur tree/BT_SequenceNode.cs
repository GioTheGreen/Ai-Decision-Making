using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_SequenceNode : BT_BaseNode
{
    [Input] public int entry;
    [Output] public int exit;
    public override string GetNodeType()
    {
        return "sequence";
    }
}