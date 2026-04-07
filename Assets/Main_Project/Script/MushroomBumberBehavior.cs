using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MushroomBumber에 공이 닿으면 점수를 올리고 이펙트와 사운드를 재생합니다.
/// </summary>
public class MushroomBumberBehavior : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ValueKeepingBehavior scoreKeep; // 점수 관리 스크립트 참조

    [Header("Effect Settings")]
    [SerializeField] private GameObject hitEffect;          // 충돌 시 발생할 파티클 프리팹
    [SerializeField] private float effectDestroyTime = 2.0f; // 이펙트 자동 삭제 시간

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;        // 사운드 재생기
    [SerializeField] private AudioClip hitSound;            // 충돌 시 재생할 음원

    /// <summary>
    /// 충돌 감지 시 점수 증가, 이펙트 생성, 사운드 재생을 처리합니다.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 물체가 Player 태그를 가졌는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 1. 점수 증가 (기존 로직 유지)
            if (scoreKeep != null)
            {
                scoreKeep.score++;
            }

            // 2. 파티클 이펙트 생성
            if (hitEffect != null)
            {
                // 충돌 지점(contact point)에 이펙트 생성
                ContactPoint contact = collision.contacts[0];
                GameObject effectInstance = Instantiate(hitEffect, contact.point, Quaternion.LookRotation(contact.normal));
                
                // 메모리 관리를 위해 일정 시간 후 삭제
                Destroy(effectInstance, effectDestroyTime);
            }

            // 3. 사운드 재생
            if (audioSource != null && hitSound != null)
            {
                audioSource.PlayOneShot(hitSound);
            }
        }
    }
}