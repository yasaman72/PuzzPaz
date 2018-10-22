using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ingredientEnum
{
    tomato = 0,
    corn = 1,
    onion = 2,
    orange = 3
}

[CreateAssetMenu(fileName = "new ingredient", menuName = "cooking/ingredient")]
public class Ingredient : ScriptableObject
{
    public Sprite sprite;
    public ingredientEnum type;

    public void SetIngredientType(int typeIndex)
    {
        type = (ingredientEnum)typeIndex;
    }
}


