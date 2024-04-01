using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EffectObj;
using static Unit;

public class SkillObj : MonoBehaviour
{
    public enum SkillType
    {
        ShortRange, // 근접 스킬
        LongRange, // 원거리 투사체 스킬
        Buff, // 버프
        Debuff, // 디버프
        Heal, // 회복
    }

    public SkillType _skillType = SkillType.ShortRange;

    public List<GameObject> _skillList = new List<GameObject>();

    public int _skillID;
    public float _damage;
    public float _recovery;
    public float _speed;
    public float _defense;
    public float _critical;
    public float _shield;
    public float _rangeX;
    public float _rangeY;
    public float _yPos;
    public float _yPosSave;
    public bool _homing;
    public float _timer;
    public float _timerForLim;
    public float _interval; // 간격
    public string _Tag;

    public Unit _owner;
    public List<Unit> _target = new List<Unit>();

    public Vector2 _startPos;
    public Vector2 _endPos;

    public BoxCollider _collider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_interval > 0)
        {
            _interval--;
            if (_skillType == SkillType.ShortRange || _skillType == SkillType.LongRange)
                DoProcess();
        }
    }

    public void SetInit()
    {
        _skillList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tObj = transform.GetChild(i).gameObject;
            _skillList.Add(tObj);

            if (i != _skillID) tObj.SetActive(false);
            else tObj.SetActive(true);
        }

        _collider.size = new Vector3(_rangeX, _rangeY,3f);
        switch (_skillType)
        {
            case SkillType.ShortRange:
                _homing = false;
                _yPos = 0;
                _yPosSave = _yPos;
                if (_owner.transform.position.x > _target[0].transform.position.x) this.transform.localScale = new Vector3(-1, 1, 1);
                break;
            case SkillType.LongRange:
                _homing = true;
                _yPos = 1f;
                _yPosSave = _yPos;
                break;
            default:
                break;
        }
    }

    public void SetSkill(SkillType type, Unit owner, List<Unit> target, float timer, SkillData skillData)
    {
        transform.position = owner.transform.position + new Vector3(0, 0, 0);

        _damage = skillData.Damage;
        _recovery = owner._unitAT * skillData.Recovery;
        _speed = skillData.Speed;
        _defense = skillData.Defense;
        _critical = skillData.Critical;
        _shield = skillData.Shield;
        _skillID = skillData.SkillID - 1;
        _skillType = type;
        _owner = owner;
        _target.Clear();
        _target = target;
        _Tag = _target[0].tag;
        _interval = skillData.DamageInterval;

        _rangeX = skillData.xRange;
        _rangeY = skillData.yRange;

        _timer = 0;
        _timerForLim = timer;

        _startPos = transform.position;

        if (type == SkillType.LongRange)
            _endPos = (Vector2)_target[0].transform.position + new Vector2(0, 0);
        else
            _endPos = _startPos;

        this.transform.localScale = new Vector3(1, 1, 1);
        SoonsoonData.Instance.Skill_Manager._poolListUse.Add(this);
        SetInit();

        gameObject.SetActive(true);
    }

    public void DoMove()
    {
        if (_homing)
        {
            if (TargetCheck())
            {
                _endPos = (Vector2)_target[0].transform.position + new Vector2(0, 0.25f);
            }
        }
        transform.localRotation = Quaternion.identity;
        switch (_skillType)
        {
            case SkillType.ShortRange:
                if (_timer >= _timerForLim)
                    SkillDone();
                break;
            case SkillType.LongRange:
                Vector2 tVec = _endPos - (Vector2)transform.position;

                float tDis = tVec.sqrMagnitude;
                if (tDis > 0.1f)
                {

                    Vector2 tDirVec = (tVec).normalized;
                    Vector3 tVVect;
                    if (_yPos == -1f)
                    {
                        tVVect = (_speed * (Vector3)tDirVec);
                    }
                    else
                    {
                        _yPos -= _yPosSave * Time.deltaTime;
                        tVVect = (_speed * (Vector3)tDirVec + new Vector3(0, _yPos, 0));
                    }

                    transform.position += tVVect * Time.deltaTime;
                    transform.up = tVVect;
                }
                else
                {
                    DoProcess();
                    SkillDone();
                }
                break;
            case SkillType.Heal:
                if (_timer >= _timerForLim || _interval <= 0)
                    SkillDone();
                else
                {
                    HealProcess();
                }
                break;
            case SkillType.Buff:
            case SkillType.Debuff:
                if (_timer >= _timerForLim)
                    SkillDone();
                else
                {
                    if (_interval <= 0)
                        return;
                    BuffProcess();
                }
                break;
        }

    }

/*    void DoProcess()
    {
        if ( > 0)
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, 1f);
            if (hit.Length > 0)
            {
                foreach (var obj in hit)
                {
                    if (obj.CompareTag(_Tag))
                    {
                        _owner.SetAttack(obj.GetComponent<Unit>());
                    }
                }
            }
        }
        else
        {
            if (TargetCheck()) _owner.SetAttack();
        }
        SkillDone();
    }*/

    void DoProcess()
    {
        Collider[] colliders = Physics.OverlapBox(_collider.bounds.center, _collider.bounds.extents);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag(_Tag))
            {
                _owner.SetAttack(_damage, collider.gameObject.GetComponent<Unit>());
                int val = _skillID + 5;
                EffectType num = (EffectType)val;
                SoonsoonData.Instance.Effect_Manager.SetEffect(num, null, _target[0].transform.position, false, 1.5f);
            }
        }
    }

    void HealProcess()
    {
        float value = _owner._unitAT * _damage;
        foreach (var unit in _target)
        {
            unit.SetHeal(unit, value);
        }
    }

    void BuffProcess()
    {
        Debug.Log("버프");
        bool BuffType = (_skillType == SkillType.Debuff) ? true : false;
        switch (BuffType)
        {
            case true:
                foreach (var unit in _target)
                {
                    SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Debuff, unit, unit.transform.position, true, _timerForLim);
                    unit.UnitBuff(_recovery, _damage, _speed, _defense, _critical, _shield, _timerForLim, BuffType);
                }
                break;
            case false:
                foreach (var unit in _target)
                {
                    SoonsoonData.Instance.Effect_Manager.SetEffect(EffectObj.EffectType.Buff, unit, unit.transform.position, true, _timerForLim);
                    unit.UnitBuff(_recovery, _damage, _speed, _defense, _critical, _shield, _timerForLim, BuffType);
                }
                break;
        }
    }

    public void SkillDone()
    {
        SoonsoonData.Instance.Skill_Manager._poolListUse.Remove(this);
        gameObject.SetActive(false);
        _owner._attackTimer = 0;
        _owner.SetState(UnitState.run);
    }

    public bool TargetCheck()
    {
        bool value = true;
        if (_target == null) // 타겟이 없음
        {
            value = false;
        }
        if (_target[0].gameObject == null) // 타겟 오브젝트가 없을 때
        {
            value = false;
        }
        if (_target[0]._unitState == UnitState.death) // 타겟이 죽음
        {
            value = false;
        }
        if (!_target[0].gameObject.activeInHierarchy) // 타겟 오브젝트 활동 상태가 false
        {
            value = false;
        }

        if (!value)
        {

        }
        return value;
    }
}
