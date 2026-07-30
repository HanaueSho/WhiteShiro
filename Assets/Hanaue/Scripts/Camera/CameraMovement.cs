using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [Header("注視点")]
    [SerializeField] private Transform target;

    [Header("回転速度")]
    [SerializeField] private float horizontalSensitivity = 0.2f;

    [SerializeField] private float verticalSensitivity = 0.2f;

    [Header("上下方向の角度制限")]
    [SerializeField] private float minPitch = -80.0f;

    [SerializeField] private float maxPitch = 0.0f;

    private float yaw;
    private float pitch;
    private float distance;

    private void Start()
    {
        if (target == null)
        {
            return;
        }

        // 注視点からカメラまでの相対位置
        Vector3 offset = transform.position - target.position;

        distance = offset.magnitude;

        // 現在のカメラ位置から初期角度を計算
        yaw = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;

        float horizontalDistance =
            new Vector2(offset.x, offset.z).magnitude;

        pitch = -Mathf.Atan2(
            offset.y,
            horizontalDistance
        ) * Mathf.Rad2Deg;

        UpdateCameraTransform();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            return;
        }

        if (mouse.rightButton.isPressed)
        {
            Vector2 mouseDelta = mouse.delta.ReadValue();

            yaw += mouseDelta.x * horizontalSensitivity;

            // 上へドラッグしたときにカメラを上へ動かす場合
            pitch += mouseDelta.y * verticalSensitivity;

            pitch = Mathf.Clamp(
                pitch,
                minPitch,
                maxPitch
            );
        }

        UpdateCameraTransform();
    }

    private void UpdateCameraTransform()
    {
        Quaternion orbitRotation = Quaternion.Euler(
            pitch,
            yaw,
            0f
        );

        Vector3 offset =
            orbitRotation * new Vector3(0f, 0f, distance);

        transform.position = target.position + offset;
        transform.LookAt(target.position, Vector3.up);
    }

}
