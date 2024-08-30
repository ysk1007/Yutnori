using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpPopup()
    {
        transform.localScale = Vector3.up;
    }

    public void OnePopup()
    {
        transform.localScale = Vector3.one;
    }

    public void ZeroPopup()
    {
        transform.localScale = Vector3.zero;
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
