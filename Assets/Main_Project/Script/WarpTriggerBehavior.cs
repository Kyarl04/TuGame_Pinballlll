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

    public AudioSource audioSource;
    public AudioClip deathSound;

    void Start()
    {
        // 인스펙터에서 연결이 누락되었을 경우를 대비해 직접 찾습니다.
        if (liveValue == null)
        {
            liveValue = FindObjectOfType<ValueKeepingBehavior>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (deathEffect != null)
            {
                Vector3 spawnPos = deathEffectTransform != null ? deathEffectTransform.position : transform.position;
                Quaternion spawnRot = deathEffectTransform != null ? deathEffectTransform.rotation : Quaternion.identity;
                GameObject effectInstance = Instantiate(deathEffect, spawnPos, spawnRot);
                Destroy(effectInstance, effectDestroyTime);
            }

            Rigidbody playerRigi = other.GetComponent<Rigidbody>();
            if (playerRigi != null)
            {
                // 2021 버전은 linearVelocity 대신 velocity를 사용합니다.
                playerRigi.velocity = Vector3.zero; 
                playerRigi.angularVelocity = Vector3.zero; 
                playerRigi.position = warpPos; 
            }

            audioSource.PlayOneShot(deathSound);
            liveValue.lives--; 
            
            if (liveValue.lives <= 0)
            {
                // 2021 버전에서는 FindFirstObjectByType 대신 FindObjectOfType을 사용합니다.
                MainMenuBehavior mainMenu = FindObjectOfType<MainMenuBehavior>();
                if (mainMenu != null)
                {
                    mainMenu.ShowRestartPanel();
                }
                else
                {
                    SceneManager.LoadScene(3);
                }
            }
        }
    }
}