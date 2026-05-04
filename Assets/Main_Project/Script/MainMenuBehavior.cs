using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Slider를 제어하기 위해 추가

public class MainMenuBehavior : MonoBehaviour
{
    [Header("Restart UI Settings")]
    [SerializeField] private GameObject restartPanel;

    [Header("Option Settings")]
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Slider volumeSlider; // 슬라이더 초기값 설정을 위한 변수
    private bool isPaused = false;

    public AudioSource uiAudioSource;
    public AudioClip clickSound;

    void Start()
    {
        // 시작할 때 슬라이더의 위치를 현재 게임 볼륨에 맞게 동기화
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
        }
    }

    public void PlayClickSound() 
    {
        uiAudioSource.PlayOneShot(clickSound);
    }

    /// <summary>
    /// 슬라이더의 값을 받아 전체 게임 볼륨을 조절합니다.
    /// </summary>
    /// <param name="volume">슬라이더에서 전달되는 0.0 ~ 1.0 사이의 값</param>
    public void SetGlobalVolume(float volume)
    {
        // 1. 전체 오디오 리스너 볼륨 조절
        AudioListener.volume = volume;

        // 2. 만약 SoundManager가 별도로 있다면 해당 소스도 직접 조절 (선택 사항)
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetVolume(volume);
        }
        
        Debug.Log($"현재 설정된 볼륨: {volume}"); // 디버그 로그로 작동 여부 확인
    }

    public void ToggleOptionMenu()
    {
        if (optionPanel == null) return;
        isPaused = !isPaused;
        optionPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void loadScene(int sceneBuildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneBuildIndex);
    }

    public void quitGame()
    {
        Application.Quit();
    }
    
    public void ShowRestartPanel()
    {
        if (restartPanel == null) return;
        restartPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}