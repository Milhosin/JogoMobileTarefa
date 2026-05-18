using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollactableBase : MonoBehaviour
{
    public string compareTag = "Player";
    public new ParticleSystem particleSystem;
    public float timeToHide = 3;
    public GameObject graphicItem;

    [Header("Sounds")]
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.CompareTag(compareTag))
        {
            Collect();
        }
    }

    private void HideObject()
    {
        gameObject.SetActive(false);
    }

    protected virtual void Collect()
    {
        if (graphicItem != null) graphicItem.SetActive(false);
        Invoke("HideObject", timeToHide);
        OnCollect();
    }

    protected virtual void OnCollect()
    {
        if (particleSystem != null)
        {
            // 1. Desanexa da moeda imediatamente
            particleSystem.transform.SetParent(null);

            // 2. Garante que a escala não está zerada (herdada do CoinsAnimationManager)
            particleSystem.transform.localScale = Vector3.one;

            // 3. Para a partícula e limpa qualquer delay acumulado no cache da Unity
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            // 4. Ativa e força a emissão instantânea das partículas no local
            particleSystem.Play();
        }

        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.CoinBounce();
        }
    }


}