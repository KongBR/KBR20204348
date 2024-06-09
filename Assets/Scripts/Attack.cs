using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float duration = 1f;
    private Transform pivot; // �θ� ������Ʈ

    private Quaternion originalRotation;
    private Vector3 initialLocalPosition; // �ʱ� ��ġ
    private bool isRotating = false; // ȸ�� ������ ���θ� �����ϴ� �÷���

    void Start()
    {
        // �θ� ������Ʈ ����
        pivot = new GameObject("pivot").transform;

        // �θ� ������Ʈ�� ��ġ�� ���� ������Ʈ�� ������ ����
        pivot.position = transform.position + new Vector3(transform.localScale.x / 2, 0, 0);

        // �θ� ������Ʈ�� ���� ������Ʈ�� �θ�� ����
        pivot.SetParent(transform.parent);

        // ���� ������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
        transform.SetParent(pivot);

        // ���� ȸ�� ���� ����
        originalRotation = pivot.rotation;
        initialLocalPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating) // ���� Ű �Է�
        {
            StartCoroutine(RotateAndRevert()); // �ð� ����� ���� ����
        }
    }

    IEnumerator RotateAndRevert()
    {
        isRotating = true; // ȸ�� ����

        // ȸ��
        float elapsedTime = 0f; // ����ð�
        Quaternion startRotation = pivot.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, rotationAngle, 0);

        while (elapsedTime < duration)
        {
            pivot.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            //���� �����ӿ� ���� �簳
            yield return null;
        }

        // ȸ�� ���� �ð� ���� ���
        yield return new WaitForSeconds(duration);

        // ���� ȸ�� ���·� ���ƿ�
        startRotation = pivot.rotation;
        elapsedTime = 0f;

        pivot.rotation = originalRotation;
        isRotating = false; // ȸ�� �Ϸ�

    }

}
