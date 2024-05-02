using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BT_SelectorNode))]
public class SelectorNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        EditorStyles.label.normal.textColor = new Color(222f / 255f, 232f / 255f, 84f / 255f);
        base.OnBodyGUI();
        EditorStyles.label.normal.textColor = Color.white;
    }
}
