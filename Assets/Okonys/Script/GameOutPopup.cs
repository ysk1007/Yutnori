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
            _gameSetText.text = "승리";
            _popupBackground.sprite = _gameSetImage[0];
            _gameSetText.colorGradientPreset = _color[0];
            _gameWinParticle.SetActive(true);
        }
        else
        {
            _gameSetText.text = "패배";
            _popupBackground.sprite = _gameSetImage[1];
            _gameSetText.colorGradientPreset = _color[1];
            _gameLoseImage.SetActive(true);
        }
        _achievementValue[0].text = _userInfoManager.userData.totalKillEnemy.ToString() + " 명";
        _achievementValue[1].text = _userInfoManager.userData.totalKillBoss.ToString() + " 명";
        _achievementValue[2].text = _userInfoManager.userData.totalGetGold.ToString() + " 냥";
        _achievementValue[3].text = _userInfoManager.userData.UserArtifacts.Count.ToString() + " 개";
        _achievementValue[4].text = _userInfoManager.userData.TurnCounter.ToString() + " 턴";
        _achievementValue[5].text = _userInfoManager.userData.totalKillEnemy.ToString() + " 바퀴";
    }

    public void SetGameWin()
    {
        _gameWin = true;
    }
}
