using Lean.Touch;
using System;
using UnityEngine;

    // This script allows you to zoom a camera in and out based on the pinch gesture
    // This supports both perspective and orthographic cameras
public class ZoomPinch : MonoBehaviour
{
    [SerializeField] private float minScale;
    [SerializeField] private float maxScale;
    
        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool ignoreGuiFingers = true;

        [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
        public int requiredFingerCount;

        [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
        [Range(-1.0f, 1.0f)]
        public float wheelSensitivity;

        [Tooltip("The current FOV/Size")]
        public float zoom = 50.0f;

        [Tooltip("Limit the FOV/Size?")]
        public bool zoomClamp;
        
        private float ZoomMin => SizeMin + Offset * minScale;

        private float ZoomMax => Size + Offset * maxScale;

        private static int Offset => (GameMode.CurrentGameMode == GameModeType.LevelEditor ? TileManager.EditorFieldSize : LevelSaveManager.LoadedLevel.FieldSize).x - 7;
        private const float Size = 4.5f;
        private const float SizeMin = 3.5f;

        private void Start()
        {
            if (GameMode.CurrentGameMode == GameModeType.SinglePlayer)
            {
                ResetCameraZoom();
            }
        }

        protected virtual void LateUpdate()
        {
            // Get the fingers we want to use
            var fingers = LeanTouch.GetFingers(ignoreGuiFingers, false, requiredFingerCount);

            // Get the pinch ratio of these fingers
            var pinchRatio = LeanGesture.GetPinchRatio(fingers, wheelSensitivity);

            // Modify the zoom value
            zoom *= pinchRatio;

            if (zoomClamp)
            {
                zoom = Mathf.Clamp(zoom, ZoomMin, ZoomMax);
            }

            // Set the new zoom
            SetZoom(zoom);
        }

        public void ResetCameraZoom()
        {
            zoom = ZoomMax;
        }

        private void SetZoom(float current)
        {
            // Make sure the camera exists
            Camera.main.orthographicSize = current;
        }
    }