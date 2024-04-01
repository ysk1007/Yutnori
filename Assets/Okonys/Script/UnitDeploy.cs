using System.Collections;
using System.Collections.Generic;
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
                    if (um._p1unitID[i] != 0)
                    {
                        GameObject newUnit = Instantiate(_unitPool._unitPrefabs[um._p1unitID[i] - 1], um._p1fieldPos[i], Quaternion.identity);
                        newUnit.transform.SetParent(gameObject.transform);
                        newUnit.transform.GetChild(0).transform.localScale = new Vector3(-1,1,1);
                        newUnit.gameObject.tag = "P1";
                        um._p1UnitList.Add(newUnit.GetComponent<Unit>());
                    }
                }
                //um.SetUnitList(gameObject.tag);
                break;
            case "P2":
                for (int i = 0; i < um._p2unitID.Count; i++)
                {
                    if (um._p2unitID[i] != 0)
                    {
                        GameObject newUnit = Instantiate(_unitPool._unitPrefabs[um._p2unitID[i] - 1], um._p2fieldPos[i], Quaternion.identity);
                        newUnit.transform.SetParent(gameObject.transform);
                        newUnit.transform.GetChild(0).transform.localScale = new Vector3(1, 1, 1); 
                        newUnit.gameObject.tag = "P2";
                        um._p2UnitList.Add(newUnit.GetComponent<Unit>());
                    }
                }
                //um.SetUnitList(gameObject.tag);
                break;
        }
    }
}
