using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float _battleTime = 60f;
    [SerializeField] private float _chestTime = 20f;
    [SerializeField] private float _timeRemaining = 60f; // 타이머 시간
    public Slider _timerSlider;

    [SerializeField] private bool _isChestTime = false;
    private bool _timerIsRunning = false;

    Unit_Manager _unitManager;
    PlayerMove _playerMove;

    public void Awake()
    {
        _timeRemaining = _battleTime;
    }

    public void Start()
    {
        _unitManager = SoonsoonData.Instance.Unit_Manager;
        _playerMove = SoonsoonData.Instance.Player_Move;
    }

    public void TimerReset()
    {
        if (_playerMove.plate[_playerMove.currentIndex]._plateType == Plate.PlateType.Chest) _isChestTime = true;

        _timeRemaining = (_isChestTime) ? _chestTime : _battleTime;
        _timerIsRunning = false;
        DisplayTime(_timeRemaining);
    }

    public void TimerStart()
    {
        _timerIsRunning = true;
    }

    public void TimerStop()
    {
        _timerIsRunning = false;
    }

    private void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
                DisplayTime(_timeRemaining);
            }
            else
            {
                TimeSet();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        _timerSlider.value = (_isChestTime) ? timeToDisplay / _chestTime : timeToDisplay / _battleTime;
    }
    
    void TimeSet()
    {
        _timeRemaining = 0;
        _timerIsRunning = false;

        if (_isChestTime) _unitManager.GameWin();
        else _unitManager.GameLose();

        _isChestTime = false;
    }
}
