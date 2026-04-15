using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueKeepingBehavior : MonoBehaviour
{
    public float score = 0;
    public float lives = 3;

    [Header("UI References")]
    public Text scoreValue;
    // 기존 livesValue 텍스트는 필요 없다면 주석 처리하거나 제거해도 됩니다.
    
    [Header("Heart UI Settings")]
    [Tooltip("하트 이미지 게임 오브젝트 3개를 순서대로 넣어주세요.")]
    public GameObject[] heartImages; 

    void Update()
    {
        // 점수 텍스트 업데이트
        scoreValue.text = "" + score.ToString();

        // 목숨 수에 따라 하트 이미지 활성화/비활성화 제어
        UpdateHeartUI();
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            // i(0, 1, 2)가 현재 남은 lives보다 작으면 활성화, 크거나 같으면 비활성화
            if (i < lives)
            {
                heartImages[i].SetActive(true);
            }
            else
            {
                heartImages[i].SetActive(false);
            }
        }
    }
}