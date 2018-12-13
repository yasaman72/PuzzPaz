using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Level[] levels;
    public int currentLevelIndex;
    [Header("Game Over")]
    public GameObject gameOverWin;
    public GameObject gameOverLose;
    public GameObject finalGameOverObj;
    public Image[] gameOverStars;
    public Sprite gainedStar, emptyStar;
    public PersianText overRewardTxt, levelCountTxt;
    [Space]
    public GameObject blockerObj;
    public GameObject inGame, GameMenu;
    public Slider levelGoalSlider;
    public GameData gameData;
    public OrderHandler orderHandler;
    public GameBoardManager gameBoardManager;
    public InGameManager inGameManager;
    public Image[] goalStars;
    public Sprite filledStar, EmptyStar;

    public Slider moodSlider;
    [Tooltip("rewardAmount += (DishRewardAmount / sadCustomerPenalty)")]
    public int sadCustomerPenalty;
    [Space]
    public PersianText moveAmountText;
    public PersianText coinAmountText;
    private int usedMoves;
    private int levelRewardAmount;
    private int[] levelCoinGoals = { 0, 0, 0 };
    private bool wonTheLevel;
    [Space, Header("Effects")]
    public GameObject rewardGameObj;
    public PersianText rewardAmountTxt;
    public ParticleSystem rewardParticleSystem;
    [Space, Header("Continue")]
    public int moreMovesAmount;
    public int continuePrice;
    public PersianText continuePriceTxt;

    public static bool finishedTheCurrentLevel;

    public GameObject notEnoughHeartPopup;

    public AudioSource levelSelectionAudioSource;

    public void RestartLevel()
    {
        if (PlayerPrefs.GetInt("ActiveHearts") < 1)
        {
            finalGameOverObj.SetActive(false);
            SetLevel(currentLevelIndex);
        }
        else
        {
            notEnoughHeartPopup.SetActive(true);
            Debug.Log("Not enough hearts!!");
            return;
        }
    }

    private bool hasUsedContinue;

    public void SetLevel(int levelIndex)
    {
        if (gameData.levelDatas[levelIndex].lvlState < 0)
        {
            Debug.Log("This level is not available!!");
            inGameManager.ShowMessageBox("هنوز این مرحله رو باز نکردید و نمی تونید بازیش کنید.");
            return;
        }
        else if (gameData.levelDatas[levelIndex].lvlState > 0)
        {
            if (gameData.levelDatas[levelIndex].starsAmount == 3)
            {
                Debug.Log("You have finished the level.");
                inGameManager.ShowMessageBox("این مرحله رو سه ستاره و تموم کرده اید.");
                return;
            }
        }

        if (PlayerPrefs.GetInt("ActiveHearts") < 1)
        {
            notEnoughHeartPopup.SetActive(true);
            Debug.Log("Not enough hearts!!");
            return;
        }

        levelSelectionAudioSource.Play();

        hasUsedContinue = false;
        finishedTheCurrentLevel = false;

        rewardGameObj.SetActive(false);

        inGame.SetActive(true);
        GameMenu.SetActive(false);

        continuePriceTxt._rawText = continuePrice.ToString();
        continuePriceTxt.enabled = false;
        continuePriceTxt.enabled = true;

        currentLevelIndex = levelIndex;
        moveAmountText._rawText = levels[currentLevelIndex].moves.ToString();
        moveAmountText.enabled = false;
        moveAmountText.enabled = true;
        gameOverWin.SetActive(false);
        gameOverLose.SetActive(false);

        coinAmountText._rawText = "0";
        coinAmountText.enabled = false;
        coinAmountText.enabled = true;

        levelCoinGoals[0] = levels[currentLevelIndex].GetLevelGoals(0);
        //Debug.Log("goal 1: " + levelCoinGoals[0]);
        levelCoinGoals[1] = levels[currentLevelIndex].GetLevelGoals(1);
        //Debug.Log("goal 2: " + levelCoinGoals[1]);
        levelCoinGoals[2] = levels[currentLevelIndex].GetLevelGoals(2);
        //Debug.Log("goal 3: " + levelCoinGoals[2]);

        foreach (Image star in goalStars)
        {
            star.sprite = EmptyStar;
        }

        levelGoalSlider.value = 0;
        levelRewardAmount = 0;

        usedMoves = 0;
        wonTheLevel = false;

        gameBoardManager.NewBoard();
        orderHandler.StartTheDay();
    }

    public void MadeAMove()
    {
        usedMoves++;
        CheckAndChangeLevelState();
    }

    public bool GetLevelState()
    {
        return finishedTheCurrentLevel;
    }
    public void SetLevelState(bool levelState)
    {
        finishedTheCurrentLevel = levelState;
    }

    public void FinishedADish(int DishRewardAmount)
    {
        //Debug.Log("Reward Amount before:" + DishRewardAmount);
        levelRewardAmount += Mathf.FloorToInt(DishRewardAmount * (moodSlider.value + 0.3f));
        // Debug.Log("Order Reward Amount:" + Mathf.FloorToInt(DishRewardAmount * (moodSlider.value + 0.3f)));
        // Debug.Log("Reward Amount after:" + levelRewardAmount);
        rewardGameObj.SetActive(true);
        rewardAmountTxt._rawText = (Mathf.FloorToInt(DishRewardAmount * (moodSlider.value + 0.3f))).ToString();
        rewardAmountTxt.enabled = false;
        rewardAmountTxt.enabled = true;
        rewardParticleSystem.Play();

        coinAmountText._rawText = levelRewardAmount.ToString();
        coinAmountText.enabled = false;
        coinAmountText.enabled = true;

        levelGoalSlider.value = (float)levelRewardAmount / (float)levelCoinGoals[2];

        //setting star shapes if coin amount reach them
        if (levelGoalSlider.value >= 1)
            goalStars[2].sprite = filledStar;
        if (levelGoalSlider.value >= 0.8)
            goalStars[1].sprite = filledStar;
        if (levelGoalSlider.value >= 0.5)
        {
            goalStars[0].sprite = filledStar;
            wonTheLevel = true;
        }

    }

    public void CheckAndChangeLevelState()
    {
        moveAmountText._rawText = (levels[currentLevelIndex].moves - usedMoves).ToString();
        moveAmountText.enabled = false;
        moveAmountText.enabled = true;
        //Debug.Log("Used moves: " + usedMoves);
        //Debug.Log("Remained moves: " + (levels[currentLevelIndex].moves - usedMoves));

        if (levels[currentLevelIndex].moves <= usedMoves && !finishedTheCurrentLevel)
        {
            blockerObj.SetActive(true);
            Debug.Log("Used all the moves");
            FinishedLevel();
            //Invoke("FinishedLevel", 1f);
        }
    }

    public void FinishedLevel()
    {
        blockerObj.SetActive(false);

        //checking game result
        if (wonTheLevel)
        {
            gameOverWin.SetActive(true);

            int gainedStars;
            if (levelGoalSlider.value >= 1)
                gainedStars = 3;
            else if (levelGoalSlider.value >= 0.8)
                gainedStars = 2;
            else
                gainedStars = 1;

            for (int i = 0; i < 3; i++)
                gameOverStars[i].sprite = emptyStar;
            for (int i = 0; i < gainedStars; i++)
                gameOverStars[i].sprite = gainedStar;

            overRewardTxt._rawText = levelRewardAmount.ToString();
            overRewardTxt.enabled = false;
            overRewardTxt.enabled = true;

            levelCountTxt._rawText = "مرحله " + (currentLevelIndex + 1).ToString();
            levelCountTxt.enabled = false;
            levelCountTxt.enabled = true;

            gameData.SetUpNewPlayerLevelData(currentLevelIndex, gainedStars, levelRewardAmount);

            //Game Analytics progression data update
            GameAnalyticsManager.Instance.SendProgressionEvent(2, "world0", "level" + (currentLevelIndex + 1));

            //adding level reward coins
            inGameManager.AddCurrency(levelRewardAmount, 0);
            GameAnalyticsManager.Instance.SendResourceEvent(1, "Coin", levelRewardAmount, "LevelReward", "LevelCoinReward");

        }
        else
        {

            inGameManager.ChangeHeartAmount(-1);

            if (!hasUsedContinue)
            {
                GameAnalyticsManager.Instance.SendDesignEvents("InGame:Continue:didn'tContinue");

                gameOverLose.SetActive(true);
                hasUsedContinue = true;
            }
            else
            {
                GameAnalyticsManager.Instance.SendDesignEvents("InGame:Continue:Continued");

                finalGameOverObj.SetActive(true);
            }
            //Game Analytics progression data update
           GameAnalyticsManager.Instance.SendProgressionEvent(3, "world 0", "level " + (currentLevelIndex + 1));
        }
    }

    public void ContinuePlaying()
    {
        if (PlayerPrefs.GetInt("playerCoins") >= continuePrice)
        {
            GameAnalyticsManager.Instance.SendDesignEvents("InGame:Continue:UsedContinue");

            PlayerPrefs.SetInt("playerCoins", PlayerPrefs.GetInt("playerCoins") - continuePrice);
            PlayerPrefs.Save();
            inGameManager.ChangeHeartAmount(1);

            usedMoves -= moreMovesAmount;
            moveAmountText._rawText = usedMoves.ToString();
            moveAmountText.enabled = false;
            moveAmountText.enabled = true;

            gameOverLose.SetActive(false);

            finishedTheCurrentLevel = false;

            CheckAndChangeLevelState();
            orderHandler.ContinueTheGame();
            //orderHandler.StartTheDay();
        }
        else
        {
            GameAnalyticsManager.Instance.SendDesignEvents("InGame:Continue:NoCoinToContinue");

            Debug.Log("Not enough coin to continue.");
        }
    }
}
