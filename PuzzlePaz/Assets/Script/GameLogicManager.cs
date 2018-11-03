using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    public DishHandler dishHandler;
    public List<DestroyedTiles> destroyedTiles = new List<DestroyedTiles>();
    public Text gameStateText;

    private void Start()
    {
        dishHandler.GoToRandomDish();
    }

    public void DestroyedConstructor(int typesCount)
    {
        for (int i = 0; i < typesCount; i++)
        {
            destroyedTiles.Add(new DestroyedTiles(i));
        }
    }

    public void CountDestroyedTilesByType(int amount, Ingredient ingredient)
    {
        foreach (DestroyedTiles destroyed in destroyedTiles)
        {
            if (destroyed.ingredientType.type == ingredient.type)
                destroyed.amount += amount;
        }

        CheckIfFinishedTheDish();
    }

    private void CheckIfFinishedTheDish()
    {
        gameStateText.text = "";
        List<int> foundIngredientIndex = new List<int>();
        int amountOfCurrentDishIngredients = dishHandler.currentDish.ingredients.Length;
        int amountOfCopletedParts = 0;

        for (int i = 0; i < dishHandler.currentDish.ingredients.Length; i++)
        {
            if (foundIngredientIndex.Exists(x => x == (int)dishHandler.currentDish.ingredients[i].ingredient.type))
                continue;
            for (int j = 0; j < destroyedTiles.Count; j++)
            {
                if (destroyedTiles[j].ingredientType.type == dishHandler.currentDish.ingredients[i].ingredient.type)
                {
                    if (destroyedTiles[j].amount >= dishHandler.currentDish.ingredients[i].Amount)
                    {
                        foundIngredientIndex.Add((int)dishHandler.currentDish.ingredients[i].ingredient.type);
                        amountOfCopletedParts++;
                        gameStateText.text = "Finished " + amountOfCopletedParts + " ingredient(s)!";
                    }
                }
            }
        }

        if (amountOfCopletedParts >= amountOfCurrentDishIngredients)
        {
            gameStateText.text = "Congratulations! You finished a dish!!";

            dishHandler.GoToRandomDish();

            //reseting the destroyed tile counter
            for (int i = 0; i < destroyedTiles.Count; i++)
            {
                destroyedTiles[i].amount = 0;
            }
        }
    }
}

[System.Serializable]
public class DestroyedTiles
{
    [HideInInspector]
    public string name;
    [HideInInspector]
    public Ingredient ingredientType;
    //[InspectorReadOnly]
    public int amount;

    public DestroyedTiles(int inIngredientIndex)
    {
        ingredientType = ScriptableObject.CreateInstance<Ingredient>();
        amount = 0;
        ingredientType.type = (ingredientEnum)inIngredientIndex;
        name = ingredientType.type.ToString();
    }
}