using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup audioMixerGroup;
    public AudioMixerGroup audioMixerBGM;
    public AudioMixerGroup audioMixerSFX;

    [Header("#배경음악")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#효과음")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    UserInfoManager _userInfoManager;

    public enum Sfx { YutSounds, 
        Do = 5,Gae = 7,Geol = 9,Yut = 11,Mo = 13,

        Dead, Hit, LevelUp, Lose, Melee, Range, Select, Win }

    private void Awake()
    {
        instance = this;
        Init();
    }

    private void Start()
    {
        _userInfoManager = UserInfoManager.Instance;
        PlayBgm(true);
    }

    void Init()
    {
        // 배경음 플레이어 초기화
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmPlayer.outputAudioMixerGroup = audioMixerBGM;
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();

        // 배경음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].bypassListenerEffects = true;
            sfxPlayers[i].volume = sfxVolume;
            sfxPlayers[i].outputAudioMixerGroup = audioMixerSFX;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (isPlay)
        {
            bgmPlayer.Play();
        }
        else
        {
            bgmPlayer.Stop();
        }
    }

    public void EffectBgm(bool isPlay)
    {
        bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        //쉬고있는 효과음 플레이어를 찾기
        for (int i = 0; i < sfxPlayers.Length; i++)
        {
            int loopIndex;

            if (sfx == Sfx.YutSounds) loopIndex = Random.Range(0, 5);
            else
            {
                loopIndex = (i + channelIndex) % sfxPlayers.Length;
            }

            if (sfxPlayers[loopIndex].isPlaying) //이미 효과음을 재생중이라면
                continue;

            //다중 효과음 있을 경우
            int ranIndex = 0;
            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
            {
                ranIndex = Random.Range(0, 2);
            }

            channelIndex = loopIndex; //마지막 채널 인덱스 갱신

            // 도개걸윷모 음성 효과일 때 남여 음성효과 구분
            if (sfx == Sfx.Do || sfx == Sfx.Gae || sfx == Sfx.Geol || sfx == Sfx.Yut || sfx == Sfx.Mo)
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + _userInfoManager.optionData.GetVoiceType()];

            else 
                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx + ranIndex];

            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
