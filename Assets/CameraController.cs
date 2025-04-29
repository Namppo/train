using UnityEngine;
using UnityEngine.EventSystems;

public class CameraContinuousRotation : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도
    private bool isRotatingLeft = false; // 왼쪽 회전 상태
    private bool isRotatingRight = false; // 오른쪽 회전 상태

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
