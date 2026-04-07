using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RolloverBehavior : MonoBehaviour
{
    [Header("Score Settings")]
    [SerializeField] private ValueKeepingBehavior scoreKeep; // 점수 관리 스크립트 참조
    [SerializeField] private float bonusScore = 10f;        // 통과 시 줄 점수

    [Header("Effect Settings")]
    [SerializeField] private GameObject passEffect;         // 재생할 파티클 프리팹
    [SerializeField] private Transform effectPosition;      // 이펙트가 나타날 위치 (비워두면 본인 위치)

    [Header("Sound Settings")]
    [SerializeField] private AudioSource audioSource;       // 사운드 재생기
    [SerializeField] private AudioClip passSound;           // 통과 시 재생할 음원

    private void OnTriggerEnter(Collider other)
    {
        // 공(Player 태그)이 들어왔을 때만 실행
        if (other.CompareTag("Player"))
        {
            // 1. 점수 추가
            if (scoreKeep != null)
            {
                scoreKeep.score += bonusScore;
            }

            // 2. 이펙트 생성
            if (passEffect != null)
            {
                Vector3 spawnPos = effectPosition != null ? effectPosition.position : transform.position;
                GameObject effectInstance = Instantiate(passEffect, spawnPos, Quaternion.identity);
                
                // 2초 뒤 이펙트 자동 삭제 (메모리 관리)
                Destroy(effectInstance, 2.0f);
            }

            // 3. 사운드 재생
            if (audioSource != null && passSound != null)
            {
                audioSource.PlayOneShot(passSound);
            }
        }
    }
}