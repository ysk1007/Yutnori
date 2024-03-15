using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff : MonoBehaviour
{
    public Unit unit;

    public float _Recovery; // ȸ����
    public float _BuffAT; // ���� ��� ����
    public float _BuffAS; // ���� �ӵ� ��� ����
    public float _BuffDF; // ��� ��� ����
    public float _BuffSD; // ��ȣ�� ����
    public float _BuffDuration; // ���� ���� �ð�
    public float timer; // ���� Ÿ�̸�

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
