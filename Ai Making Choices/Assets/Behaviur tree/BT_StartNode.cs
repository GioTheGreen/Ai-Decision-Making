using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class BT_StartNode : BT_BaseNode {
    [Output] public int exit;

 
    public override string GetNodeType()
    {
        return "start";
    }
}