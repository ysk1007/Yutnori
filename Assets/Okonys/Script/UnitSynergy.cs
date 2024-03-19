using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SynergyData;
using static Unit;

public class UnitSynergy : MonoBehaviour
{
    public SynergyData synergyData;
    public SynergyData.SynergyType _synergyType;
    public SynergyData.Type _Type;

    public int Level;
    public float interval; // 버프 간격
    public float timer; // 버프 타이머
    SynergyManager sm;

    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        sm = SoonsoonData.Instance.Synergy_Manager;
        ApplySynergy();
        if(_Type == Type.Start) SpecialSynergy();
    }

    void Update()
    {
        if (_Type != Type.Continue) return;

        timer += Time.deltaTime;
        if (timer >= interval)
            SpecialSynergy();
    }

    public void Init()
    {
        if (!synergyData) return;
        _synergyType = synergyData.synergyType;
        _Type = synergyData.type;
        timer = 0;
    }

    private void ApplySynergy()
    {
        switch (_synergyType)
        {
            case SynergyData.SynergyType.Warrior:
                WarriorSynergy();
                break;
            case SynergyData.SynergyType.Archer:
                break;
            case SynergyData.SynergyType.Wizard:
                break;
            case SynergyData.SynergyType.Assassin:
                break;
            case SynergyData.SynergyType.Healer:
                break;
            case SynergyData.SynergyType.Merchant:
                break;
        }
    }

    private void SpecialSynergy()
    {
        timer = 0;
        switch (_synergyType)
        {
            case SynergyData.SynergyType.Warrior:
                WarriorSpecialSynergy();
                break;
            case SynergyData.SynergyType.Archer:
                break;
            case SynergyData.SynergyType.Wizard:
                break;
            case SynergyData.SynergyType.Assassin:
                break;
            case SynergyData.SynergyType.Healer:
                break;
            case SynergyData.SynergyType.Merchant:
                break;
        }
    }

    void WarriorSynergy()
    {
        Debug.Log("시너지");
        float Buff = 0.0f;
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1SynergyList[0] >= 5)
                {
                    Debug.Log("워리어 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.MaxHp[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p1SynergyList[0] >= 3)
                {
                    Debug.Log("워리어 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.MaxHp[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p1SynergyList[0] >= 1)
                {
                    Debug.Log("워리어 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.MaxHp[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    Debug.Log("시너지 버프");
                    sm._p1UnitList[i]._unitMaxHp += Buff;
                    sm._p1UnitList[i]._unitHp += Buff;
                }
                break;
            case "P2":
                if (sm._p2SynergyList[0] >= 5)
                {
                    Debug.Log("워리어 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.MaxHp[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p2SynergyList[0] >= 3)
                {
                    Debug.Log("워리어 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.MaxHp[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p2SynergyList[0] >= 1)
                {
                    Debug.Log("워리어 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.MaxHp[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i]._unitMaxHp += Buff;
                    sm._p2UnitList[i]._unitHp += Buff;
                }
                break;
        }
    }

    void WarriorSpecialSynergy()
    {
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    sm._p1UnitList[i].UnitBuff(0, 0, 0, 0, 0, synergyData.Shield[Level], synergyData.Duration[Level], false);
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i].UnitBuff(0, 0, 0, 0, 0, synergyData.Shield[Level], synergyData.Duration[Level], false);
                }
                break;
        }
    }
}
