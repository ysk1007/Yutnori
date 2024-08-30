using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private RectTransform _imageToMove; // 이동할 이미지
    [SerializeField] private RectTransform _targetImage; // 목표 위치 이미지

    [SerializeField] private RectTransform _shopVisitButton;
    [SerializeField] private RectTransform _yutThrowButton;

    [SerializeField] private float _moveDuration = 1.0f; // 이동 시간
    public Popup _popup;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private float _elapsedTime = 0f;
    private bool _isMoving = false;

    private void Awake()
    {
        _popup = this.GetComponent<Popup>();
    }


    void Start()
    {
        // 초기 목표 위치 설정
        SetTargetPosition();
    }

    void Update()
    {
        if (_isMoving)
        {
            // 이동 로직
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _moveDuration;
            _imageToMove.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, t);

            // 이동이 완료되면
            if (_elapsedTime >= _moveDuration)
            {
                _isMoving = false;
                _elapsedTime = 0f;
            }
        }
    }

    void SetTargetPosition()
    {
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _targetImage.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_imageToMove.parent, screenPoint, null, out _targetPosition);
    }

    void StartMoving()
    {
        // 이동 시작
        _isMoving = true;
        _elapsedTime = 0f;
        _startPosition = _imageToMove.anchoredPosition;
        SetTargetPosition(); // 목표 위치 갱신
    }

    public void SetTargetImage(RectTransform rect)
    {
        if (!rect) 
        { 
            _popup.ZeroPopup();
            return;
        }
        _popup.OnePopup();
        _targetImage = rect;
        StartMoving();
    }

    public void ShopGuide()
    {
        _popup.OnePopup();
        _targetImage = _shopVisitButton;
        StartMoving();
    }

    public void ThrowGuide()
    {
        _popup.OnePopup();
        _targetImage = _yutThrowButton;
        StartMoving();
    }
}
