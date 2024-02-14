using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.AI;
using XNode;

public class cd_Robot : MonoBehaviour
{
    public GameObject spot1, spot2, spot3;
    public NG_BehaivurTree BT;


    private NavMeshAgent navMeshAgent;
    private GameObject current_tartget;
    private string Current_Action;
    private bool idel = true;
    private GameObject HeldItem;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        current_tartget = spot2;
        BT.Refreash();
    }
    void Update()
    {
        navMeshAgent.destination = current_tartget.transform.position;
        //if (Vector3.Distance(current_tartget.transform.position,transform.position) < 1)
        //{
        //    if (current_tartget == spot1)
        //    {
        //        current_tartget = spot2;
        //    }
        //    else if (current_tartget == spot2)
        //    {
        //        current_tartget = spot3;
        //    }
        //    else
        //    {
        //        current_tartget = spot1;
        //    }
        //}
        if (idel)
        {
            foreach (NodePort p in BT.curent.Ports)
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


                        BT.curent = p.Connection.node as BT_BaseNode;
                        string nodetype = BT.curent.GetNodeType();
                        switch (nodetype)
                        {
                            case "action":
                                BT_ActionNode actionNode = p.Connection.node as BT_ActionNode;
                                Current_Action = actionNode.GetAction();
                                Debug.Log(Current_Action);
                                break;
                            default:
                                break;
                        }
                        idel = false;
                    }
                    else //return to start
                    {
                        BT.Refreash();
                    }
                }
            }
        }
        else
        {
            if (!CheckAction(Current_Action))
            {
                DoAction(Current_Action);
            }
            else
            {
                idel = true;
            }
        }
    }
    public void DoAction(string Action)
    {
        string CAPmessage = Action.ToUpper();
        string[] Componentes = CAPmessage.Split('/');
        if (Componentes.Length != 2)// error check 1
        {
            Debug.Log("Incoreect Message layout");
            return;
        }
        switch (Componentes[0]) //can convert this to boolian numbers and treat like emulator
        {
            case "GO":
                switch (Componentes[1])
                {
                    case "SPOT1":
                        setCurrentTarget(spot1);
                        break;
                    case "SPOT2":
                        setCurrentTarget(spot2);
                        break;
                    case "SPOT3":
                        setCurrentTarget(spot3);
                        break;
                    default:
                        Debug.Log("Incoreect Message componet 2"); // error check 3
                        break;
                }
                break;
            case "PICK":

                break;
            case "DROP":

                break;
            default:
                Debug.Log("Incoreect Message componet 1"); // error check 2
                break;
        }
    }
    public bool CheckAction(string Action) 
    {
        string CAPmessage = Action.ToUpper();
        string[] Componentes = CAPmessage.Split('/');
        switch (Componentes[0])
        {
            case "GO":
                return (Vector3.Distance(current_tartget.transform.position, transform.position) < 1);
            case "PICK":
                return false;
            case "DROP":
                return false;
        }
        return false;
    }
    private void setCurrentTarget(GameObject GO)
    {
        current_tartget = GO;
    }
    //public int getHeldItemType()
    //{
    //    switch (HeldItem.type)
    //    {
    //        case null:
    //            return -1;
    //        case paper:
    //            return 0;
    //        case paslic:
    //            return 1;
    //        case genaral:
    //            return 2;
    //        default:
    //            break;
    //    }
    //}
}

