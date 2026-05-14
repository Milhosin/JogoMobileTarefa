using UnityEngine;
using Ebac.Core.Singleton;
using TMPro;
using DG.Tweening;
using System;

public class PlayerController : Singleton<PlayerController>
{
    //public
    [Header("Lerp")]
    public Transform target;
    public float lerpSpeed = 1f;

    public float speed = 1f;

    public string tagToCheckEnemy = "Enemy";
    public string tagToCheckEndLine = "EndLine";

    public GameObject endScreen;
    public bool invencible = false;

    [Header("Animation")]
    public AnimatorManager animatorManager;

    [Header("Settings de Surgimento")]
    [SerializeField] private float spawnDuration = 0.6f;

    //private
    private bool _canRun;
    private Vector3 _pos;
    private float _currentSpeed;
    private Vector3 _startPosition;
    private float _baseSpeedToAnimation = 7;

    private void Start()
    {
        _startPosition = transform.position;
        ResetSpeed();

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, spawnDuration).SetEase(Ease.OutBack);
    }

    public void Bounce()
    {
        transform.localScale = Vector3.one;

        Sequence bounceSequence = DOTween.Sequence();

        bounceSequence.Append(transform.DOScale(1.2f, 0.15f).SetEase(Ease.OutQuad))
                      .Append(transform.DOScale(1.0f, 0.15f).SetEase(Ease.InQuad));
    }

    public void CoinBounce()
    {
        transform.localScale = Vector3.one;

        Sequence coinSequence = DOTween.Sequence();

        coinSequence.Append(transform.DOScale(1.08f, 0.08f).SetEase(Ease.OutQuad)) 
                    .Append(transform.DOScale(1f, 0.08f).SetEase(Ease.InQuad));   
    }


    void Update()
    {
        if (!_canRun) return;


        _pos = target.position;
        _pos.y = transform.position.y;
        _pos.z = transform.position.z;

        transform.position = Vector3.Lerp(transform.position, _pos, lerpSpeed * Time.deltaTime);
        transform.Translate(transform.forward * _currentSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == tagToCheckEnemy)
        {
            if (!invencible)
            {
                MoveBack();
                EndGame(AnimatorManager.AnimationType.DEAD);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == tagToCheckEndLine)
        {
            EndGame();
        }
    }

    private void MoveBack()
    {
        transform.DOMoveZ(-1f, .3f).SetRelative();
    }

    private void EndGame(AnimatorManager.AnimationType animationType = AnimatorManager.AnimationType.IDLE)
    {
        _canRun = false;
        endScreen.SetActive(true);
        animatorManager.Play(animationType);
    }

    public void StartToRun()
    {
        _canRun = true;
        animatorManager.Play(AnimatorManager.AnimationType.RUN, _currentSpeed / _baseSpeedToAnimation);
    }

    #region POWER UPS

    public void PowerUpSpeedUp(float f)
    {
        _currentSpeed = f;
    }
    public void ResetSpeed()
    {
        _currentSpeed = speed;
    }

    public void SetInvencible(bool b = true)
    {
        invencible = b;
    }

    public void ChangeHeight(float amount, float duration, float animationDuration, Ease ease)
    {
        transform.DOMoveY(_startPosition.y + amount, animationDuration).SetEase(ease);
        Invoke(nameof(ResetHeight), duration);
    }

    public void ResetHeight()
    {
        transform.DOMoveY(_startPosition.y, .1f);
    }
    #endregion
}
