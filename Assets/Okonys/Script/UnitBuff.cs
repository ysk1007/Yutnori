using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff : MonoBehaviour
{
    public Unit unit;

    public float _Recovery; // 회복량
    public float _BuffAT; // 공격 상승 버프
    public float _BuffAS; // 공격 속도 상승 버프
    public float _BuffDF; // 방어 상승 버프
    public float _BuffSD; // 보호막 버프
    public float _BuffDuration; // 버프 지속 시간
    public float timer; // 버프 타이머

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= _BuffDuration)
            ReturnBuff();
    }

    public void Init(float RC, float AT, float AS, float DF, float SD, float Duration)
    {
        if (!unit)
            unit = this.gameObject.transform.parent.GetComponentInParent<Unit>();
        gameObject.SetActive(true);
        timer = 0;
        _BuffDuration = Duration;
        _BuffAT = AT;
        _BuffAS = AS;
        _BuffDF = DF;
        _BuffSD = SD;

        if (RC > 0) unit.SetHeal(unit, RC);
        unit._buffAT += _BuffAT;
        unit._buffAS += _BuffAS;
        unit._buffDF += _BuffDF;
        unit._buffSD += _BuffSD;
        unit._unitSD += _BuffSD;
    }

    public void ReturnBuff()
    {
        unit._buffAT -= _BuffAT;
        unit._buffAS -= _BuffAS;
        unit._buffDF -= _BuffDF;
        if (unit._buffSD - _BuffSD < 0)
            unit._buffSD = 0;
        else
            unit._buffSD -= _BuffSD;
        gameObject.SetActive(false);
    }
}
