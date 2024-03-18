using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissileObj;
using static UnityEngine.GraphicsBuffer;

public class EffectObj : MonoBehaviour
{
    public enum EffectType
    {
        Hit  = 0,
        CriticalHit = 1,
        Heal = 2,
        Buff = 3,
        Debuff = 4,
        Explosion = 5,
    }

    public EffectType _effectType = EffectType.Hit;

    public List<GameObject> _effectList = new List<GameObject>();

    public bool _homing;
    public float _timer;
    public float _timerForLim;

    GameObject _nowEffectObj;

    public Unit _owner;

    public void SetEffectObj(EffectType type, Unit owner, Vector3 pos, bool homing, float timer)
    {
        _effectType = type;
        _owner = owner;
        _homing = homing;
        _timer = 0;
        _timerForLim = timer;

        // 이펙트가 주인을 따라다닐지 말지
        if (owner != null) 
        {
            transform.position = owner.transform.position;
        }
        else
        {
            transform.position = pos;
        }

        SoonsoonData.Instance.Effect_Manager._poolListUse.Add(this);
        SetInit();
    }

    public void SetInit()
    {
        int tnum = (int)_effectType;
        _nowEffectObj = transform.GetChild(tnum).gameObject;
        _nowEffectObj.SetActive(true);
        gameObject.SetActive(true);
    }

    public void EffectMove()
    {
        if (_owner == null) return;

        if (_homing)
            transform.position = _owner.transform.position;
    }

    public void EffectDone()
    {
        SoonsoonData.Instance.Effect_Manager._poolListUse.Remove(this);
        _nowEffectObj.SetActive(false);
        gameObject.SetActive(false);
    }
}
