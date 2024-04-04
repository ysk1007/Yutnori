using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Transform _skillPool;

    public List<SkillObj> _poolListSave = new List<SkillObj>(); // 저장해놓은 총 오브젝트 개수

    public List<SkillObj> _poolListUse = new List<SkillObj>(); // 사용할 오브젝트

    private void Awake()
    {
        SoonsoonData.Instance.Skill_Manager = this;
    }

    void Start()
    {
        GetSkillList();
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
                SkillObj skill = _poolListUse[i];
                skill._timer += Time.deltaTime;

                if (skill._timer > skill._timerForLim)
                {
                    skill.SkillDone();
                }
                else
                {
                    skill.DoMove();
                }
            }
        }
    }

    public void GetSkillList()
    {
        _poolListUse.Clear();
        _poolListSave.Clear();

        for (int i = 0; i < _skillPool.childCount; i++)
        {
            SkillObj skill = _skillPool.GetChild(i).GetComponent<SkillObj>();
            _poolListSave.Add(skill);
        }
    }

    public void RunSkill(SkillObj.SkillType type, Unit owner, List<Unit> target, float timer, SkillData skillData)
    {
        SkillObj skill = null;

        foreach (var obj in _poolListSave)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                skill = obj;
                break;
            }
        }

        if (skill != null)
        {
/*            if (type == SkillObj.SkillType.ShortRange)
            {
                owner.SetDirection();
                Vector2 newPos;
                if (owner.transform.position.x > target[0].transform.position.x)
                    newPos = new Vector2(target[0].transform.position.x + 1, target[0].transform.position.y);
                else
                    newPos = new Vector2(target[0].transform.position.x - 1, target[0].transform.position.y);
                owner.transform.position = newPos;
            }*/
            skill.SetSkill(type, owner, target, timer, skillData);
        }
    }

    public void ResetSkill()
    {
        for (int i = 0; i < _skillPool.childCount; i++)
        {
            SkillObj skill = _skillPool.GetChild(i).GetComponent<SkillObj>();
            skill.SendMessage("SkillDone", SendMessageOptions.DontRequireReceiver);
        }
    }
}
