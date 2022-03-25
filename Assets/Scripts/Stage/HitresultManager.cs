using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitresultManager : MonoSingleton<HitresultManager>
{



    public void SendHitresult(ResultGroupChart data, EnemyBase target, HeroBase caster)
    {
        List<HitresultChart> hitresults = CsvData.Ins.HitresultChart[data.Hitresult];

        if (data.Projectile == null)
        {
            for (int i = 0; i < hitresults.Count; i++)
            {
                float randNo = Random.Range(0, 100);

                if (hitresults[i].Prob >= randNo)
                {
                    if (hitresults[i].HitFx != null)
                        EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);

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
            Vector2 dir = target.transform.position - caster.ProjectileAnchor.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            caster.ProjectileAnchor.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
                    EffectManager.Ins.ShowFx(hitresults[i].HitFx, target.transform);

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
            }
            else
            {
                Debug.Log("미스");
            }

            //온힛 추가 예정?

        }
    }

}
