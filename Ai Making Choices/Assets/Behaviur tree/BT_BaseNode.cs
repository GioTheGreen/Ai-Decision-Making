using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class BT_BaseNode : Node {
    public enum EState
    {
        eUnkown,
        eInProgress,
        eFailed,
        eSuccess
    }
    public EState state = EState.eUnkown;
    public virtual string GetNodeType() 
    {
        return "base";
    }
}