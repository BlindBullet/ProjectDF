using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    BasicAttack attack;
    HeroBase me;
    bool usingSKill = false;
    bool isAttacking = false;
    Coroutine cAttack = null;

    public void Setup(HeroBase heroBase)
    {
        me = heroBase;
        HeroChart heroChart = CsvData.Ins.HeroChart[me.Data.Id];
        
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
                    if (!isAttacking)
                    {
                        cAttack = StartCoroutine(AttackSequence(me.Range.AllTargetColl[0].GetComponent<EnemyBase>()));                 
                    }   
                }
                else
                {

                }
            }

            yield return null;
        }
    }

    IEnumerator AttackSequence(EnemyBase target)
    {
        isAttacking = true;
        float totalTime = 0.2f;
        float fireTime = 0.1f;
                        
        me.Tween.Attack(me.Data.Spd > 1f ? me.Data.Spd : 1f);

        if (me.Data.Spd >= 1f)
        {
            totalTime = totalTime / me.Data.Spd;
            fireTime = fireTime / me.Data.Spd;
        }

        if (attack.Data.BeginFx != "")
        {
            
        }

        bool isCrit = Random.Range(0f, 100f) < me.Data.CritChance ? true : false;

        yield return new WaitForSeconds(fireTime);

        if (attack.Data.HitFx != "")
        {
            EffectManager.Ins.ShowFx(attack.Data.HitFx, target.transform);
        }

        double dmg = me.Data.Atk;
        double resultDmg = 0;

        if (isCrit)
            dmg = dmg * (1 + (me.Data.CritDmg / 100f));

        Vector3 pos = target.transform.position;
        resultDmg = target.TakeDmg(dmg);
        FloatingTextManager.Ins.ShowDmg(pos, resultDmg.ToString());

        yield return new WaitForSeconds((1f / me.Data.Spd) - fireTime);

        isAttacking = false;
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