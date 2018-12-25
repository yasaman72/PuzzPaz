using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BazaarPlugin;

public class ShopManager : MonoBehaviour
{

    public InGameManager inGameManager;
    public GameObject loadingPopup;

#if UNITY_ANDROID
    //public enum SkuNames
    //{
    //    SomeCoins
    //}

    //public Dictionary<SkuNames, string> SkuNamesDictionary = new Dictionary<SkuNames, string>();


    private void Start()
    {
        string key = "MIHNMA0GCSqGSIb3DQEBAQUAA4G7ADCBtwKBrwDU4vrRbWfev5hVh2Ms9wmk8SfTSBmqH8MmwSWyDqC6NSFLKwLIDZQ9D62iwpnA+3GUi/g7IXR5Bsr2feLp1pGYVXciMc0AO6hzF6VVin7RlIoxLFiZ2dyX1EMWwS1kOsideNuiRZ4XMW+LCQgqaG63wNyB0N0cX6dwjwwAkRjvY85IdtCXsIO/hBObGFW+UhWUMbrGmKfRkFwUoQbNJC3yM6YYAcXnvgnlqYmJ+2kCAwEAAQ==";
        BazaarIAB.init(key);

        //SkuNamesDictionary = new Dictionary<SkuNames, string>();
        //SkuNamesDictionary.Add(SkuNames.SomeCoins, "Coin_100");
    }

    public void OnBuyCoinPackBtnClicked(string sku)
    {
        BazaarIAB.purchaseProduct(sku);
        loadingPopup.SetActive(true);
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
        loadingPopup.SetActive(false);
    }

    void billingNotSupportedEvent(string error)
    {
        Debug.Log("billingNotSupportedEvent: " + error);
        loadingPopup.SetActive(false);
    }

    void queryInventorySucceededEvent(List<BazaarPurchase> purchases, List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count));
        loadingPopup.SetActive(false);

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
        loadingPopup.SetActive(false);
    }

    private void querySkuDetailsSucceededEvent(List<BazaarSkuInfo> skus)
    {
        Debug.Log(string.Format("querySkuDetailsSucceededEvent. total skus: {0}", skus.Count));
        loadingPopup.SetActive(false);

        for (int i = 0; i < skus.Count; ++i)
        {
            Debug.Log(skus[i].ToString());
        }
    }

    private void querySkuDetailsFailedEvent(string error)
    {
        Debug.Log("querySkuDetailsFailedEvent: " + error);
        loadingPopup.SetActive(false);
    }

    private void queryPurchasesSucceededEvent(List<BazaarPurchase> purchases)
    {
        Debug.Log(string.Format("queryPurchasesSucceededEvent. total purchases: {0}", purchases.Count));
        loadingPopup.SetActive(false);

        for (int i = 0; i < purchases.Count; ++i)
        {
            Debug.Log(purchases[i].ToString());
        }
    }

    private void queryPurchasesFailedEvent(string error)
    {
        Debug.Log("queryPurchasesFailedEvent: " + error);
        loadingPopup.SetActive(false);
    }

    void purchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("purchase Succeeded Event: " + purchase);
        loadingPopup.SetActive(false);
        BazaarIAB.consumeProduct(purchase.ProductId);        
    }

    void purchaseFailedEvent(string error)
    {
        Debug.Log("purchaseFailedEvent: " + error);
        loadingPopup.SetActive(false);
        //todo: inform the player about the purchase fail in a popup
    }

    void consumePurchaseSucceededEvent(BazaarPurchase purchase)
    {
        Debug.Log("consumePurchaseSucceededEvent: " + purchase);
        loadingPopup.SetActive(false);

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
        loadingPopup.SetActive(false);
        Debug.Log("consumePurchaseFailedEvent: " + error);
        inGameManager.ShowMessageBox("متاسفانه مشکلی در خرید پیش آمد.");
        //todo: inform the player about the purchase fail in a popup
    }
    #endregion

#endif
}
