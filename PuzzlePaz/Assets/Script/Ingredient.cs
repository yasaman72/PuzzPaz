using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ingredientEnum
{
    gerdoo = 0,
    balal = 1,
    laboo = 2,
    baghali = 3
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


