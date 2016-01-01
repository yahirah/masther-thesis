using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class GameController : MonoBehaviour {
    public bool testwin = false;
    public GameObject cube;

    public Button winMessage;
    public Button newLevel;
    public GameObject mothership;
    
    public int monsterCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GameObject ins;
    public GameObject outs;
    public GameObject fists;
    public GameObject pitchs;
    public GameObject spreads;

    public AudioClip levelEnd;
    public AudioClip gameEnd;

    public GameObject finalReady;

    public InputField nameField;
    public InputField ageField;
    public InputField trialField;

    public static Text countText;
    static public bool win = false;
    static public Text failureCount;
    static private int failures = 0;

    private int level = 1;
    private List<GameObject> engines = new List<GameObject>();
    private string fileName;


    
    //the order is important for levels order
    public static string[] colors = { "blue", "magenta", "green", "yellow", "red" };
    private Dictionary<string, Color> colorDict = new Dictionary<string, Color>();
    static private Dictionary<string, int> statisticsSuccesses = new Dictionary<string, int>();
    static private Dictionary<string, int> statisticsFailures = new Dictionary<string, int>();
    static private Dictionary<int, Vector4> levelDifficulties = new Dictionary<int, Vector4>();

    public static int[] rocks = { 5, 10, 15, 20, 25 };
    public static Dictionary<string, bool> poseOn = new Dictionary<string, bool>();
    private static string[] levelNames = { "wave in", "spread fingers", "wave out", "double tap", "fist" };
    private static AudioSource source;

    private string childName;
    private string age;
    private string trial;
    void Start()
    {
        source = GetComponent<AudioSource>();
        failureCount = GameObject.Find("Fail Text").GetComponent<Text>();
        finalReady.SetActive(false);
    }

    public void CollectNames()
    {
        childName = nameField.text;
        age = ageField.text;
        trial = trialField.text;
        Initialize();
        finalReady.SetActive(true);
        nameField.gameObject.SetActive(false);
        ageField.gameObject.SetActive(false);
        trialField.gameObject.SetActive(false);

    }
    void Initialize()
    {
       
        colorDict["red"] = Color.red;
        colorDict["green"] = Color.green;
        colorDict["blue"] = Color.blue;
        colorDict["yellow"] = Color.yellow;
        colorDict["magenta"] = Color.magenta;
        
        statisticsSuccesses["red"] = 0;
        statisticsSuccesses["green"] = 0;
        statisticsSuccesses["blue"] = 0;
        statisticsSuccesses["yellow"] = 0;
        statisticsSuccesses["magenta"] = 0;

        statisticsFailures["red"] = 0;
        statisticsFailures["green"] = 0;
        statisticsFailures["blue"] = 0;
        statisticsFailures["yellow"] = 0;
        statisticsFailures["magenta"] = 0;

        //order: monster count, spawn wait, start wait, wave wait
        levelDifficulties[1] = new Vector4(5,5,2,5);
        levelDifficulties[2] = new Vector4(10,4,2,5);
        levelDifficulties[3] = new Vector4(15,3,2,5);
        levelDifficulties[4] = new Vector4(20,2,2,5);
        levelDifficulties[5] = new Vector4(25,1,2,5);
        
        InitializePositive();
        InitializeEngines();
    }

    private void InitializeEngines()
    {
        int max = mothership.transform.childCount;
        for (int k = 0; k < max; k++)
        {
            if (mothership.transform.GetChild(k).tag == "Gate")
            {
                engines.Add(mothership.transform.GetChild(k).gameObject);
            }
        }
        int i = 0;
        max = engines.Count;
        while(i < max)
        {
            int localMax = engines[i].transform.childCount;
            int j = 0;
            for(; j < localMax; j++) {
                if (engines[i].transform.GetChild(j).tag == "Gate") {
                    engines.Add(engines[i].transform.GetChild(j).gameObject);
                }
            }
            max = engines.Count;
            i++;
        }
        Debug.Log(engines.Count);
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
                SaveStatistics();
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
                source.PlayOneShot(gameEnd);
            }
            else
            {
                SaveStatistics();
                DestroyByContact.winningValue = rocks[level];
                newLevel.GetComponent<Button>().onClick.RemoveAllListeners();
                newLevel.gameObject.SetActive(true);
                newLevel.GetComponentInChildren<Text>().text = "You won level " + level.ToString() + "! \n Now you will have acces to additional pose: " + levelNames[level] +
                ".\n Destroy " + DestroyByContact.winningValue.ToString() + " rocks to advance to next level. \n So, are you ready?";
                UnityEngine.Events.UnityAction action = () => { StartSpawning(level); newLevel.gameObject.SetActive(false); };
                newLevel.GetComponent<Button>().onClick.AddListener(action);
                source.PlayOneShot(levelEnd);
                level++;
            }
        }
    }
    public void StartFirst() {
        InitializeNegative();
        DestroyByContact.winningValue = rocks[0];
        StartSpawning(1);
        InitializeFile();
    }

    private void InitializeFile()
    {
        string path = Application.dataPath;
        Debug.Log(Application.persistentDataPath);
        string title = childName + "_" + age + "_" + trial;
        fileName = path + "/" + title + ".txt";
        Debug.Log(fileName);
        File.WriteAllText(fileName, title + "_" + DateTime.Now.ToString() + "\n");

    }

    void StartSpawning(int level)
    {
        failures = 0;
        DisplayFailures();
        DestroyByContact.count = 0;
        DestroyByContact.displayCount();
        monsterCount = (int) levelDifficulties[level].x;
        spawnWait = levelDifficulties[level].y;
        startWait = levelDifficulties[level].z;
        waveWait = levelDifficulties[level].w;
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
                int engine = UnityEngine.Random.Range(0, engines.Count);
                
                Vector3 spawnPosition = engines[engine].transform.position;
                Quaternion spawnRotation = Quaternion.identity;
                GameObject duplicate = (GameObject) Instantiate(cube, spawnPosition, spawnRotation);
                
                int rand = UnityEngine.Random.Range(0, level);
                duplicate.gameObject.transform.GetChild(0).tag = colors[rand] + "Monster";
                duplicate.tag = colors[rand] + "Monster";
                
                MeshRenderer r = duplicate.transform.GetChild(0).GetComponent<MeshRenderer>();
                r.material.color = colorDict[colors[rand]];

                duplicate.transform.parent = GameObject.FindGameObjectWithTag("Monster").transform;
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
        }
    }

    static public void AddFailure(string tag)
    {
        failures++;
        failureCount.text = "Failures: " + failures.ToString();
        foreach (string key in statisticsFailures.Keys)
        {
            if (tag.Contains(key))
            {
                statisticsFailures[key] += 1;
                break;
            }
        }
    }

    private void DisplayFailures() {
        failureCount.text = "Failures: " + failures.ToString();
    }

    static public void PlayDestruction(AudioClip explosion)
    {
        source.PlayOneShot(explosion);
    }


    internal static void AddToStatistics(string color)
    {
        statisticsSuccesses[color] += 1;
    }

    private void SaveStatistics()
    {
        using (StreamWriter sw = File.AppendText(fileName))
        {
            sw.WriteLine("Level " + level.ToString());
            foreach (string color in colors)
            {
                sw.WriteLine(color + ": S" + statisticsSuccesses[color] + "F" + statisticsFailures[color]);
                statisticsSuccesses[color] = 0;
                statisticsFailures[color] = 0;
            }
        }	
    }
}
