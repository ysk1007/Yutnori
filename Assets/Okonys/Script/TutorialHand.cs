using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private RectTransform _imageToMove; // �̵��� �̹���
    [SerializeField] private RectTransform _targetImage; // ��ǥ ��ġ �̹���

    [SerializeField] private RectTransform _shopVisitButton;
    [SerializeField] private RectTransform _yutThrowButton;

    [SerializeField] private float _moveDuration = 1.0f; // �̵� �ð�
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
        // �ʱ� ��ǥ ��ġ ����
        SetTargetPosition();
    }

    void Update()
    {
        if (_isMoving)
        {
            // �̵� ����
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _moveDuration;
            _imageToMove.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, t);

            // �̵��� �Ϸ�Ǹ�
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
        // �̵� ����
        _isMoving = true;
        _elapsedTime = 0f;
        _startPosition = _imageToMove.anchoredPosition;
        SetTargetPosition(); // ��ǥ ��ġ ����
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
