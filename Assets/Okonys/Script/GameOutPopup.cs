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
        _achievementValue[0].text = _userInfoManager.userData.totalKillEnemy.ToString() + " ��";
        _achievementValue[1].text = _userInfoManager.userData.totalKillBoss.ToString() + " ��";
        _achievementValue[2].text = _userInfoManager.userData.totalGetGold.ToString() + " ��";
        _achievementValue[3].text = _userInfoManager.userData.UserArtifacts.Count.ToString() + " ��";
        _achievementValue[4].text = _userInfoManager.userData.TurnCounter.ToString() + " ��";
        _achievementValue[5].text = _userInfoManager.userData.totalKillEnemy.ToString() + " ����";
    }
}
