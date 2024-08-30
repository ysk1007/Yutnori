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
    public float _BuffCC; // 크리티컬 확률 버프
    public float _BuffSD; // 보호막 버프
    public float _BuffDuration; // 버프 지속 시간
    public bool _isDebuff = false; // 디버프 플래그
    public float timer; // 버프 타이머

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= _BuffDuration)
            ReturnBuff();
    }

    public void Init(float RC, float AT, float AS, float DF, float CC, float SD, float Duration, bool Debuff)
    {
        if (!unit)
            unit = this.gameObject.transform.parent.GetComponentInParent<Unit>();
        gameObject.SetActive(true);
        timer = 0;
        _BuffDuration = Duration;
        _BuffAT = AT;
        _BuffAS = AS;
        _BuffDF = DF;
        _BuffCC = CC;
        _BuffSD = SD;
        _isDebuff = Debuff;

        ApplyBuffs(RC, AT, AS, DF, CC, SD);
    }

    private void ApplyBuffs(float RC, float AT, float AS, float DF, float CC, float SD)
    {
        switch (_isDebuff)
        {
            case true:
                unit._deBuffAT += AT;
                unit._deBuffAS += AS;
                unit._deBuffDF += DF;
                unit._deBuffCC += CC;
                break;
            case false:
                if (RC > 0) unit.SetHeal(unit, RC);
                unit._buffAT += AT;
                unit._buffAS += AS;
                unit._buffDF += DF;
                unit._buffCC += CC;
                unit._buffSD += SD;
                unit._unitSD += SD;
                break;
        }
    }

    public void ReturnBuff()
    {
        switch (_isDebuff)
        {
            case true:
                unit._deBuffAT -= _BuffAT;
                unit._deBuffAS -= _BuffAS;
                unit._deBuffDF -= _BuffDF;
                unit._deBuffCC -= _BuffCC;
                break;
            case false:
                unit._buffAT -= _BuffAT;
                unit._buffAS -= _BuffAS;
                unit._buffDF -= _BuffDF;
                unit._buffCC -= _BuffCC;
                if (unit._buffSD - _BuffSD < 0)
                    unit._buffSD = 0;
                else
                    unit._buffSD -= _BuffSD;
                break;
        }
        gameObject.SetActive(false);
    }
}
