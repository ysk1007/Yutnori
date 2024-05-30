using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Animator _canvasAnimator;
    public Animator _goldTextAnimator;
    public TextMeshProUGUI _goldText;

    public GameObject _uiCanvas;
    public GameObject _yutCanvas;
    public Transform _units;

    UserInfoManager _userInfoManager;
    private void Awake()
    {
        SoonsoonData.Instance.Canvas_Manager = this;
        _userInfoManager = UserInfoManager.Instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        _userInfoManager._canvasManager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUi()
    {
        _canvasAnimator.SetTrigger("Show");
        _yutCanvas.SetActive(false);
        _units.transform.localScale = Vector3.one;
    }

    public void FadeUi()
    {
        _canvasAnimator.SetTrigger("Fade");
        _yutCanvas.SetActive(true);
        _units.transform.localScale = Vector3.zero;
    }

    public void FadeImage()
    {
        _canvasAnimator.SetTrigger("FadeImage");
    }

    public void GetGoldAnimation(int value)
    {
        if (value == 0) return;

        _goldText.text = (value > 0) ? "+" + value.ToString() : value.ToString();
        _goldTextAnimator.SetTrigger("Show");

    }
}
