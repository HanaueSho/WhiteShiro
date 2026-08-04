/*
    CameraMovement.cs
    2026084  hanaue sho
    カメラの制御
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class CameraMovement : MonoBehaviour
{
    // ==================================================
    // ----- Propaty -----
    // ==================================================
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

    // ==================================================
    // ----- Smartphone Touch -----
    // ==================================================
    // 現在カメラ操作に使用している指
    private Finger _cameraFinger; 
    // 前フレームの指の位置
    private Vector2 _previousTouchPosition; 
    // UI判定時のGC Allocを抑えるため、リストを使い回す
    private readonly List<RaycastResult> _raycastResults = new();


    // ==================================================
    // ----- Unity Events -----
    // ==================================================
    private void OnEnable()
    { 
        // 新Input Systemの高レベルなタッチ取得機能を有効化
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable() 
    { 
        _cameraFinger = null; 
        EnhancedTouchSupport.Disable(); 
    }

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

        // タッチ中はスマホ操作を優先
        if (EnhancedTouch.activeTouches.Count > 0)
        {
            UpdateTouchInput();
        }
        else
        {
            UpdateMouseInput();
        }

        UpdateCameraTransform();
    }

    private void UpdateMouseInput()
    {
        Mouse mouse = Mouse.current;

        if (mouse == null)
        {
            return;
        }

        // PCでは右ドラッグで回転
        if (!mouse.rightButton.isPressed)
        {
            return;
        }

        Vector2 mouseDelta = mouse.delta.ReadValue();

        RotateCamera(mouseDelta);
    }
    private void UpdateTouchInput()
    {
        foreach (EnhancedTouch touch in EnhancedTouch.activeTouches)
        {
            // タッチ開始時にカメラ操作へ使用できるか判定
            if (touch.phase == TouchPhase.Began)
            {
                TryBeginCameraTouch(touch);
            }

            // カメラ操作に採用した指以外は無視
            if (touch.finger != _cameraFinger)
            {
                continue;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 currentPosition = touch.screenPosition;

                Vector2 touchDelta =
                    currentPosition - _previousTouchPosition;

                _previousTouchPosition = currentPosition;

                RotateCamera(touchDelta);
            }

            // 指を離したら操作を終了
            if (touch.phase == TouchPhase.Ended ||
                touch.phase == TouchPhase.Canceled)
            {
                _cameraFinger = null;
            }
        }
    }

    // ==================================================
    // ----- Touch -----
    // ==================================================
    private void TryBeginCameraTouch(EnhancedTouch touch)
    {
        // すでに別の指でカメラを操作している場合
        if (_cameraFinger != null)
        {
            return;
        }

        // UI上から始まったタッチは無視
        if (IsPointerOverUI(touch.screenPosition))
        {
            return;
        }

        _cameraFinger = touch.finger;
        _previousTouchPosition = touch.screenPosition;
    }

    private bool IsPointerOverUI(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            return false;
        }

        PointerEventData pointerEventData =
            new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

        _raycastResults.Clear();

        // 指定座標にあるUIを取得
        EventSystem.current.RaycastAll(
            pointerEventData,
            _raycastResults
        );

        foreach (RaycastResult result in _raycastResults)
        {
            // UIのGraphicRaycasterによるヒットだけを判定
            if (result.module is GraphicRaycaster)
            {
                return true;
            }
        }

        return false;
    }

    private void RotateCamera(Vector2 delta)
    {
        yaw += delta.x * horizontalSensitivity;

        pitch += delta.y * verticalSensitivity;

        pitch = Mathf.Clamp(
            pitch,
            minPitch,
            maxPitch
        );
    }

    // ==================================================
    // ----- UpdateCameraTransform -----
    // ==================================================
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
