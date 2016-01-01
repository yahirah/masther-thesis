using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestroyByContact : MonoBehaviour {
    public GameObject explosion;
    public AudioClip explosionSound;

    public static int count = 0;
    public static int winningValue = 4;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gate" || other.tag == "Pointer" || (!tag.Contains("Monster") && !other.tag.Contains("Monster")))
        {
            return;
        }
        //explosion start if enters a gate or is shot
        Instantiate(explosion, transform.position, transform.rotation);
        //Destroy(gameObject.transform.parent.gameObject);

        switch(tag) {
            case "yellowMonster":
                if (other.tag == "yellowLaser")
                {
                    DestroyReaction(other.gameObject, "yellow");
                }
                break;
            case "redMonster": 
                if (other.tag == "redLaser")
                {
                    DestroyReaction(other.gameObject, "red");
                }
                break;
            case "magentaMonster": 
                if (other.tag == "magentaLaser")
                {
                    DestroyReaction(other.gameObject, "magenta");
                }
                break;
            case "greenMonster": 
                if (other.tag == "greenLaser")
                {
                    DestroyReaction(other.gameObject, "green");
                }
                break;
            case "blueMonster": 
                if (other.tag == "blueLaser")
                {
                    DestroyReaction(other.gameObject, "blue");
                }
                break;
        }
       
        GameObject.Find("Count Text").GetComponent<Text>().text = "Your score: " + count.ToString();
        if (count >= winningValue)
        {

            GameController.win = true;            
        }
    }

    internal static void displayCount()
    {
        GameObject.Find("Count Text").GetComponent<Text>().text = "Your score: " + count.ToString();
    }

    void DestroyReaction(GameObject other, string color)
    {
        GameController.PlayDestruction(explosionSound);
        GameController.AddToStatistics(color);
        count++;
        Destroy(other);
        PointerController.ChangeTo(Color.green);
        Destroy(transform.parent.gameObject);
    }
}
