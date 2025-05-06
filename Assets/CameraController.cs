using UnityEngine;
using GameConstants;
using static UnityEngine.XR.Hands.XRHandSubsystemDescriptor;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 50f; // ȸ�� �ӵ�
    private bool isRotatingLeft = false; // ���� ȸ�� ����
    private bool isRotatingRight = false; // ������ ȸ�� ����
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // ī�޶��� ���Ʒ� ����
    public static float maxPitch = 90f; // ���� �ִ� ����
    public static float minPitch = -90f; // �Ʒ��� �ּ� ����

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

        if (isZoomingIn)
        {
            ZoomIn();
        }
        if (isZoomingOut)
        {
            ZoomOut();
        }

        HandleMouseDrag();
        HandleMouseScroll();
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

    public Camera mainCamera; // ī�޶� ����
    public float zoomSpeed = 20f; // Ȯ��/��� �ӵ�

    private bool isZoomingIn = false;
    private bool isZoomingOut = false;

    public void StartZoomIn()
    {
        isZoomingIn = true;
    }

    public void StopZoomIn()
    {
        isZoomingIn = false;
    }

    public void StartZoomOut()
    {
        isZoomingOut = true;
    }

    public void StopZoomOut()
    {
        isZoomingOut = false;
    }

    void ZoomIn()
    {
        mainCamera.fieldOfView -= zoomSpeed * Time.deltaTime;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, CameraConstants.minFOV, CameraConstants.maxFOV);
    }

    void ZoomOut()
    {
        mainCamera.fieldOfView += zoomSpeed * Time.deltaTime;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, CameraConstants.minFOV, CameraConstants.maxFOV);
    }






    public float mouseRotationSpeed = 0.001f;
    public float movementThreshold = 9f; // ���� �� �̻� ������ ���� ����
    public float yawThreshold = 15f; // �¿� �̵��� ũ�� ���Ʒ� ȸ�� ����
    public float pitchReductionFactor = 0.5f; // ���Ʒ� ȸ�� ���� ����
    public float pitchThreshold = 15f; // ���Ʒ� �̵��� Ŭ ��� �¿� ȸ�� ����
    private bool isYawActive = false; // �¿� �̵� Ȱ��ȭ ����
    private bool isPitchActive = false; // ���Ʒ� �̵� Ȱ��ȭ ����
    private float lastPitchChange = 0f; // ���� �������� pitch �� ����

    public float minDragDuration = 0.1f; // �ּ� �巡�� �ð� (��)
    private float dragStartTime = 0f; // ���콺 Ŭ�� ���� �ð�


    private Vector2 lastMousePosition;


    void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // ���콺�� ó�� Ŭ���Ǹ� ��ġ ���� (������ ����)
            dragStartTime = Time.time; // ���콺 Ŭ���� ������ �ð� ����
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (Time.time - dragStartTime < minDragDuration)
                return;

            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = (currentMousePosition - lastMousePosition) * mouseRotationSpeed;
            lastMousePosition = currentMousePosition;

            // �ʹ� ���� �������̸� ����
            if (delta.magnitude < movementThreshold)
                return;

            float yaw = -delta.x * Time.deltaTime * 5f; // �¿� ȸ��
            float pitchChange = delta.y * Time.deltaTime; // ���Ʒ� ȸ��

            if (Mathf.Abs(delta.x) > yawThreshold)
            {
                isYawActive = true;
                isPitchActive = false;
            }
            // ���Ʒ� �̵��� ũ�� �¿츦 ��� ��Ȱ��ȭ
            if (Mathf.Abs(delta.y) > pitchThreshold)
            {
                isPitchActive = true;
                isYawActive = false;
            }

            if (Mathf.Sign(pitchChange) != Mathf.Sign(lastPitchChange)) // ������ ����� ��
            {
                pitchChange *= 0.7f; // ���� ����
            }
            lastPitchChange = pitchChange; // ���� pitch ��ȭ ����

            // ���� Ȱ��ȭ�� ���⸸ ����
            if (isYawActive) pitchChange = 0f;
            if (isPitchActive) yaw = 0f;

            pitch = Mathf.Clamp(pitch + pitchChange, minPitch, maxPitch); // ���� ����

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y + yaw, 0f),
                0.08f // �ε巯�� ȸ�� ����
            );
        }

    }

    public float mouseZoomSpeed = 0.1f;

    void HandleMouseScroll()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            mainCamera.fieldOfView -= scroll * mouseZoomSpeed;
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, CameraConstants.minFOV, CameraConstants.maxFOV);
        }
    }

}