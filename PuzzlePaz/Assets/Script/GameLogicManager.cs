using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicManager : MonoBehaviour
{
    public OrderHandler orderHandler;
    public List<DestroyedTiles> destroyedTiles = new List<DestroyedTiles>();
    public Text gameStateText;

    public int amountOfCompletedParts = 0;
    private List<int> foundIngredientIndex = new List<int>();

    public void DestroyedConstructor(int typesCount)
    {
        for (int i = 0; i < typesCount; i++)
        {
            destroyedTiles.Add(new DestroyedTiles(i));
        }
    }

    public void CountDestroyedTilesByType(int amount, Ingredient ingredient)
    {
        gameStateText.text = "";

        foreach (DestroyedTiles destroyed in destroyedTiles)
        {
            if (destroyed.ingredientType.type == ingredient.type)
                destroyed.amount += amount;
        }

        //CheckIfFinishedTheDish();
    }

    //private void CheckIfFinishedTheDish()
    //{
    //    int amountOfCurrentDishIngredients = orderHandler.currentDish.ingredients.Length;

    //    for (int i = 0; i < orderHandler.currentDish.ingredients.Length; i++)
    //    {
    //        //check if current dish exists in founded ingredients
    //        if (foundIngredientIndex.Exists(x => x == (int)orderHandler.currentDish.ingredients[i].ingredient.type))
    //            continue;

    //        for (int t = 0; t < destroyedTiles.Count; t++)
    //        {
    //            if (destroyedTiles[t].ingredientType.type == orderHandler.currentDish.ingredients[i].ingredient.type)
    //            {
    //                if (destroyedTiles[t].amount >= orderHandler.currentDish.ingredients[i].Amount)
    //                {
    //                    foundIngredientIndex.Add((int)orderHandler.currentDish.ingredients[i].ingredient.type);
    //                    amountOfCompletedParts++;
    //                    Debug.Log("Finished an ingredient!");
    //                }
    //            }
    //        }
    //    }

    //    if (amountOfCompletedParts >= amountOfCurrentDishIngredients)
    //    {
    //        Debug.Log("Congratulations! You finished a dish!!");

    //        amountOfCompletedParts = 0;
    //        foundIngredientIndex = new List<int>();

    //        //reseting the destroyed tile counter
    //        for (int i = 0; i < destroyedTiles.Count; i++)
    //        {
    //            destroyedTiles[i].amount = 0;
    //        }

    //        orderHandler.GoToNextDish();
    //    }
    //}

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
        ingredientType.type = (ingredientEnum)inIngredientIndex;
        name = ingredientType.type.ToString();
    }
}