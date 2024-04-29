using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_Condition : Node {

    [Input] public int entry;
    public string Condition;

    public string GetCondition()
    {
        return Condition;
;
    }
}