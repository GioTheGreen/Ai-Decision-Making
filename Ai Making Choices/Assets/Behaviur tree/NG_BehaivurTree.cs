using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class NG_BehaivurTree : NodeGraph {
	public BT_BaseNode curent;

    public void Refreash()
    {
        foreach (BT_BaseNode i in nodes)
        {
            if (i.GetNodeType() == "start")
            {
                curent = i;
            }
            else if (i.GetNodeType() == "action")
            {
                BT_ActionNode a = i as BT_ActionNode;
                a.done = false;
            }
        }
    }


}