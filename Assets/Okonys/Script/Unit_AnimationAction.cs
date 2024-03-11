using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_AnimationAction : MonoBehaviour
{
    public Unit unit;

    private void Awake()
    {
        unit = this.gameObject.GetComponentInParent<Unit>();
    }

    public void AttackDone()
    {
        switch (unit._attackType)
        {
            case Unit.AttackType.sword:
            case Unit.AttackType.Assassin:
                unit.SetAttack();
                break;
            case Unit.AttackType.bow:
                unit.AttackMissile();
                break;
            case Unit.AttackType.magic:
                unit.AttackMissile();
                break;
            default:
                break;
        }
    }

    public void SkillDone()
    {
        switch (unit._attackType)
        {
            case Unit.AttackType.sword:
            //case Unit.AttackType.Assassin:
                unit.AttackSkill();
                break;
/*            case Unit.AttackType.bow:
                unit.AttackSkill();
                break;
            case Unit.AttackType.magic:
                unit.AttackSkill();
                break;
            default:
                break;*/
        }
    }

    public void DeathDone()
    {
        unit.SetDeathDone();
    }

    public void SkillEnd()
    {
        unit.SetState(Unit.UnitState.run);
    }
}
