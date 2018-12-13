using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPMAnager : MonoBehaviour {

	public void SendShopOpenEvent()
    {
       GameAnalyticsManager.Instance.SendDesignEvents("Shop:OpenedShop");
    }
}
