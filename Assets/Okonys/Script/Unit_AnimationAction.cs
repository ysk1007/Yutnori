using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class Unit_AnimationAction : MonoBehaviour
{
    public Unit unit;
    public Animator unit_animator;

    private void Awake()
    {
        unit = this.gameObject.GetComponentInParent<Unit>();
        unit_animator = this.gameObject.GetComponent<Animator>();
    }

    public void Update()
    {
        unit_animator.SetFloat("AttackSpeed",unit._unitAS * unit._buffAS);
    }

    public void AttackDone()
    {
        unit.SetState(UnitState.attack);
        switch (unit._attackType)
        {
            case Unit.AttackType.Warrior:
            case Unit.AttackType.Assassin:
            case Unit.AttackType.Merchant:
                unit.SetAttack();    
                break;
            case Unit.AttackType.Archer:
                unit.AttackMissile();
                break;
            case Unit.AttackType.Wizard:
            case AttackType.Healer:
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
            case Unit.AttackType.Warrior:
            case Unit.AttackType.Assassin:
            case Unit.AttackType.Merchant:
                unit.AttackSkill();
                break;
            case Unit.AttackType.Archer:
                unit.AttackSkill();
                break;
            case Unit.AttackType.Wizard:
            case AttackType.Healer:
                unit.AttackSkill();
                break;
            default:
                break;
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
