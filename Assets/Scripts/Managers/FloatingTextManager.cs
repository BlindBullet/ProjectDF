using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class FloatingTextManager : MonoSingleton<FloatingTextManager>
{
    DamageNumber Dmg;
    DamageNumber CritDmg;
    DamageNumber Heal;
    DamageNumber SymbolBuff;

    private void Start()
    {
        Dmg = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/DamagePrefab");
        CritDmg = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/CritDamagePrefab");
        Heal = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/HealPrefab");
        SymbolBuff = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/SymbolBuffPrefab");
    }

    public void ShowDmg(Vector3 pos, string text)
    {
        Dmg.Spawn(pos, text);
    }

    public void ShowCritDmg(Vector3 pos, string text)
    {
        CritDmg.Spawn(pos, text);
    }

    public void ShowHeal(Vector3 pos, string text)
    {
        Heal.Spawn(pos, text);
    }

    public void ShowSymbolBuff(Vector3 pos, string text)
    {
        SymbolBuff.Spawn(pos, text);
    }



}
