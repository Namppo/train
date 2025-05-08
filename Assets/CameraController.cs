using UnityEngine;
using GameConstants;
using static UnityEngine.XR.Hands.XRHandSubsystemDescriptor;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    float rotationSpeed = 50f; // ȸ�� �ӵ�
    private bool isRotatingLeft = false; // ���� ȸ�� ����
    private bool isRotatingRight = false; // ������ ȸ�� ����
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // ī�޶��� ���Ʒ� ����
    static float maxPitch = 90f; // ���� �ִ� ����
    static float minPitch = -90f; // �Ʒ��� �ּ� ����

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
    float zoomSpeed = 20f; // Ȯ��/��� �ӵ�

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




    float movementThreshold = 5f;
    float minDragDuration = 0.1f; // �ּ� �巡�� �ð� (��)
    private Vector2 lastMousePosition;
    private float dragStartTime = 0f; // ���콺 Ŭ�� ���� �ð�
    private bool isDragging = false; // �巡�� ���� Ȯ��


    void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            dragStartTime = Time.time; // ���콺 Ŭ���� ������ �ð� ����
            lastMousePosition = Mouse.current.position.ReadValue();
            isDragging = true;
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            // �巡�� �ð��� �ʹ� ª���� ����
            if (Time.time - dragStartTime < minDragDuration)
                return;

            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = currentMousePosition - lastMousePosition;

            if (delta.magnitude < movementThreshold) return; // �ʹ� ���� ������ ����

            lastMousePosition = currentMousePosition;

            float yaw = -delta.x * Time.deltaTime * 60f;
            float pitchChange = delta.y * Time.deltaTime * 15f;

            if( Mathf.Abs(delta.x) < 1f)
            {
                yaw = 0;
            }

            if (Mathf.Abs(delta.y) < 4f)
            {
                pitchChange = 0;
            }

            pitch = Mathf.Clamp(pitch + pitchChange, minPitch, maxPitch);

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y + yaw, 0f),
                0.08f
            );
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false; // ��ư�� ���� �巡�� ����
        }
    }


    float mouseZoomSpeed = 5.0f;
    void HandleMouseScroll()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            mainCamera.fieldOfView -= scroll * mouseZoomSpeed;
            Debug.Log($"field of view : {mainCamera.fieldOfView}, delta : {scroll * mouseZoomSpeed}");
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView, CameraConstants.minFOV, CameraConstants.maxFOV);
            Debug.Log($"field of view : {mainCamera.fieldOfView}, scroll : {scroll}");
        }
    }

}