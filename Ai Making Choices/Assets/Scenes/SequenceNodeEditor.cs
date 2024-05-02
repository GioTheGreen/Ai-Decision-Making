using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(BT_SequenceNode))]
public class SequenceNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        EditorStyles.label.normal.textColor = new Color(84f / 255f, 232f / 255f, 156f / 255f);
        base.OnBodyGUI();
        EditorStyles.label.normal.textColor = Color.white;
    }
}
