using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
	public const string ProductAdRemove = "AdRemove";
	private const string AdRemoveId = "";

	public IStoreController Controller;
	public IExtensionProvider Extension;

	public bool IsInit => Controller != null && Extension != null;
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		InitUnityIAP();
	}

	void InitUnityIAP()
	{
		if (IsInit)
			return;

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		builder.AddProduct(ProductAdRemove, ProductType.NonConsumable, new IDs()
		{
			{ AdRemoveId, GooglePlay.Name }			
		});

		UnityPurchasing.Initialize(this, builder);
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		Debug.Log("유니티 IAP 초기화 성공");
		Controller = controller;
		Extension = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogError($"유니티 IAP 초기화 실패 {error}");
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		Debug.Log($"구매 성공 - ID: {purchaseEvent.purchasedProduct.definition.id}");

		if(purchaseEvent.purchasedProduct.definition.id == AdRemoveId)
		{
			//광고 제거 실행

		}

		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log($"구매 실패 - {product.definition.id}, {failureReason}");
	}

	public void Purchase(string productId)
	{
		if (!IsInit)
			return;

		var product = Controller.products.WithID(productId);
		
		if(product != null && product.availableToPurchase)
		{
			Debug.Log($"구매 시도 - {product.definition.id}");
			Controller.InitiatePurchase(product);
		}
		else
		{
			Debug.Log($"구매 시도 불가 - {productId}");
		}
	}

	public bool HadPurchased(string productId)
	{
		if (!IsInit)
			return false;

		var product = Controller.products.WithID(productId);

		if(product != null)
		{
			return product.hasReceipt;
		}

		return false;
	}

}
