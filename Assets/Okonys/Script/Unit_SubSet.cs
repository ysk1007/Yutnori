using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit_SubSet : MonoBehaviour
{
    public List<TextMeshProUGUI> TextList;
    public List<Animator> AnimatorList;
    public Color[] TextColorList;
    public Slider HpSlider;
    public Slider CTSlider;
    public int TextNum;

    Unit unit;
    float _unitMaxHp;
    float _unitSkillCT;
    // Start is called before the first frame update
    void Start()
    {
        unit = this.gameObject.GetComponentInParent<Unit>();
        _unitMaxHp = unit._unitHp;
        _unitSkillCT = unit._unitCT;
        TextNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HpSlider.value = unit._unitHp / _unitMaxHp;
        CTSlider.value = unit._skillTimer / _unitSkillCT;
    }

    public void ShowDamageText(float value)
    {
        TextList[TextNum].text = value.ToString();
        TextList[TextNum].color = TextColorList[0];
        AnimatorList[TextNum].SetTrigger("Show");
        if (TextNum == TextList.Count - 1)
        {
            TextNum = 0;
        }
        else
        {
            TextNum++;
        }
    }

    public void ShowHealText(float value)
    {
        TextList[TextNum].text = value.ToString();
        TextList[TextNum].color = TextColorList[1];
        AnimatorList[TextNum].SetTrigger("Show");
        if (TextNum == TextList.Count - 1)
        {
            TextNum = 0;
        }
        else
        {
            TextNum++;
        }
    }
}
