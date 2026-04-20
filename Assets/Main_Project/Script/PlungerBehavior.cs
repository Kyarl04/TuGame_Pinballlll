using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlungerBehavior : MonoBehaviour
{
    float force;
    // float minForce = 0f; // 경고 원인: 사용되지 않는 변수 제거
    public float maxForce = 100;
    public Slider slider;
    List<Rigidbody> ally;
    bool ball;

    [Header("Launch Effect Settings")]
    [SerializeField] private GameObject launchEffect;      
    [SerializeField] private Transform launchPoint;        
    [SerializeField] private float effectDestroyTime = 2f; 

    void Start()
    {
        slider.minValue = 0f;
        slider.maxValue = maxForce;
        ally = new List<Rigidbody>();
    }

    void Update()
    {
        // 슬라이더 활성화 로직
        slider.gameObject.SetActive(ball);
        slider.value = force;

        if(ally.Count > 0)
        {
            ball = true;

            // 차징
            if(Input.GetKey(KeyCode.Mouse0))
            {
                if(force <= maxForce)
                {
                    force += 500 * Time.deltaTime;
                }
            }

            // 발사
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (launchEffect != null)
                {
                    Vector3 spawnPos = launchPoint != null ? launchPoint.position : transform.position;
                    GameObject effectInstance = Instantiate(launchEffect, spawnPos, Quaternion.LookRotation(Vector3.forward));
                    Destroy(effectInstance, effectDestroyTime);
                }

                foreach(Rigidbody rigi in ally)
                {
                    rigi.AddForce(force * Vector3.forward);
                }
                
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