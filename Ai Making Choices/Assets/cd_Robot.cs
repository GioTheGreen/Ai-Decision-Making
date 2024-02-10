using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class cd_Robot : MonoBehaviour
{
    public GameObject spot1, spot2, spot3;
    public bool idel = true;
    private NavMeshAgent navMeshAgent;
    private GameObject current_tartget;
    private string Current_Action;
    private GameObject HeldItem;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        current_tartget = spot1;
    }
    void Update()
    {
        navMeshAgent.destination = current_tartget.transform.position;
        if (Vector3.Distance(current_tartget.transform.position,transform.position) < 1)
        {
            if (current_tartget == spot1)
            {
                current_tartget = spot2;
            }
            else if (current_tartget == spot2)
            {
                current_tartget = spot3;
            }
            else
            {
                current_tartget = spot1;
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
    private void setCurrentTarget(GameObject GO)
    {
        current_tartget = GO;
    }
    public void setCurrentAction(string action)
    {
        Current_Action = action;
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

