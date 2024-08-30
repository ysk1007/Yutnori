using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : MonoBehaviour
{
    public List<GameObject> _unitPrefabs = new List<GameObject>();
    public List<GameObject> _objectPrefabs = new List<GameObject>();
    public List<GameObject> _bossPrefabs = new List<GameObject>();

    public List<UnitData> _unitDatas = new List<UnitData>();
    public List<UnitData> _objectDatas = new List<UnitData>();
    public List<UnitData> _bossDatas = new List<UnitData>();

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

    public UnitData ReturnRewardUnit(int GameLevel)
    {
        UnitData rewardUnit;
        switch (GameLevel)
        {
            case 0:
                rewardUnit = _commonUnits[Random.Range(0, _commonUnits.Count)];
                break;
            case 1:
                rewardUnit = _uncommonUnits[Random.Range(0, _uncommonUnits.Count)];
                break;
            case 2:
                rewardUnit = _rareUnits[Random.Range(0, _rareUnits.Count)];
                break;
            case 3:
                rewardUnit = _epicUnits[Random.Range(0, _epicUnits.Count)];
                break;
            default:
                rewardUnit = _epicUnits[Random.Range(0, _epicUnits.Count)];
                break;
        }
        return rewardUnit;
    }

    public int RandomUnitRate()
    {
        float randomNumber = Random.value; // 0부터 1 사이의 랜덤 값

        int _unitRate = 0;

        if (randomNumber < 0.667f) // 66.7%
        {
            _unitRate = 0;
        }
        else if (randomNumber < 0.889f) // 22.2%
        {
            _unitRate = 1;
        }
        else // 11.1%
        {
            _unitRate = 2;
        }
        return _unitRate; ;
    }
}
