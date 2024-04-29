using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

public class test_chat : MonoBehaviour
{
    public NG_BehaivurTree BT;
    public float dt = 0;
    private bool taskDone = false;
    public float wait = 0;
    public enum ETimeTask
    {
        enone,
        eWait
    }
    public ETimeTask task = ETimeTask.enone;
    void Start()
    {
        BT.Refreash();
    }
    void Update()
    {
        dt = Time.deltaTime;

        if ((BT.curent.state == BT_BaseNode.EState.eInProgress) && (!BT.curent.conditionCheck()))
        {
            switch (BT.curent.GetNodeType())
            {
                case "action":
                    BT_ActionNode action = BT.curent as BT_ActionNode;
                    if (!checkPossablity(action.getcondition()))
                    {
                        BT.Set_Stat(BT_BaseNode.EState.eFailed);
                    }
                        break;
                case "sequence":
                    BT_SequenceNode sequence = BT.curent as BT_SequenceNode;
                    if (!checkPossablity(sequence.getcondition()))
                    {
                        BT.Set_Stat(BT_BaseNode.EState.eFailed);
                    }
                    break;
                case "selector":
                    BT_SelectorNode selector = BT.curent as BT_SelectorNode;
                    if (!checkPossablity(selector.getcondition()))
                    {
                        BT.Set_Stat(BT_BaseNode.EState.eFailed);
                    }
                    break;
                default:

                     break;
            }
            BT.curent.cChecked(true);
        }
        else if (BT.curent.GetNodeType() == "action" && BT.curent.state == BT_BaseNode.EState.eInProgress)
        {
            if (BT.curent.conditionCheck()) // check condition every call
            {
                BT_ActionNode action = BT.curent as BT_ActionNode;
                if (checkPossablity(action.getcondition()))
                {
                    DoAction(BT.Read_Action());
                }
                else
                {
                    BT.Set_Stat(BT_BaseNode.EState.eFailed);
                }
            }
            else
            {
                DoAction(BT.Read_Action());
            }
            
            
        }
        else if (BT.curent.GetNodeType() == "start" && BT.curent.state == BT_BaseNode.EState.eSuccess)
        {
            //end
        }
        else
        {
            BT.next();
        }
    }

    public void DoAction(string action)
    {
        string CAPmessage = action.ToUpper();
        string[] Componentes = CAPmessage.Split('/');
        if (Componentes.Length < 2)// error check 1
        {
            Debug.Log("Incoreect Message layout");
            return;
        }

        if (TaskDone())
        {
            if (BT.curent.GetNodeType() == "action" && BT.curent.state == BT_BaseNode.EState.eInProgress)
            {
                switch (Componentes[0]) //can convert this to boolian numbers and treat like emulator
                {
                case "PRINT":
                    Debug.Log(Componentes[1]);
                    BT.Set_Stat(BT_BaseNode.EState.eSuccess);
                    break;
                case "FAIL":
                    Debug.Log("Error: task failed");
                    BT.Set_Stat(BT_BaseNode.EState.eFailed);
                    break;
                case "WAIT":
                    Debug.Log("wait for " + int.Parse(Componentes[1]) + " secounds...");
                    task = ETimeTask.eWait;
                    wait = int.Parse(Componentes[1]);

                    break;
                default:
                    Debug.Log("Incoreect Message componet 1"); // error check 2
                    break;
                }
            }
        }
        else 
        {
            DoTask();
        }
    }

    public bool checkPossablity(string action)
    {
        return true;
    }
    public bool TaskDone()
    {
        switch (task)
        {
            case ETimeTask.enone:
                return true;
            case ETimeTask.eWait:
                if (wait <= 0 || BT.curent.state == BT_BaseNode.EState.eSuccess)
                {
                    wait = 0;
                    task = ETimeTask.enone;
                    BT.Set_Stat(BT_BaseNode.EState.eSuccess);
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return true;
        }
    }
    public void DoTask() 
    {
        switch (task)
        {
            case ETimeTask.enone:
                break;
            case ETimeTask.eWait:
                wait -= dt;
                break;
            default:
                break;
        }
    }
}
