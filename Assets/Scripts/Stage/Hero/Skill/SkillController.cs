using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    BasicAttack attack;

    public Skill Attack;
    public Skill Skill;

    HeroBase me;
    bool usingSKill = false;    
    Coroutine cAttack = null;

    public void Init(HeroBase heroBase)
    {
        me = heroBase;
        HeroChart heroChart = CsvData.Ins.HeroChart[me.Data.Id];

        Attack = new Skill(heroChart.BasicAttack, 1);
        Skill = new Skill(heroChart.Skill, 1);

        attack = new BasicAttack(heroChart.BasicAttack);
        
        StartCoroutine(UseSkill());
    }

    public IEnumerator UseSkill()
    {
        while (true)
        {
            if (me.Range.AllTargetColl.Count > 0 && EnemyBase.Enemies.Count > 0)
            {
                if (!usingSKill)
                {                    
                    if (cAttack == null)
                    {
                        cAttack = StartCoroutine(AttackSequence(Attack));                 
                    }   
                }
                else
                {

                }
            }

            yield return null;
        }
    }

    IEnumerator SkillSequence(Skill skill)
    {
        yield return null;
    }

    IEnumerator AttackSequence(Skill skill)
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

        float totalTime = skill.Data.TotalFrame / 30f;
        float progressTime = 0;        

        if (me.Data.Spd > 1f)
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
            ResultGroupSequence(CsvData.Ins.ResultGroupChart[data.ResultGroup[i]]);
        }
        
        if(totalTime - progressTime > 0)
            yield return new WaitForSeconds(totalTime - progressTime);
        
        //double dmg = me.Data.Atk;
        //bool isCrit = Random.Range(0f, 100f) < me.Data.CritChance ? true : false;

        //if (Attack.Data.Projectile == null)
        //{
        //    if (attack.Data.HitFx != null)
        //        EffectManager.Ins.ShowFx(attack.Data.HitFx, target.transform);

        //    double resultDmg = 0;

        //    if (isCrit)
        //        dmg = dmg * (1 + (me.Data.CritDmg / 100f));

        //    Vector3 pos = target.transform.position;
        //    resultDmg = target.TakeDmg(dmg);
        //    FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToString(), isCrit);
        //}
        //else
        //{
        //    LookTarget(target);

        //}

        //yield return new WaitForSeconds((1f / me.Data.Spd) - fireTime);

        cAttack = null;
    }

    void ResultGroupSequence(List<ResultGroupChart> datas)
    {
        for(int i = 0; i < datas.Count; i++)
        {

        }
    }

    void LookTarget(EnemyBase target)
    {
        Vector3 dir = target.transform.position - me.ProjectileAnchor.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        me.ProjectileAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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