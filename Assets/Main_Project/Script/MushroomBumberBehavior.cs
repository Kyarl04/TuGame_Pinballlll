using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBumberBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ValueKeepingBehavior scoreKeep;

    [Header("Score Settings")]
    [SerializeField] private float scoreAmount = 10f;

    [Header("Bouncing Settings")]
    [SerializeField] private float bounceForce = 500f;

    [Header("Animation Settings")]
    [Tooltip("눌렸을 때의 최소 크기입니다. (예: 0.8)")]
    [SerializeField] private float minScale = 0.8f; 
    [Tooltip("애니메이션 속도입니다.")]
    [SerializeField] private float animationSpeed = 10f;
    
    private Vector3 originalScale; // 원래 크기 저장용
    private bool isAnimating = false;

    [Header("Effect & Sound")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private float effectDestroyTime = 2.0f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    void Start()
    {
        // 시작할 때 원래의 크기를 기억해둡니다.
        originalScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 점수 및 물리 처리
            if (scoreKeep != null) scoreKeep.score += scoreAmount;
            
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 bounceDirection = collision.contacts[0].normal;
                rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);
            }

            // 2. 부드러운 튕김 애니메이션 실행
            if (!isAnimating)
            {
                StartCoroutine(BounceAnimation());
            }

            // 3. 이펙트 및 사운드
            if (hitEffect != null)
            {
                ContactPoint contact = collision.contacts[0];
                GameObject effectInstance = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
                Destroy(effectInstance, effectDestroyTime);
            }

            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
    }

    // 코루틴을 이용한 크기 애니메이션
    IEnumerator BounceAnimation()
    {
        isAnimating = true;

        // 1. 작아지는 단계 (1.0 -> 0.8)
        Vector3 targetScale = originalScale * minScale;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // 2. 다시 원래대로 커지는 단계 (0.8 -> 1.0)
        t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * animationSpeed;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        // 마지막에 정확히 원래 크기로 고정
        transform.localScale = originalScale;
        isAnimating = false;
    }
}