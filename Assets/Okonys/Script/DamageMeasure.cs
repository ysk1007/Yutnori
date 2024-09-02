using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMeasure : MonoBehaviour
{
    // �ܺο��� ������ �� �ִ� ���� ������
    public GameObject MeasureObj; // ���� ������Ʈ ������
    public Unit[] _units; // ���� �迭
    public float _highestDamage; // ���� ���� ���� ��

    private Unit_Manager um; // ���� �Ŵ���
    public Transform[] _childObjects = new Transform[0]; // �ڽ� ������Ʈ��

    private void Awake()
    {
        SoonsoonData.Instance.Damage_Measure = this;
    }

    private void Start()
    {
        // ���� �Ŵ����� �ʱ�ȭ�մϴ�.
        um = SoonsoonData.Instance.Unit_Manager;
    }

    private void Update()
    {
        // ������ �Ͻ� ���� ���°� �ƴ� ���� �ڽ� ������Ʈ�� �����մϴ�.
        if (um._gamePause) return;
        childSort();
    }

    // ������ �����ϰ� ������ �ʱ�ȭ�մϴ�.
    public void MeasureStart()
    {
        _highestDamage = 0f;

        // ���� �ڽ� ������Ʈ�� ��� �����մϴ�.
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // ���� �迭�� �ڽ� Ʈ������ �迭�� �ʱ�ȭ�մϴ�.
        int unitCount = um._p1UnitList.Count;
        _units = new Unit[unitCount];
        _childObjects = new Transform[unitCount];

        // ���ֿ� ���� ���� ������Ʈ�� �����ϰ� �ʱ�ȭ�մϴ�.
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

    // �ڽ� ������Ʈ�� ���� ���� ���� �����մϴ�.
    private void childSort()
    {
        // �ڽ� ������Ʈ�� ���� ���� ���� ������������ �����մϴ�.
        System.Array.Sort(_childObjects, (x, y) => -x.GetComponent<MeasureObj>()._damageValue.CompareTo(y.GetComponent<MeasureObj>()._damageValue));

        // ���ĵ� ������� �ڽ� ������Ʈ�� �ε����� �����մϴ�.
        for (int i = 0; i < _childObjects.Length; i++)
        {
            _childObjects[i].SetSiblingIndex(i);
        }

        // ���ĵ� �迭���� ���� ū ���� ���� �����ɴϴ�.
        _highestDamage = _childObjects.Length > 0 ? _childObjects[0].GetComponent<MeasureObj>()._damageValue : 0f;
    }

    // ������ �����ϰ� ��� �ڽ� ������Ʈ�� �����մϴ�.
    public void MeasureReset()
    {
        _highestDamage = 0f;

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
