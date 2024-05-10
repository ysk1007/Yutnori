using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup : MonoBehaviour
{
    [SerializeField] private Transform _topButton;
    [SerializeField] private Transform _panel;

    [SerializeField] private List<Button> _topButtons;
    [SerializeField] private List<Image> _buttonImages;
    [SerializeField] private List<RectTransform> _panels;

    [SerializeField] private Color[] _colors;

    [SerializeField] private int _selectedIndex;

    private void Awake()
    {

        for (int i = 0; i < _topButton.childCount; i++)
        {
            _topButtons.Add(_topButton.GetChild(i).GetComponent<Button>()); ;
            _buttonImages.Add(_topButton.GetChild(i).GetComponent<Image>()); ;
        }

        for (int i = 0; i < _panel.childCount; i++)
        {
            _panels.Add(_panel.GetChild(i).GetComponent<RectTransform>()); ;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectPanel(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OptionOne()
    {
        this.transform.localScale = Vector3.one;
    }

    public void OptionZero()
    {
        this.transform.localScale = Vector3.zero;
    }

    public void SelectPanel(int index)
    {
        _selectedIndex = index;

        for (int i = 0; i < _buttonImages.Count; i++)
        {
            _buttonImages[i].color = (i == _selectedIndex) ? _colors[1] : _colors[0];
        }

        for (int i = 0; i < _panels.Count; i++)
        {
            _panels[i].localScale = (i == _selectedIndex) ? Vector3.one : Vector3.zero;
        }
    }
}
