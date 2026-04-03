using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CUIMapCamera : CUIComponentBase
    {
        [SerializeField] protected Camera mapCamera;
        [SerializeField] protected float minDragSpeed;
        [SerializeField] protected float maxDragSpeed;
        [SerializeField] protected float minZoomSpeed;
        [SerializeField] protected float maxZoomSpeed;
        [SerializeField] protected float minZoom;
        [SerializeField] protected float maxZoom;
        [SerializeField] protected Button lookAtPlayerButton;

        private Vector3 dragOrigin;
        private Vector3 originPosition;
        private Vector3 difference;

        protected override void Awake()
        {
            base.Awake();
            lookAtPlayerButton.onClick.AddListener(LookAtPlayer);
        }

        private void Update()
        {
            HandleDrag();
            HandleZoom();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            LookAtPlayer();
        }


        private void HandleDrag()
        {
            if (ui.IsMapDragBeginInput())
            {
                dragOrigin = ui.CheckMousePosition();
                originPosition = mapCamera.transform.position;
            }

            if (ui.IsMapDragInput())
            {
                difference = dragOrigin - ui.CheckMousePosition();
                mapCamera.transform.position =
                    originPosition
                    + difference * (minDragSpeed + (maxDragSpeed - minDragSpeed) * (mapCamera.orthographicSize - minZoom) / (maxZoom - minZoom));
            }
        }

        private void HandleZoom()
        {
            float scroll = ui.CheckZoomInput();
            float newSize =
                mapCamera.orthographicSize
                - scroll * (minZoomSpeed + (maxZoomSpeed - minZoomSpeed) * (mapCamera.orthographicSize - minZoom) / (maxZoom - minZoom));
            mapCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }

        public void LookAtPlayer()
        {
            Vector3 playerPosition = ui.CheckPlayerTransform().position;
            playerPosition.z = -1;
            mapCamera.transform.position = playerPosition;
            mapCamera.orthographicSize = minZoom;
        }
    }
}

