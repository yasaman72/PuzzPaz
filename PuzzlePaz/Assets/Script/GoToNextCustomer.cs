using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToNextCustomer : MonoBehaviour {

    public OrderHandler orderHandler;

    public void ChangeTheCustomer()
    {
        orderHandler.ShowTheNewCustomer();
    }
}
