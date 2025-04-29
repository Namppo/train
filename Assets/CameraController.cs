using UnityEngine;

public class CameraContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    private bool isRotatingLeft = false; // ���� ȸ�� ����
    private bool isRotatingRight = false; // ������ ȸ�� ����
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // ī�޶��� ���Ʒ� ����
    public float maxPitch = 90f; // ���� �ִ� ����
    public float minPitch = -90f; // �Ʒ��� �ּ� ����

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

        if (isRotatingUp) // ���� ȸ��
        {
            pitch -= rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // ���� ����
            transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }

        if (isRotatingDown) // �Ʒ��� ȸ��
        {
            pitch += rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // ���� ����
            transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
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

    public void StartRotateUp()
    {
        isRotatingUp = true;
    }

    public void StopRotateUp()
    {
        isRotatingUp = false;
    }

    public void StartRotateDown()
    {
        isRotatingDown = true;
    }

    public void StopRotateDown()
    {
        isRotatingDown = false;
    }
}