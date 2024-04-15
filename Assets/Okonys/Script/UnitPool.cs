using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public List<GameObject> _unitPrefabs = new List<GameObject>();
    public List<UnitData> _unitDatas = new List<UnitData>();

    public List<UnitData> _commonUnits = new List<UnitData>();
    public List<UnitData> _uncommonUnits = new List<UnitData>();
    public List<UnitData> _rareUnits = new List<UnitData>();
    public List<UnitData> _epicUnits = new List<UnitData>();
    public List<UnitData> _legendaryUnits = new List<UnitData>();

    void Awake()
    {
        SoonsoonData.Instance.Unit_pool = this;
        unitSort();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void unitSort() // 등급 별 정리
    {
        for (int i = 0; i < _unitDatas.Count; i++)
        {
            switch (_unitDatas[i]._nobleRate)
            {
                case UnitData.nobleRate.common:
                    _commonUnits.Add(_unitDatas[i]);
                    break;
                case UnitData.nobleRate.uncommon:
                    _uncommonUnits.Add(_unitDatas[i]);
                    break;
                case UnitData.nobleRate.rare:
                    _rareUnits.Add(_unitDatas[i]);
                    break;
                case UnitData.nobleRate.epic:
                    _epicUnits.Add(_unitDatas[i]);
                    break;
                case UnitData.nobleRate.legendary:
                    _legendaryUnits.Add(_unitDatas[i]);
                    break;
                default:
                    break;
            }
        }
    }
}
