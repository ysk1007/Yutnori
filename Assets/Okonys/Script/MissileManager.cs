using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    public Transform _missilePool;

    public List<MissileObj> _poolListSave = new List<MissileObj>(); // 저장해놓은 총 오브젝트 개수

    public List<MissileObj> _poolListUse = new List<MissileObj>(); // 사용할 오브젝트

    private void Awake()
    {
        SoonsoonData.Instance.Missile_Manager = this;
    }

    void Start()
    {
        GetMissileList();
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
                MissileObj missile = _poolListUse[i];
                missile._timer += Time.deltaTime;

                if (missile._timer > missile._timerForLim)
                {
                    missile.MissileDone();
                }
                else
                {
                    missile.DoMove();
                }
            }
        }
    }

    public void GetMissileList()
    {
        _poolListUse.Clear();
        _poolListSave.Clear();

        for (int i = 0; i < _missilePool.childCount; i++)
        {
            MissileObj missile = _missilePool.GetChild(i).GetComponent<MissileObj>();
            _poolListSave.Add(missile);
        }
    }

    public void FireMissile(MissileObj.MissileType type, Unit owner, Unit target)
    {
        MissileObj Missile = null;

        foreach (var obj in _poolListSave)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                Missile = obj;
                break;
            }
        }

        if (Missile!=null)
        {
            Missile.SetMissile(type, owner, target);
        }
    }

    public void ResetMissile()
    {
        for (int i = 0; i < _missilePool.childCount; i++)
        {
            MissileObj missile = _missilePool.GetChild(i).GetComponent<MissileObj>();
            missile.SendMessage("MissileDone", SendMessageOptions.DontRequireReceiver);
        }
    }
}
