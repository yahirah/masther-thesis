using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
    public bool testwin = false;
    public GameObject cube;

    public Button newLevel;
    public Button winMessage;

    public Vector3 spawnValues;
    public float yShift;

    public int monsterCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GameObject ins;
    public GameObject outs;
    public GameObject fists;
    public GameObject pitchs;
    public GameObject spreads;


    public static Text countText;
    static public bool win = false;
    static public Text failureCount;
    static private int failures = 0;

    private int level = 1;

    //the order is important for levels order
    public static string[] colors = { "blue", "magenta", "green", "yellow", "red" };
    private Dictionary<string, Color> colorDict = new Dictionary<string, Color>();

    public static int[] rocks = { 5, 10, 15, 20, 25 };
    public static Dictionary<string, bool> poseOn = new Dictionary<string, bool>();
    private static string[] levelNames = { "wave in", "spread fingers", "wave out", "double tap", "fist" };

    void Start()
    {
        failureCount = GameObject.Find("Fail Text").GetComponent<Text>();
        colorDict["red"] = Color.red;
        colorDict["green"] = Color.green;
        colorDict["blue"] = Color.blue;
        colorDict["yellow"] = Color.yellow;
        colorDict["magenta"] = Color.magenta;
        
        InitializePositive();
    }
    void InitializePositive()
    {

        ins.SetActive(true);
        outs.SetActive(true);
        fists.SetActive(true);
        spreads.SetActive(true);
        pitchs.SetActive(true);

        poseOn["fist"] = true;
        poseOn["spread"] = true;
        poseOn["in"] = true;
        poseOn["out"] = true;
        poseOn["pitch"] = true;
    }
    void InitializeNegative() 
    {

        ins.SetActive(false);
        outs.SetActive(false);
        fists.SetActive(false);
        spreads.SetActive(false);
        pitchs.SetActive(false);

        poseOn["fist"] = false;
        poseOn["spread"] = false;
        poseOn["in"] = false;
        poseOn["out"] = false;
        poseOn["pitch"] = false;
    }
    void Update() {
        if (testwin) { win = true; testwin = false; }
        if (win) {

            win = false;
            StopAllCoroutines();
            Transform monsterParent = GameObject.FindGameObjectWithTag("Monster").transform;

            foreach (Transform child in monsterParent)
            {
                Destroy(child.gameObject);
            }
            if (level == 5)
            {
                Start();
                level = 1;
                winMessage.GetComponent<Button>().onClick.RemoveAllListeners();
                winMessage.gameObject.SetActive(true);
                winMessage.GetComponentInChildren<Text>().text = "You won the whole game, good job! Do you feel like trying one more time?";
                UnityEngine.Events.UnityAction action2 = () =>
                {
                    StartFirst();
                    winMessage.gameObject.SetActive(false);
                };
                winMessage.GetComponent<Button>().onClick.AddListener(action2);
            }
            else
            {
                
                DestroyByContact.winningValue = rocks[level];
                newLevel.GetComponent<Button>().onClick.RemoveAllListeners();
                newLevel.gameObject.SetActive(true);
                newLevel.GetComponentInChildren<Text>().text = "You won level " + level.ToString() + "! \n Now you will have acces to additional pose: " + levelNames[level] +
                ".\n Destroy " + DestroyByContact.winningValue.ToString() + " rocks to advance to next level. \n So, are you ready?";
                UnityEngine.Events.UnityAction action = () => { StartSpawning(level); newLevel.gameObject.SetActive(false); };
                newLevel.GetComponent<Button>().onClick.AddListener(action);
                level++;
            }
        }
    }
    public void StartFirst() {
        InitializeNegative();
        DestroyByContact.winningValue = rocks[0];
        StartSpawning(1);
    }

    void StartSpawning(int level)
    {
        Debug.Log("Im spawning" + level.ToString());
        failures = 0;
        DisplayFailures();
        DestroyByContact.count = 0;
        DestroyByContact.displayCount();

        switch (level)
        {
            case 1: 
                poseOn["in"] = true; 
                ins.SetActive(true);
                break;
            case 2: 
                poseOn["spread"] = true; 
                spreads.SetActive(true);
                break;
            case 3: 
                poseOn["out"] = true;
                outs.SetActive(true);
                break;
            case 4: 
                poseOn["pitch"] = true;
                pitchs.SetActive(true);
                break;
            case 5: 
                poseOn["fist"] = true;
                fists.SetActive(true);
                break;
        }

        StartCoroutine(SpawnWaves());
    }


    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < monsterCount; i++)
            {
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), Random.Range(-spawnValues.y + yShift, spawnValues.y + yShift), spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                GameObject duplicate = (GameObject) Instantiate(cube, spawnPosition, spawnRotation);

                int rand = Random.Range(0, level);
                duplicate.gameObject.transform.GetChild(0).tag = colors[rand] + "Monster";
                
                MeshRenderer r = duplicate.transform.GetChild(0).GetComponent<MeshRenderer>();
                r.material.color = colorDict[colors[rand]];

                duplicate.transform.parent = GameObject.FindGameObjectWithTag("Monster").transform;
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    static public void AddFailure()
    {
        failures++;
        failureCount.text = "Failures: " + failures.ToString();
    }

    private void DisplayFailures() {
        failureCount.text = "Failures: " + failures.ToString();
    }


}
