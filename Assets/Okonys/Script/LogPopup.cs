using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogPopup : MonoBehaviour
{
    public TextMeshProUGUI _logText;
    public Animator _animator;

    private void Awake()
    {
        SoonsoonData.Instance.LogPopup = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowLog(string text)
    {
        _logText.text = text;
        _animator.SetTrigger("Show");
    }
}
