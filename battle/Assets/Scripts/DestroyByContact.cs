using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DestroyByContact : MonoBehaviour {
    public GameObject explosion;

    public Button retry;
   // public Text score = null;

    public static int count = 0;
    public static int winningValue = 4;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pointer" || (!tag.Contains("Monster") && !other.tag.Contains("Monster")))
        {
            return;
        }

        Instantiate(explosion, transform.position, transform.rotation);
        //Destroy(gameObject.transform.parent.gameObject);

        switch(tag) {
            case "yellowMonster":
                if (other.tag == "yellowLaser")
                {
                    Destroy(gameObject);
                    count++;
                    Destroy(other.gameObject);
                }
                break;
            case "redMonster": 
                if (other.tag == "redLaser")
                {
                    Destroy(gameObject);
                    count++;
                    Destroy(other.gameObject);
                }
                break;
            case "magentaMonster": 
                if (other.tag == "magentaLaser")
                {
                    Destroy(gameObject);
                    count++;
                    Destroy(other.gameObject);
                }
                break;
            case "greenMonster": 
                if (other.tag == "greenLaser")
                {
                    Destroy(gameObject);
                    count++;
                    Destroy(other.gameObject);
                }
                break;
            case "blueMonster": 
                if (other.tag == "blueLaser")
                {
                    Destroy(gameObject);
                    count++;
                    Destroy(other.gameObject);
                }
                break;
        }
        if(other.tag == "Wand" || other.tag == "Wall") 
        {
            GameController.AddFailure();
            Destroy(gameObject);
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
}
