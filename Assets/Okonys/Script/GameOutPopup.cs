using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOutPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _achievementValue;


    UserInfoManager _userInfoManager;

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        _achievementValue[0].text = _userInfoManager.userData.totalKillEnemy.ToString() + " 명";
        _achievementValue[1].text = _userInfoManager.userData.totalKillBoss.ToString() + " 명";
        _achievementValue[2].text = _userInfoManager.userData.totalGetGold.ToString() + " 냥";
        _achievementValue[3].text = _userInfoManager.userData.UserArtifacts.Count.ToString() + " 개";
        _achievementValue[4].text = _userInfoManager.userData.TurnCounter.ToString() + " 턴";
        _achievementValue[5].text = _userInfoManager.userData.totalKillEnemy.ToString() + " 바퀴";
    }
}
