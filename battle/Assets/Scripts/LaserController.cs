using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;


public class LaserController : MonoBehaviour {

   
    private Rigidbody rb;
    private Vector3 move;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
    void Update ()
    {
       
    }

	
}
