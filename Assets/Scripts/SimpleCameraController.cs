using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SimpleCameraController : MonoBehaviour
{
    protected class CameraState
    {
        public float yaw;
        public float pitch;
        public float roll;
        public float x;
        public float y;
        public float z;

        public void SetFromTransform(Transform t)
        {
            pitch = t.eulerAngles.x;
            yaw = t.eulerAngles.y;
            roll = t.eulerAngles.z;
            x = t.position.x;
            y = t.position.y;
            z = t.position.z;
        }

        public void Translate(Vector3 translation)
        {
            Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

            x += rotatedTranslation.x;
            y += rotatedTranslation.y;
            z += rotatedTranslation.z;
        }

        public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
        {
            yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
            pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
            roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);

            x = Mathf.Lerp(x, target.x, positionLerpPct);
            y = Mathf.Lerp(y, target.y, positionLerpPct);
            z = Mathf.Lerp(z, target.z, positionLerpPct);
        }
    }
    
    CameraState m_TargetCameraState = new CameraState();
    CameraState m_InterpolatingCameraState = new CameraState();

    Transform m_targetTransform;

    bool isKeyboard = true;

    [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
    public float positionLerpTime = 0.2f;

    [Range(1, 10f)]
    public float rotationMultiplier = 1f;

    [Header("Rotation Settings")]
    [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
    public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

    [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
    public float rotationLerpTime = 0.01f;

    [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
    public bool invertY = false;
    public int screenshotSize = 1; //Multiples of 1920x720

    public bool canQuitWithEsc = true;
    public bool isLocked = false;
    void OnEnable()
    {
        if(m_targetTransform != null)
        {
            transform.position = m_targetTransform.position;
            transform.forward = m_targetTransform.forward;
        }

        m_TargetCameraState.SetFromTransform(transform);
        m_InterpolatingCameraState.SetFromTransform(transform);
        isLocked = false;
    }

    Vector3 GetInputTranslationDirection()
    {
        Vector3 direction = new Vector3();
        
        if ( Keyboard.current.wKey.isPressed )
        {
            direction += Vector3.forward;
        }
        if ( Keyboard.current.sKey.isPressed )
        {
            direction += Vector3.back;
        }
        if ( Keyboard.current.aKey.isPressed )
        {
            direction += Vector3.left;
        }
        if ( Keyboard.current.dKey.isPressed )
        {
            direction += Vector3.right;
        }
        if ( Keyboard.current.qKey.isPressed )
        {
            direction += Vector3.down;
        }
        if ( Keyboard.current.eKey.isPressed )
        {
            direction += Vector3.up;
        }
        return direction.normalized;
    }
    private void Awake()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        Vector3 translation = Vector3.zero;

        // Exit Sample  
        if ( Keyboard.current.escapeKey.wasPressedThisFrame && canQuitWithEsc)
        {
            Application.Quit();
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; 
			#endif
        }
        // Hide and lock cursor when right mouse button pressed

        // Rotation
        
        Vector2 delta = Mouse.current.delta.value;
        var mouseMovement = new Vector2(delta.x, delta.y * (invertY ? 1 : -1));

        if ( Keyboard.current.lKey.wasPressedThisFrame )
        {
            isLocked = !isLocked;
        }

        if ( Keyboard.current.kKey.wasPressedThisFrame )
        {
            string path = Application.dataPath + "/Screenshots/";
            Directory.CreateDirectory(path);

            path += DateTime.Now.ToString("yyyy-dd-M-HH-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(path, screenshotSize );
            Debug.Log($"Created Screenshot at: {path}");
        }

        // Translation
        var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude * 0.005f) * rotationMultiplier;
        translation = GetInputTranslationDirection();

        // Speed up movement when shift key held
        if ( Keyboard.current.leftShiftKey.wasPressedThisFrame )
        {
            translation *= 10.0f;
        }
        if ( Keyboard.current.leftCtrlKey.wasPressedThisFrame )
        {
            translation *= 5.0f;
        }
        
        if(!isLocked)
        {
            m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
            m_TargetCameraState.pitch = Mathf.Clamp(m_TargetCameraState.pitch + mouseMovement.y * mouseSensitivityFactor, -89.9f, 89.9f);
            m_TargetCameraState.Translate(translation * Time.unscaledDeltaTime);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.unscaledDeltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.unscaledDeltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            UpdateTransform(m_InterpolatingCameraState);
        }
    }

    public void SetTarget(Transform transform)
    {
        m_targetTransform = transform;
    }

    private void UpdateTransform(CameraState state)
    {
            
        transform.eulerAngles = new Vector3(state.pitch, state.yaw, state.roll);
        transform.position = new Vector3(state.x, state.y, state.z);
    }
}