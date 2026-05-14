using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Singleton;
using DG.Tweening;
using System.Linq;

public class CoinsAnimationManager : Singleton<CoinsAnimationManager>
{
    public List<ItemCollactableBase> itens;

    [Header("Animation")]
    public float scaleDuration = .2f;
    public float scaleTimeBetweenPieces = .1f;
    public Ease ease = Ease.OutBack;

    [Header("Delay Configuration")]
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private float resetMapDelay = 0.2f;

    private void Start()
    {
        Invoke(nameof(StartAnimations), startDelay);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            TriggerResetAnimations();
        }
    }

    public void TriggerResetAnimations()
    {
        StopAllCoroutines();
        itens.Clear();
        StartCoroutine(WaitAndSpawnCoins());
    }

    IEnumerator WaitAndSpawnCoins()
    {
        yield return new WaitForSeconds(resetMapDelay);

        var newCoins = FindObjectsOfType<ItemCollactableBase>().ToList();

        if (PlayerController.Instance != null)
        {
            newCoins = newCoins.OrderBy(coin => Vector3.Distance(PlayerController.Instance.transform.position, coin.transform.position)).ToList();
        }

        foreach (var coin in newCoins)
        {
            RegisterCoin(coin);
        }

        foreach (var p in itens)
        {
            if (p != null) p.transform.localScale = Vector3.zero;
        }

        yield return null;

        for (int i = 0; i < itens.Count; i++)
        {
            if (itens[i] != null)
            {
                itens[i].transform.DOScale(1, scaleDuration).SetEase(ease);

                Collider c = itens[i].GetComponent<Collider>();
                if (c != null) c.enabled = true;

                yield return new WaitForSeconds(scaleTimeBetweenPieces);
            }
        }
    }

    public void RegisterCoin(ItemCollactableBase i)
    {
        if (!itens.Contains(i))
        {
            itens.Add(i);
            i.transform.localScale = Vector3.zero;

            Collider c = i.GetComponent<Collider>();
            if (c != null) c.enabled = false;
        }
    }

    public void StartAnimations()
    {
        TriggerResetAnimations();
    }
}
