using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlungerBehavior : MonoBehaviour
{
    float force;
    float minForce = 0f;
    public float maxForce = 100;
    public Slider slider;
    List<Rigidbody> ally;
    bool ball;

    [Header("Launch Effect Settings")]
    [SerializeField] private GameObject launchEffect;      // 발사 시 터질 이펙트 프리팹
    [SerializeField] private Transform launchPoint;        // 이펙트가 생성될 위치
    [SerializeField] private float effectDestroyTime = 2f; // 이펙트 자동 삭제 시간

    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = maxForce;
        ally = new List<Rigidbody>();
    }

    void Update()
    {
        if(ball) slider.gameObject.SetActive(true);
        else slider.gameObject.SetActive(false);

        slider.value = force;

        if(ally.Count > 0)
        {
            ball = true;

            // 차징 중 (마우스 누르고 있을 때)
            if(Input.GetKey(KeyCode.Mouse0))
            {
                if(force <= maxForce)
                {
                    force += 500 * Time.deltaTime;
                }
            }

            // 발사 (마우스 버튼을 뗄 때)
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                // 1. 발사 이펙트 생성 (추가된 로직)
                if (launchEffect != null)
                {
                    Vector3 spawnPos = launchPoint != null ? launchPoint.position : transform.position;
                    // 발사대 방향(Vector3.forward)으로 이펙트 생성
                    GameObject effectInstance = Instantiate(launchEffect, spawnPos, Quaternion.LookRotation(Vector3.forward));
                    Destroy(effectInstance, effectDestroyTime);
                }

                // 2. 물리적인 힘 가하기
                foreach(Rigidbody rigi in ally)
                {
                    rigi.AddForce(force * Vector3.forward);
                }
                
                // 발사 후 힘 초기화
                force = 0f;
            }
        }
        else
        {
            ball = false;
            force = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ally.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ally.Remove(other.GetComponent<Rigidbody>());
            force = 0f;
        }
    }
}