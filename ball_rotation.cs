using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

public class JointOrientation : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;

    // A rotation that compensates for the Myo armband's orientation parallel to the ground, i.e. yaw.
    // Once set, the direction the Myo armband is facing becomes "forward" within the program.
    // Set by making the fingers spread pose or pressing "r".
    private Quaternion _antiYaw = Quaternion.identity;

    // A reference angle representing how the armband is rotated about the wearer's arm, i.e. roll.
    // Set by making the fingers spread pose or pressing "r".
    private float _referenceRoll = 0.0f;

    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    // Update is called once per frame.
    void Update ()
    {
        // Access the ThalmicMyo component attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

        // Update references when the pose becomes fingers spread or the q key is pressed.
        bool updateReference = false;
        if (thalmicMyo.pose != _lastPose) {
            _lastPose = thalmicMyo.pose;

			if (thalmicMyo.pose == Pose.FingerSpread) {
                updateReference = true;
                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
        if (Input.GetKeyDown ("r")) {
            updateReference = true;
        }
        if (updateReference) {
            // _antiYaw represents a rotation of the Myo armband about the Y axis (up) which aligns the forward
            // vector of the rotation with Y = 1 when the wearer's arm is pointing in the reference direction.
            _antiYaw = Quaternion.FromToRotation (
                new Vector3 (myo.transform.forward.x, 0, myo.transform.forward.z),
                new Vector3 (0, 1, 0)
              )
              Vector3 referenceZeroRoll = computeZeroRollVector (myo.transform.forward);
            _referenceRoll = rollFromZero (referenceZeroRoll, myo.transform.forward, myo.transform.up);
            }
            bool holdBall = false;
            if (thalmicMyo.pose == Pose.Fist) {
              thalmicMyo.Vibrate (VibrationType.Long)
              holdBall = true;
              ExtendUnlockAndNotifyUserAction(thalmicMyo);
              while (holdBall){
                Vector3 zeroRoll = computeZeroRollVector (myo.transform.forward);
                float roll = rollFromZero (zeroRoll, myo.transform.forward, myo.transform.up);
                // The relative roll is simply how much the current roll has changed relative to the reference roll.
                // adjustAngle simply keeps the resultant value within -180 to 180 degrees.
                float relativeRoll = normalizeAngle (roll - _referenceRoll);
                // antiRoll represents a rotation about the myo Armband's forward axis adjusting for reference roll.
                Quaternion antiRoll = Quaternion.AngleAxis (relativeRoll, myo.transform.forward);
                transform.rotation = _antiYaw * antiRoll * Quaternion.LookRotation (myo.transform.forward);
                if (thalmicMyo.xDirection == Thalmic.Myo.XDirection.TowardWrist) {
                  // Mirror the rotation around the XZ plane in Unity's coordinate system (XY plane in Myo's coordinate
                  // system). This makes the rotation reflect the arm's orientation, rather than that of the Myo armband.
                  transform.rotation = new Quaternion(transform.localRotation.x,
                                               -transform.localRotation.y,
                                               transform.localRotation.z,
                                               -transform.localRotation.w);
              }
              float rollFromZero (Vector3 zeroRoll, Vector3 forward, Vector3 up)
              {
                float cosine = Vector3.Dot (up, zeroRoll);
                Vector3 cp = Vector3.Cross (up, zeroRoll);
                float directionCosine = Vector3.Dot (forward, cp);
                float sign = directionCosine < 0.0f ? 1.0f : -1.0f;
                return sign * Mathf.Rad2Deg * Mathf.Acos (cosine);
              }
              Vector3 computeZeroRollVector (Vector3 forward)
              {
                Vector3 antigravity = Vector3.up;
                Vector3 m = Vector3.Cross (myo.transform.forward, antigravity);
                Vector3 roll = Vector3.Cross (m, myo.transform.forward);
                return roll.normalized;
              }
              float normalizeAngle (float angle)
              {
                if (angle > 180.0f) {
                  return angle - 360.0f;
                }
                if (angle < -180.0f) {
                  return angle + 360.0f;
                }
                return angle;
              }
            }
