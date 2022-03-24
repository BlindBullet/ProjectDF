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
            StartCoroutine(ResultGroupSequence(CsvData.Ins.ResultGroupChart[data.ResultGroup[i]]));
        }
        
        if(totalTime - progressTime > 0)
            yield return new WaitForSeconds(totalTime - progressTime);
        
        cAttack = null;
        cSkill = null;
    }

    IEnumerator ResultGroupSequence(List<ResultGroupChart> datas)
    {
        for(int i = 0; i < datas.Count; i++)
        {
            switch (datas[i].TargetType)
            {
                case TargetType.Enemy:
                    List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i]);

                    if(datas[i].RangeType == RangeType.None)
                    {
                        for(int k = 0; k < enemyTargets.Count; k++)
                        {
                            //히트리절트 전달
                            SendHitresult(datas[i], enemyTargets[k], me);
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(datas[i].DelayTime);

                        //범위에 따라 다시 타겟을 지정
                        for (int k = 0; k < enemyTargets.Count; k++)
                        {
                            List<EnemyBase> delayTargets = me.Range.SearchTarget(datas[i], enemyTargets[i]);

                            //히트리절트 전달
                            SendHitresult(datas[i], delayTargets[k], me);
                        }
                    }
                    break;
                case TargetType.Hero:
                    List<HeroBase> heroTargets = SearchHeroTargets(datas[i]);
                    for(int k = 0; k < heroTargets.Count; k++)
                    {
                        //히트리절트 전달
                        SendHitresult(datas[i], heroTargets[k]);
                    }
                    break;
                case TargetType.Me:
                    //히트리절트 전달
                    SendHitresult(datas[i], me);
                    break;
            }
        }
    }

    void SendHitresult(ResultGroupChart data, EnemyBase target, HeroBase caster)
    {
        List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];

        if (data.Projectile == null)
        {
            for(int i = 0; i < hitresults.Count; i++)
            {
                float randNo = Random.Range(0, 100);

                if(hitresults[i].Prob >= randNo)
                {
                    if (hitresults[i].HitFx != null)
                        EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);

                    switch (hitresults[i].FactorOwner)
                    {
                        case FactorOwner.Caster:
                            double dmg = hitresults[i].Value + (caster.Data.Atk * (hitresults[i].ValuePercent / 100f));
                            bool isCrit = Random.Range(0f, 100f) < me.Data.CritChance ? true : false;
                            double resultDmg = 0;

                            if (isCrit)
                                dmg = dmg * (1 + (me.Data.CritDmg / 100f));

                            Vector3 pos = target.transform.position;
                            resultDmg = target.TakeDmg(dmg);
                            FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToString(), isCrit);
                            break;
                        case FactorOwner.Target:

                            break;
                    }
                }
                else
                {
                    Debug.Log("미스");
                }
                                
                //온힛 추가 예정?

            }
        }
        else
        {
            Vector2 dir = target.transform.position - me.ProjectileAnchor.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            me.ProjectileAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //프로젝타일 발사
            List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart[data.Projectile];

            for(int i = 0; i < projectiles.Count; i++)
            {
                GameObject obj = Instantiate(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject, me.ProjectileAnchor.position.WithX(me.ProjectileAnchor.position.x + projectiles[i].PosX), Quaternion.AngleAxis(angle - 90, Vector3.forward));
                ProjectileController projectile = obj.GetComponent<ProjectileController>();
                //ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);
                //projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                //projectile.transform.position = me.ProjectileAnchor.position.WithX(me.ProjectileAnchor.position.x + projectiles[i].PosX);
                projectile.Setup(projectiles[i], hitresults, me);
            }
        }
    }

    void SendHitresult(ResultGroupChart data, HeroBase target)
    {

    }


    List<HeroBase> SearchHeroTargets(ResultGroupChart data)
    {
        List<HeroBase> targets = new List<HeroBase>();

        switch (data.TargetDetail)
        {
            case TargetDetail.All:
                targets = HeroBase.Heroes;
                break;            
        }

        return targets;
    }

    List<EnemyBase> SearchEnemyTargets(ResultGroupChart data)
    {
        List<EnemyBase> targets = new List<EnemyBase>();

        switch (data.TargetDetail)
        {
            case TargetDetail.All:
                for(int i = 0; i < me.Range.AllTargetColl.Count; i++)
                {
                    targets.Add(me.Range.AllTargetColl[i].GetComponent<EnemyBase>());
                }
                break;
            case TargetDetail.Closest:
                for(int i = 0; i < data.TargetCount; i++)
                {
                    if(i < me.Range.AllTargetColl.Count)
                        targets.Add(me.Range.AllTargetColl[i].GetComponent<EnemyBase>());
                }
                break;
        }

        return targets;
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