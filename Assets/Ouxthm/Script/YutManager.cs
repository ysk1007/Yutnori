using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class YutManager : MonoBehaviour
{
    [SerializeField] private Transform _yutSimulation;
    VideoPlayer _yutSimulationPlayer;
    Vector3 _playSize = new Vector3(3f, 3f, 3f);

    [SerializeField] private VideoClip[] _doClip;
    [SerializeField] private VideoClip[] _gaeClip;
    [SerializeField] private VideoClip[] _geolClip;
    [SerializeField] private VideoClip[] _yutClip;
    [SerializeField] private VideoClip[] _moClip;

    [SerializeField] private TextMeshProUGUI _yutText;
    [SerializeField] private Animator _textAnim;
    [SerializeField] private TMP_ColorGradient[] _textColors;

    [SerializeField] private Color[] _particleColorParent;
    [SerializeField] private Color[] _particleColorChild;
    [SerializeField] private ParticleSystem _particleParent;
    [SerializeField] private ParticleSystem _particleChild;

    public static YutManager instance;
    public PlayerMove playerMv;

    public float rand;
    public int _moveDistance;      // 윷 이동 거리
    public List<Plate> _plateList;
    [SerializeField]
    private int[] PlateData = new int[29];

    public float _randomPlateProbability; // 랜덤 칸이 등장할 확률

    public Sprite[] _icons;

    public Button btn;

    AudioManager _audioManager;
    UserInfoManager _userInfoManager;
    CanvasManager _canvasManager;
    private void Awake()
    {
        instance = this;
        _yutSimulationPlayer = _yutSimulation.GetComponent<VideoPlayer>();
        _yutSimulationPlayer.loopPointReached += OnVideoEnd;
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        _audioManager = AudioManager.instance;
        _canvasManager = SoonsoonData.Instance.Canvas_Manager;
    }

    // 윷 시뮬레이터가 끝나면 실행되는 메소드
    void OnVideoEnd(VideoPlayer vp)
    {
        _textAnim.SetTrigger("Show");
        switch (_moveDistance)
        {
            case 1:
                _audioManager.PlaySfx(AudioManager.Sfx.Do);
                break;
            case 2:
                _audioManager.PlaySfx(AudioManager.Sfx.Gae);
                break;
            case 3:
                _audioManager.PlaySfx(AudioManager.Sfx.Geol);
                break;
            case 4:
                _audioManager.PlaySfx(AudioManager.Sfx.Yut);
                break;
            case 5:
                _audioManager.PlaySfx(AudioManager.Sfx.Mo);
                break;
        }
    }

    // 움직임 코루틴 실행
    public void PlayerMove()
    {
        _yutSimulation.localScale = Vector3.zero;
        playerMv.StartCoroutine("Move");
    }

    // 발판 배치를 불러오거나 생성하는 셋업 메소드
    public void SetPlate()
    {
        if (!_userInfoManager.userData.isPlateData)
        {
            RandomPlate();
        }
        else
        {
            GetPlateData();
        }
    }

    // 발판 데이터 랜덤 생성
    public void RandomPlate()
    {
        // 발판 랜덤 배치
        for (int i = 0; i < _plateList.Count; i++)
        {
            switch (_plateList[i]._plateType.GetHashCode())
            {
                case 0:
                case 1:
                    int type;
                    float randomNumber = Random.value;
                    type = (_randomPlateProbability > randomNumber) ? 1 : 0;
                    _plateList[i].init(type);
                    PlateData[i] = type;
                    break;
                case 2:
                    PlateData[i] = 2;
                    break;
                case 3:
                    PlateData[i] = 3;
                    break;
                case 4:
                    PlateData[i] = 4;
                    break;
            }
        }

        _userInfoManager.userData.PlatesData = PlateData;
        _userInfoManager.userData.isPlateData = true;
        _userInfoManager.UserDataSave();
    }

    public void GetPlateData()
    {
        PlateData = _userInfoManager.userData.PlatesData;

        // 발판 배치
        for (int i = 0; i < PlateData.Length; i++)
        {
            switch (PlateData[i])
            {
                case 0:
                    _plateList[i]._plateType = Plate.PlateType.Enemy;
                    _plateList[i].init();
                    break;
                case 1:
                    _plateList[i]._plateType = Plate.PlateType.Random;
                    _plateList[i].init();
                    break;
                case 2:
                    _plateList[i]._plateType = Plate.PlateType.Home;
                    _plateList[i].init();
                    break;
                case 3:
                    _plateList[i]._plateType = Plate.PlateType.Elite;
                    _plateList[i].init();
                    break;
                case 4:
                    _plateList[i]._plateType = Plate.PlateType.Chest;
                    _plateList[i].init();
                    break;
            }
        }
    }

    public void ThrowYut()
    {
        _canvasManager._tutorialHand._popup.ZeroPopup();

        rand = Random.Range(0f, 1f);
        int cilpNum = Random.Range(0, 3);
        if(rand <= 0.2f)
        {
            _yutText.text = "도";
            SetParticleColor(0);
            _yutText.colorGradientPreset = _textColors[0];
            _yutSimulationPlayer.clip = _doClip[cilpNum];
            _moveDistance = 1;
        }
        else if(0.2f < rand && rand <= 0.5f)
        {
            _yutText.text = "개";
            SetParticleColor(1);
            _yutText.colorGradientPreset = _textColors[1];
            _yutSimulationPlayer.clip = _gaeClip[cilpNum];
            _moveDistance = 2;
        }
        else if(0.5f < rand && rand <= 0.7f)
        {
            _yutText.text = "걸";
            SetParticleColor(2);
            _yutText.colorGradientPreset = _textColors[2];
            _yutSimulationPlayer.clip = _geolClip[cilpNum];
            _moveDistance = 3;
        }
        else if(0.7f < rand && rand <= 0.85f)
        {
            _yutText.text = "윷";
            SetParticleColor(3);
            _yutText.colorGradientPreset = _textColors[3];
            _yutSimulationPlayer.clip = _yutClip[cilpNum];
            _moveDistance = 4;
        }
        else
        {
            _yutText.text = "모";
            SetParticleColor(4);
            _yutText.colorGradientPreset = _textColors[4];
            _yutSimulationPlayer.clip = _moClip[cilpNum];
            _moveDistance = 5;
        }
        _yutSimulation.localScale = _playSize;
        _yutSimulationPlayer.Play();
        _audioManager.PlaySfx(AudioManager.Sfx.YutSounds);
    }

    public void ExactThrowYut(int i)
    {
        _canvasManager._tutorialHand._popup.ZeroPopup();

        int cilpNum = Random.Range(0, 3);
        if (i == 0)
        {
            _yutText.text = "도";
            SetParticleColor(0);
            _yutText.colorGradientPreset = _textColors[0];
            _yutSimulationPlayer.clip = _doClip[cilpNum];
            _moveDistance = 1;
        }
        else if (i == 1)
        {
            _yutText.text = "개";
            SetParticleColor(1);
            _yutText.colorGradientPreset = _textColors[1];
            _yutSimulationPlayer.clip = _gaeClip[cilpNum];
            _moveDistance = 2;
        }
        else if (i == 2)
        {
            _yutText.text = "걸";
            SetParticleColor(2);
            _yutText.colorGradientPreset = _textColors[2];
            _yutSimulationPlayer.clip = _geolClip[cilpNum];
            _moveDistance = 3;
        }
        else if (i == 3)
        {
            _yutText.text = "윷";
            SetParticleColor(3);
            _yutText.colorGradientPreset = _textColors[3];
            _yutSimulationPlayer.clip = _yutClip[cilpNum];
            _moveDistance = 4;
        }
        else
        {
            _yutText.text = "모";
            SetParticleColor(4);
            _yutText.colorGradientPreset = _textColors[4];
            _yutSimulationPlayer.clip = _moClip[cilpNum];
            _moveDistance = 5;
        }
        _yutSimulation.localScale = _playSize;
        _yutSimulationPlayer.Play();
        _audioManager.PlaySfx(AudioManager.Sfx.YutSounds);
    }

    public void SetYut(int i)
    {
        _moveDistance = i;
    }

    [System.Obsolete]
    void SetParticleColor(int index)
    {
        _particleChild.startColor = _particleColorChild[index];
        _particleParent.startColor = _particleColorParent[index];
    }
}
