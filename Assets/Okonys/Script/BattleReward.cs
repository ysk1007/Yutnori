using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleReward : MonoBehaviour
{
    // 보상 등장 확률
    [SerializeField] private int[] _rewardProbability; // 0 돈, 1 소모품, 2 유닛, 3 아티팩트
    [SerializeField] private int[] _bonusProbability = { 66, 22, 9, 3 }; // 각 아이템에 대한 확률
    [SerializeField] private int _bonusReward;
    [SerializeField] private Transform _rewards;
    [SerializeField] private List<Reward> _rewardList;
    [SerializeField] private Sprite _moneyicon;
    UserInfoManager _userInfoManager;
    UnitPool _unitPool;
    ItemShop _itemShop;
    Popup _popup;

    private void Awake()
    {
        SoonsoonData.Instance.Battle_Reward = this;
        _popup = gameObject.GetComponent<Popup>();
        for (int i = 0; i < _rewards.childCount; i++)
        {
            _rewardList.Add(_rewards.GetChild(i).GetComponent<Reward>());
            _rewardList[i].Reset();
        }
    }

    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _itemShop = SoonsoonData.Instance.ItemShop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalBattleReward()
    {
        RewardReset();

        int RewardPro = Random.Range(0, 100);

        // 첫번째 기본 골드 보상
        if (_rewardProbability[0] > RewardPro)
            _rewardList[0].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // 두번째 확률 소모품 보상
        if (_rewardProbability[1] > RewardPro)
            _rewardList[1].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // 세번째 확률 유닛 보상
        if (_rewardProbability[2] > RewardPro)
            _rewardList[2].init(Reward.RewardType.unit, null, null, _unitPool.ReturnRewardUnit(_userInfoManager.userData.GameLevel));

        // 네번째 확률 유물 보상
        if (_rewardProbability[3] > RewardPro)
            _rewardList[3].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnRewardItem());

        for (int i = 0; i < _bonusReward; i++)
        {
            RandomReward(4 + i);
        }

        _popup.OnePopup();
    }

    public void ChestReward(float RemainHp)
    {
        RewardReset();

        float totalDamage = 7777777f - RemainHp;

        int selectedGrade = 0; // 레어

        if (totalDamage > 100000)
        {
            selectedGrade = 2; // 에픽
        }
        else if (totalDamage > 10000)
        {
            selectedGrade = 1; // 전설
        }

        // 유물 보상
        _rewardList[0].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnChestItem(selectedGrade));

        // 만약 유물을 받지 못 했다면 골드로 대체
        if(_rewardList[0].GetItemStock() == null)
        {
            switch (selectedGrade)
            {
                case 0:
                    _rewardList[0].init(Reward.RewardType.money, _moneyicon, "777");
                    break;
                case 1:
                    _rewardList[0].init(Reward.RewardType.money, _moneyicon, "7777");
                    break;
                case 2:
                    _rewardList[0].init(Reward.RewardType.money, _moneyicon, "77777");
                    break;
            }
        }

        _popup.OnePopup();
    }

    int GoldReward()
    {
        float range = Random.Range(1f, 10f);
        int reward = (100 * _userInfoManager.userData.GameLevel) + (int)((_userInfoManager.userData.GameLevel + 1) * range * 10);
        return reward;
    }

    public void RewardReset()
    {
        for (int i = 0; i < _rewardList.Count; i++)
        {
            _rewardList[i].Reset();
        }
    }

    public void RandomReward(int index)
    {
        int rewardType = 0;
        int RewardPro = Random.Range(0, _bonusProbability.Sum());

        if (RewardPro < _bonusProbability[0])           // 66% 확률로 0 반환
            rewardType = 0;
        else if (RewardPro < _bonusProbability[0] + _bonusProbability[1])  // 66% + 22% 확률로 1 반환
            rewardType = 1;
        else if (RewardPro < _bonusProbability[0] + _bonusProbability[1] + _bonusProbability[2])  // 66% + 22% + 9% 확률로 2 반환
            rewardType = 2;
        else                                    // 나머지 3% 확률로 3 반환
            rewardType = 3;

        switch (rewardType)
        {
            case 0:
                _rewardList[index].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());
                break;
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
        for (int i = 0; i < _rewardList.Count; i++)
        {
            if (_rewardList[i].transform.gameObject.activeInHierarchy)
            {
                _rewardList[i].RewardGet();
                continue;
            }
        }
    }

    public void SetBonusReward(int value)
    {
        _bonusReward = value;
    }
}
