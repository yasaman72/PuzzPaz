using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new order", menuName = "cooking/Order")]
public class Order : ScriptableObject {

    public enum difficulty{
        easy,
        mediem,
        hard
    };

    public new string name;
    public difficulty orderDifficulty;
    public Dish[] dishes;

}
