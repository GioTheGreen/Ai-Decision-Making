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
    public enum ESort 
    {
        eHeight,
        ePriority,
        eRandom
    }
    public EState state = EState.eUnkown;
    public ESort PriorotySort = ESort.eHeight;
    public int priority = 0;
    public virtual string GetNodeType() 
    {
        return "base";
    }
}