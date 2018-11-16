using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameManager : MonoBehaviour {

    public GameObject[] activateThese;
    public GameObject[] deactiveThese;
    public GameData gameData;

    private void Start()
    {       

        foreach(GameObject activateObj in activateThese)
        {
            activateObj.SetActive(true);
        }

        foreach (GameObject deactivateObj in deactiveThese)
        {
            deactivateObj.SetActive(false);
        }

    }

    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        gameData.SaveGame();
    }
    private void OnApplicationPause(bool pause)
    {
        gameData.SaveGame();
    }

    public void ClearPlayerData()
    {
        PlayerPrefs.DeleteAll();
    }
}
