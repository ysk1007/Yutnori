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
        Init();
        ApplySynergy();
        if(_Type == Type.Start) SpecialSynergy();
    }

    void Update()
    {
        if (SoonsoonData.Instance.Unit_Manager._gamePause) return;
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
        Level = 0;
        interval = 0;
    }

    private void ApplySynergy()
    {
        switch (_synergyType)
        {
            case SynergyData.SynergyType.Warrior:
                WarriorSynergy();
                break;
            case SynergyData.SynergyType.Archer:
                ArcherSynergy();
                break;
            case SynergyData.SynergyType.Wizard:
                WizardSynergy();
                break;
            case SynergyData.SynergyType.Assassin:
                AssassinSynergy();
                break;
            case SynergyData.SynergyType.Healer:
                HealerSynergy();
                break;
            case SynergyData.SynergyType.Merchant:
                break;
            case SynergyData.SynergyType.Human:
                HumanSynergy();
                break;
            case SynergyData.SynergyType.Ghost:
                GhostSynergy();
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
                ArcherSpecialSynergy();
                break;
            case SynergyData.SynergyType.Wizard:
                break;
            case SynergyData.SynergyType.Assassin:
                AssassinSpecialSynergy();
                break;
            case SynergyData.SynergyType.Healer:
                HealerSpecialSynergy();
                break;
            case SynergyData.SynergyType.Merchant:
                break;
        }
    }

    void WarriorSynergy()
    {
        Debug.Log("시너지");
        float BuffHP = 0.0f;
        float BuffDF = 0.0f;
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1AttackTypeSynergyList[0] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("워리어 시너지 3 레벨");
                    Level = 3;
                    BuffHP = synergyData.MaxHp[2];
                    BuffDF = synergyData.Defense[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p1AttackTypeSynergyList[0] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("워리어 시너지 2 레벨");
                    Level = 2;
                    BuffHP = synergyData.MaxHp[1];
                    BuffDF = synergyData.Defense[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p1AttackTypeSynergyList[0] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("워리어 시너지 1 레벨");
                    Level = 1;
                    BuffHP = synergyData.MaxHp[0];
                    BuffDF = synergyData.Defense[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    Debug.Log("시너지 버프");
                    sm._p1UnitList[i]._unitMaxHp += BuffHP;
                    sm._p1UnitList[i]._unitHp += BuffHP;
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 워리어만 버프
                    if (sm._p1UnitList[i]._attackType != AttackType.Warrior) continue;
                    sm._p1UnitList[i]._unitDF += BuffDF;
                }

                break;
            case "P2":
                if (sm._p2AttackTypeSynergyList[0] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("워리어 시너지 3 레벨");
                    Level = 3;
                    BuffHP = synergyData.MaxHp[2];
                    BuffDF = synergyData.Defense[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p2AttackTypeSynergyList[0] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("워리어 시너지 2 레벨");
                    Level = 2;
                    BuffHP = synergyData.MaxHp[1];
                    BuffDF = synergyData.Defense[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p2AttackTypeSynergyList[0] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("워리어 시너지 1 레벨");
                    Level = 1;
                    BuffHP = synergyData.MaxHp[0];
                    BuffDF = synergyData.Defense[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i]._unitMaxHp += BuffHP;
                    sm._p2UnitList[i]._unitHp += BuffHP;
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    // 워리어만 버프
                    if (sm._p2UnitList[i]._attackType != AttackType.Warrior) continue;
                    sm._p2UnitList[i]._unitDF += BuffDF;
                }
                break;
        }
    }

    void WarriorSpecialSynergy()
    {
        if (Level == 0) return;
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    sm._p1UnitList[i].UnitBuff(0, 0, 0, 0, 0, synergyData.Shield[Level - 1], synergyData.Duration[Level - 1], false);
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i].UnitBuff(0, 0, 0, 0, 0, synergyData.Shield[Level - 1], synergyData.Duration[Level - 1], false);
                }
                break;
        }
    }


    void ArcherSynergy()
    {
        float Buff = 0.0f;
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1AttackTypeSynergyList[1] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("아처 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.Speed[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p1AttackTypeSynergyList[1] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("아처 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.Speed[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p1AttackTypeSynergyList[1] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("아처 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.Speed[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 아처만 버프
                    if (sm._p1UnitList[i]._attackType != AttackType.Archer) continue;
                    sm._p1UnitList[i]._unitAS += Buff;
                }
                break;
            case "P2":
                if (sm._p2AttackTypeSynergyList[1] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("아처 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.Speed[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p2AttackTypeSynergyList[1] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("아처 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.Speed[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p2AttackTypeSynergyList[1] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("아처 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.Speed[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    // 아처만 버프
                    if (sm._p2UnitList[i]._attackType != AttackType.Archer) continue;
                    sm._p2UnitList[i]._unitAS += Buff;
                }
                break;
        }
    }

    void ArcherSpecialSynergy()
    {
        if (Level != 2) return;
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    sm._p1UnitList[i].UnitBuff(0, 0, synergyData.Speed[Level], 0, 0, 0, synergyData.Duration[Level], false);
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i].UnitBuff(0, 0, synergyData.Speed[Level], 0, 0, 0, synergyData.Duration[Level], false);
                }
                break;
        }
    }

    void WizardSynergy()
    {
        float Buff = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1AttackTypeSynergyList[2] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("마법사 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.CoolTime[2];
                }
                else if (sm._p1AttackTypeSynergyList[2] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("마법사 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.CoolTime[1];
                }
                else if (sm._p1AttackTypeSynergyList[2] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("마법사 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.CoolTime[0];
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 마법사만 버프
                    if (sm._p1UnitList[i]._attackType != AttackType.Wizard || !sm._p1UnitList[i].gameObject.activeInHierarchy) continue;
                    sm._p1UnitList[i]._unitCT -= sm._p1UnitList[i]._unitCT * Buff;
                    sm._p1UnitList[i]._unit_SubSet.CT_Update();
                }
                break;
            case "P2":
                if (sm._p2AttackTypeSynergyList[2] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("마법사 시너지 3 레벨");
                    Level = 2;
                    Buff = synergyData.CoolTime[2];
                }
                else if (sm._p2AttackTypeSynergyList[2] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("마법사 시너지 2 레벨");
                    Level = 1;
                    Buff = synergyData.CoolTime[1];
                }
                else if (sm._p2AttackTypeSynergyList[2] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("마법사 시너지 1 레벨");
                    Level = 0;
                    Buff = synergyData.CoolTime[0];
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    // 마법사만 버프
                    if (sm._p2UnitList[i]._attackType != AttackType.Wizard || !sm._p2UnitList[i].gameObject.activeInHierarchy) continue;
                    sm._p2UnitList[i]._unitCT -= sm._p2UnitList[i]._unitCT * Buff;
                    sm._p2UnitList[i]._unit_SubSet.CT_Update();
                }
                break;
        }

        if (Level == 2) WizardSpecialSynergy();
    }

    void WizardSpecialSynergy()
    {
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    sm._p1UnitList[i]._wizardPower = true;
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i]._wizardPower = true;
                }
                break;
        }
    }

    void AssassinSynergy()
    {
        float BuffAT = 0.0f;
        float BuffCC = 0.0f;
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1AttackTypeSynergyList[3] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("암살자 시너지 3 레벨");
                    Level = 2;
                    BuffAT = synergyData.Damage[2];
                    BuffCC = synergyData.Critical[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p1AttackTypeSynergyList[3] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("암살자 시너지 2 레벨");
                    Level = 1;
                    BuffAT = synergyData.Damage[1];
                    BuffCC = synergyData.Critical[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p1AttackTypeSynergyList[3] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("암살자 시너지 1 레벨");
                    Level = 0;
                    BuffAT = synergyData.Damage[0];
                    BuffCC = synergyData.Critical[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 시너지 버프
                    if (sm._p1UnitList[i]._attackType != AttackType.Assassin) continue;
                    sm._p1UnitList[i]._unitAT += BuffAT;
                    sm._p1UnitList[i]._unitCC += BuffCC;
                }
                break;
            case "P2":
                if (sm._p2AttackTypeSynergyList[3] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("암살자 시너지 3 레벨");
                    Level = 2;
                    BuffAT = synergyData.Damage[2];
                    BuffCC = synergyData.Critical[2];
                    interval = synergyData.Interval[2];
                }
                else if (sm._p2AttackTypeSynergyList[3] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("암살자 시너지 2 레벨");
                    Level = 1;
                    BuffAT = synergyData.Damage[1];
                    BuffCC = synergyData.Critical[1];
                    interval = synergyData.Interval[1];
                }
                else if (sm._p2AttackTypeSynergyList[3] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("암살자 시너지 1 레벨");
                    Level = 0;
                    BuffAT = synergyData.Damage[0];
                    BuffCC = synergyData.Critical[0];
                    interval = synergyData.Interval[0];
                }

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    if (sm._p2UnitList[i]._attackType != AttackType.Assassin) return;
                    sm._p2UnitList[i]._unitAT += BuffAT;
                    sm._p2UnitList[i]._unitCC += BuffCC;
                }
                break;
        }
    }

    void AssassinSpecialSynergy()
    {
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    if (sm._p1UnitList[i]._attackType == AttackType.Assassin && sm._p1UnitList[i].gameObject.activeInHierarchy) sm._p1UnitList[i].SetDash();
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    if (sm._p2UnitList[i]._attackType == AttackType.Assassin && sm._p2UnitList[i].gameObject.activeInHierarchy) sm._p2UnitList[i].SetDash();
                }
                break;
        }
    }

    void HealerSynergy()
    {
        Debug.Log("시너지");
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1AttackTypeSynergyList[4] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("힐러 시너지 3 레벨");
                    Level = 2;
                    interval = synergyData.Interval[2];
                }
                else if (sm._p1AttackTypeSynergyList[4] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("힐러 시너지 2 레벨");
                    Level = 1;
                    interval = synergyData.Interval[1];
                }
                else if (sm._p1AttackTypeSynergyList[4] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("힐러 시너지 1 레벨");
                    Level = 0;
                    interval = synergyData.Interval[0];
                }
                break;
            case "P2":
                if (sm._p2AttackTypeSynergyList[4] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("힐러 시너지 3 레벨");
                    Level = 2;
                    interval = synergyData.Interval[2];
                }
                else if (sm._p2AttackTypeSynergyList[4] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("힐러 시너지 2 레벨");
                    Level = 1;
                    interval = synergyData.Interval[1];
                }
                else if (sm._p2AttackTypeSynergyList[4] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("힐러 시너지 1 레벨");
                    Level = 0;
                    interval = synergyData.Interval[0];
                }
                break;
        }
    }

    void HealerSpecialSynergy()
    {
        if (Level != 2) return;

        switch (gameObject.tag)
        {
            case "P1":
                Heal(sm._p1UnitList[0]);
                break;
            case "P2":
                Heal(sm._p2UnitList[0]);
                break;
        }
    }

    void HumanSynergy()
    {
        float BuffHP = 0.0f;
        float BuffAS = 0.0f;
        float BuffAT = 0.0f;
        float BuffCC = 0.0f;
        float BuffDF = 0.0f;
        float BuffCT = 0.0f;
        interval = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1UnitTypeSynergyList[0] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("인간 시너지 3 레벨");
                    Level = 3;
                    interval = synergyData.Interval[2];
                    BuffHP = synergyData.MaxHp[2];
                    BuffAS = synergyData.Speed[2];
                    BuffAT = synergyData.Damage[2];
                    BuffCC = synergyData.Critical[2];
                    BuffDF = synergyData.Defense[2];
                    BuffCT = synergyData.CoolTime[2];
                }
                else if (sm._p1UnitTypeSynergyList[0] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("인간 시너지 2 레벨");
                    Level = 2;
                    interval = synergyData.Interval[1];
                    BuffHP = synergyData.MaxHp[1];
                    BuffAS = synergyData.Speed[1];
                    BuffAT = synergyData.Damage[1];
                    BuffCC = synergyData.Critical[1];
                    BuffDF = synergyData.Defense[1];
                    BuffCT = synergyData.CoolTime[1];
                }
                else if (sm._p1UnitTypeSynergyList[0] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("인간 시너지 1 레벨");
                    Level = 1;
                    BuffHP = synergyData.MaxHp[0];
                    BuffAS = synergyData.Speed[0];
                    BuffAT = synergyData.Damage[0];
                    BuffCC = synergyData.Critical[0];
                    BuffDF = synergyData.Defense[0];
                    BuffCT = synergyData.CoolTime[0];
                    interval = synergyData.Interval[0];
                }

                if (Level < 1) break;

                List<Unit> p1humans = new List<Unit>();


                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 인간 찾기
                    if (sm._p1UnitList[i]._unitType != UnitType.Human) continue;
                    p1humans.Add(sm._p1UnitList[i]);
                }

                int randomIndex1 = Random.Range(0, p1humans.Count);
                p1humans[randomIndex1]._unitMaxHp += BuffHP;
                p1humans[randomIndex1]._unitHp += BuffHP;
                p1humans[randomIndex1]._unitAS += BuffAS;
                p1humans[randomIndex1]._unitAT += BuffAT;
                p1humans[randomIndex1]._unitCC += BuffCC;
                p1humans[randomIndex1]._unitDF += BuffDF;
                p1humans[randomIndex1]._unitCT -= p1humans[randomIndex1]._unitCT * BuffCT;
                p1humans[randomIndex1]._unit_SubSet.CT_Update();
                p1humans[randomIndex1].transform.localScale = new Vector3(1.3f, 1.3f, 2);
                break;
            case "P2":
                if (sm._p2UnitTypeSynergyList[0] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("인간 시너지 3 레벨");
                    Level = 3;
                    interval = synergyData.Interval[2];
                    BuffHP = synergyData.MaxHp[2];
                    BuffAS = synergyData.Speed[2];
                    BuffAT = synergyData.Damage[2];
                    BuffCC = synergyData.Critical[2];
                    BuffDF = synergyData.Defense[2];
                    BuffCT = synergyData.CoolTime[2];
                }
                else if (sm._p2UnitTypeSynergyList[0] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("인간 시너지 2 레벨");
                    Level = 2;
                    interval = synergyData.Interval[1];
                    BuffHP = synergyData.MaxHp[1];
                    BuffAS = synergyData.Speed[1];
                    BuffAT = synergyData.Damage[1];
                    BuffCC = synergyData.Critical[1];
                    BuffDF = synergyData.Defense[1];
                    BuffCT = synergyData.CoolTime[1];
                }
                else if (sm._p2UnitTypeSynergyList[0] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("인간 시너지 1 레벨");
                    Level = 1;
                    interval = synergyData.Interval[0];
                    BuffHP = synergyData.MaxHp[0];
                    BuffAS = synergyData.Speed[0];
                    BuffAT = synergyData.Damage[0];
                    BuffCC = synergyData.Critical[0];
                    BuffDF = synergyData.Defense[0];
                    BuffCT = synergyData.CoolTime[0];
                    interval = synergyData.Interval[0];
                }

                if (Level < 1) break;

                List<Unit> p2humans = new List<Unit>();


                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 인간 찾기
                    if (sm._p1UnitList[i]._unitType != UnitType.Human) continue;
                    p2humans.Add(sm._p1UnitList[i]);
                }

                int randomIndex2 = Random.Range(0, p2humans.Count);
                p2humans[randomIndex2]._unitMaxHp += BuffHP;
                p2humans[randomIndex2]._unitHp += BuffHP;
                p2humans[randomIndex2]._unitAS += BuffAS;
                p2humans[randomIndex2]._unitAT += BuffAT;
                p2humans[randomIndex2]._unitCC += BuffCC;
                p2humans[randomIndex2]._unitDF += BuffDF;
                p2humans[randomIndex2]._unitCT -= p2humans[randomIndex2]._unitCT * BuffCT;
                p2humans[randomIndex2]._unit_SubSet.CT_Update();
                p2humans[randomIndex2].transform.localScale = new Vector3(1.3f, 1.3f, 2);
                break;
        }
    }
        void GhostSynergy()
    {
        float BuffAT = 0.0f;
        switch (this.gameObject.tag)
        {
            case "P1":
                if (sm._p1UnitTypeSynergyList[1] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("요괴 시너지 3 레벨");
                    Level = 2;
                    BuffAT = synergyData.Damage[2];
                }
                else if (sm._p1UnitTypeSynergyList[1] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("요괴 시너지 2 레벨");
                    Level = 1;
                    BuffAT = synergyData.Damage[1];
                }
                else if (sm._p1UnitTypeSynergyList[1] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("요괴 시너지 1 레벨");
                    Level = 0;
                    BuffAT = synergyData.Damage[0];
                }

                // 아군 요괴가 적 인간 보다 적다면
                if (sm._p1UnitTypeSynergyList[1] <= sm._p2UnitTypeSynergyList[0]) break;

                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    // 시너지 버프
                    if (sm._p1UnitList[i]._unitType != UnitType.Ghost) continue;
                    sm._p1UnitList[i]._unitAT += BuffAT;
                }
                break;
            case "P2":
                if (sm._p2UnitTypeSynergyList[1] >= synergyData.RequiredNumber[2])
                {
                    Debug.Log("요괴 시너지 3 레벨");
                    Level = 2;
                    BuffAT = synergyData.Damage[2];
                }
                else if (sm._p2UnitTypeSynergyList[1] >= synergyData.RequiredNumber[1])
                {
                    Debug.Log("요괴 시너지 2 레벨");
                    Level = 1;
                    BuffAT = synergyData.Damage[1];
                }
                else if (sm._p2UnitTypeSynergyList[1] >= synergyData.RequiredNumber[0])
                {
                    Debug.Log("요괴 시너지 1 레벨");
                    Level = 0;
                    BuffAT = synergyData.Damage[0];
                }

                // 아군 요괴가 적 인간 보다 적다면
                if (sm._p2UnitTypeSynergyList[1] <= sm._p1UnitTypeSynergyList[0]) break;

                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    if (sm._p2UnitList[i]._unitType != UnitType.Ghost) return;
                    sm._p2UnitList[i]._unitAT += BuffAT;
                }
                break;
        }
        GhostSpecialSynergy();
    }

    void GhostSpecialSynergy()
    {
        if (Level <= 0) return;
        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < sm._p1UnitList.Count; i++)
                {
                    sm._p1UnitList[i]._ghostPower = true;
                    sm._p1UnitList[i]._ghostTime = synergyData.Duration[Level];
                    sm._p1UnitList[i]._ghostTimer = synergyData.Duration[Level];
                }
                break;
            case "P2":
                for (int i = 0; i < sm._p2UnitList.Count; i++)
                {
                    sm._p2UnitList[i]._ghostPower = true;
                    sm._p2UnitList[i]._ghostTime = synergyData.Duration[Level];
                    sm._p2UnitList[i]._ghostTimer = synergyData.Duration[Level];
                }
                break;
        }
    }

    void Heal(Unit unit)
    {
        List<Unit> Targets = new List<Unit>();

        if (Level == 2)
        {
            Targets = SoonsoonData.Instance.Unit_Manager.GetSquadTeam(unit, false);
        }
        else
        {
            Targets.Add(SoonsoonData.Instance.Unit_Manager.GetLeastTeam(unit));
        }

        for (int i = 0; i < Targets.Count; i++)
        {
            Targets[i].SetHeal(null, synergyData.Recovery[Level]);
        }
    }
}
