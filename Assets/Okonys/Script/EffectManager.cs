using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Transform _effectPool;

    public List<EffectObj> _poolListSave = new List<EffectObj>(); // 저장해놓은 총 오브젝트 개수

    public List<EffectObj> _poolListUse = new List<EffectObj>(); // 사용할 오브젝트

    private void Awake()
    {
        SoonsoonData.Instance.Effect_Manager = this;
    }

    void Start()
    {
        GetEffectList();
    }

    // Update is called once per frame
    void Update()
    {
        Tread();
    }

    void Tread()
    {
        if (_poolListUse.Count > 0)
        {
            for (int i = 0; i < _poolListUse.Count; i++)
            {
                if (!_poolListUse[i].gameObject.activeInHierarchy) return;
                EffectObj effect = _poolListUse[i];
                effect._timer += Time.deltaTime;

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
    }

    public void GetEffectList()
    {
        _poolListUse.Clear();
        _poolListSave.Clear();

        for (int i = 0; i < _effectPool.childCount; i++)
        {
            EffectObj effect = _effectPool.GetChild(i).GetComponent<EffectObj>();
            _poolListSave.Add(effect);
        }
    }

    public void SetEffect(EffectObj.EffectType type, Unit owner, Vector3 pos, bool homing, float timer)
    {
        EffectObj effect = null;

        foreach (var obj in _poolListSave)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                effect = obj;
                break;
            }
        }

        if (effect != null)
        {
            effect.SetEffectObj(type, owner,pos,homing,timer);
        }
    }
}
