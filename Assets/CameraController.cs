using UnityEngine;
using GameConstants;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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

        if (EventSystem.current.IsPointerOverGameObject())
            return;

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

            RotateCamera(delta);

            lastMousePosition = currentMousePosition;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false; // ��ư�� ���� �巡�� ����
        }
    }

    const float MOUSE_DELTA_THRESHOLD = 6f;
    void RotateCamera(Vector2 mouseDelta)
    {
        // �ʹ� ���� ������ ����
        if (mouseDelta.magnitude < MOUSE_DELTA_THRESHOLD)
        {
            return;
        }

        float yawDiff = -mouseDelta.x * Time.deltaTime * 160f;
        float pitchDiff = mouseDelta.y * Time.deltaTime * 15f;

        float abs_delta_x = Mathf.Abs(mouseDelta.x);
        float abs_delta_y = Mathf.Abs(mouseDelta.y);

        if (abs_delta_x < 1f)
        {
            yawDiff = 0;
        }

        if (abs_delta_y < 4f)
        {
            pitchDiff = 0;
        }

        pitch = Mathf.Clamp(pitch + pitchDiff, minPitch, maxPitch);

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y + yawDiff, 0f),
            0.08f
        );
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