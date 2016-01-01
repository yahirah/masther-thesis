using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;

// Change the material when certain poses are made with the Myo armband.
// Vibrate the Myo armband when a fist pose is made.
public class LaserController : MonoBehaviour
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

    public AudioClip fistSound;
    public AudioClip waveInSound;
    public AudioClip waveOutSound;
    public AudioClip pitchSound;
    public AudioClip spreadSound;

    private AudioSource source;
    // The pose from the last update. This is used to determine if the pose has changed
    // so that actions are only performed upon making them rather than every frame during
    // which they are active.
    private Pose _lastPose = Pose.Unknown;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }


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
                PerformShoot(fistSound, fistMaterial, laserFist, thalmicMyo);
            }
            else if (thalmicMyo.pose == Pose.WaveIn && GameController.poseOn["in"])
            {
                PerformShoot(waveInSound, waveInMaterial, laserWaveIn, thalmicMyo);

            }
            else if (thalmicMyo.pose == Pose.WaveOut && GameController.poseOn["out"])
            {
                PerformShoot(waveOutSound, waveOutMaterial, laserWaveOut, thalmicMyo);

            }
            else if (thalmicMyo.pose == Pose.DoubleTap && GameController.poseOn["pitch"])
            {
                PerformShoot(pitchSound, doubleTapMaterial, laserPitch, thalmicMyo);

            }
            else if (thalmicMyo.pose == Pose.FingersSpread && GameController.poseOn["spread"])
            {
                PerformShoot(spreadSound, spreadMaterial, laserSpread, thalmicMyo);

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

    void PerformShoot(AudioClip sound, Material material, GameObject laser, ThalmicMyo myo)
    {
        source.PlayOneShot(sound);
        GetComponent<Renderer>().material = material;
        CreateShot(laser);
        ExtendUnlockAndNotifyUserAction(myo);
    }
}
