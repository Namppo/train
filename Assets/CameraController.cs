using UnityEngine;
using GameConstants;
using static UnityEngine.XR.Hands.XRHandSubsystemDescriptor;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    float rotationSpeed = 50f; // 회전 속도
    private bool isRotatingLeft = false; // 왼쪽 회전 상태
    private bool isRotatingRight = false; // 오른쪽 회전 상태
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // 카메라의 위아래 각도
    static float maxPitch = 90f; // 위쪽 최대 각도
    static float minPitch = -90f; // 아래쪽 최소 각도

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

    public Camera mainCamera; // 카메라 참조
    float zoomSpeed = 20f; // 확대/축소 속도

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
    float minDragDuration = 0.1f; // 최소 드래그 시간 (초)
    private Vector2 lastMousePosition;
    private float dragStartTime = 0f; // 마우스 클릭 시작 시간
    private bool isDragging = false; // 드래그 상태 확인


    void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            dragStartTime = Time.time; // 마우스 클릭한 순간의 시간 저장
            lastMousePosition = Mouse.current.position.ReadValue();
            isDragging = true;
        }

        if (Mouse.current.leftButton.isPressed && isDragging)
        {
            // 드래그 시간이 너무 짧으면 무시
            if (Time.time - dragStartTime < minDragDuration)
                return;

            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = currentMousePosition - lastMousePosition;

            if (delta.magnitude < movementThreshold) return; // 너무 작은 움직임 무시

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
            isDragging = false; // 버튼을 떼면 드래그 종료
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