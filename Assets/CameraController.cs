using UnityEngine;

public class CameraContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도
    private bool isRotatingLeft = false; // 왼쪽 회전 상태
    private bool isRotatingRight = false; // 오른쪽 회전 상태
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // 카메라의 위아래 각도
    public float maxPitch = 90f; // 위쪽 최대 각도
    public float minPitch = -90f; // 아래쪽 최소 각도

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

        if (isRotatingUp) // 위로 회전
        {
            pitch -= rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // 각도 제한
            transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }

        if (isRotatingDown) // 아래로 회전
        {
            pitch += rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); // 각도 제한
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