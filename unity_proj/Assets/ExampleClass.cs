using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {
	void Update() {
		if (Input.GetKey("escape"))
			Application.Quit();
	}
}