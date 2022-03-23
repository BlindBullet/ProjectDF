using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AttackRange))]
[RequireComponent(typeof(SkillController))]
[RequireComponent(typeof(HeroTween))]
[RequireComponent(typeof(HeroUi))]
public class HeroBase : MonoBehaviour
{
    public static List<HeroBase> Heroes = new List<HeroBase>();

    public HeroData Data;
    [HideInInspector] public HeroUi Ui;
    [HideInInspector] public SkillController SkillCon;
    [HideInInspector] public AttackRange Range;
    [HideInInspector] public HeroTween Tween;
    public Transform ProjectileAnchor;

    public void Init(HeroData data)
    {
        Data = data;

        Tween = GetComponent<HeroTween>();
        Tween.SetTween();

        Range = GetComponent<AttackRange>();
        
        SkillCon = GetComponent<SkillController>();
        SkillCon.Setup(this);

        Ui = GetComponent<HeroUi>();
        Ui.SetUp(Data);

        Range.StartSearch(this);
        Heroes.Add(this);
    }

    public void ChangeSpeed(float value)
    {
        Data.ChangeSpeed(value);    
    }
    


}
