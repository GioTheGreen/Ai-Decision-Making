﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using XNode;

public class BT_ActionNode : BT_BaseNode
{
    [Input] public int entry;
	[Output] public int exit;
    [Output] public int condition;
    public string Action;
	//public bool done = false;

	public string GetAction() 
	{
		return Action;
	}
    public string getcondition()
	{
        foreach (NodePort p in Ports)
        {
            if (p.fieldName == "condition")
            {
                BT_Condition alpha = p.Connection.node as BT_Condition;
                return alpha.GetCondition();
                
            }
        }
        return "none";
	}
    public override string GetNodeType()
    {
		return "action";
    }
}