using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	public int count = 0;
	public string funct = "Enter a function";
	public string xmin = "Enter a minumum value for x";
	public string xmax = "Enter a maximum value for x";
	public string ymin = "Enter a minumum value for y";
	public string ymax = "Enter a maximum value for y";
	public string zmin = "Enter a minumum value for z";
	public string zmax = "Enter a maximum value for z";
	int i = 0;
	void OnGUI(){
		GUI.color = Color.magenta;
		string[] vals = { zmax, zmin, ymax, ymin, xmax, xmin, funct };
		for(i=0; i<=6; i++){
			vals[i] = GUI.TextField (new Rect (transform.position.x + 165, transform.position.y + 15, 200, 30), vals[i]);
		}
	}
	void MoveBack(){
		while (count < 7){
		if (Event.current.Equals (Event.KeyboardEvent ("[return]"))) {
			GUI.BringWindowToBack (0);
			count++;
			}
		}
	}
}
		
	