using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOutPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _achievementValue;
    [SerializeField] private TextMeshProUGUI _gameSetText;
    [SerializeField] private GameObject _gameWinParticle;
    [SerializeField] private GameObject _gameLoseImage;

    [SerializeField] private Sprite[] _gameSetImage;
    [SerializeField] private TMP_ColorGradient[] _color;

    [SerializeField] private bool _gameWin = false;
    UserInfoManager _userInfoManager;
    Image _popupBackground;

    private void Awake()
    {
        _popupBackground = this.GetComponent<Image>();
    }

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
        if (_gameWin)
        {
            _gameSetText.text = "�¸�";
            _popupBackground.sprite = _gameSetImage[0];
            _gameSetText.colorGradientPreset = _color[0];
            _gameWinParticle.SetActive(true);
        }
        else
        {
            _gameSetText.text = "�й�";
            _popupBackground.sprite = _gameSetImage[1];
            _gameSetText.colorGradientPreset = _color[1];
            _gameLoseImage.SetActive(true);
        }
        _achievementValue[0].text = _userInfoManager.userData.totalKillEnemy.ToString() + " ��";
        _achievementValue[1].text = _userInfoManager.userData.totalKillBoss.ToString() + " ��";
        _achievementValue[2].text = _userInfoManager.userData.totalGetGold.ToString() + " ��";
        _achievementValue[3].text = _userInfoManager.userData.UserArtifacts.Count.ToString() + " ��";
        _achievementValue[4].text = _userInfoManager.userData.TurnCounter.ToString() + " ��";
        _achievementValue[5].text = _userInfoManager.userData.totalKillEnemy.ToString() + " ����";
    }

    public void SetGameWin()
    {
        _gameWin = true;
    }
}
