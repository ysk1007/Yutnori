using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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
        Warrior = 0,
        Archer = 1,
        Wizard = 2,
        Assassin = 3,
        Healer = 4,
        Merchant = 5,
        Object = 6
    }

    public enum UnitType
    {
        Human = 0,
        Ghost = 1,
        Great = 2,
        Devil = 3,
    }

    public int _fieldindex;

    public UnitState _unitState = UnitState.idle;
    public AttackType _attackType = AttackType.Warrior;
    public UnitType _unitType = UnitType.Human;

    public Unit _target;

    public UnitData _unitData;
    public UnitData.RateType _unitRate; // 등급
    public float _unitMaxHp; // 최대 체력
    public float _unitHp; // 체력
    public float _unitSD; // 보호막
    public float _unitAT; // 공격력
    public float _unitAR; // 사정거리
    public float _unitAS; // 공격 속도
    public float _unitMS; // 이동 속도
    public float _unitDF; // 방어력
    public float _unitCC; // 크리티컬 찬스
    public float _unitCT; // 쿨타임
    public float _unitFR; // 유닛 서칭 범위

    public float _buffAT = 1; // 공격력 버프
    public float _buffAS = 1; // 공격 속도 버프
    public float _buffDF = 1; // 방어력 버프
    public float _buffCC = 0; // 치명타 확률 버프
    public float _buffSD = 0; // 보호막 버프

    public float _deBuffAT = 0; // 공격력 디버프
    public float _deBuffAS = 0; // 공격 속도 디버프
    public float _deBuffDF = 0; // 방어력 디버프
    public float _deBuffCC = 0; // 치명타 확률 디버프

    public float _damageinflicted = 0; // 가한 데미지

    public bool _wizardPower = false;
    public bool _merchantPower = false;
    public bool _ghostPower = false;
    public float _ghostTime = 0;
    public bool _ghostDieFlag = false;


    public Transform _buffPool;
    public List<UnitBuff> BuffList = new List<UnitBuff>();

    public SkillData _unitSkill;

    // Move
    public Vector2 _dirVec;
    public Vector2 _tempDis;

    public float _findTimer;
    public float _attackTimer;
    public float _skillTimer;
    public float _ghostTimer;

    UserInfoManager _userInfoManager;

    void Awake()
    {
        init();
        GetBuffList();
    }

    // Start is called before the first frame update
    void Start()
    {
        _spumPref = this.gameObject.GetComponent<SPUM_Prefabs>();
        _unit_SubSet = this.gameObject.GetComponentInChildren<Unit_SubSet>();
        _userInfoManager = UserInfoManager.Instance;
    }



    // Update is called once per frame
    void Update()
    {
        if (SoonsoonData.Instance.Unit_Manager._gamePause) return;
        if (_attackType == AttackType.Object) return;

        CheckState();
        if (_unitState != UnitState.skill)
            _skillTimer += Time.deltaTime;

        if (_ghostDieFlag)
            if (_ghostTimer > 0)
                _ghostTimer -= Time.deltaTime;
            else
                SetDeath();
    }

    public void init()
    {
        if (!_unitData) return;
        _attackType = _unitData.AttackType;
        _unitType = _unitData.UnitType;
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

        _target = null;
        _wizardPower = false;
        _merchantPower = false;
        _ghostPower = false;
        _ghostTime = 0;
        _ghostDieFlag = false;

        _findTimer = 0;
        _attackTimer = 0;
        _skillTimer = 0;
        _ghostTimer = 0;
}

    // Z축 정렬
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
            if (_target != null && !SoonsoonData.Instance.Unit_Manager._gamePause) SetState(UnitState.run);
            else SetState(UnitState.idle);
            _findTimer = 0;
        }
    }

    void Move()
    {
        if(!CheckTarget()) return;
        if (CheckSkill()) return; // 이동중 스킬 쓸지 말지
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
        if (_attackTimer > 3 - (_unitAS * (_buffAS - _deBuffAS)))
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
            case AttackType.Warrior:
            case AttackType.Assassin:
            case AttackType.Merchant:
                _spumPref.PlayAnimation(4);
                break;
            case AttackType.Archer:
                _spumPref.PlayAnimation(5);
                break;
            case AttackType.Wizard:
            case AttackType.Healer:
                _spumPref.PlayAnimation(6);
                break;
        }

        if (_wizardPower) _skillTimer += 1;
    }

    void DoSkill()
    {
        _dirVec = (Vector2)(_target.transform.position - transform.position).normalized;
        SetDirection();
        SetState(UnitState.skilling);
        switch (_attackType)
        {
            case AttackType.Warrior:
            case AttackType.Assassin:
            case AttackType.Merchant:
                _spumPref.PlayAnimation(7);
                break;
            case AttackType.Archer:
                _spumPref.PlayAnimation(8);
                break;
            case AttackType.Wizard:
            case AttackType.Healer:
                _spumPref.PlayAnimation(9);
                break;
        }
    }

    public void SetAttack(Unit target = null)
    {
        float dmg = _unitAT * (_buffAT - _deBuffAT);
        bool critical = GetRandomBool();
        dmg *= critical ? 2 : 1;
        dmg += (_merchantPower) ? dmg * (_userInfoManager.userData.GetUserGold()/10000) : 0; 
        if (target == null)
        {
            _target?.SetDamage(this, dmg, critical);
        }
        else
        {
            target.SetDamage(this, dmg, critical);
        }
    }

    public void SetAttack(float Count, Unit target = null)
    {
        float dmg = _unitAT * (_buffAT - _deBuffAT) * Count;
        bool critical = GetRandomBool();
        dmg *= critical == true ? 2 : 1;
        if (target == null)
        {
            _target.SetDamage(this, dmg, critical);
        }
        else
        {
            target.SetDamage(this, dmg, critical);
        }
    }

    public void AttackMissile()
    {
        switch (_attackType)
        {
            case AttackType.Archer:
                SoonsoonData.Instance.Missile_Manager.FireMissile(MissileObj.MissileType.Arrow, this, _target);
                break;
            case AttackType.Wizard:
            case AttackType.Healer:
                SoonsoonData.Instance.Missile_Manager.FireMissile(MissileObj.MissileType.FireBall, this, _target);
                break;
        }
    }

    public void AttackSkill()
    {
        List<Unit> targets = new List<Unit>();

        switch (_unitSkill.targetType)
        {
            case SkillData.TargetType.None:
                targets.Add(_target);
                break;
            case SkillData.TargetType.Solo:
                targets.Add(this);
                break;
            case SkillData.TargetType.CurrentTarget:
                targets.Add(_target);
                break;
            case SkillData.TargetType.LeastHp:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetLeastTeam(this));
                break;
            case SkillData.TargetType.LeastHpEnemy:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetLeastEnemy(this));
                break;
            case SkillData.TargetType.MostHp:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetGreatestHpTarget(this, false));
                break;
            case SkillData.TargetType.MostHpEnemy:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetGreatestHpTarget(this, true));
                break;
            case SkillData.TargetType.MySquad:
                targets = SoonsoonData.Instance.Unit_Manager.GetSquadTeam(this, false);
                break;
            case SkillData.TargetType.EnemySquad:
                targets = SoonsoonData.Instance.Unit_Manager.GetSquadTeam(this, true);
                break;
            case SkillData.TargetType.FarAwayEnemy:
                targets.Add(SoonsoonData.Instance.Unit_Manager.GetFarAwayTraget(this));
                break;
            default:
                targets.Add(_target);
                break;
        }

        switch (_unitSkill.skillType)
        {
            case SkillData.SkillType.ShortRange:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.ShortRange, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.LongRange:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.LongRange, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.Heal:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.Heal, this, targets, _unitSkill.Duration, _unitSkill) ;
                break;
            case SkillData.SkillType.Buff:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.Buff, this, targets, _unitSkill.Duration, _unitSkill);
                break;
            case SkillData.SkillType.Debuff:
                SoonsoonData.Instance.Skill_Manager.RunSkill(SkillObj.SkillType.Debuff, this, targets, _unitSkill.Duration, _unitSkill);
                break;
        }
    }

    public void SetDamage(Unit target, float dmg, bool critical)
    {
        switch (target._attackType)
        {
            case AttackType.Warrior:
            case AttackType.Assassin:
            case AttackType.Archer:
            case AttackType.Wizard:
            case AttackType.Healer:
            case AttackType.Merchant:
                SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Hit, null, this.transform.position, false, 1f);
                break;
        }


        float L = 0.8f; // 최대 피해감소 값
        float k = 0.05f; // 경사도
        float x0 = 50f; // 중심점

        // x 값이 0에 가까워질수록 y가 0에 수렴하고, x 값이 100에 가까워질수록 y가 0.8에 수렴하는 시그모이드 함수
        float DecreaseDamage = L / (1 + Mathf.Exp(-k * (_unitDF * (_buffDF - _deBuffDF) - x0)));

        float newDmg = Mathf.Floor((dmg - (dmg * DecreaseDamage)));

        if (newDmg < 0)
            newDmg = 0;

        if (_buffSD > 0)
        {
            _buffSD -= newDmg;
            if (_buffSD < 0)
            {
                _unitHp -= Mathf.Abs(_buffSD);
                _buffSD = 0;
            }
        }
        else
        {
            _unitHp -= newDmg;
        }

        target._damageinflicted += newDmg;

        // 데미지 텍스트
        _unit_SubSet.ShowDamageText(newDmg, critical);

        if (_unitHp <= 0)
        {
            if (_ghostPower) _ghostDieFlag = true;
            else SetDeath();
        }
    }

    public void SetHeal(Unit target, float value)
    {
        if (_ghostDieFlag) return;
        SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Heal, null, this.transform.position, false, 1f);
        _unitHp += value;
        if (_unitHp > _unitMaxHp)
        {
            _unitHp = _unitMaxHp;
        }

        // 회복 텍스트
        _unit_SubSet.ShowHealText(value);
    }

    public void UnitBuff(float RC, float AT, float AS, float DF, float CC, float SD, float Duration, bool Debuff)
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
            newBuff.Init(RC, AT, AS, DF, CC, SD, Duration, Debuff);
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
                SoonsoonData.Instance.Unit_Manager._p2UnitList.Remove(this);
                break;
        }
        SetState(UnitState.death);
    }

    public void SetDeathDone()
    {
        // 유닛이 죽을때 승패 결과를 확인
        SoonsoonData.Instance.Unit_Manager.CheckGameResult(this);
        if (this.tag == "P2") _userInfoManager.userData.totalKillEnemy++;

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

    public void SetDash()
    {
        Invoke("Dash", 0.3f);
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

    // 주어진 확률에 따라 true 또는 false를 반환하는 함수
    bool GetRandomBool() //0.0 ~ 1.0
    {
        return Random.value <= _unitCC + _buffCC - _deBuffCC;
    }

    public void UnitReset()
    {
        init();
        for (int i = 0; i < BuffList.Count; i++)
        {
            if (BuffList[i].timer > 0) BuffList[i].ReturnBuff();
        }
        _unit_SubSet.TextListReset();
        SetState(UnitState.idle);
        switch (gameObject.tag)
        {
            case "P1":
                this.transform.position = SoonsoonData.Instance.Unit_Manager._p1fieldPos[_fieldindex];
                break;

            case "P2":
                this.transform.position = SoonsoonData.Instance.Unit_Manager._p2fieldPos[_fieldindex];
                break;
        }

        this.gameObject.SetActive(true);

    }
}
