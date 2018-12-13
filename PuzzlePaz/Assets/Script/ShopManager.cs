using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BazaarPlugin;

public class ShopManager : MonoBehaviour
{

    public InGameManager inGameManager;

#if UNITY_ANDROID
    //public enum SkuNames
    //{
    //    SomeCoins
    //}

    //public Dictionary<SkuNames, string> SkuNamesDictionary = new Dictionary<SkuNames, string>();


    private void Start()
    {
        string key = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwCs/topXMZorj7EhSkUjYwnSerOb0CfPHDyNNoMEhN01RTEUiVz8AoDzWVgU64LIjZHqlP8Y4mlID1CisAgC23pcQaNgwGlBJJdZiynzIYOe44W1bObJ3e0C1IN5dyOyT/tbE2S1xvbTVx1YyWA4QYo5LPcxQ1yJBIvfhio19aIzqYx9GVrVCBZXodQyINRCnnBSSwZ1jiSBrQN1EocGU5mCyRMH3MBHZa/GAb+vmsCAwEAAQ==";
        BazaarIAB.init(key);

        //SkuNamesDictionary = new Dictionary<SkuNames, string>();
        //SkuNamesDictionary.Add(SkuNames.SomeCoins, "Coin_100");
    }

    public void OnBuyCoinPackBtnClicked(string sku)
    {
        BazaarIAB.purchaseProduct(sku);
    }

    #region event listeners

    void OnEnable()
    {
        // Listen to all events for illustration purposes
        IABEventManager.billingSupportedEvent += billingSupportedEvent;
        IABEventManager.billingNotSupportedEvent += billingNotSupportedEvent;
        IABEventManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
        IABEventManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
        IABEventManager.querySkuDetailsSucceededEvent += querySkuDetailsSucceededEvent;
        IABEventManager.querySkuDetailsFailedEvent += querySkuDetailsFailedEvent;
        IABEventManager.queryPurchasesSucceededEvent += queryPurchasesSucceededEvent;
        IABEventManager.queryPurchasesFailedEvent += queryPurchasesFailedEvent;
        IABEventManager.purchaseSucceededEvent += purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent += purchaseFailedEvent;
        IABEventManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
        IABEventManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
    }

    void OnDisable()
    {
        // Remove all event handlers
        IABEventManager.billingSupportedEvent -= billingSupportedEvent;
        IABEventManager.billingNotSupportedEvent -= billingNotSupportedEvent;
        IABEventManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
        IABEventManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
        IABEventManager.querySkuDetailsSucceededEvent -= querySkuDetailsSucceededEvent;
        IABEventManager.querySkuDetailsFailedEvent -= querySkuDetailsFailedEvent;
        IABEventManager.queryPurchasesSucceededEvent -= queryPurchasesSucceededEvent;
        IABEventManager.queryPurchasesFailedEvent -= queryPurchasesFailedEvent;
        IABEventManager.purchaseSucceededEvent -= purchaseSucceededEvent;
        IABEventManager.purchaseFailedEvent -= purchaseFailedEvent;
        IABEventManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
        IABEventManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
    }


    void billingSupportedEvent()
    {
        Debug.Log("billingSupportedEvent");
    }

    void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
    }

    void queryInventorySucceededEvent(List<BazaarPurchase> purchases, List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));

        for (int i = 0; i < purchases.Count; ++i)
        {
            Debug.Log(purchases[i].ToString());
        }

        Debug.Log("-----------------------------");

        for (int i = 0; i < skus.Count; ++i)
        {
            Debug.Log(skus[i].ToString());
        }
    }

    void queryInventoryFailedEvent(string error)
    {
        Debug.Log("queryInventoryFailedEvent: " + error);
    }

    private void querySkuDetailsSucceededEvent(List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("querySkuDetailsSucceededEvent. total skus: {0}", skus.Count));

        for (int i = 0; i < skus.Count; ++i)
        {
            Debug.Log(skus[i].ToString());
        }
    }

    private void querySkuDetailsFailedEvent(string error)
    {
        Debug.Log("querySkuDetailsFailedEvent: " + error);
    }

    private void queryPurchasesSucceededEvent(List<BazaarPurchase> purchases)
    {
        Debug.Log(string.Format("queryPurchasesSucceededEvent. total purchases: {0}", purchases.Count));

        for (int i = 0; i < purchases.Count; ++i)
        {
            Debug.Log(purchases[i].ToString());
        }
    }

    private void queryPurchasesFailedEvent(string error)
    {
        Debug.Log("queryPurchasesFailedEvent: " + error);
    }

    void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("purchase Succeeded Event: " + purchase);

        BazaarIAB.consumeProduct(purchase.ProductId);        
    }


    void purchaseFailedEvent(string error)
    {
        Debug.Log("purchaseFailedEvent: " + error);

        //todo: inform the player about the purchase fail in a popup
    }

    void consumePurchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);

        switch (purchase.ProductId)
        {
            case "Coin_1":
                {
                    inGameManager.AddCurrency(500, 0);
                    inGameManager.ShowCurrencyPopup(500, 0);
                    //todo: give player the coins...
                    break;
                }
            case "Coin_2":
                {
                    inGameManager.AddCurrency(2000, 0);
                    inGameManager.ShowCurrencyPopup(2000, 0);
                    //todo: give player the coins...
                    break;
                }
            case "Coin_3":
                {
                    inGameManager.AddCurrency(8000, 0);
                    inGameManager.ShowCurrencyPopup(8000, 0);
                    //todo: give player the coins...
                    break;
                }
            case "Coin_4":
                {
                    inGameManager.AddCurrency(20000, 0);
                    inGameManager.ShowCurrencyPopup(20000, 0);
                    //todo: give player the coins...
                    break;
                }
        }
    }

    void consumePurchaseFailedEvent(string error)
    {
        Debug.Log("consumePurchaseFailedEvent: " + error);
        inGameManager.ShowMessageBox("متاسفانه مشکلی در خرید پیش آمد.");
        //todo: inform the player about the purchase fail in a popup
    }
    #endregion

#endif
}
