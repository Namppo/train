using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    private bool isRotatingLeft = false; // ���� ȸ�� ����
    private bool isRotatingRight = false; // ������ ȸ�� ����

    void Update()
    {
        if (isRotatingLeft)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        if (isRotatingRight)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    public void StartRotateLeft()
    {
        isRotatingLeft = true;
    }

    public void StopRotateLeft()
    {
        isRotatingLeft = false;
    }

    public void StartRotateRight()
    {
        isRotatingRight = true;
    }

    public void StopRotateRight()
    {
        isRotatingRight = false;
    }
}
