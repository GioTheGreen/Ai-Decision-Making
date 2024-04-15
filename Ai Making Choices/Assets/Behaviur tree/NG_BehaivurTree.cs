using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using XNode;
using static BT_BaseNode;

[CreateAssetMenu]
public class NG_BehaivurTree : NodeGraph {
	public BT_BaseNode curent;
    public bool error = false;
    public string DefultAction = "print/error";
    public void Refreash()
    {
        foreach (BT_BaseNode i in nodes)
        {
            i.state = EState.eUnkown;
            if (i.GetNodeType() == "start")
            {
                curent = i;
            }
            else if (i.GetNodeType() == "action")
            {
                //BT_ActionNode a = i as BT_ActionNode;
                //a.done = false;
            }
        }
    }

    public void NextActionNode()
    {
        int connections;
        BT_BaseNode[] childNodes = new BT_BaseNode[0];
        foreach (NodePort p in curent.Ports)
        {
            if (p.fieldName == "exit")
            {
                connections = p.GetConnections().Count;
                NodePort[] conections = p.GetConnections().ToArray();
                childNodes = new BT_BaseNode[conections.Length];
                for (int i = 0; i < conections.Length; i++)
                {
                    childNodes[i] = conections[i].Connection.node as BT_BaseNode;     //get all child nodes
                }
            }
        }
        BT_BaseNode[] OrderedNodes = new BT_BaseNode[childNodes.Length];
        OrderedNodes = InOrder(childNodes);

        switch (curent.GetNodeType())//loop till current is action? maybe? not sure if infinate loop, loop detetion or provention needed possibaly
        {
            case "start"://if no avalable paths start_stat = success, if there is go to most approprate
                switch (curent.state)
                {
                    case EState.eUnkown:
                        bool found = false;
                        for (int i = 0; i < OrderedNodes.Length; i++)
                        {
                            if (OrderedNodes[i].state != EState.eUnkown)
                            {
                                continue;
                            }
                            found = true;
                            curent = OrderedNodes[i];
                            break;
                        }
                        if (!found) //if no reasonable children, succeed
                        {
                            curent.state = EState.eSuccess;
                        }
                        break;
                    case EState.eSuccess:
                        Refreash();
                        break;
                    default:
                        break;
                }
                break;
            case "selector":
                switch (curent.state)
                {
                    case EState.eUnkown:
                        curent.state = EState.eInProgress;
                        break;
                    case EState.eInProgress:
                        bool found = false;
                        for (int i = 0; i < OrderedNodes.Length; i++)
                        {
                            if (OrderedNodes[i].state == EState.eSuccess)
                            {
                                continue;
                            }
                            if (OrderedNodes[i].state == EState.eFailed)
                            {
                                curent.state = EState.eFailed;
                                break;
                            }
                            found = true;
                            curent = OrderedNodes[i];
                            break;
                        }
                        if (!found)
                        {
                            curent.state = EState.eSuccess;
                        }
                        break;
                    case EState.eFailed:
                        ParentNode();
                        break;
                    case EState.eSuccess:
                        ParentNode();
                        break;
                    default:
                        break;
                }
                break;
            case "sequence":
                switch (curent.state)
                {
                    case EState.eUnkown:
                        curent.state = EState.eInProgress;
                        break;
                    case EState.eInProgress:
                        break;
                    case EState.eFailed:
                        break;
                    case EState.eSuccess:
                        break;
                    default:
                        break;
                }
                break;
            case "action":

                break;
            default:
                break;
        }




        switch (curent.state)
        {
            case EState.eUnkown:
                break;
            case EState.eInProgress:
                break;
            case EState.eFailed:
                break;
            case EState.eSuccess:
                break;
            default:
                break;
        }
    }

    public string Read_Action() 
    {
        BT_ActionNode a = curent as BT_ActionNode;
        return a.GetAction();
    }

    //public string Read_Action_condition()                not sure if i need this yet
    //{
    //    BT_ActionNode a = curent as BT_ActionNode;
    //    return a.GetActionCondition();
    //}

    public void Set_Stat(EState State) 
    {
        curent.state = State;
    }

    public void next()
    {
        switch (curent.GetNodeType())//unsure if selector and sequence will be needed for this section but decided to include it to stop any furture errors
        {
            case "start":
                if (NumberOfUNused() == 0) //reset tree
                {
                    Refreash();
                }
                else
                {
                    NextActionNode();
                }// may not be needed
                break;
            case "selector":
                NextActionNode();
                break;
            case "sequence":
                NextActionNode();
                break;
            case "action":
                BT_ActionNode a = curent as BT_ActionNode;
                if (a.state != EState.eInProgress && a.state != EState.eUnkown)
                {
                    NextActionNode();
                }
                break;
            default:
                break;
        }
    }

    public int NumberOfUNused() //to check if tree is compleate
    {
        int unused = 0;
        foreach (BT_BaseNode i in nodes)
        {
            if (i.state == EState.eUnkown)
            {
                unused++;
            }
        }
        return unused;
    }

    private BT_BaseNode[] InOrder(BT_BaseNode[] nodes) // order array in height(y in graph) order
    {
        BT_BaseNode[] toReturn = nodes;

        //while (nodes.Length > 0)
        //{
        //    int currnetHighest = 1;
        //    
        //    for (int i = 0; i < nodes.Length; i++)
        //    {
        //        if (nodes[i].position.y < nodes[currnetHighest].position.y)
        //        {
        //            currnetHighest = i;
        //            cHighestNode = nodes[i];
        //        }
        //    }

        //    Array.Resize(ref toReturn, toReturn.Length + 1);
        //    toReturn[toReturn.Length] = nodes[currnetHighest];

        //    for (int i = currnetHighest; i < nodes.Length - 1; i++)
        //    {
        //        nodes[i] = nodes[i + 1];
        //    }
        //    Array.Resize(ref nodes, toReturn.Length - 1);
        //}

        //bubble sort

        int rounds = nodes.Length;
        BT_BaseNode temp = null;

        for (int i = 0; i < rounds; i++)
        {
            if (rounds < 2)
            {
                break;
            }
            for (int j = 0; j < rounds - 1; j++)
            {
                float highta = toReturn[j].position.y;
                float hightb = toReturn[j+1].position.y;

                if (highta >= hightb)
                {
                    temp = toReturn[j];
                    toReturn[j] = toReturn[j+1];
                    toReturn[j+1] = temp;
                }
            }
            rounds--;
        }

        return toReturn;
    }
}