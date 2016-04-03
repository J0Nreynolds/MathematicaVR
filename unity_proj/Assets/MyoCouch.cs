using UnityEngine;
using System.Collections;
using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class MyoCouch : MonoBehaviour {
	
	public GameObject myo = null;
	public Material WaveInMaterial;
	public Material FingersSpreadMaterial;
	public Material doubleTapMaterial;
	public Material FistMaterial;
	private Pose _lastPose = Pose.Unknown;
	public float speed;
	public Transform target;
	public Vector3 ReferenceVec = new Vector3(10, 10, 10);

	void Rotate(){
		Camera.main.transform.RotateAround (this.gameObject.transform.position, Vector3.up, 30 * Time.deltaTime);
	}
	void StopRotate(){
		Camera.main.transform.RotateAround (this.gameObject.transform.position, Vector3.up, -30 * Time.deltaTime);
	}

	void Update(){
		ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
		if (thalmicMyo.pose != _lastPose) {
			_lastPose = thalmicMyo.pose;
		}
		if (thalmicMyo.pose == Pose.Fist) { 
			Vector3 dirvect = Camera.main.transform.position - this.gameObject.transform.position;
			Camera.main.transform.Translate (-dirvect * Time.deltaTime);
		} 
		else if (thalmicMyo.pose == Pose.FingersSpread) { 
			Vector3 dirvect = Camera.main.transform.position - this.gameObject.transform.position;
			Camera.main.transform.Translate (dirvect * Time.deltaTime);
		}
		if (thalmicMyo.pose == Pose.WaveOut) {
			Rotate ();
		}
		if (thalmicMyo.pose == Pose.WaveIn) {
			StopRotate ();
		}
		if (thalmicMyo.pose == Pose.DoubleTap) {
			Camera.main.transform.position = this.gameObject.transform.position + ReferenceVec;
		}
	}
}