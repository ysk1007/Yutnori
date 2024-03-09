using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class SPUM_Prefabs : MonoBehaviour
{
    public float _version;
    public SPUM_SpriteList _spriteOBj;
    public bool EditChk;
    public string _code;
    public Animator _anim;
    public bool _horse;
    public bool isRideHorse{
        get => _horse;
        set {
            _horse = value;
            UnitTypeChanged?.Invoke();
        }
    }
    public string _horseString;

    public UnityEvent UnitTypeChanged = new UnityEvent();
    private AnimationClip[] _animationClips;
    public AnimationClip[] AnimationClips => _animationClips;
    private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();
    private void InitAnimPair(){
        _nameToHashPair.Clear();
        _animationClips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            int hash = Animator.StringToHash(clip.name);
            _nameToHashPair.Add(clip.name, hash);
        }
    }
    private void Awake() {
        InitAnimPair();
    }
    private void Start() {
        UnitTypeChanged.AddListener(InitAnimPair);
    }
    // 이름으로 애니메이션 실행
    public void PlayAnimation(string name){

        Debug.Log(name);
        
        foreach (var animationName in _nameToHashPair)
        {
            if(animationName.Key.ToLower().Contains(name.ToLower()) ){
                _anim.Play(animationName.Value, 0);
                break;
            }
        }
    }

    public void PlayAnimation(int num)
    {
        switch (num)
        {
            case 0: //Idle
                _anim.SetFloat("RunState", 0f);
                break;
            case 1: // Run
                _anim.SetFloat("RunState", 0.5f);
                break;
            case 2: // Death
                _anim.SetTrigger("Die");
                _anim.SetBool("EditChk", EditChk);
                break;
            case 3: // Stun
                _anim.SetFloat("RunState", 1f);
                break;
            case 4: // Attack Sword
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 0.0f);
                _anim.SetFloat("NormalState", 0.0f);
                break;
            case 5: // Attack Bow
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 0.0f);
                _anim.SetFloat("NormalState", 0.5f);
                break;
            case 6: // Attack Magic
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 0.0f);
                _anim.SetFloat("NormalState", 1.0f);
                break;
            case 7: // Skill Sword
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 1.0f);
                _anim.SetFloat("NormalState", 0.0f);
                break;
            case 8: // Skill Bow
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 1.0f);
                _anim.SetFloat("NormalState", 0.5f);
                break;
            case 9: // Skill Magic
                _anim.SetTrigger("Attack");
                _anim.SetFloat("AttackState", 1.0f);
                _anim.SetFloat("NormalState", 1.0f);
                break;
        }
    }

}
