using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using XNode;
using static BT_BaseNode;

[CreateAssetMenu]
public class NG_BehaivurTree : NodeGraph {
	public BT_BaseNode curent;
    public bool error = false;
    public string DefultAction = "print/defult";
    public void Refreash()
    {
        foreach (BT_BaseNode i in nodes)
        {
            i.state = EState.eUnkown;
            i.cChecked(false);
            if (i.GetNodeType() == "start")
            {
                curent = i;
            }
            else if (i.GetNodeType() == "action")
            {
                //BT_ActionNode a = i as BT_ActionNode;
                //a.done = false;
            }

            //if (i.PriorotySort == ESort.eRandom)
            //{
            //    foreach (NodePort p in curent.Ports)
            //    {
            //        if (p.fieldName == "exit")
            //        {
            //            int childaren = p.GetConnections().Count;

            //        }
            //    }
            //}
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
                    childNodes[i] = conections[i].node as BT_BaseNode;     //get all child nodes
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
                        //Refreash();//loop tree
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
                            if (OrderedNodes[i].state == EState.eSuccess) // see first is it has succesed if so no need to do others
                            {
                                found = true;
                                curent.state = EState.eSuccess;
                                break;
                            }
                            if (OrderedNodes[i].state == EState.eUnkown)
                            {
                                found = true;
                                curent = OrderedNodes[i];
                                break;
                            }
                        }
                        if (!found) // if no tast succeded then return false
                        {
                            curent.state = EState.eFailed;
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
                                found = true;
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
            case "action":
                switch (curent.state)
                {
                    case EState.eUnkown:
                        curent.state = EState.eInProgress;
                        break;
                    case EState.eInProgress:
                        //wait for response...
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
            default:
                break;
        }
    }

    public string Read_Action() 
    {
        if (curent.GetNodeType() == "action")
        {
            BT_ActionNode a = curent as BT_ActionNode;
            return a.GetAction();
        }
        else
        {
            return DefultAction;
        }
        
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
        if (curent.GetNodeType() == "action")
        {
            if (curent.state != EState.eInProgress)
            {
                NextActionNode();
            }
        }
        else
        {
            NextActionNode();
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

    private BT_BaseNode[] InOrder(BT_BaseNode[] nodes) // order array
    {
        BT_BaseNode[] toReturn = nodes;

        //bubble sort
        BT_BaseNode temp = null;
        int rounds = nodes.Length;

        switch (curent.PriorotySort)
        {
            case ESort.eHeight: // order array in height(y in graph) order
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
                break;
            case ESort.ePriority: // order array in priority order
                for (int i = 0; i < rounds; i++)
                {
                    if (rounds < 2)
                    {
                        break;
                    }
                    for (int j = 0; j < rounds - 1; j++)
                    {
                        int prioritya = toReturn[j].priority;
                        int priorityb = toReturn[j + 1].priority;

                        if (prioritya >= priorityb)
                        {
                            temp = toReturn[j];
                            toReturn[j] = toReturn[j + 1];
                            toReturn[j + 1] = temp;
                        }
                    }
                    rounds--;
                }
                break;
            case ESort.eRandom:  // mix array in random order
                BT_BaseNode[] done = new BT_BaseNode[rounds];
                int done_u = 0;
                BT_BaseNode[] not_done = new BT_BaseNode[rounds];
                int not_done_u = 0;
                int[] read = new int[rounds];
                while (rounds > 0)
                {
                    int rand = UnityEngine.Random.Range(0,rounds);
                    if (rounds != read.Length)
                    {
                        if (read.Contains<int>(rand))
                        {
                            continue;
                        }
                        else
                        {
                            read[read.Length - rounds] = rand;
                        }
                    }
                    if (toReturn[rand].state == EState.eUnkown)
                    {
                        not_done[not_done_u] = toReturn[rand];
                    }
                    else
                    {
                        done[done_u] = toReturn[rand];
                    }
                    rounds--;
                }
                for (int i = 0; i < not_done_u; i++)
                {
                    int rand = UnityEngine.Random.Range(0, not_done_u);
                    temp = not_done[i];
                    not_done[i] = not_done[rand];
                    not_done[rand] = temp;
                }
                for (int i = 0; i < done_u; i++)
                {
                    int rand = UnityEngine.Random.Range(0, done_u);
                    temp = done[i];
                    done[i] = done[rand];
                    done[rand] = temp;
                }
                for (int i = 0; i < done_u; i++)
                {
                    toReturn[i] = done[i];
                }
                for (int i = 0; i < not_done_u; i++)
                {
                    toReturn[i + done_u] = not_done[i];
                }
                break;
            default:
                break;
        }
        return toReturn;
    }
    public void ParentNode()
    {
        foreach (NodePort p in curent.Ports)
        {
            if (p.fieldName == "entry")
            {
                NodePort[] conections = p.GetConnections().ToArray();
                curent = conections[0].node as BT_BaseNode;
            }
        }
    }
}