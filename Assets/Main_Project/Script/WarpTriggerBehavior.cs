using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarpTriggerBehavior : MonoBehaviour
{
    [SerializeField] private Vector3 warpPos;
    [SerializeField] private ValueKeepingBehavior liveValue;

    [Header("Death Effect Settings")]
    [SerializeField] private GameObject deathEffect;       
    [SerializeField] private Transform deathEffectTransform; 
    [SerializeField] private float effectDestroyTime = 2.0f; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 1. 이펙트 생성
            if (deathEffect != null)
            {
                Vector3 spawnPos = deathEffectTransform != null ? deathEffectTransform.position : transform.position;
                Quaternion spawnRot = deathEffectTransform != null ? deathEffectTransform.rotation : Quaternion.identity;
                GameObject effectInstance = Instantiate(deathEffect, spawnPos, spawnRot);
                Destroy(effectInstance, effectDestroyTime);
            }

            // 2. 물리 리셋 및 워프
            Rigidbody playerRigi = other.GetComponent<Rigidbody>();
            if (playerRigi != null)
            {
                playerRigi.linearVelocity = Vector3.zero; 
                playerRigi.angularVelocity = Vector3.zero; 
                playerRigi.position = warpPos; 
            }

            // 3. 라이프 감소 및 체크
            liveValue.lives--; 
            
            if (liveValue.lives <= 0)
            {
                // 유니티 6 권장 API: FindFirstObjectByType 사용
                MainMenuBehavior mainMenu = Object.FindFirstObjectByType<MainMenuBehavior>();
                if (mainMenu != null)
                {
                    mainMenu.ShowRestartPanel();
                }
                else
                {
                    // 패널을 찾을 수 없는 경우 안전장치로 씬 전환
                    SceneManager.LoadScene(3);
                }
            }
        }
    }
}