using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_ActionNode : BT_BaseNode
{
	[Input] public int entry;
	[Output] public int exit;
	public string Action;

	public string GetAction() 
	{
		return Action;
	}

    public override string GetNodeType()
    {
		return "action";
    }
}