using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Transform _effectPool; // ȿ�� Ǯ�� ��� �ִ� Ʈ������

    public List<EffectObj> _poolListSave = new List<EffectObj>(); // ����� �� ȿ�� ������Ʈ ����Ʈ
    public List<EffectObj> _poolListUse = new List<EffectObj>();  // ���� ��� ���� ȿ�� ������Ʈ ����Ʈ

    private void Awake()
    {
        // EffectManager �ν��Ͻ��� �����մϴ�.
        SoonsoonData.Instance.Effect_Manager = this;
    }

    private void Start()
    {
        // ȿ�� ����Ʈ�� �����ɴϴ�.
        GetEffectList();
    }

    private void Update()
    {
        // ȿ�� ������Ʈ�� ���¸� ������Ʈ�մϴ�.
        Tread();
    }

    private void Tread()
    {
        if (_poolListUse.Count == 0) return;

        // ��� ���� ȿ�� ������Ʈ�� ��ȸ�ϸ� ������Ʈ�մϴ�.
        for (int i = 0; i < _poolListUse.Count; i++)
        {
            EffectObj effect = _poolListUse[i];

            // ��Ȱ��ȭ�� ������Ʈ�� �����մϴ�.
            if (!effect.gameObject.activeInHierarchy) continue;

            effect._timer += Time.deltaTime;

            // Ÿ�̸Ӱ� ���� �ð��� �ʰ��ϸ� ȿ���� �Ϸ��մϴ�.
            if (effect._timer > effect._timerForLim)
            {
                effect.EffectDone();
            }
            else
            {
                effect.EffectMove();
            }
        }
    }

    public void GetEffectList()
    {
        // ���� ��� ���� ȿ�� ����Ʈ�� ����� ȿ�� ����Ʈ�� �ʱ�ȭ�մϴ�.
        _poolListUse.Clear();
        _poolListSave.Clear();

        // ȿ�� Ǯ�� �ڽ� ������Ʈ���� ����Ʈ�� �߰��մϴ�.
        for (int i = 0; i < _effectPool.childCount; i++)
        {
            EffectObj effect = _effectPool.GetChild(i).GetComponent<EffectObj>();
            _poolListSave.Add(effect);
        }
    }

    public void SetEffect(EffectObj.EffectType type, Unit owner, Vector3 pos, bool homing, float timer)
    {
        // ��Ȱ��ȭ�� ȿ�� ������Ʈ�� ã�Ƽ� �����մϴ�.
        foreach (var obj in _poolListSave)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.SetEffectObj(type, owner, pos, homing, timer);
                return; // ���� �� ��� �����մϴ�.
            }
        }
    }

    public void ResetEffect()
    {
        // ȿ�� Ǯ�� ��� �ڽ� ������Ʈ�� ��ȸ�ϸ� ȿ���� �Ϸ��մϴ�.
        for (int i = 0; i < _effectPool.childCount; i++)
        {
            EffectObj effect = _effectPool.GetChild(i).GetComponent<EffectObj>();
            effect.SendMessage("EffectDone", SendMessageOptions.DontRequireReceiver);
        }
    }
}
