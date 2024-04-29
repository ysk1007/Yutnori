using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gambling : MonoBehaviour
{
    public Transform[] _cups;
    public Button[] _buttons;
    public GameObject _startButton;
    public Image _balliamge;
    public Transform _ball;
    public Animator _animator;

    int _ballIndex = 0;
    public bool _doPlay = false;

    UserInfoManager _infoManager;
    private int _gambleMoney;

    // Start is called before the first frame update
    void Start()
    {
        _infoManager = UserInfoManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mix(int gambleMoney)
    {
        if (_doPlay) return;
        if (_infoManager.userData.UserGold < gambleMoney)
        {
            SoonsoonData.Instance.LogPopup.ShowLog("골드가 부족 합니다.");
            return;
        }

        _doPlay = true;
        _infoManager.userData.UserGold -= gambleMoney;
        _gambleMoney = gambleMoney;
        _startButton.SetActive(false);
        _animator.SetTrigger("Start");
        _ballIndex = Random.Range(0, 3);
    }

    public void SelectCup(int index)
    {
        if (!_doPlay) return;

        _infoManager.userData.UserGold += (_ballIndex == index) ? _gambleMoney * 3 : 0;
        if (_ballIndex == index) SoonsoonData.Instance.LogPopup.ShowLog("승리!");
        else SoonsoonData.Instance.LogPopup.ShowLog("패배");

        _gambleMoney = 0;
        _doPlay = false;
        ButtonSetting();
        _animator.SetTrigger("End");
    }

    public void HideBall()
    {
        _balliamge.enabled = false;
        _ball.position = new Vector3(_cups[_ballIndex].position.x, _ball.position.y, 0f);
    }

    public void ShowBall()
    {
        _balliamge.enabled = true;
    }

    public void ButtonSetting()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].enabled = (_doPlay) ? true : false;
        }
    }

    public void GameButtonSetting()
    {
        _startButton.SetActive(true);
    }
}
