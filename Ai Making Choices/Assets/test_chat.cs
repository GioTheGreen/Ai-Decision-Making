using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class test_chat : MonoBehaviour
{
    public NG_BehaivurTree BT;

    void Start()
    {
        BT.Refreash();
    }
    void Update()
    {
        if (BT.curent.GetNodeType() == "action" && BT.curent.state == BT_BaseNode.EState.eInProgress)
        {
            if (checkPossablity(BT.Read_Action()))
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
            default:
                Debug.Log("Incoreect Message componet 1"); // error check 2
                break;
        }
    }

    public bool checkPossablity(string action)
    {
        return true;
    }
}
