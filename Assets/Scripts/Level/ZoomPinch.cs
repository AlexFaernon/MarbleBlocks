using Lean.Touch;
using System;
using UnityEngine;

    // This script allows you to zoom a camera in and out based on the pinch gesture
    // This supports both perspective and orthographic cameras
    [ExecuteInEditMode]
    public class ZoomPinch : MonoBehaviour
    {
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

        [Tooltip("The minimum FOV/Size we want to zoom to")]
        public float zoomMin = 10.0f;

        [Tooltip("The maximum FOV/Size we want to zoom to")]
        public float zoomMax = 60.0f;

        private void Awake()
        {
            var size = 4.5f;
            var sizeMin = 3.5f;
            var offset = (LevelSaveManager.LoadedLevel.FieldSize.x - 7) / 2;
            zoom = size + offset;
            zoomMin = sizeMin + offset;
            zoomMax = zoom;
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
                zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
            }

            // Set the new zoom
            SetZoom(zoom);
        }

        private void SetZoom(float current)
        {
            // Make sure the camera exists
            Camera.main.orthographicSize = current;
        }
    }