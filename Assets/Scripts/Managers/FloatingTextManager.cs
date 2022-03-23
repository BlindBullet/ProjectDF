using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;

public class FloatingTextManager : MonoSingleton<FloatingTextManager>
{
    DamageNumber Dmg;
    DamageNumber CritDmg;
    DamageNumber Heal;
    DamageNumber Buff;

    private void Start()
    {
        Dmg = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/DamagePrefab");
        CritDmg = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/CritDamagePrefab");
        Heal = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/HealPrefab");
        Buff = Resources.Load<DamageNumber>("Prefabs/FloatingTexts/BuffTextPrefab");
    }

    public void ShowDmg(Vector3 pos, string text, bool isCrit)
    {        
        if (isCrit)
            CritDmg.Spawn(pos, text);
        else
            Dmg.Spawn(pos, text);
    }

    public void ShowHeal(Vector3 pos, string text)
    {
        Heal.Spawn(pos, text);
    }

    public void ShowSymbolBuff(Vector3 pos, string text)
    {
        Buff.Spawn(pos, text);
    }



}
