using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MushroomBumber에 공이 닿으면 설정된 점수만큼 올리고, 물리적인 반발력을 주며 이펙트와 사운드를 재생합니다.
/// </summary>
public class MushroomBumberBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ValueKeepingBehavior scoreKeep; // 점수 관리 스크립트 참조

    [Header("Score Settings")]
    [Tooltip("공이 닿았을 때 증가시킬 점수량입니다.")]
    [SerializeField] private float scoreAmount = 10f;        // 증가할 점수 변수 추가

    [Header("Bouncing Settings")]
    [Tooltip("공을 튕겨내는 힘의 세기입니다.")]
    [SerializeField] private float bounceForce = 500f;       // 반발력 변수

    [Header("Effect Settings")]
    [SerializeField] private GameObject hitEffect;          // 충돌 시 발생할 파티클 프리팹
    [SerializeField] private float effectDestroyTime = 2.0f; // 이펙트 자동 삭제 시간

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;        // 사운드 재생기
    [SerializeField] private AudioClip hitSound;            // 충돌 시 재생할 음원

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체가 Player 태그를 가졌는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 점수 증가 (고정값이 아닌 변수 scoreAmount 사용)
            if (scoreKeep != null)
            {
                scoreKeep.score += scoreAmount; // 기존 score++ 에서 변경
            }

            // 2. 물리적 반발력 가하기
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 bounceDirection = collision.contacts[0].normal;
                rb.AddForce(-bounceDirection * bounceForce, ForceMode.Impulse);
            }

            // 3. 파티클 이펙트 생성
            if (hitEffect != null)
            {
                ContactPoint contact = collision.contacts[0];
                GameObject effectInstance = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
                Destroy(effectInstance, effectDestroyTime);
            }

            // 4. 사운드 재생
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
    }
}