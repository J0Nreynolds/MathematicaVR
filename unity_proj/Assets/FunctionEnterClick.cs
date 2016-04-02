using UnityEngine;
using System.Collections;

public class FunctionEnterClick : MonoBehaviour {
	public string stringToEdit = "enter your function";
	void OnGUI() {
		stringToEdit = GUI.TextField(new Rect(65, 3, 500, 55), stringToEdit);
	}
}