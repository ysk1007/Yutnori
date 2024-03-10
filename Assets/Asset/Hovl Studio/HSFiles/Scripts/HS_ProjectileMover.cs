using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
    // 발사체가 일정 시간 후에 비활성화되는 시간
    public float dissableAfterTime = 5f;
    // 발사체의 속도
    public float speed = 15f;
    private float oroginalSpeed; // 초기 속도를 저장하는 변수
    // 충돌 시 발사체와 충돌 지점 간의 거리
    public float hitOffset = 0f;
    // 발사체의 회전을 발사 지점의 회전으로 사용할지 여부
    public bool UseFirePointRotation;
    // 발사체 회전의 오프셋
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    // 충돌 시 재생될 효과 오브젝트
    public GameObject hit;
    // 발사 시 재생될 플래시 효과 오브젝트
    public GameObject flash;
    public bool useFlash = false;
    public Rigidbody rb; // 발사체의 Rigidbody
    public ParticleSystem ps; // 발사체의 파티클 시스템
    public Collider sc; // 발사체의 충돌체
    public Light li; // 발사체의 라이트
    public GameObject[] Detached; // 분리된 오브젝트 배열
    private RigidbodyConstraints originalConstraints; // 초기 Rigidbody 제약 조건
    public Transform parentObject; // 부모 오브젝트

/*    void Awake()
    {
        // 초기 설정
        if (li != null)
            li.enabled = false;
        sc.enabled = false;
        ps.Stop();
        originalConstraints = rb.constraints;
        oroginalSpeed = speed;
        speed = 0;
        parentObject = transform.parent;
    }*/

    void Start()
    {
    }

    /*    // 부모 오브젝트 변경 시 호출되는 함수
        void OnTransformParentChanged()
        {
            if (parentObject != transform.parent)
            {
                // 발사체가 부모 오브젝트를 잃으면 호출되는 로직
                if (li != null)
                    li.enabled = true;
                sc.enabled = true;
                rb.constraints = originalConstraints;
                speed = oroginalSpeed;
                ps.Play();
                if (flash != null && useFlash)
                {
                    // 플래시 효과 재생
                    var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
                    flashInstance.transform.forward = gameObject.transform.forward;
                    var flashPs = flashInstance.GetComponent<ParticleSystem>();
                    if (flashPs != null)
                    {
                        Destroy(flashInstance, flashPs.main.duration);
                    }
                    else
                    {
                        var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(flashInstance, flashPsParts.main.duration);
                    }
                }

                *//*StartCoroutine(nameof(LateCall));*//*
            }
        }*/

    void FixedUpdate()
    {
        if (speed != 0)
        {
            rb.velocity = transform.forward * speed;
        }
    }

    // 충돌 시 호출되는 함수
    void OnCollisionEnter(Collision collision)
    {
        /*// 발사체의 동작을 멈추고 초기화
        rb.constraints = RigidbodyConstraints.FreezeAll;
        transform.parent = parentObject;
        speed = 0;
        sc.enabled = false;
        if (li != null)
            li.enabled = false;
        StopCoroutine(nameof(LateCall));
        ps.Stop();
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                StartCoroutine(TouchCall(detachedPrefab));
            }
        }
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);*/

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point + contact.normal * hitOffset;

        // 충돌 시 효과 재생
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(contact.point + contact.normal); }

            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
    }

    /*    // 일정 시간 후에 호출되는 함수
        private IEnumerator LateCall()
        {
            yield return new WaitForSeconds(dissableAfterTime);
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            sc.enabled = false;
            if (li != null)
                li.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            transform.parent = parentObject;
            speed = 0;
            yield break;
        }

        private IEnumerator TouchCall(GameObject detachedPrefab)
        {
            yield return new WaitForSeconds(1);
            detachedPrefab.transform.SetParent(gameObject.transform);
            detachedPrefab.transform.position = gameObject.transform.position;
            detachedPrefab.transform.rotation = gameObject.transform.rotation;
            yield break;
        }*/
}
