using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    public SPUM_Prefabs _spumPref;
    public Unit_SubSet _unit_SubSet;

    // 유닛 상태
    public enum UnitState 
    {
        idle,
        run,
        attack,
        attacking,
        stun,
        skill,
        skilling,
        death,
    }

    public enum AttackType
    {
        sword,
        bow,
        magic,
        Assassin,
        healer
    }

    public UnitState _unitState = UnitState.idle;
    public AttackType _attackType = AttackType.sword;

    public Unit _target;

    public float _unitRate; // 등급
    public float _unitMaxHp; // 최대 체력
    public float _unitHp; // 체력
    public float _unitAT; // 공격력
    public float _unitAR; // 사정거리
    public float _unitAS; // 공격 속도
    public float _unitMS; // 이동 속도
    public float _unitDF; // 방어력
    public float _unitCC; // 크리티컬 찬스
    public float _unitCT; // 쿨타임
    public float _unitFR; // 유닛 서칭 범위

    public SkillData _unitSkill;

    // Move
    public Vector2 _dirVec;
    public Vector2 _tempDis;

    public float _findTimer;
    public float _attackTimer;
    public float _skillTimer;
    // Start is called before the first frame update
    void Start()
    {
        _spumPref = this.gameObject.GetComponent<SPUM_Prefabs>();
        _unit_SubSet = this.gameObject.GetComponentInChildren<Unit_SubSet>();
        if (_attackType == AttackType.Assassin)
            Invoke("Dash",0.3f);
    }



    // Update is called once per frame
    void Update()
    {
        CheckState();
        if (_unitState != UnitState.skill)
            _skillTimer += Time.deltaTime;
    }

    // Z축 정렬
    void SetZpos()
    {
        Vector3 tPos = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.1f);
        transform.localPosition = tPos;
    }

    void CheckState()
    {
        switch (_unitState)
        {
            case UnitState.idle:
                FindTarget();
                break;
            case UnitState.run:
                FindTarget();
                Move();
                break;
            case UnitState.attack:
                CheckAttack();
                break;
            case UnitState.stun:
                break;
            case UnitState.skill:
                CheckSkill();
                break;
            case UnitState.death:
                break;
        }
    }

    public void SetState(UnitState state)
    {
        _unitState = state;
        switch (_unitState)
        {
            case UnitState.idle:
                _spumPref.PlayAnimation(0);
                break;
            case UnitState.run:
                _spumPref.PlayAnimation(1);
                break;
            case UnitState.attack:
                _spumPref.PlayAnimation(0);
                break;
            case UnitState.stun:
                _spumPref.PlayAnimation(3);
                break;
            case UnitState.skill:
                _spumPref.PlayAnimation(7);
                break;
            case UnitState.death:
                _spumPref.PlayAnimation(2);
                break;
        }
    }

    void FindTarget()
    {
        _findTimer += Time.deltaTime;
        if (_target == null)
        {
            _target = SoonsoonData.Instance.Unit_Manager.GetTraget(this);
            if (_target != null) SetState(UnitState.run);
            else SetState(UnitState.idle);
            _findTimer = 0;
        }
    }

    void Move()
    {
        if(!CheckTarget()) return;
        CheckDistance();
        _dirVec = _tempDis.normalized;
        SetDirection();
        transform.position += (Vector3)_dirVec * _unitMS * Time.deltaTime;
    }

    bool CheckDistance() // 공격 사거리 체크
    {
        _tempDis = (Vector2)(_target.transform.localPosition - transform.position);

        float tDis = _tempDis.sqrMagnitude;

        if (tDis <= _unitAR * _unitAR)
        {
            SetState(UnitState.attack);
            return true;
        }
        else
        {
            if (!CheckTarget()) SetState(UnitState.idle);
            else SetState(UnitState.run);
            return false;
        }
    }

    void CheckAttack()
    {
        if (!CheckTarget())
        {
            FindTarget();
            return; // 타겟이 없으면 반환
        }
        if (!CheckDistance()) return; // 공격 사거리 아니라면 반환

        if (CheckSkill()) return;

        _attackTimer += Time.deltaTime;
        if (_attackTimer > _unitAS)
        {
            DoAttack();
            _attackTimer = 0;
        }
    }

    bool CheckSkill()
    {
        if (_skillTimer > _unitCT && _unitState != UnitState.attacking)
        {
            SetState(UnitState.skill);
            DoSkill();
            _skillTimer = 0;
            return true;
        }
        else
            return false;
    }

    void DoAttack()
    {
        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;
        SetDirection();
        SetState(UnitState.attacking);
        switch (_attackType)
        {
            case AttackType.sword:
            case AttackType.Assassin:
                _spumPref.PlayAnimation(4);
                break;
            case AttackType.bow:
                _spumPref.PlayAnimation(5);
                break;
            case AttackType.magic:
            case AttackType.healer:
                _spumPref.PlayAnimation(6);
                break;
        }
    }

    void DoSkill()
    {
        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;
        SetDirection();
        SetState(UnitState.skilling);
        switch (_attackType)
        {
            case AttackType.sword:
            case AttackType.Assassin:
                _spumPref.PlayAnimation(7);
                break;
            case AttackType.bow:
                _spumPref.PlayAnimation(8);
                break;
            case AttackType.magic:
            case AttackType.healer:
                _spumPref.PlayAnimation(9);
                break;
        }
    }

    public void SetAttack(Unit target = null)
    {
        float dmg = _unitAT;
        if (target == null)
        {
            _target.SetDamage(this, dmg);
        }
        else
        {
            target.SetDamage(this, dmg);
        }
    }

    public void SetAttack(float Count, Unit target = null)
    {
        float dmg = _unitAT * Count;
        if (target == null)
        {
            _target.SetDamage(this, dmg);
        }
        else
        {
            target.SetDamage(this, dmg);
        }
    }

    public void AttackMissile()
    {
        switch (_attackType)
        {
            case AttackType.bow:
                SoonsoonData.Instance.Missile_Manager.FireMissile(MissileObj.MissileType.Arrow, this, _target);
                break;
            case AttackType.magic:
            case AttackType.healer:
                SoonsoonData.Instance.Missile_Manager.FireMissile(MissileObj.MissileType.FireBall, this, _target);
                break;
        }
    }

    public void AttackSkill()
    {
        switch (_unitSkill.skillType)
        {
            case SkillData.SkillType.ShortRange:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.ShortRange, this, _target, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.LongRange:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.LongRange, this, _target, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.LeastHeal:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.LeastHeal, this, SoonsoonData.Instance.Unit_Manager.GetLeastTeam(this), _unitSkill.Duration, _unitSkill) ;
                break;
        }
    }

    public void SetDamage(Unit target, float dmg)
    {
        switch (target._attackType)
        {
            case AttackType.sword:
            case AttackType.Assassin:
            case AttackType.bow:
            case AttackType.magic:
            case AttackType.healer:
                SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Hit, null, this.transform.position, false, 0.5f);
                break;
        }
        float newDmg = dmg - _unitDF * 70/100;

        if (newDmg < 0)
            newDmg = 0;
        Debug.Log(newDmg);
        _unitHp -= newDmg;

        // 데미지 텍스트
        _unit_SubSet.ShowDamageText(newDmg);

        if (_unitHp <= 0)
        {
            SetDeath();
        }
    }

    public void SetHeal(Unit target, float dmg)
    {
        float newDmg = dmg * target._unitAT;
        SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Heal, null, this.transform.position, false, 0.5f);
        _unitHp += newDmg;
        if (_unitHp > _unitMaxHp)
        {
            _unitHp = _unitMaxHp;
        }

        // 회복 텍스트
        _unit_SubSet.ShowHealText(newDmg);
    }

        public void SetDeath()
    {
        switch (gameObject.tag)
        {
            case "P1":
                SoonsoonData.Instance.Unit_Manager._p1UnitList.Remove(this);
                break;

            case "P2":
                SoonsoonData.Instance.Unit_Manager._p1UnitList.Remove(this);
                break;
        }
        SetState(UnitState.death);
    }

    public void SetDeathDone()
    {
        gameObject.SetActive(false);
    }

    public void SetDirection()
    {
        if (_dirVec.x >= 0)
        {
            _spumPref._anim.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            _spumPref._anim.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    bool CheckTarget() //타겟 검사 함수
    {
        bool value = true;
        if (_target == null) // 타겟이 없음
        {
            value = false;
            _target = null;
            return false;
        }
        if (_target.gameObject == null) // 타겟 오브젝트가 없을 때
        {
            value = false;
            _target = null;
            return false;
        }
        if (_target._unitState == UnitState.death) // 타겟이 죽음
        {
            value = false;
            _target = null;
            return false;
        }
        if (!_target.gameObject.activeInHierarchy) // 타겟 오브젝트 활동 상태가 false
        {
            value = false;
            _target = null;
            return false;
        }

        if (!value)
        {
            SetState(UnitState.idle);
            _target = null;
        }
        return value;
    }
    void Dash()
    {
        string temp = gameObject.tag;
        Vector3 tPos;
        switch (gameObject.tag)
        {
            case "P1":
                gameObject.tag = "Ghost";
                tPos = new Vector3(6, transform.position.y, transform.position.z);
                transform.localPosition = tPos;
                break;

            case "P2":
                gameObject.tag = "Ghost";
                tPos = new Vector3(-6, transform.position.y, transform.position.z);
                transform.localPosition = tPos;
                break;
        }
        _target = null;
        FindTarget();
        gameObject.tag = temp;
    }
}
