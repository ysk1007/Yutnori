using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMeasure : MonoBehaviour
{
    // 외부에서 설정할 수 있는 공개 변수들
    public GameObject MeasureObj; // 측정 오브젝트 프리팹
    public Unit[] _units; // 유닛 배열
    public float _highestDamage; // 가장 높은 피해 값

    private Unit_Manager um; // 유닛 매니저
    public Transform[] _childObjects = new Transform[0]; // 자식 오브젝트들

    private void Awake()
    {
        SoonsoonData.Instance.Damage_Measure = this;
    }

    private void Start()
    {
        // 유닛 매니저를 초기화합니다.
        um = SoonsoonData.Instance.Unit_Manager;
    }

    private void Update()
    {
        // 게임이 일시 정지 상태가 아닐 때만 자식 오브젝트를 정렬합니다.
        if (um._gamePause) return;
        childSort();
    }

    // 측정을 시작하고 유닛을 초기화합니다.
    public void MeasureStart()
    {
        _highestDamage = 0f;

        // 기존 자식 오브젝트를 모두 제거합니다.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 유닛 배열과 자식 트랜스폼 배열을 초기화합니다.
        int unitCount = um._p1UnitList.Count;
        _units = new Unit[unitCount];
        _childObjects = new Transform[unitCount];

        // 유닛에 대해 측정 오브젝트를 생성하고 초기화합니다.
        for (int i = 0; i < unitCount; i++)
        {
            GameObject newObj = Instantiate(MeasureObj, transform.position, Quaternion.identity);
            newObj.transform.SetParent(transform);
            _childObjects[i] = newObj.transform;
            var measureObj = newObj.GetComponent<MeasureObj>();
            measureObj.init(this, um._p1UnitList[i]);
            _units[i] = um._p1UnitList[i];
        }
    }

    // 자식 오브젝트를 피해 값에 따라 정렬합니다.
    private void childSort()
    {
        // 자식 오브젝트를 피해 값에 따라 내림차순으로 정렬합니다.
        System.Array.Sort(_childObjects, (x, y) => -x.GetComponent<MeasureObj>()._damageValue.CompareTo(y.GetComponent<MeasureObj>()._damageValue));

        // 정렬된 순서대로 자식 오브젝트의 인덱스를 설정합니다.
        for (int i = 0; i < _childObjects.Length; i++)
        {
            _childObjects[i].SetSiblingIndex(i);
        }

        // 정렬된 배열에서 가장 큰 피해 값을 가져옵니다.
        _highestDamage = _childObjects.Length > 0 ? _childObjects[0].GetComponent<MeasureObj>()._damageValue : 0f;
    }

    // 측정을 리셋하고 모든 자식 오브젝트를 제거합니다.
    public void MeasureReset()
    {
        _highestDamage = 0f;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
