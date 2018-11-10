using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Level[] levels;
    public int currentLevelIndex;
    public GameObject gameOverObj;
    public GameObject blockerObj;
    public GameObject inGame, GameMenu;
    public Slider levelGoalSlider;
    public OrderHandler orderHandler;
    public GameBoardManager gameBoardManager;
    public Image[] goalStars;
    public Sprite filledStar, EmptyStar;
    public PersianText gameOverTxt;
    public Slider moodSlider;
    [Tooltip("rewardAmount += (DishRewardAmount / sadCustomerPenalty)")]
    public int sadCustomerPenalty;
    [Space]
    public PersianText moveAmountText;
    public PersianText coinAmountText;
    private int usedMoves;
    private int rewardAmount;
    private int[] levelCoinGoals = { 0, 0, 0 };
    private bool wonTheLevel;

    public void SetLevel(int levelIndex)
    {
        inGame.SetActive(true);
        GameMenu.SetActive(false);

        currentLevelIndex = levelIndex;
        moveAmountText._rawText = levels[currentLevelIndex].moves.ToString();
        moveAmountText.enabled = false;
        moveAmountText.enabled = true;
        gameOverObj.SetActive(false);

        coinAmountText._rawText = "0";
        coinAmountText.enabled = false;
        coinAmountText.enabled = true;

        levelCoinGoals[0] = levels[currentLevelIndex].GetLevelGoals(0);
        Debug.Log("goal 1: " + levelCoinGoals[0]);
        levelCoinGoals[1] = levels[currentLevelIndex].GetLevelGoals(1);
        Debug.Log("goal 2: " + levelCoinGoals[1]);
        levelCoinGoals[2] = levels[currentLevelIndex].GetLevelGoals(2);
        Debug.Log("goal 3: " + levelCoinGoals[2]);

        foreach (Image star in goalStars)
        {
            star.sprite = EmptyStar;
        }

        levelGoalSlider.value = 0;
        rewardAmount = 0;

        usedMoves = 0;
        wonTheLevel = false;

        gameBoardManager.NewBoard();
        orderHandler.StartTheDay();
    }

    public void MadeAMove(int moves)
    {
        usedMoves = moves;
        CheckAndChangeLevelState();
    }

    public void FinishedADish(int DishRewardAmount)
    {
        //check customer mood and add reward
        if (moodSlider.value >= 0.3)
            rewardAmount += DishRewardAmount;
        else
        {
            rewardAmount += Mathf.CeilToInt(DishRewardAmount / sadCustomerPenalty);
        }
        coinAmountText._rawText = rewardAmount.ToString();
        coinAmountText.enabled = false;
        coinAmountText.enabled = true;


        levelGoalSlider.value = (float)rewardAmount / (float)levelCoinGoals[2];

        //setting star shapes if coin amount reach them
        if (levelGoalSlider.value >= 0.5)
        {
            goalStars[0].sprite = filledStar;
            wonTheLevel = true;
        }
        if (levelGoalSlider.value >= 0.8)
            goalStars[1].sprite = filledStar;
        if (levelGoalSlider.value >= 1)
            goalStars[2].sprite = filledStar;

    }

    public void CheckAndChangeLevelState()
    {
        moveAmountText._rawText = (levels[currentLevelIndex].moves - usedMoves).ToString();
        moveAmountText.enabled = false;
        moveAmountText.enabled = true;

        if (levels[currentLevelIndex].moves <= usedMoves)
        {
            blockerObj.SetActive(true);
            Debug.Log("Used all the moves");
            Invoke("FinishedLevel", 1f);
        }
    }

    //takes true for a success and false for failure
    public void FinishedLevel()
    {
        blockerObj.SetActive(false);
        gameOverObj.SetActive(true);

        //checking game result
        if (wonTheLevel)
        {
            gameOverTxt._rawText = "ایول بردی!";
        }
        else
        {
            gameOverTxt._rawText = "باختی جانم!";
        }
        gameOverTxt.enabled = false;
        gameOverTxt.enabled = true;
    }
}
