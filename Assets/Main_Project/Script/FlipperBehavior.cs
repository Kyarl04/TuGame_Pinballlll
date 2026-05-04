using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipperBehavior : MonoBehaviour
{
    // 기존 변수명들을 유지하면서 로직만 수정했습니다.
    public float restPosition = 0f;
    public float pressedPosition = 45f;
    public float hitStrength = 10000f;
    public float flipperDamper = 150f;

    public AudioSource audioSource;
    public AudioClip hitSound;
    
    // 이 변수는 인스펙터에서 'Space'라고 입력하거나 
    // 아래 Update문처럼 직접 KeyCode를 써도 됩니다.
    public string inputName; 

    private HingeJoint hinge;

    void Start()
    {
        hinge = GetComponent<HingeJoint>();
        hinge.useSpring = true;
    }

    void Update()
    {
        JointSpring spr = hinge.spring;

        // 1. inputName을 사용하거나, 확실하게 하려면 KeyCode.Space를 사용하세요.
        // Input.GetAxis보다 Input.GetKey가 핀볼 플리퍼 반응속도에 더 적합합니다.
        if (Input.GetKey(KeyCode.Space) || (!string.IsNullOrEmpty(inputName) && Input.GetButton(inputName)))
        {
            spr.targetPosition = pressedPosition;
        }
        else
        {
            spr.targetPosition = restPosition;
        }

        // 설정한 스프링 값을 다시 힌지에 적용
        spr.spring = hitStrength;
        spr.damper = flipperDamper;
        hinge.spring = spr;
    }

    private void OnCollisionEnter(Collision collision) 
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            audioSource.PlayOneShot(hitSound);
        }
    }

}