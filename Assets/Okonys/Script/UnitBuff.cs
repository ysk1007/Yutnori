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
    public float _BuffCC; // ũ��Ƽ�� Ȯ�� ����
    public float _BuffSD; // ��ȣ�� ����
    public float _BuffDuration; // ���� ���� �ð�
    public bool _isDebuff = false; // ����� �÷���
    public float timer; // ���� Ÿ�̸�

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
