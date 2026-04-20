using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehavior : MonoBehaviour
{
    [Header("Restart UI Settings")]
    [SerializeField] private GameObject restartPanel; // 리스타트 패널 연결

    [Header("Option Settings")]
    [SerializeField] private GameObject optionPanel; // 연결할 옵션 패널 오브젝트
    private bool isPaused = false;                 // 현재 일시정지 상태인지 확인

    /// <summary>
    /// 옵션 창을 켜고 끄며 게임을 일시정지/재개합니다.
    /// </summary>
    public void ToggleOptionMenu()
    {
        if (optionPanel == null) return;

        // 상태 반전
        isPaused = !isPaused;

        // 패널 활성화/비활성화
        optionPanel.SetActive(isPaused);

        // 시간 배율 조절
        // Time.timeScale = 0이면 모든 물리와 시간이 멈춥니다. 1이면 정상 작동합니다.
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void loadScene(int sceneBuildIndex)
    {
        // 다른 씬으로 넘어갈 때 시간이 멈춰있으면 안 되므로 1로 초기화
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
        Time.timeScale = 0f; // 게임 일시정지
    }

    // 버튼에 연결할 씬 재시작 함수
    public void RestartGame()
    {
        Time.timeScale = 1f; // 시간 초기화 필수
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 로드
    }
}