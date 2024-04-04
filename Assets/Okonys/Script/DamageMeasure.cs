using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DamageMeasure : MonoBehaviour
{
    public GameObject MeasureObj;
    public Unit[] _units;
    public float _highestDamage;

    Unit_Manager um;
    public Transform[] _childObjects = new Transform[0];
    //public List<Transform> _childObjects = new List<Transform>();

    void Awake()
    {
        SoonsoonData.Instance.Damage_Measure = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        um = SoonsoonData.Instance.Unit_Manager;
    }

    // Update is called once per frame
    void Update()
    {
        if (um._gamePause) return;
        childSort();
    }

    public void MeasureStart()
    {
        _highestDamage = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        _units = new Unit[um._p1UnitList.Count];
        _childObjects = new Transform[um._p1UnitList.Count];

        for (int i = 0; i < um._p1UnitList.Count; i++)
        {
            GameObject newObj = Instantiate(MeasureObj, transform.position, Quaternion.identity);
            newObj.transform.SetParent(gameObject.transform);
            _childObjects[i] = newObj.transform;
            newObj.GetComponent<MeasureObj>()._damageMeasure = this;
            newObj.GetComponent<MeasureObj>()._unit = um._p1UnitList[i];
            _units[i] = um._p1UnitList[i];
        }
    }

    void childSort()
    {
        System.Array.Sort(_childObjects, (x, y) => -x.GetComponent<MeasureObj>()._damageValue.CompareTo(y.GetComponent<MeasureObj>()._damageValue));

        // 정렬된 순서대로 자식 오브젝트를 배치합니다.
        for (int i = 0; i < _childObjects.Length; i++)
        {
            _childObjects[i].SetSiblingIndex(i);
        }

        // 정렬된 배열에서 가장 큰 값을 가져옵니다.
        _highestDamage = _childObjects.Length > 0 ? _childObjects[0].GetComponent<MeasureObj>()._damageValue : 0f;
    }

    public void MeasureReset()
    {
        _highestDamage = 0;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
