using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitDeploy : MonoBehaviour
{
    Unit_Manager um;
    UnitPool _unitPool;

    void Start()
    {
        um = SoonsoonData.Instance.Unit_Manager;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        UnitDeployment();
    }

    public void UnitDeployment()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        switch (gameObject.tag)
        {
            case "P1":
                for (int i = 0; i < um._p1unitID.Count; i++)
                {
                    if (um._p1unitID[i]._unitData != null)
                    {
                        GameObject newUnit = Instantiate(_unitPool._unitPrefabs[um._p1unitID[i]._unitData.UnitID - 1], um._p1fieldPos[i], Quaternion.identity);
                        switch (um._p1unitID[i]._unitRate)
                        {
                            case 0:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.lower;
                                break;
                            case 1:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.middle;
                                break;
                            case 2:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.upper;
                                break;
                        }
                        newUnit.GetComponent<Unit>()._fieldindex = i;
                        newUnit.transform.SetParent(gameObject.transform);
                        newUnit.transform.GetChild(0).transform.localScale = new Vector3(-1,1,1);
                        newUnit.gameObject.tag = "P1";
                        newUnit.GetComponent<Unit>().init();
                        um._p1UnitList.Add(newUnit.GetComponent<Unit>());
                    }
                }
                //um.SetUnitList(gameObject.tag);
                break;
            case "P2":
                for (int i = 0; i < um._p2unitID.Count; i++)
                {
                    if (um._p2unitID[i]._unitData != null)
                    {
                        GameObject newUnit = Instantiate(_unitPool._unitPrefabs[um._p2unitID[i]._unitData.UnitID - 1], um._p2fieldPos[i], Quaternion.identity);
                        switch (um._p2unitID[i]._unitRate)
                        {
                            case 0:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.lower;
                                break;
                            case 1:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.middle;
                                break;
                            case 2:
                                newUnit.GetComponent<Unit>()._unitRate = UnitData.RateType.upper;
                                break;
                        }
                        newUnit.GetComponent<Unit>()._fieldindex = i;
                        newUnit.transform.SetParent(gameObject.transform);
                        newUnit.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1); 
                        newUnit.gameObject.tag = "P2";
                        newUnit.GetComponent<Unit>().init();
                        um._p2UnitList.Add(newUnit.GetComponent<Unit>());
                    }
                }
                //um.SetUnitList(gameObject.tag);
                break;
        }
    }
}
