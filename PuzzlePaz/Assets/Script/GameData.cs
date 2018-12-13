using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEditor;

public class GameData : MonoBehaviour
{
    public List<LevelData> levelDatas;
    public LevelManager levelManager;
    public GameMenuManager gameMenuManager;

    //private string dataFilePath = "PlayerData.json";
    //string folderPath;
    //string filePath;

    private void Start()
    {
        //folderPath = (Application.platform == RuntimePlatform.Android ||
        //    Application.platform == RuntimePlatform.IPhonePlayer ?
        //    Application.persistentDataPath :
        //    Application.dataPath) + "/myDataFolder/PlayerData.json";

        //filePath = Path.Combine(Application.persistentDataPath, folderPath);

        //Debug.Log("file Path: " + filePath);

        levelDatas = new List<LevelData>();

        for (int i = 0; i < levelManager.levels.Length; i++)
        {
            LevelData myLevelData = new LevelData();
            myLevelData.LevelIndex = i;
            myLevelData.lvlState = -1;
            levelDatas.Add(myLevelData);
        }

        ////setting up the current active level
        //if (PlayerPrefs.HasKey("CurrentLvl"))
        //{
        //    levelDatas[PlayerPrefs.GetInt("CurrentLvl")].lvlState = 0;
        //}
        //else
        //{
        //    PlayerPrefs.SetInt("CurrentLvl", 0);
        //}
        //int activeLevelIndex = PlayerPrefs.GetInt("CurrentLvl");

        ////set levels with smaller index's state to passed
        //for (int i = 0; i < activeLevelIndex; i++)
        //{
        //    levelDatas[i].lvlState = 1;
        //}

        if (PlayerPrefs.HasKey("dataJSON"))
        {
            Debug.Log("loading the level.");
            LoadGame();
        }
        else
        {
            Debug.Log("start game from beginning.");
            levelDatas[0].lvlState = 0;
            //ResetJSON();
        }

        gameMenuManager.MakeLevelNodes(levelManager.levels.Length);
    }

    //will setup the game based on player saved data
    public void SetInitialLevel(List<GameObject> levelNodes)
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            //check if player has not yet passed the level
            if (levelDatas[i].lvlState < 0)
            {
                //future nodes
                //levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.future;
                levelNodes[i].transform.GetChild(1).gameObject.SetActive(false);
            }
            else if (levelDatas[i].lvlState == 0)
            {
                //current level settings
                //levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.current;
                levelNodes[i].transform.GetChild(1).gameObject.SetActive(true);
                gameMenuManager.SetUpLevelStarsAndShape(i, levelDatas[i].starsAmount, true);
                levelNodes[i].transform.GetChild(2).gameObject.SetActive(false);

            }
            else
            {
                //passed level settings
                //levelNodes[i].transform.GetChild(0).GetComponent<Image>().color = gameMenuManager.passed;
                levelNodes[i].transform.GetChild(1).gameObject.SetActive(false);
                gameMenuManager.SetUpLevelStarsAndShape(i, levelDatas[i].starsAmount, true);

            }
        }
    }

    //will only be called when player passes a level
    public void SetUpNewPlayerLevelData(int levelIndex, int stars, int rewardAmount)
    {
        if (PlayerPrefs.HasKey("CurrentLvl"))
        {
            if (PlayerPrefs.GetInt("CurrentLvl") < levelIndex + 1)
            {
                PlayerPrefs.SetInt("CurrentLvl", levelIndex + 1);
                levelDatas[levelIndex + 1].lvlState = 0;
                Debug.Log("Setuped a new current level: " + levelIndex + 1);
            }
            //Debug.Log("old current level.");
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLvl", 1);
            levelDatas[1].lvlState = 0;
            Debug.Log("Setuped a new current level: " + levelIndex);
        }

        levelDatas[levelIndex].lvlState = 1;
        levelDatas[levelIndex].score = rewardAmount;

        //check if the new star amount is bigger than the current one and then sets it
        if (levelDatas[levelIndex].starsAmount < stars)
            levelDatas[levelIndex].starsAmount = stars;

        gameMenuManager.SetUpLevelStarsAndShape(levelIndex, stars, false);
        PlayerPrefs.Save();
    }

    public void ResetJSON()
    {
        //FileUtil.DeleteFileOrDirectory(filePath);
        //Debug.Log("deleted file at : " + filePath);
        for (int i = 0; i < levelDatas.Count; i++)
        {
            levelDatas[i].lvlState = -1;
            levelDatas[i].score = 0;
            levelDatas[i].starsAmount = 0;

            if (i == 0)
            {
                levelDatas[i].lvlState = 0;
            }
        }
        SaveGame();
    }

    public void SaveGame()
    {
        string json = "";

        //json += "[";
        for (int i = 0; i < levelDatas.Count; i++)
        {
            json += JsonUtility.ToJson(levelDatas[i], false);
            if (i < levelDatas.Count - 1)
                json += '/';
        }
        //json += "]";
        //json = JsonUtility.ToJson(levelDatas, false);

        PlayerPrefs.SetString("dataJSON", json);
        Debug.Log("json content: " + json);
        PlayerPrefs.Save();

        Debug.Log("Saved Data!!!");
        string temp = PlayerPrefs.GetString("dataJSON");
        Debug.Log("JSON content: " + temp);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("dataJSON"))
        {
            string json = PlayerPrefs.GetString("dataJSON");
            string[] jsonChunks = json.Split('/');

            //Debug.Log("json content: " + json);
            //Debug.Log("JSON chunks length: " + jsonChunks.Length);

            for (int i = 0; i < levelDatas.Count; i++)
            {
                //Debug.Log(jsonChunks[i]);
                levelDatas[i] = JsonUtility.FromJson<LevelData>(jsonChunks[i]);
            }
            //levelDatas = new List<LevelData>();
            //levelDatas = JsonUtility.FromJson<List<LevelData>>(json);

            Debug.Log("Loaded Data!!");
            Debug.Log("JSON content: " + PlayerPrefs.GetString("dataJSON"));
        }
        else
        {
            string errorMessage = "levelsData JSON pref doesn't exist!";
            Debug.Log(errorMessage);
            GameAnalyticsManager.Instance.SendErrorEvent(5, errorMessage);
        }
    }
}
