using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "LayerManager")]
[ResourcePath("GameSystem/LayerManager")]
public class LayerManager : ResourceSingleton<LayerManager>
{
	public LayerMask Hero;
	public LayerMask Enemy;
	public LayerMask Minion;
	public LayerMask FieldAndSupplies;

	//public int TagToTeamLayerAndBlocker(string tag)
	//{
	//	if (!BattleInputManager.CharacterTags.Contains(tag))
	//	{
	//		new Exception("캐릭터 태그가 아닌듯." + String.Join(" , ", BattleInputManager.CharacterTags) + " 중에 고르시오");
	//	}
	//	if (tag == BattleInputManager.EnemyTag)
	//	{
	//		return EnemyAndUnwalkable;
	//	}
	//	else
	//	{
	//		return PlayerAndUnwalkable;
	//	}
	//}
}

public static class LayerManagerExtension
{

	public static int ToLayer(this LayerMask mask)
	{
		return Mathf.RoundToInt(Mathf.Log(mask.value, 2));
	}
}