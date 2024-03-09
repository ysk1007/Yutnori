using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    public SPUM_Prefabs _spumPref;

    // ���� ����
    public enum UnitState 
    {
        idle,
        run,
        attack,
        stun,
        skill,
        death,
    }

    public UnitState _unitState = UnitState.idle;

    public Unit _target;

    public float _unitRate; // ���
    public float _unitHp; // ü��
    public float _unitAT; // ���ݷ�
    public float _unitAR; // �����Ÿ�
    public float _unitAS; // ���� �ӵ�
    public float _unitMS; // �̵� �ӵ�
    public float _unitDF; // ����
    public float _unitCC; // ũ��Ƽ�� ����
    public float _unitCT; // ��Ÿ��
    public float _unitFR; // ���� ��Ī ����

    // Move
    public Vector2 _dirVec;
    public Vector2 _tempDis;

    public float _findTimer;
    public float _attackTimer;
    // Start is called before the first frame update
    void Start()
    {
        _spumPref = this.gameObject.GetComponent<SPUM_Prefabs>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }

    // Z�� ����
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
                break;
            case UnitState.death:
                break;
        }
    }

    void SetState(UnitState state)
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
                _spumPref.PlayAnimation(4);
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
        if (_findTimer > SoonsoonData.Instance.Unit_Manager._findTimer)
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
        if (!CheckTarget()) return; // Ÿ���� ������ ��ȯ
        if (!CheckDistance()) return; // ���� ��Ÿ� �ƴ϶�� ��ȯ

        _attackTimer += Time.deltaTime;
        if (_attackTimer > _unitAS)
        {
            DoAttack();
            _attackTimer = 0;
        }
    }

    void DoAttack()
    {
        _spumPref.PlayAnimation("2");
    }

    void SetDirection()
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
            SetState(UnitState.idle);
        }
        return value;
    }
}
