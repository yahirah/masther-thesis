using UnityEngine;
using System.Collections;

public class ChangeColorByContact : MonoBehaviour {

    public Material naturalColor;

    public Material contactColor;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        rend.sharedMaterial.color = Color.red;
    }

    void OnTriggerExit(Collider other)
    {
        rend.sharedMaterial.color = Color.green;
    }
}
