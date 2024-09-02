using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleReward : MonoBehaviour
{
    // ���� ���� Ȯ�� (0: ��, 1: �Ҹ�ǰ, 2: ����, 3: ��Ƽ��Ʈ)
    [SerializeField] private int[] _rewardProbability;
    [SerializeField] private int[] _bonusProbability = { 66, 22, 9, 3 }; // �߰� ���� Ȯ��
    [SerializeField] private int _bonusReward; // �߰� ���� ����
    [SerializeField] private Transform _rewards; // ������ ��� �ִ� �θ� ������Ʈ
    [SerializeField] private List<Reward> _rewardList; // ���� ����Ʈ
    [SerializeField] private Sprite _moneyicon; // �� ������
    private UserInfoManager _userInfoManager;
    private UnitPool _unitPool;
    private ItemShop _itemShop;
    private Popup _popup;

    private void Awake()
    {
        SoonsoonData.Instance.Battle_Reward = this;

        // Popup ������Ʈ �ʱ�ȭ
        _popup = gameObject.GetComponent<Popup>();

        // ���� ����Ʈ �ʱ�ȭ �� ����
        for (int i = 0; i < _rewards.childCount; i++)
        {
            Reward reward = _rewards.GetChild(i).GetComponent<Reward>();
            reward.Reset();
            _rewardList.Add(reward);
        }
    }

    private void Start()
    {
        // �ʿ��� �Ŵ��� �� Ǯ �ʱ�ȭ
        _userInfoManager = UserInfoManager.Instance;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _itemShop = SoonsoonData.Instance.ItemShop;
    }

    public void NormalBattleReward()
    {
        RewardReset(); // ���� �ʱ�ȭ

        int rewardPro = Random.Range(0, 100);

        // ù��° �⺻ ��� ����
        if (_rewardProbability[0] > rewardPro)
            _rewardList[0].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // �ι�° Ȯ�� �Ҹ�ǰ ����
        if (_rewardProbability[1] > rewardPro)
            _rewardList[1].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // ����° Ȯ�� ���� ����
        if (_rewardProbability[2] > rewardPro)
            _rewardList[2].init(Reward.RewardType.unit, null, null, _unitPool.ReturnRewardUnit(_userInfoManager.userData.GameLevel));

        // �׹�° Ȯ�� ��Ƽ��Ʈ ����
        if (_rewardProbability[3] > rewardPro)
            _rewardList[3].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnRewardItem());

        // �߰� ���� ó��
        for (int i = 0; i < _bonusReward; i++)
        {
            RandomReward(4 + i);
        }

        // ���� �˾� ǥ��
        _popup.OnePopup();
    }

    public void ChestReward(float remainHp)
    {
        RewardReset(); // ���� �ʱ�ȭ

        float totalDamage = 7777777f - remainHp;
        int selectedGrade = 0; // �⺻������ ����(0)

        if (totalDamage > 100000)
            selectedGrade = 2; // ����
        else if (totalDamage > 10000)
            selectedGrade = 1; // ����

        // ���� ���� �ʱ�ȭ
        _rewardList[0].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnChestItem(selectedGrade));

        // ���� ������ ���� �� �ߴٸ� ���� ��ü
        if (_rewardList[0].GetItemStock() == null)
        {
            string goldAmount = selectedGrade switch
            {
                0 => "777",
                1 => "7777",
                2 => "77777",
                _ => "0"
            };

            _rewardList[0].init(Reward.RewardType.money, _moneyicon, goldAmount);
        }

        // ���� �˾� ǥ��
        _popup.OnePopup();
    }

    private int GoldReward()
    {
        // ��� ���� ���
        float range = Random.Range(1f, 10f);
        int reward = (100 * _userInfoManager.userData.GameLevel) + (int)((_userInfoManager.userData.GameLevel + 1) * range * 10);
        return reward;
    }

    public void RewardReset()
    {
        // ���� �ʱ�ȭ
        foreach (var reward in _rewardList)
        {
            reward.Reset();
        }
    }

    public void RandomReward(int index)
    {
        int rewardType = 0;
        int rewardPro = Random.Range(0, _bonusProbability.Sum());

        // Ȯ���� ���� ���� Ÿ�� ����
        if (rewardPro < _bonusProbability[0])
            rewardType = 0;
        else if (rewardPro < _bonusProbability[0] + _bonusProbability[1])
            rewardType = 1;
        else if (rewardPro < _bonusProbability[0] + _bonusProbability[1] + _bonusProbability[2])
            rewardType = 2;
        else
            rewardType = 3;

        // ������ Ÿ�Կ� ���� ���� �ʱ�ȭ
        switch (rewardType)
        {
            case 0:
            case 1:
                _rewardList[index].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());
                break;
            case 2:
                _rewardList[index].init(Reward.RewardType.unit, null, null, _unitPool.ReturnRewardUnit(_userInfoManager.userData.GameLevel));
                break;
            case 3:
                _rewardList[index].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnRewardItem());
                break;
        }
    }

    public void GetAllReward()
    {
        // ��� Ȱ��ȭ�� ���� ����
        foreach (var reward in _rewardList)
        {
            if (reward.transform.gameObject.activeInHierarchy)
            {
                reward.RewardGet();
            }
        }
    }

    public void SetBonusReward(int value)
    {
        // �߰� ���� ���� ����
        _bonusReward = value;
    }
}
