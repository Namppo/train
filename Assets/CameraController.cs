using UnityEngine;
using GameConstants;
using static UnityEngine.XR.Hands.XRHandSubsystemDescriptor;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 50f; // 회전 속도
    private bool isRotatingLeft = false; // 왼쪽 회전 상태
    private bool isRotatingRight = false; // 오른쪽 회전 상태
    private bool isRotatingUp = false;
    private bool isRotatingDown = false;

    private float pitch = 0f; // 카메라의 위아래 각도
    public static float maxPitch = 90f; // 위쪽 최대 각도
    public static float minPitch = -90f; // 아래쪽 최소 각도

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
    public float zoomSpeed = 20f; // 확대/축소 속도

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
    public float movementThreshold = 9f; // 일정 값 이상 움직일 때만 반응
    public float yawThreshold = 15f; // 좌우 이동이 크면 위아래 회전 제한
    public float pitchReductionFactor = 0.5f; // 위아래 회전 감도 감소
    public float pitchThreshold = 15f; // 위아래 이동이 클 경우 좌우 회전 제한
    private bool isYawActive = false; // 좌우 이동 활성화 상태
    private bool isPitchActive = false; // 위아래 이동 활성화 상태
    private float lastPitchChange = 0f; // 이전 프레임의 pitch 값 저장

    public float minDragDuration = 0.1f; // 최소 드래그 시간 (초)
    private float dragStartTime = 0f; // 마우스 클릭 시작 시간


    private Vector2 lastMousePosition;


    void HandleMouseDrag()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 마우스가 처음 클릭되면 위치 저장 (움직임 없이)
            dragStartTime = Time.time; // 마우스 클릭한 순간의 시간 저장
            lastMousePosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            if (Time.time - dragStartTime < minDragDuration)
                return;

            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 delta = (currentMousePosition - lastMousePosition) * mouseRotationSpeed;
            lastMousePosition = currentMousePosition;

            // 너무 작은 움직임이면 무시
            if (delta.magnitude < movementThreshold)
                return;

            float yaw = -delta.x * Time.deltaTime * 5f; // 좌우 회전
            float pitchChange = delta.y * Time.deltaTime; // 위아래 회전

            if (Mathf.Abs(delta.x) > yawThreshold)
            {
                isYawActive = true;
                isPitchActive = false;
            }
            // 위아래 이동이 크면 좌우를 잠시 비활성화
            if (Mathf.Abs(delta.y) > pitchThreshold)
            {
                isPitchActive = true;
                isYawActive = false;
            }

            if (Mathf.Sign(pitchChange) != Mathf.Sign(lastPitchChange)) // 방향이 변경될 때
            {
                pitchChange *= 0.7f; // 감속 적용
            }
            lastPitchChange = pitchChange; // 현재 pitch 변화 저장

            // 현재 활성화된 방향만 적용
            if (isYawActive) pitchChange = 0f;
            if (isPitchActive) yaw = 0f;

            pitch = Mathf.Clamp(pitch + pitchChange, minPitch, maxPitch); // 각도 제한

            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y + yaw, 0f),
                0.08f // 부드러운 회전 적용
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