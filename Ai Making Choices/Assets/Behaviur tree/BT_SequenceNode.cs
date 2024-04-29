using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_SequenceNode : BT_BaseNode
{
    [Input] public int entry;
    [Output] public int exit;
    [Output] public int condition;
    public string getcondition()
    {
        foreach (NodePort p in Ports)
        {
            if (p.fieldName == "condition" && p.ConnectionCount > 0)
            {
                BT_Condition alpha = p.Connection.node as BT_Condition;
                return alpha.GetCondition();

            }
        }
        return "none";
    }
    public override string GetNodeType()
    {
        return "sequence";
    }
}