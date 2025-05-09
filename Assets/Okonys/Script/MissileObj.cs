using ES3Types;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unit;
using static UnityEngine.GraphicsBuffer;

public class MissileObj : MonoBehaviour
{
    public enum MissileType
    {
        Arrow,
        FireBall
    }

    public MissileType _missileType = MissileType.Arrow;

    public List<GameObject> _missileList = new List<GameObject>();

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
        _missileList.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject tObj = transform.GetChild(i).gameObject;
            _missileList.Add(tObj);

            if (i != (int)_missileType) tObj.SetActive(false);
            else tObj.SetActive(true);
        }

        switch (_missileType)
        {
            case MissileType.Arrow:
                _speed = 5f;
                _timerForLim = 10f;
                _homing = false;

                _range = 0;
                _yPos = 5f;
                _yPosSave = _yPos;
                break;
            case MissileType.FireBall:
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

    public void SetMissile( MissileType type, Unit owner, Unit target)
    {
        transform.position = owner.transform.position + new Vector3(0, 0.25f, 0);
        _startPos = transform.position;

        _missileType = type;
        _owner = owner;
        _target = target;

        _Tag = _target?.tag;

        _endPos = (Vector2)_target.transform.position + new Vector2(0, 0.25f);
        _timer = 0;

        SoonsoonData.Instance.Missile_Manager._poolListUse.Add(this);
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
                tVVect = (_speed * (Vector3)tDirVec + new Vector3(0,_yPos,0));
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
        MissileDone();
    }

    public void MissileDone()
    {
        SoonsoonData.Instance.Missile_Manager._poolListUse.Remove(this);
        gameObject.SetActive(false);
    }

    public bool TargetCheck()
    {
        bool value = true;
        if (_target == null) // Ÿ���� ����
        {
            value = false;
        }
        if (_target.gameObject == null) // Ÿ�� ������Ʈ�� ���� ��
        {
            value = false;
        }
        if (_target._unitState == UnitState.death) // Ÿ���� ����
        {
            value = false;
        }
        if (!_target.gameObject.activeInHierarchy) // Ÿ�� ������Ʈ Ȱ�� ���°� false
        {
            value = false;
        }

        if (!value)
        {

        }
        return value;
    }
}
