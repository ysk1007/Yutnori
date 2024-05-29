using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public Animator _canvasAnimator;

    public GameObject _uiCanvas;
    public GameObject _yutCanvas;
    public Transform _units;

    private void Awake()
    {
        SoonsoonData.Instance.Canvas_Manager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
}
