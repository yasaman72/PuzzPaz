using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new dish", menuName = "cooking/dish"),System.Serializable]
public class Dish : ScriptableObject
{
    public new string name;
    public Sprite sprite;
    public ingredientstruct[] ingredients;
}

[System.Serializable]
public class ingredientstruct
{
    public Ingredient ingredient;
    public int Amount;
}
