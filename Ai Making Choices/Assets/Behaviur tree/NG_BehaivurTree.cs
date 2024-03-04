using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class NG_BehaivurTree : NodeGraph {
	public BT_BaseNode curent;
    public enum EState
    {
        eUnkown,
        eInProgress,
        eFailed,
        eSuccess
    }


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

    public BT_BaseNode NextNode()
    {return null;

        foreach (NodePort p in curent.Ports)
        {
            if (p.fieldName == "exit")
            {
                if (p.ConnectionCount > 0)
                {
                    NodePort[] conections = p.GetConnections().ToArray();
                    BT_BaseNode[] childNodes = new BT_BaseNode[conections.Length];
                    for (int i = 0; i < conections.Length; i++)
                    {
                        childNodes[i] = conections[i].Connection.node as BT_BaseNode;     //get all child nodes
                    }

                    

                }
            }
        }
    }

    private BT_BaseNode[] InOrder(BT_BaseNode[] nodes)
    {
        BT_BaseNode[] toReturn = new BT_BaseNode[0];

        while (nodes.Length > 0)
        {
            int currnetHighest = 0;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].position.y < nodes[currnetHighest].position.y)
                {
                    currnetHighest = i;
                }
            }

            Array.Resize(ref toReturn, toReturn.Length + 1);
            toReturn[toReturn.Length] = nodes[currnetHighest];

            for (int i = currnetHighest; i < nodes.Length - 1; i++)
            {
                nodes[i] = nodes[i + 1];
            }
            Array.Resize(ref nodes, toReturn.Length - 1);
        }
        return toReturn;
    }
}