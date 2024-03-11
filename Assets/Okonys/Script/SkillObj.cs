using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class SkillObj : MonoBehaviour
{
    public enum SkillType
    {
        ShortRange,
        LongRange,
        Buff
    }

    public SkillType _skillType = SkillType.ShortRange;

    public List<GameObject> _skillList = new List<GameObject>();

    public int _skillID;
    public float _speed;
    public float _rangeX;
    public float _rangeY;
    public float _yPos;
    public float _yPosSave;
    public bool _homing;
    public float _timer;
    public float _timerForLim;
    public string _Tag;

    public Unit _owner;
    public Unit _target;

    public Vector2 _startPos;
    public Vector2 _endPos;

    public BoxCollider _collider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DoProcess();
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
                _speed = 5f;
                _homing = false;
                _yPos = 5f;
                _yPosSave = _yPos;
                break;
            case SkillType.LongRange:
                _speed = 12f;
                _homing = true;
                _yPos = -1f;
                _yPosSave = _yPos;
                break;
            default:
                break;
        }
    }

    public void SetSkill(SkillType type, Unit owner, Unit target, float timer, SkillData skillData)
    {
        transform.position = owner.transform.position + new Vector3(0, 0, 0);

        _skillID = skillData.SkillID;
        _skillType = type;
        _owner = owner;
        _target = target;
        _Tag = _target.tag;

        _rangeX = skillData.xRange;
        _rangeY = skillData.yRange;

        _timer = 0;
        _timerForLim = timer;

        _startPos = transform.position;

        if (type == SkillType.LongRange)
            _endPos = (Vector2)_target.transform.position + new Vector2(0, 0);
        else
            _endPos = _startPos;


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
                _endPos = (Vector2)_target.transform.position + new Vector2(0, 0.25f);
            }
        }

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
                    SkillDone();
                }
                break;
            case SkillType.Buff:
                break;
            default:
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
                _owner.SetAttack(collider.gameObject.GetComponent<Unit>());
            }
            else
            {
                if (TargetCheck()) _owner.SetAttack();
            }
        }
    }

    public void SkillDone()
    {
        SoonsoonData.Instance.Skill_Manager._poolListUse.Remove(this);
        gameObject.SetActive(false);
        _owner._attackTimer = 0;
        _owner.SetState(UnitState.attack);
    }

    public bool TargetCheck()
    {
        bool value = true;
        if (_target == null) // 타겟이 없음
        {
            value = false;
        }
        if (_target.gameObject == null) // 타겟 오브젝트가 없을 때
        {
            value = false;
        }
        if (_target._unitState == UnitState.death) // 타겟이 죽음
        {
            value = false;
        }
        if (!_target.gameObject.activeInHierarchy) // 타겟 오브젝트 활동 상태가 false
        {
            value = false;
        }

        if (!value)
        {

        }
        return value;
    }
}
