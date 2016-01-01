using UnityEngine;
using System.Collections;

public class PointerController : MonoBehaviour {

    public Material naturalColor;

    public Material contactColor;

    public GameObject examplePointer;
    
    private static Renderer rend;

    void Start()
    {
        
        rend = examplePointer.GetComponent<Renderer>();
        rend.enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.tag.Contains("Monster"))
        {
            return;
        }
        ChangeTo(Color.red);
    }

    static public void ChangeTo(Color color) {
        rend.sharedMaterial.color = color;
    }

    void OnTriggerExit(Collider other)
    {
        ChangeTo(Color.green);
    }
}
