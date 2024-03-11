using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;

public class SkillObj : MonoBehaviour
{
    public enum SkillType
    {
        ShortDis,
        LongDist
    }

    public SkillType _skillType = SkillType.ShortDis;

    public List<GameObject> _skillList = new List<GameObject>();

    public float _speed;
    public float _range;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInit()
    {
        _skillList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tObj = transform.GetChild(i).gameObject;
            _skillList.Add(tObj);

            if (i != (int)_skillType) tObj.SetActive(false);
            else tObj.SetActive(true);
        }

        switch (_skillType)
        {
            case SkillType.ShortDis:
                _speed = 5f;
                _timerForLim = 10f;
                _homing = false;

                _range = 0;
                _yPos = 5f;
                _yPosSave = _yPos;
                break;
            case SkillType.LongDist:
                _speed = 12f;
                _timerForLim = 10f;
                _homing = true;

                _range = 0;
                _yPos = -1f;
                _yPosSave = _yPos;
                break;
            default:
                break;
        }
    }

    public void SetSkill(SkillType type, Unit owner, Unit target)
    {
        transform.position = owner.transform.position + new Vector3(0, 0, 0);
        _startPos = transform.position;

        _skillType = type;
        _owner = owner;
        _target = target;
        _Tag = _target.tag;

        _endPos = (Vector2)_target.transform.position + new Vector2(0, 0);
        _timer = 0;

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
        }
    }

    void DoProcess()
    {
        if (_range > 0)
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
    }

    public void SkillDone()
    {
        SoonsoonData.Instance.Skill_Manager._poolListUse.Remove(this);
        gameObject.SetActive(false);
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
