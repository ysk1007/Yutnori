using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    [SerializeField] private SPUM_Prefabs _loadingUnit;

    [SerializeField] private TextMeshProUGUI _tipTextArea;
    [SerializeField] private string[] _tipTexts;

    [SerializeField] private TextMeshProUGUI _loadTextArea;
    [SerializeField] private string[] _loadTexts;


    [SerializeField] private Slider _loadingBar;
    [SerializeField] private float _updateTerm;
    private float _timer = 0f;
    private int _index = 0;

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _updateTerm)
        {
            LoadTextUpadate();
        }
    }

    void LoadTextUpadate()
    {
        _loadTextArea.text = _loadTexts[_index];
        _index = (_index >= _loadTexts.Length - 1) ? 0 : _index + 1;
        _timer = 0f;
    }

    IEnumerator LoadSceneProcess()
    {
        _loadingUnit.PlayAnimation("1");
        _tipTextArea.text = _tipTexts[UnityEngine.Random.Range(0, _tipTexts.Length)];
        //_loadingUnit.PlayAnimation("run");

        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);

        // 씬의 로딩이 끝나면 자동으로 불러온 씬으로 이동할 것인지 설정
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                _loadingBar.value = op.progress;
            }
            // 90% 로드가 완료되면 1초동안 페이크 로딩하고 다음 씬을 불러옴
            else
            {
                timer += Time.unscaledDeltaTime;
                _loadingBar.value = Mathf.Lerp(0.9f, 1f, timer);
                if (_loadingBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
