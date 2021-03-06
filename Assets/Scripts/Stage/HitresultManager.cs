using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitresultManager : MonoSingleton<HitresultManager>
{
    public void RunResultGroup(List<ResultGroupChart> datas, Vector2 pos, HeroBase caster)
    {
        StartCoroutine(ResultGroupSequence(datas, pos, caster));
    }

    public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, Vector2 pos, HeroBase caster)
    {
        for (int i = 0; i < datas.Count; i++)
        {   
            switch (datas[i].TargetType)
            {
                case TargetType.Enemy:                    
                    yield return new WaitForSeconds(datas[i].DelayTime);
                    
                    List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], pos);
                    
                    //범위에 따라 타겟을 지정
                    for (int k = 0; k < enemyTargets.Count; k++)
                    {   
                        //히트리절트 전달
                        SendResultGroup(datas[i], enemyTargets[k], caster);
                    }
                    break;                
            }
        }
    }

    public IEnumerator ResultGroupSequence(List<ResultGroupChart> datas, HeroBase caster)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            switch (datas[i].TargetType)
            {
                case TargetType.Enemy:
                    List<EnemyBase> enemyTargets = SearchEnemyTargets(datas[i], caster);

                    if (datas[i].RangeType == RangeType.None)
                    {
                        for (int k = 0; k < enemyTargets.Count; k++)
                        {
                            //히트리절트 전달
                            SendResultGroup(datas[i], enemyTargets[k], caster);
                        }
                    }
                    else
                    {
                        yield return new WaitForSeconds(datas[i].DelayTime);

                        //범위에 따라 다시 타겟을 지정
                        for (int k = 0; k < enemyTargets.Count; k++)
                        {
                            List<EnemyBase> delayTargets = caster.Range.SearchTarget(datas[i], enemyTargets[i]);

                            if (delayTargets.Count > 0)
                            {
                                //히트리절트 전달
                                SendResultGroup(datas[i], delayTargets[k], caster);
                            }
                        }
                    }
                    break;
                case TargetType.Hero:
                    List<HeroBase> heroTargets = SearchHeroTargets(datas[i], caster);
                    for (int k = 0; k < heroTargets.Count; k++)
                    {
                        //히트리절트 전달
                        SendHitresult(datas[i], heroTargets[k]);
                    }
                    break;
                case TargetType.Me:
                    //히트리절트 전달
                    SendHitresult(datas[i], caster);
                    break;
            }
        }
    }

    public void SendResultGroup(ResultGroupChart data, EnemyBase target, HeroBase caster)
    {
        List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];

        if (data.Projectile == null)
        {
            SendHitresult(hitresults, target, caster);
        }
        else
        {
            Vector2 dir = target.transform.position - caster.ProjectileAnchor.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //caster.ProjectileAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //프로젝타일 발사
            List<ProjectileChart> projectiles = CsvData.Ins.ProjectileChart[data.Projectile];

            for (int i = 0; i < projectiles.Count; i++)
            {
                ProjectileController projectile = ObjectManager.Ins.Pop<ProjectileController>(Resources.Load("Prefabs/Projectiles/" + projectiles[i].Model) as GameObject);
                projectile.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
                projectile.transform.position = caster.ProjectileAnchor.position.WithX(caster.ProjectileAnchor.position.x + projectiles[i].PosX);
                projectile.Setup(projectiles[i], hitresults, caster);
            }
        }
    }

    public void SendHitresult(ResultGroupChart data, HeroBase target)
    {

    }

    public void SendHitresult(List<HitresultChart> hitresults, EnemyBase target, HeroBase caster)
    {
        for (int i = 0; i < hitresults.Count; i++)
        {
            float randNo = Random.Range(0, 100);

            if (hitresults[i].Prob >= randNo)
            {
                if (hitresults[i].HitFx != null)
                {                    
                    EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);
                }   

                switch (hitresults[i].Type)
                {
                    case HitType.Dmg:
                        switch (hitresults[i].FactorOwner)
                        {
                            case FactorOwner.Caster:
                                double dmg = hitresults[i].Value + (caster.Data.Atk * (hitresults[i].ValuePercent / 100f));
                                bool isCrit = Random.Range(0f, 100f) < caster.Data.CritChance ? true : false;
                                double resultDmg = 0;

                                if (isCrit)
                                    dmg = dmg * (1 + (caster.Data.CritDmg / 100f));

                                Vector3 pos = target.transform.position;
                                resultDmg = target.TakeDmg(dmg);
                                FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToString(), isCrit);
                                break;
                            case FactorOwner.Target:

                                break;
                        }
                        break;
                    case HitType.Push:                        
                        //target.Push(hitresults[i].Value, hitresults[i].DurationTime);                        
                        break;
                    case HitType.Stun:

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

    List<HeroBase> SearchHeroTargets(ResultGroupChart data, HeroBase caster)
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

    List<EnemyBase> SearchEnemyTargets(ResultGroupChart data, HeroBase caster)
    {
        List<EnemyBase> targets = new List<EnemyBase>();

        switch (data.TargetDetail)
        {
            case TargetDetail.All:
                for (int i = 0; i < caster.Range.AllTargetColl.Count; i++)
                {
                    targets.Add(caster.Range.AllTargetColl[i].GetComponent<EnemyBase>());
                }
                break;
            case TargetDetail.Closest:
                for (int i = 0; i < data.TargetCount; i++)
                {
                    if (i < caster.Range.AllTargetColl.Count)
                        targets.Add(caster.Range.AllTargetColl[i].GetComponent<EnemyBase>());
                }
                break;
        }

        return targets;
    }

    List<EnemyBase> SearchEnemyTargets(ResultGroupChart data, Vector2 pos)
    {   
        List<EnemyBase> targets = new List<EnemyBase>();
        List<EnemyBase> results = new List<EnemyBase>();
        Collider2D[] colls = null;
        
        switch (data.RangeType)
        {
            case RangeType.Circle:
                colls = Physics2D.OverlapCircleAll(pos, data.RnageSize[0]);
                break;
        }

        for (int i = 0; i < colls.Length; i++)
        {
            if(colls[i].CompareTag("Enemy"))
                targets.Add(colls[i].GetComponent<EnemyBase>());
        }

        switch (data.TargetDetail)
        {
            case TargetDetail.All:
                results = targets;
                break;
            case TargetDetail.Closest:
                targets.Sort(delegate (EnemyBase A, EnemyBase B)
                {
                    float distA = Vector2.Distance(pos, A.transform.position);
                    float distB = Vector2.Distance(pos, B.transform.position);

                    if(distA < distB)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                });

                for(int i = 0; i < data.TargetCount; i++)
                {
                    results.Add(targets[i]);
                }
                break;
        }

        return results;
    }

}
