using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillController : MonoBehaviour
{
    BasicAttack attack;

    public Skill Attack;
    public Skill Skill;

    HeroBase me;    
    Coroutine cAttack = null;
    Coroutine cSkill = null;

    public void Init(HeroBase heroBase)
    {
        me = heroBase;
        HeroChart heroChart = CsvData.Ins.HeroChart[me.Data.Id];

        Attack = new Skill(heroChart.BasicAttack, me.Data.AtkLv);
        Skill = new Skill(heroChart.Skill, me.Data.SkillLv);

        attack = new BasicAttack(heroChart.BasicAttack);
        
        StartCoroutine(UseSkill());
    }

    public void ReStart()
    {
        StartCoroutine(UseSkill());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public IEnumerator UseSkill()
    {
        while (true)
        {
            if (me.Range.AllTargetColl.Count > 0 && EnemyBase.Enemies.Count > 0)
            {
                if (cSkill == null)
                {                    
                    if (cAttack == null)
                    {
                        cAttack = StartCoroutine(SkillSequence(Attack));                 
                    }   
                }
                else
                {
                    if(cAttack != null)
                    {
                        StopCoroutine(cAttack);
                        cAttack = null;
                    }   

                    cSkill = StartCoroutine(SkillSequence(Skill));
                }
            }

            yield return null;
        }
    }

    IEnumerator SkillSequence(Skill skill)
    {   
        SkillChart data = skill.Data;        

        switch (data.Type)
        {
            case SkillType.Attack:
                
                break;
            case SkillType.Active:

                break;
        }

        if (data.BeginFx != null)
            EffectManager.Ins.ShowFx(attack.Data.BeginFx, this.transform);

        if (data.Anim == "Attack")
        {
            me.Tween.Attack(me.Data.Spd > 1f ? me.Data.Spd : 1f, me.Range.AllTargetColl[0].GetComponent<EnemyBase>());
            
        }
        else
        {

        }

        float totalTime = skill.Data.TotalFrame / 30f;
        float progressTime = 0;        

        if (me.Data.Spd > 1f && data.Type == SkillType.Attack)
        {
            totalTime = totalTime / me.Data.Spd;
        }

        for (int i = 0; i < data.FireFrame.Length; i++)
        {
            if(i == 0)
            {
                if(data.Type == SkillType.Attack)
                {
                    progressTime += data.FireFrame[i] / 30f / me.Data.Spd;
                    yield return new WaitForSeconds(data.FireFrame[i] / 30f / me.Data.Spd);
                }
                else
                {
                    progressTime += data.FireFrame[i] / 30f;
                    yield return new WaitForSeconds(data.FireFrame[i] / 30f);
                }
            }
            else
            {
                if (data.Type == SkillType.Attack)
                {
                    progressTime += (data.FireFrame[i] - data.FireFrame[i - 1]) / 30f / me.Data.Spd;
                    yield return new WaitForSeconds((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f / me.Data.Spd);
                }
                else
                {
                    progressTime += (data.FireFrame[i] - data.FireFrame[i - 1]) / 30f;
                    yield return new WaitForSeconds((data.FireFrame[i] - data.FireFrame[i - 1]) / 30f);
                }
                    
            }

            //리절트 그룹 실행            
            StartCoroutine(HitresultManager.Ins.ResultGroupSequence(CsvData.Ins.ResultGroupChart[data.ResultGroup[i]], me));
        }
        
        if(totalTime - progressTime > 0)
            yield return new WaitForSeconds(totalTime - progressTime);
        
        cAttack = null;
        cSkill = null;
    }

    

    

}

public class Skill
{
    public string Id;
    public int Lv;
    public float CoolTime;
    public SkillChart Data;

    public Skill(string id, int lv)
    {
        Id = id;
        Lv = lv;
        CoolTime = 0;
        SetData();
    }

    void SetData()
    {
        foreach(KeyValuePair<string, SkillChart> elem in CsvData.Ins.SkillChart)
        {
            if(elem.Key == Id)
            {
                if(elem.Value.Lv == Lv)
                {
                    Data = elem.Value;
                }
            }
        }
    }

    public void IncreaseLv()
    {
        Lv++;
        CoolTime = 0;
        SetData();
    }
}

public class BasicAttack
{
    public string Id;
    public AttackChart Data;

    public BasicAttack(string id)
    {
        Id = id;
        Data = CsvData.Ins.AttackChart[id];
    }
}