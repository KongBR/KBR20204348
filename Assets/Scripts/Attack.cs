using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float duration = 1f;
    private Transform pivot; // 부모 오브젝트

    private Quaternion originalRotation;
    private Vector3 initialLocalPosition; // 초기 위치
    private bool isRotating = false; // 회전 중인지 여부를 추적하는 플래그

    void Start()
    {
        // 부모 오브젝트 생성
        pivot = new GameObject("pivot").transform;

        // 부모 오브젝트의 위치를 현재 오브젝트의 끝으로 설정
        pivot.position = transform.position + new Vector3(transform.localScale.x / 2, 0, 0);

        // 부모 오브젝트를 현재 오브젝트의 부모로 설정
        pivot.SetParent(transform.parent);

        // 현재 오브젝트를 부모 오브젝트의 자식으로 설정
        transform.SetParent(pivot);

        // 원래 회전 상태 저장
        originalRotation = pivot.rotation;
        initialLocalPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating) // 공격 키 입력
        {
            StartCoroutine(RotateAndRevert()); // 시간 경과에 따라 수행
        }
    }

    IEnumerator RotateAndRevert()
    {
        isRotating = true; // 회전 시작

        // 회전
        float elapsedTime = 0f; // 경과시간
        Quaternion startRotation = pivot.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0);

        while (elapsedTime < duration)
        {
            pivot.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            //다음 프레임에 실행 재개
            yield return null;
        }

        // 회전 유지 시간 동안 대기
        yield return new WaitForSeconds(duration);

        // 원래 회전 상태로 돌아옴
        startRotation = pivot.rotation;
        elapsedTime = 0f;

        pivot.rotation = originalRotation;
        isRotating = false; // 회전 완료

    }

}
