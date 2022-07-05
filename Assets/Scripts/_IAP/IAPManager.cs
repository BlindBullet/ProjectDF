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
		Debug.Log("����Ƽ IAP �ʱ�ȭ ����");
		Controller = controller;
		Extension = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.LogError($"����Ƽ IAP �ʱ�ȭ ���� {error}");
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
	{
		Debug.Log($"���� ���� - ID: {purchaseEvent.purchasedProduct.definition.id}");

		if(purchaseEvent.purchasedProduct.definition.id == AdRemoveId)
		{
			//���� ���� ����

		}

		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log($"���� ���� - {product.definition.id}, {failureReason}");
	}

	public void Purchase(string productId)
	{
		if (!IsInit)
			return;

		var product = Controller.products.WithID(productId);
		
		if(product != null && product.availableToPurchase)
		{
			Debug.Log($"���� �õ� - {product.definition.id}");
			Controller.InitiatePurchase(product);
		}
		else
		{
			Debug.Log($"���� �õ� �Ұ� - {productId}");
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
