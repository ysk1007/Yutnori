using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Unit : MonoBehaviour
{
    public SPUM_Prefabs _spumPref;
    public Unit_SubSet _unit_SubSet;

    // ���� ����
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

    public UnitData _unitData;
    public UnitData.RateType _unitRate; // ���
    public float _unitMaxHp; // �ִ� ü��
    public float _unitHp; // ü��
    public float _unitAT; // ���ݷ�
    public float _unitAR; // �����Ÿ�
    public float _unitAS; // ���� �ӵ�
    public float _unitMS; // �̵� �ӵ�
    public float _unitDF; // ����
    public float _unitCC; // ũ��Ƽ�� ����
    public float _unitCT; // ��Ÿ��
    public float _unitFR; // ���� ��Ī ����

    public float _buffAT = 1; // ���ݷ� ����
    public float _buffAS = 1; // ���� �ӵ� ����
    public float _buffDF = 1; // ���� ����

    public Transform _buffPool;
    public List<UnitBuff> BuffList = new List<UnitBuff>();

    public SkillData _unitSkill;

    // Move
    public Vector2 _dirVec;
    public Vector2 _tempDis;

    public float _findTimer;
    public float _attackTimer;
    public float _skillTimer;

    void Awake()
    {
        if (_unitData)
            init();
        GetBuffList();
    }

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

    void init()
    {
        _unitRate = _unitData._unitRate;
        _unitMaxHp = _unitData._unitMaxHp[_unitRate.GetHashCode()];
        _unitHp = _unitMaxHp;
        _unitAT = _unitData._unitAT[_unitRate.GetHashCode()];
        _unitAR = _unitData._unitAR[_unitRate.GetHashCode()];
        _unitAS = _unitData._unitAS[_unitRate.GetHashCode()];
        _unitMS = _unitData._unitMS[_unitRate.GetHashCode()];
        _unitDF = _unitData._unitDF[_unitRate.GetHashCode()];
        _unitCC = _unitData._unitCC[_unitRate.GetHashCode()];
        _unitCT = _unitData._unitCT[_unitRate.GetHashCode()];
        _unitFR = _unitData._unitFR[_unitRate.GetHashCode()];
        _unitSkill = _unitData._unitSkill;
    }

    // Z�� ����
    void SetZpos()
    {
        Vector3 tPos = new Vector3(transform.position.x, transform.position.y, transform.position.y * 0.1f);
        transform.localPosition = tPos;
    }

    public void GetBuffList()
    {
        BuffList.Clear();

        for (int i = 0; i < _buffPool.childCount; i++)
        {
            UnitBuff buff = _buffPool.GetChild(i).GetComponent<UnitBuff>();
            BuffList.Add(buff);
        }
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
        if (CheckSkill()) return; // �̵��� ��ų ���� ����
        CheckDistance();
        _dirVec = _tempDis.normalized;
        SetDirection();
        transform.position += (Vector3)_dirVec * _unitMS * Time.deltaTime;
    }

    bool CheckDistance() // ���� ��Ÿ� üũ
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
            return; // Ÿ���� ������ ��ȯ
        }

        if (!CheckDistance()) return; // ���� ��Ÿ� �ƴ϶�� ��ȯ

        if (CheckSkill()) return;

        _attackTimer += Time.deltaTime;
        if (_attackTimer > 3 - (_unitAS * _buffAS))
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
        float dmg = _unitAT * _buffAT;
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
        float dmg = _unitAT * _buffAT * Count;
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
        List<Unit> targets = new List<Unit>();
        switch (_unitSkill.skillType)
        {
            case SkillData.SkillType.ShortRange:
                targets.Add(_target);
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.ShortRange, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.LongRange:
                targets.Add(_target);
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.LongRange, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.LeastHeal:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetLeastTeam(this));
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.LeastHeal, this, targets, _unitSkill.Duration, _unitSkill) ;
                break;
            case SkillData.SkillType.SquadHeal:
                targets = SoonsoonData.Instance.Unit_Manager.GetSquadTeam(this);
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.SquadHeal, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.SoloBuff:
                targets.Add(this);
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.SoloBuff, this, targets, _unitSkill.Duration, _unitSkill);
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
                SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Hit, null, this.transform.position, false, 1f);
                break;
        }


        float L = 0.8f; // �ִ� ���ذ��� ��
        float k = 0.05f; // ��絵
        float x0 = 50f; // �߽���

        // x ���� 0�� ����������� y�� 0�� �����ϰ�, x ���� 100�� ����������� y�� 0.8�� �����ϴ� �ñ׸��̵� �Լ�
        float DecreaseDamage = L / (1 + Mathf.Exp(-k * (_unitDF * _buffDF - x0)));

        float newDmg = Mathf.Floor((dmg - (dmg * DecreaseDamage)));

        if (newDmg < 0)
            newDmg = 0;
        _unitHp -= newDmg;

        // ������ �ؽ�Ʈ
        _unit_SubSet.ShowDamageText(newDmg);

        if (_unitHp <= 0)
        {
            SetDeath();
        }
    }

    public void SetHeal(Unit target, float value)
    {
        SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Heal, null, this.transform.position, false, 1f);
        _unitHp += value;
        if (_unitHp > _unitMaxHp)
        {
            _unitHp = _unitMaxHp;
        }

        // ȸ�� �ؽ�Ʈ
        _unit_SubSet.ShowHealText(value);
    }

    public void UnitBuff(float AT, float AS, float DF, float Duration)
    {
        UnitBuff newBuff = null;

        foreach (var obj in BuffList)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                newBuff = obj;
                break;
            }
        }

        if (newBuff != null)
        {
            newBuff.Init(AT, AS, DF, Duration);
        }
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

    bool CheckTarget() //Ÿ�� �˻� �Լ�
    {
        bool value = true;
        if (_target == null) // Ÿ���� ����
        {
            value = false;
            _target = null;
            return false;
        }
        if (_target.gameObject == null) // Ÿ�� ������Ʈ�� ���� ��
        {
            value = false;
            _target = null;
            return false;
        }
        if (_target._unitState == UnitState.death) // Ÿ���� ����
        {
            value = false;
            _target = null;
            return false;
        }
        if (!_target.gameObject.activeInHierarchy) // Ÿ�� ������Ʈ Ȱ�� ���°� false
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
