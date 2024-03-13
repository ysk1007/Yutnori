using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBuff : MonoBehaviour
{
    public Unit unit;

    public float _BuffAT;
    public float _BuffAS;
    public float _BuffDF;
    public float _BuffDuration;
    public float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= _BuffDuration)
            ReturnBuff();
    }

    public void Init(float AT, float AS, float DF, float Duration)
    {
        if (!unit)
            unit = this.gameObject.transform.parent.GetComponentInParent<Unit>();
        gameObject.SetActive(true);
        timer = 0;
        _BuffDuration = Duration;
        _BuffAT = AT;
        _BuffAS = AS;
        _BuffDF = DF;

        unit._buffAT += _BuffAT;
        unit._buffAS += _BuffAS;
        unit._buffDF += _BuffDF;
    }

    public void ReturnBuff()
    {
        unit._buffAT -= _BuffAT;
        unit._buffAS -= _BuffAS;
        unit._buffDF -= _BuffDF;
        gameObject.SetActive(false);
    }
}
