using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_BaseNode : Node {
    public bool success = false;
    public virtual string GetNodeType() 
    {
        return "base";
    }
}