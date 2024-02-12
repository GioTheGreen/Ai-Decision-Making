using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_BaseNode : Node {
    public virtual string GetNodeType() 
    {
        return "base";
    }
}