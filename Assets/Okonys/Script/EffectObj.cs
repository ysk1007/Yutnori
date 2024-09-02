using System.Collections.Generic;
using UnityEngine;

public class EffectObj : MonoBehaviour
{
    public enum EffectType
    {
        Hit = 0,
        Heal = 1,
        Buff = 2,
        Debuff = 3,
        StonesHit = 4,
        FireBallHit = 5,
        Explosion_ice = 6,
        ElectroHit = 7,
        Explosion_Arcane = 8,
        SnowHit = 9,
        RedHit = 10,
        Explosion_Dark = 11,
        WaterHit = 12,
        PurpleHit = 13,
        ElectroHit2 = 14,
        Explosion_Fire_blue = 15,
    }

    [SerializeField] private EffectType _effectType = EffectType.Hit;
    [SerializeField] private List<GameObject> _effectList = new List<GameObject>();

    [SerializeField] private bool _homing;
    public float _timer;
    public float _timerForLim;

    private GameObject _nowEffectObj;
    private Unit _owner;

    private void OnEnable()
    {
        // ȿ���� Ȱ��ȭ�� �� ����� ȿ���� ����մϴ�.
        AudioManager.instance.PlayEffect((int)_effectType);
    }

    public void SetEffectObj(EffectType type, Unit owner, Vector3 pos, bool homing, float timer)
    {
        _effectType = type;
        _owner = owner;
        _homing = homing;
        _timer = 0;
        _timerForLim = timer;

        // ȿ���� ��ġ�� ���� �Ǵ� ������ ��ġ�� �����մϴ�.
        transform.position = owner != null ? owner.transform.position : pos;

        // ���� ȿ�� ������Ʈ�� ��� ���� ��Ͽ� �߰��մϴ�.
        SoonsoonData.Instance.Effect_Manager._poolListUse.Add(this);
        SetInit();
    }

    private void SetInit()
    {
        int effectIndex = (int)_effectType;

        // ����Ʈ Ÿ�Կ� �ش��ϴ� �ڽ� ������Ʈ�� Ȱ��ȭ�մϴ�.
        if (effectIndex < transform.childCount)
        {
            _nowEffectObj = transform.GetChild(effectIndex).gameObject;
            _nowEffectObj.SetActive(true);
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Effect type index out of range!");
        }
    }

    public void EffectMove()
    {
        if (_owner != null && _homing)
        {
            // ���� ������Ʈ�� ���� �̵��մϴ�.
            transform.position = _owner.transform.position;
        }
    }

    public void EffectDone()
    {
        // ��� ���� ȿ�� ��Ͽ��� �����ϰ�, ��Ȱ��ȭ�մϴ�.
        SoonsoonData.Instance.Effect_Manager._poolListUse.Remove(this);
        if (_nowEffectObj != null)
        {
            _nowEffectObj.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
