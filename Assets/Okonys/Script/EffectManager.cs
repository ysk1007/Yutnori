using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Transform _effectPool; // 효과 풀을 담고 있는 트랜스폼

    public List<EffectObj> _poolListSave = new List<EffectObj>(); // 저장된 총 효과 오브젝트 리스트
    public List<EffectObj> _poolListUse = new List<EffectObj>();  // 현재 사용 중인 효과 오브젝트 리스트

    private void Awake()
    {
        // EffectManager 인스턴스를 설정합니다.
        SoonsoonData.Instance.Effect_Manager = this;
    }

    private void Start()
    {
        // 효과 리스트를 가져옵니다.
        GetEffectList();
    }

    private void Update()
    {
        // 효과 오브젝트의 상태를 업데이트합니다.
        Tread();
    }

    private void Tread()
    {
        if (_poolListUse.Count == 0) return;

        // 사용 중인 효과 오브젝트를 순회하며 업데이트합니다.
        for (int i = 0; i < _poolListUse.Count; i++)
        {
            EffectObj effect = _poolListUse[i];

            // 비활성화된 오브젝트는 무시합니다.
            if (!effect.gameObject.activeInHierarchy) continue;

            effect._timer += Time.deltaTime;

            // 타이머가 제한 시간을 초과하면 효과를 완료합니다.
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
        // 현재 사용 중인 효과 리스트와 저장된 효과 리스트를 초기화합니다.
        _poolListUse.Clear();
        _poolListSave.Clear();

        // 효과 풀의 자식 오브젝트들을 리스트에 추가합니다.
        for (int i = 0; i < _effectPool.childCount; i++)
        {
            EffectObj effect = _effectPool.GetChild(i).GetComponent<EffectObj>();
            _poolListSave.Add(effect);
        }
    }

    public void SetEffect(EffectObj.EffectType type, Unit owner, Vector3 pos, bool homing, float timer)
    {
        // 비활성화된 효과 오브젝트를 찾아서 설정합니다.
        foreach (var obj in _poolListSave)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.SetEffectObj(type, owner, pos, homing, timer);
                return; // 설정 후 즉시 종료합니다.
            }
        }
    }

    public void ResetEffect()
    {
        // 효과 풀의 모든 자식 오브젝트를 순회하며 효과를 완료합니다.
        for (int i = 0; i < _effectPool.childCount; i++)
        {
            EffectObj effect = _effectPool.GetChild(i).GetComponent<EffectObj>();
            effect.SendMessage("EffectDone", SendMessageOptions.DontRequireReceiver);
        }
    }
}
