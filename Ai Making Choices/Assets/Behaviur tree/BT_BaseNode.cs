using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BT_BaseNode : Node {

    [Input] public float value;
    [Output] public float result;
	

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}
}