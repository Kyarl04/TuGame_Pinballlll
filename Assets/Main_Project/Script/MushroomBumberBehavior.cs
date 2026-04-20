using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 정해진 구역을 왕복하며 공과 충돌 시 튕김 효과와 애니메이션을 발생시킵니다.
/// </summary>
public class MovingMushroomBumber : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 moveOffset;      // 시작 위치로부터의 이동 범위 (예: X로 5만큼)
    [SerializeField] private float moveSpeed = 2f;    // 이동 속도
    
    private Vector3 startPos;
    private Vector3 targetPos;

    [Header("Bouncing & Score (기존 로직)")]
    [SerializeField] private ValueKeepingBehavior scoreKeep;
    [SerializeField] private float scoreAmount = 10f;
    [SerializeField] private float bounceForce = 500f;

    [Header("Animation Settings")]
    [SerializeField] private float minScale = 0.8f; 
    [SerializeField] private float animSpeed = 10f;
    private Vector3 originalScale;
    private bool isAnimating = false;

    [Header("Effects")]
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hitSound;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + moveOffset;
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 1. 정해진 구역 왕복 이동 (PingPong 함수 활용)
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPos, targetPos, t);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 점수 추가 및 반발력 적용
            if (scoreKeep != null) scoreKeep.score += scoreAmount;
            
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 bounceDirection = collision.contacts[0].normal;
                rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);
            }

            // 튕김 애니메이션 실행
            if (!isAnimating) StartCoroutine(BounceAnimation());

            // 이펙트 및 사운드
            if (hitEffect != null)
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
            }
            if (audioSource != null && hitSound != null) audioSource.PlayOneShot(hitSound);
        }
    }

    IEnumerator BounceAnimation()
    {
        isAnimating = true;
        Vector3 targetScale = originalScale * minScale;
        
        // 작아지기
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime * animSpeed;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }
        // 커지기
        t = 0;
        while (t < 1) {
            t += Time.deltaTime * animSpeed;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }
        transform.localScale = originalScale;
        isAnimating = false;
    }
}