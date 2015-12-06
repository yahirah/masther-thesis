using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

// Change the material when certain poses are made with the Myo armband.
// Vibrate the Myo armband when a fist pose is made.
public class ColorBoxByPose : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    public GameObject laserWaveIn;
    public GameObject laserWaveOut;
    public GameObject laserFist;
    public GameObject laserSpread;
    public GameObject laserPitch;

    public float speed;
    // Materials to change to when poses are made.
    public Material waveInMaterial;
    public Material waveOutMaterial;
    public Material doubleTapMaterial;
    public Material fistMaterial;
    public Material spreadMaterial;


    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    // Update is called once per frame.
    void Update ()
    {
        // Access the ThalmicMyo component attached to the Myo game object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();

        // Check if the pose has changed since last update.
        // The ThalmicMyo component of a Myo game object has a pose property that is set to the
        // currently detected pose (e.g. Pose.Fist for the user making a fist). If no pose is currently
        // detected, pose will be set to Pose.Rest. If pose detection is unavailable, e.g. because Myo
        // is not on a user's arm, pose will be set to Pose.Unknown.
        if (thalmicMyo.pose != _lastPose) {
            _lastPose = thalmicMyo.pose;

            // Vibrate the Myo armband when a fist is made.
            if (thalmicMyo.pose == Pose.Fist && GameController.poseOn["fist"]) {
                GetComponent<Renderer>().material = fistMaterial;
                CreateShot(laserFist);
                ExtendUnlockAndNotifyUserAction (thalmicMyo);
            // Change material when wave in, wave out or double tap poses are made.
            }
            else if (thalmicMyo.pose == Pose.WaveIn && GameController.poseOn["in"])
            {
                GetComponent<Renderer>().material = waveInMaterial;
                CreateShot(laserWaveIn);
                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveOut && GameController.poseOn["out"])
            {
                GetComponent<Renderer>().material = waveOutMaterial;
                CreateShot(laserWaveOut);
                ExtendUnlockAndNotifyUserAction (thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.DoubleTap && GameController.poseOn["pitch"])
            {
                GetComponent<Renderer>().material = doubleTapMaterial;
                CreateShot(laserPitch);
                ExtendUnlockAndNotifyUserAction (thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.FingersSpread && GameController.poseOn["spread"])
            {
                GetComponent<Renderer>().material = spreadMaterial;
                CreateShot(laserSpread);
                ExtendUnlockAndNotifyUserAction(thalmicMyo);
            }
        }
    }

    // Extend the unlock if ThalmcHub's locking policy is standard, and notifies the given myo that a user action was
    // recognized.
    void ExtendUnlockAndNotifyUserAction (ThalmicMyo myo)
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (hub.lockingPolicy == LockingPolicy.Standard) {
            myo.Unlock (UnlockType.Timed);
        }

        myo.NotifyUserAction ();
    }

    void CreateShot(GameObject laserType)
    {
        Vector3 move = Vector3.MoveTowards(new Vector3(0, 0, 0), transform.position, 0.1f);
        Quaternion rotate = transform.rotation;
        rotate *= Quaternion.Euler(0, 0, 0);
        GameObject clone = Instantiate(laserType, transform.position + new Vector3(0.1f, 0.1f, 0.1f), rotate) as GameObject;
        clone.GetComponent<Rigidbody>().AddForce(move * speed);
    }
}
