using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLabel : MonoBehaviour
{
    [SerializeField]
    private TypewriterByCharacter _typewriterByCharacter;

    public void StartShow()
    {
        _typewriterByCharacter.StartShowingText();
    }

    public void StopShow()
    {
        _typewriterByCharacter.StopShowingText();
    }
}
