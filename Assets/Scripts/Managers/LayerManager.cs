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
	//		new Exception("ĳ���� �±װ� �ƴѵ�." + String.Join(" , ", BattleInputManager.CharacterTags) + " �߿� ���ÿ�");
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