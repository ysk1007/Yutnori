using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleReward : MonoBehaviour
{
    // 보상 등장 확률 (0: 돈, 1: 소모품, 2: 유닛, 3: 아티팩트)
    [SerializeField] private int[] _rewardProbability;
    [SerializeField] private int[] _bonusProbability = { 66, 22, 9, 3 }; // 추가 보상 확률
    [SerializeField] private int _bonusReward; // 추가 보상 개수
    [SerializeField] private Transform _rewards; // 보상을 담고 있는 부모 오브젝트
    [SerializeField] private List<Reward> _rewardList; // 보상 리스트
    [SerializeField] private Sprite _moneyicon; // 돈 아이콘
    private UserInfoManager _userInfoManager;
    private UnitPool _unitPool;
    private ItemShop _itemShop;
    private Popup _popup;

    private void Awake()
    {
        SoonsoonData.Instance.Battle_Reward = this;

        // Popup 컴포넌트 초기화
        _popup = gameObject.GetComponent<Popup>();

        // 보상 리스트 초기화 및 리셋
        for (int i = 0; i < _rewards.childCount; i++)
        {
            Reward reward = _rewards.GetChild(i).GetComponent<Reward>();
            reward.Reset();
            _rewardList.Add(reward);
        }
    }

    private void Start()
    {
        // 필요한 매니저 및 풀 초기화
        _userInfoManager = UserInfoManager.Instance;
        _unitPool = SoonsoonData.Instance.Unit_pool;
        _itemShop = SoonsoonData.Instance.ItemShop;
    }

    public void NormalBattleReward()
    {
        RewardReset(); // 보상 초기화

        int rewardPro = Random.Range(0, 100);

        // 첫번째 기본 골드 보상
        if (_rewardProbability[0] > rewardPro)
            _rewardList[0].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // 두번째 확률 소모품 보상
        if (_rewardProbability[1] > rewardPro)
            _rewardList[1].init(Reward.RewardType.money, _moneyicon, GoldReward().ToString());

        // 세번째 확률 유닛 보상
        if (_rewardProbability[2] > rewardPro)
            _rewardList[2].init(Reward.RewardType.unit, null, null, _unitPool.ReturnRewardUnit(_userInfoManager.userData.GameLevel));

        // 네번째 확률 아티팩트 보상
        if (_rewardProbability[3] > rewardPro)
            _rewardList[3].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnRewardItem());

        // 추가 보상 처리
        for (int i = 0; i < _bonusReward; i++)
        {
            RandomReward(4 + i);
        }

        // 보상 팝업 표시
        _popup.OnePopup();
    }

    public void ChestReward(float remainHp)
    {
        RewardReset(); // 보상 초기화

        float totalDamage = 7777777f - remainHp;
        int selectedGrade = 0; // 기본적으로 레어(0)

        if (totalDamage > 100000)
            selectedGrade = 2; // 에픽
        else if (totalDamage > 10000)
            selectedGrade = 1; // 전설

        // 유물 보상 초기화
        _rewardList[0].init(Reward.RewardType.artifact, null, null, null, _itemShop.ReturnChestItem(selectedGrade));

        // 만약 유물을 받지 못 했다면 골드로 대체
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

        // 보상 팝업 표시
        _popup.OnePopup();
    }

    private int GoldReward()
    {
        // 골드 보상 계산
        float range = Random.Range(1f, 10f);
        int reward = (100 * _userInfoManager.userData.GameLevel) + (int)((_userInfoManager.userData.GameLevel + 1) * range * 10);
        return reward;
    }

    public void RewardReset()
    {
        // 보상 초기화
        foreach (var reward in _rewardList)
        {
            reward.Reset();
        }
    }

    public void RandomReward(int index)
    {
        int rewardType = 0;
        int rewardPro = Random.Range(0, _bonusProbability.Sum());

        // 확률에 따라 보상 타입 결정
        if (rewardPro < _bonusProbability[0])
            rewardType = 0;
        else if (rewardPro < _bonusProbability[0] + _bonusProbability[1])
            rewardType = 1;
        else if (rewardPro < _bonusProbability[0] + _bonusProbability[1] + _bonusProbability[2])
            rewardType = 2;
        else
            rewardType = 3;

        // 결정된 타입에 따라 보상 초기화
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
        // 모든 활성화된 보상 수령
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
        // 추가 보상 개수 설정
        _bonusReward = value;
    }
}
