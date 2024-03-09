using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    public SPUM_Prefabs _spumPref;

    // 유닛 상태
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

    public float _unitRate; // 등급
    public float _unitHp; // 체력
    public float _unitAT; // 공격력
    public float _unitAR; // 사정거리
    public float _unitAS; // 공격 속도
    public float _unitMS; // 이동 속도
    public float _unitDF; // 방어력
    public float _unitCC; // 크리티컬 찬스
    public float _unitCT; // 쿨타임
    public float _unitFR; // 유닛 서칭 범위

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
        if (!CheckTarget()) return; // 타겟이 없으면 반환
        if (!CheckDistance()) return; // 공격 사거리 아니라면 반환

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

    bool CheckTarget() //타겟 검사 함수
    {
        bool value = true;
        if (_target == null) // 타겟이 없음
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
            SetState(UnitState.idle);
        }
        return value;
    }
}
