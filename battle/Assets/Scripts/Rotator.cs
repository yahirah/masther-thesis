using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
    public float tumble;
    private Rigidbody rd;

    void Start ()
    {
        rd = GetComponent<Rigidbody>();
        rd.angularVelocity = Random.insideUnitSphere * tumble; 
    }
}
