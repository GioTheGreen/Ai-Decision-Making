using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BT_ActionNode))]
public class ActionNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        EditorStyles.label.normal.textColor = new Color(140f / 255f, 244f / 255f, 255f / 255f);
        base.OnBodyGUI();
        EditorStyles.label.normal.textColor = Color.white;
    }
}
