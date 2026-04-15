using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 공이 바닥에 닿았을 때 워프시키고 목숨을 깎으며, 지정된 위치에서 이펙트를 발생시킵니다.
/// </summary>
public class WarpTriggerBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 warpPos;
    [SerializeField] private ValueKeepingBehavior liveValue;

    [Header("Death Effect Settings")]
    [SerializeField] private GameObject deathEffect;       // 공이 죽을 때 터질 이펙트 프리팹
    [SerializeField] private Transform deathEffectTransform; // 이펙트가 발생할 특정 위치 (추가됨)
    [SerializeField] private float effectDestroyTime = 2.0f; // 이펙트 삭제 시간

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) //
        {
            // 1. 지정된 위치에서 죽음 이펙트 생성 (수정된 로직)
            if (deathEffect != null)
            {
                // deathEffectTransform이 지정되어 있으면 그 위치에, 없으면 이 트리거의 위치에 생성합니다.
                Vector3 spawnPos = deathEffectTransform != null ? deathEffectTransform.position : transform.position;
                Quaternion spawnRot = deathEffectTransform != null ? deathEffectTransform.rotation : Quaternion.identity;

                GameObject effectInstance = Instantiate(deathEffect, spawnPos, spawnRot);
                Destroy(effectInstance, effectDestroyTime);
            }

            // 2. 플레이어 물리 상태 리셋 및 워프
            Rigidbody playerRigi = other.GetComponent<Rigidbody>();
            if (playerRigi != null)
            {
                playerRigi.linearVelocity = Vector3.zero; //
                playerRigi.angularVelocity = Vector3.zero; //
                playerRigi.position = warpPos; //
            }

            // 3. 라이프 감소
            liveValue.lives--; //
        }

        // 4. 게임 오버 체크
        if (liveValue.lives <= 0) //
        {
            SceneManager.LoadScene(3); //
        }
    }
}