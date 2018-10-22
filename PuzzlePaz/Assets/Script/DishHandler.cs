using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishHandler : MonoBehaviour
{
    public Dish[] dishes;
    [Space]
    public Image dishImage;
    public GameObject requirementsObj;
    public Transform requirementsHolder;
    public Text orderNameText;
    [Space]
    public List<RequirementsC> requirements;
    [Space]
    public Dish currentDish;

    //private void Start()
    //{
    //    currentDish = dishes[Random.Range(0, dishes.Length)];
    //    dishImage.sprite = currentDish.sprite;
    //    orderNameText.text = currentDish.name.ToString();
    //    SetRequirements();
    //}

    public void GoToNextDish(int dishIndex)
    {
        currentDish = dishes[dishIndex];

        //clear requirements holder
        foreach (Transform child in requirementsHolder)
        {
            Destroy(child.gameObject);
        }

        orderNameText.text = currentDish.name.ToString();
        dishImage.sprite = currentDish.sprite;
        SetRequirements();
    }

    public void GoToRandomDish()
    {
        GoToNextDish(Random.Range(0, dishes.Length));
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

    public void ChangeRequirementsAmount(int removedAmount, Ingredient clearedIngredient)
    {
        for (int i = 0; i < currentDish.ingredients.Length; i++)
        {
            if (currentDish.ingredients[i].ingredient.type == clearedIngredient.type)
            {
                requirements[i].amount -= removedAmount;
                if (requirements[i].amount < 0)
                    requirements[i].amount = 0;
                requirements[i].SetAppearance();
            }
        }
    }


    public class RequirementsC
    {
        [InspectorReadOnly]
        public GameObject gameObject;
        [InspectorReadOnly]
        public Sprite sprite;
        [InspectorReadOnly]
        public int amount;

        public void SetAppearance()
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            gameObject.transform.GetChild(1).GetComponent<Text>().text = "X" + amount.ToString();
        }
    }
}
