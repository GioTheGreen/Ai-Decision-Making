using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using XNodeEditor;
using static XNode.Node;
using static XNodeEditor.NodeEditor;

[CustomNodeEditor(typeof(BT_StartNode))]
[NodeTint(1.0f, 0.0f, 0.0f)]
public class StartNodeEditor : NodeEditor
{
    public override void OnBodyGUI()
    {
        EditorStyles.label.normal.textColor = new Color(255f/255f, 97f/255f, 79f/255f);
        base.OnBodyGUI();
        EditorStyles.label.normal.textColor = Color.white;
    }

}