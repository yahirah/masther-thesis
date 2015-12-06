using UnityEngine;
using System.Collections;

// Draw simple instructions for sample scene.
// Check to see if a Myo armband is paired.
public class SampleSceneGUI : MonoBehaviour
{
    // Myo game object to connect with.
    // This object must have a ThalmicMyo script attached.
    public GameObject myo = null;
    public GameObject myo2 = null;

    // Draw some basic instructions.
    void OnGUI ()
    {
        GUI.skin.label.fontSize = 20;

        ThalmicHub hub = ThalmicHub.instance;

        // Access the ThalmicMyo script attached to the Myo object.
        ThalmicMyo thalmicMyo = myo.GetComponent<ThalmicMyo> ();
        ThalmicMyo thalmicMyo2 = myo2.GetComponent<ThalmicMyo>();

        if (!hub.hubInitialized) {
            GUI.Label(new Rect (12, 8, Screen.width, Screen.height),
                "Cannot contact Myo Connect. Is Myo Connect running?\n" +
                "Press Q to try again."
            );
        } else if (!thalmicMyo.isPaired || !thalmicMyo2.isPaired) {
            GUI.Label(new Rect (12, 8, Screen.width, Screen.height),
                "Two Myos must be paired."
            );
        } else if (!thalmicMyo.armSynced || !thalmicMyo2.armSynced) {
            GUI.Label(new Rect (12, 8, Screen.width, Screen.height),
                "Perform the Sync Gesture."
            );
        } else {
            GUI.Label (new Rect (12, 8, Screen.width, Screen.height),
                "Use different gestures for different lasers: \n" +
                "Fist, Wave in, Wave out, Double tap, Fingers spread.\n" + 
                "Press R to point the Wand forward."
            ); 
        }
    }

    void Update ()
    {
        ThalmicHub hub = ThalmicHub.instance;

        if (Input.GetKeyDown ("q")) {
            hub.ResetHub();
        }
    }
}
