using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public Level[] levels;
    public int currentLevelIndex;
    public GameObject gameOverObj;
    public OrderHandler orderHandler;
    [Space]
    public PersianText moveAmountText;

    private void Start()
    {
        SetLevel();
    }

    private void SetLevel()
    {
        currentLevelIndex = Random.Range(0, levels.Length);
        moveAmountText._rawText = levels[currentLevelIndex].moves.ToString();
        moveAmountText.enabled = false;
        moveAmountText.enabled = true;

        orderHandler.StartTheDay();
    }

    public void CheckAndChangeLevelState()
    {
        
    }

    public void FinishedLevel()
    {
        gameOverObj.SetActive(true);
    }
}
