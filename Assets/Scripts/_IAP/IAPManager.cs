using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoSingleton<IAPManager>, IStoreListener
{
	public const string ProductSoulStone1 = "soulstone1";
	private const string SoulStone1Id = "soulstone1";
	public const string ProductSoulStone2 = "soulstone2";
	private const string SoulStone2Id = "soulstone2";
	public const string ProductSoulStone3 = "soulstone3";
	private const string SoulStone3Id = "soulstone3";
	public const string ProductSoulStone4 = "soulstone4";
	private const string SoulStone4Id = "soulstone4";	
	public const string ProductAdRemove = "remove_ad";
	private const string AdRemoveId = "remove_ad";

	public IStoreController Controller;
	public IExtensionProvider Extension;

	public bool IsInit => Controller != null && Extension != null;
	
	void Start()
	{
		DontDestroyOnLoad(gameObject);
		InitUnityIAP();
	}

	void InitUnityIAP()
	{
		if (IsInit)
			return;

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

		builder.AddProduct(ProductSoulStone1, ProductType.Consumable, new IDs() 
		{
			{ SoulStone1Id, GooglePlay.Name }
		});
		builder.AddProduct(ProductSoulStone2, ProductType.Consumable, new IDs()
		{
			{ SoulStone2Id, GooglePlay.Name }
		});
		builder.AddProduct(ProductSoulStone3, ProductType.Consumable, new IDs()
		{
			{ SoulStone3Id, GooglePlay.Name }
		});
		builder.AddProduct(ProductSoulStone4, ProductType.Consumable, new IDs()
		{
			{ SoulStone4Id, GooglePlay.Name }
		});		
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
			StageManager.Ins.PlayerStat.RemoveAd = true;
			StageManager.Ins.ChangeSoulStone(5000f);
			DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, 5000f);
		}

		if(purchaseEvent.purchasedProduct.definition.id == SoulStone1Id)
		{
			StageManager.Ins.ChangeSoulStone(1500f);
			DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, 1500f);
		}

		if (purchaseEvent.purchasedProduct.definition.id == SoulStone2Id)
		{
			StageManager.Ins.ChangeSoulStone(4200f);
			DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, 4200f);
		}

		if (purchaseEvent.purchasedProduct.definition.id == SoulStone3Id)
		{
			StageManager.Ins.ChangeSoulStone(9000f);
			DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, 9000f);
		}

		if (purchaseEvent.purchasedProduct.definition.id == SoulStone4Id)
		{
			StageManager.Ins.ChangeSoulStone(22000f);
			DialogManager.Ins.OpenReceiveReward(RewardType.SoulStone, 22000f);
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

	public Product GetProduct(string _productId)
	{
		return Controller.products.WithID(_productId);
	}

}
