using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class InGameManager : MonoBehaviour {

    public GameObject[] activateThese;
    public GameObject[] deactiveThese;

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


}
