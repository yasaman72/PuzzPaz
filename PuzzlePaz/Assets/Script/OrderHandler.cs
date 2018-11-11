using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OrderHandler : MonoBehaviour
{
    public GameObject blockingObj;
    public LevelManager levelManager;
    [Space]
    public Image dishImage;
    public GameObject requirementsObj;
    public GameObject customerObj;
    public PersianText dishesText;
    public Sprite[] customerImages;
    public Transform requirementsHolder;
    public Text orderNameText;
    public Animator customerAnimator, bubbleAnimator;
    [Space, Header("Customer Mood")]
    public Slider moodSlider;
    public Image sliderFill;
    public Color superHappy, happy;
    public float dishComplishmentModePlus;
    public float ModeMinus;
    [Space]
    public List<RequirementsC> requirements;
    [HideInInspector]
    public Dish currentDish;

    private int dishIndex;
    private int orderIndex;

    private Order[] orders;
    private int customerImageInt;
    private int finishedIngredients;
    private List<int> ordersHolder = new List<int>();
    private int ordersCounter;

    private List<int> collectedIngredientsIndex = new List<int>();

    public void StartTheDay()
    {
        finishedIngredients = 0;
        dishIndex = 0;
        ordersCounter = 0;
        orders = levelManager.levels[levelManager.currentLevelIndex].ordersList;
        customerImageInt = Random.Range(0, customerImages.Length);
        customerObj.GetComponent<Image>().sprite = customerImages[customerImageInt];

        customerAnimator.SetTrigger("Enter");
        bubbleAnimator.SetTrigger("Enter");

        ordersHolder = new List<int>();
        orderIndex = NextOrderIndex();

        GoToNextDish();
    }


    public void GoToNextOrder()
    {
        ordersCounter++;

        bubbleAnimator.SetTrigger("Exit");
        customerAnimator.SetTrigger("Exit");

        if (ordersCounter >= orders.Length)
        {
            Debug.Log("Finished the level!");
            levelManager.FinishedLevel();           
            return;
        }
        dishIndex = 0;
        orderIndex = NextOrderIndex();

        blockingObj.SetActive(true);
    }

    private int NextCustomerImageIndex()
    {

        if (customerImages.Length == 1) return customerImageInt = 0;

        int myCustomerImageInt = Random.Range(0, customerImages.Length);
        if (myCustomerImageInt == customerImageInt)
        {
            return NextCustomerImageIndex();
        }
        return myCustomerImageInt;
    }

    private int NextOrderIndex()
    {
        int myOrderIndex = Random.Range(0, orders.Length);

        if (ordersHolder.Contains(myOrderIndex))
        {
            return NextOrderIndex();
        }

        ordersHolder.Add(myOrderIndex);
        return myOrderIndex;
    }

    //will  be called when customer goes out of the screen
    public void ShowTheNewCustomer()
    {
        if (ordersCounter >= orders.Length)
        {
            return;
        }

        //Mood slider setup
        moodSlider.value = 1;
        sliderFill.color = superHappy;

        customerImageInt = NextCustomerImageIndex();
        customerObj.GetComponent<Image>().sprite = customerImages[customerImageInt];

        customerAnimator.Rebind();
        customerAnimator.SetTrigger("Enter");
        bubbleAnimator.SetTrigger("Enter");

        GoToNextDish();
    }

    public void DisableBlocking()
    {
        blockingObj.SetActive(false);
    }

    public void GoToNextDish()
    {
        //check if there are still any dish in the order
        if (orders[orderIndex].dishes.Length > dishIndex)
        {
            moodSlider.value += dishComplishmentModePlus;
            currentDish = orders[orderIndex].dishes[dishIndex];

            Debug.Log("current level: " + levelManager.levels[levelManager.currentLevelIndex].levelName +
            " | current order: " + orders[orderIndex].name +
            " | current dish: " + currentDish.name);

            if (orders[orderIndex].dishes.Length > (dishIndex + 1))
                dishesText._rawText = "و " + (orders[orderIndex].dishes.Length - 1) + " سفارش دیگر...";
            if (orders[orderIndex].dishes.Length - 1 == dishIndex)
                dishesText._rawText = ("");

            //Debug.Log("Now in dish: " + orders[orderIndex].dishes[dishIndex].name);
            dishIndex++;
        }
        else
        {
            GoToNextOrder();
            return;
        }

        dishesText.enabled = false;
        dishesText.enabled = true;

        //clear requirements holder
        foreach (Transform child in requirementsHolder)
        {
            Destroy(child.gameObject);
        }

        if (orderNameText != null)
            orderNameText.text = currentDish.name.ToString();

        dishImage.sprite = currentDish.sprite;
        SetRequirements();
    }

    private void SetRequirements()
    {
        requirements = new List<RequirementsC>();

        for (int i = 0; i < currentDish.ingredients.Length; i++)
        {
            requirements.Add(new RequirementsC());
            requirements[i].gameObject = Instantiate(requirementsObj, requirementsHolder);
            requirements[i].amount = currentDish.ingredients[i].Amount;
            requirements[i].sprite = currentDish.ingredients[i].ingredient.sprite;
            requirements[i].SetAppearance();
        }
    }

    //returns true if there was something from current dish
    public bool ChangeRequirementsAmount(int removedAmount, Ingredient clearedIngredient)
    {
        bool madeAnOrderInger = false;

        for (int i = 0; i < currentDish.ingredients.Length; i++)
        {
            if (currentDish.ingredients[i].ingredient.type == clearedIngredient.type)
            {
                requirements[i].amount -= removedAmount;
                if (requirements[i].amount <= 0)
                {
                    if (collectedIngredientsIndex.Contains(i)) break;

                    collectedIngredientsIndex.Add(i);
                    requirements[i].amount = 0;
                    requirements[i].gameObject.SetActive(false);
                    finishedIngredients++;
                    //Debug.Log("You finished an ingredient!!");
                    madeAnOrderInger = true;
                }
                requirements[i].SetAppearance();
                CheckDishFinish();
                madeAnOrderInger = true;
            }
        }

        //      Checks Customer Mood
        moodSlider.value -= ModeMinus;

        if (moodSlider.value <= 0)
        {
            collectedIngredientsIndex.Clear();
            finishedIngredients = 0;
            GoToNextOrder();
        }

        if (moodSlider.value < 0.3)
            sliderFill.color = happy;
        else
            sliderFill.color = superHappy;

        return madeAnOrderInger;
    }

    private void CheckDishFinish()
    {
        if (finishedIngredients >= currentDish.ingredients.Length)
        {
            collectedIngredientsIndex.Clear();
            finishedIngredients = 0;
            //Debug.Log("Congratulations! You finished a dish!!");
            levelManager.FinishedADish(currentDish.rewardAmount);
            GoToNextDish();
        }
    }

    public class RequirementsC
    {
        //[InspectorReadOnly]
        public GameObject gameObject;
        //[InspectorReadOnly]
        public Sprite sprite;
        //[InspectorReadOnly]
        public int amount;
        //[InspectorReadOnly]


        public void SetAppearance()
        {
            gameObject.transform.GetChild(1).GetComponent<Image>().sprite = sprite;
            gameObject.transform.GetChild(2).GetComponent<Text>().text = "X" + amount.ToString();
        }
    }
}
