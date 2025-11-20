using UnityEngine;

public class MapController : MonoBehaviour
{
    public Camera mainCamera;

    public float minDragSpeed;
    public float maxDragSpeed;
    public float minZoomSpeed;
    public float maxZoomSpeed;
    public float minZoom;
    public float maxZoom;
    public float branchYOffset;

    private Camera mapCamera;
    private Vector3 dragOrigin;
    private Vector3 originPosition;
    private Vector3 difference;

    private void Awake()
    {
        mapCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampCameraPosition();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            originPosition = mapCamera.transform.position;
        }

        if (Input.GetMouseButton(0))
        {
            difference = dragOrigin - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mapCamera.transform.position = 
                originPosition 
                + difference * (minDragSpeed + (maxDragSpeed - minDragSpeed) * (mapCamera.orthographicSize - minZoom) / (maxZoom - minZoom));
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        float newSize = 
            mapCamera.orthographicSize 
            - scroll * (minZoomSpeed + (maxZoomSpeed - minZoomSpeed) * (mapCamera.orthographicSize - minZoom) / (maxZoom - minZoom));
        mapCamera.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    private void ClampCameraPosition()
    {
        Vector3 pos = mapCamera.transform.position;
        float yOffset = branchYOffset / mapCamera.orthographicSize;
        float yMin = PlayerManager.instance.player.transform.position.y - branchYOffset;
        float yMax = PlayerManager.instance.player.transform.position.y + branchYOffset;
        pos.y = Mathf.Clamp(pos.y, yMin, yMax);

        mapCamera.transform.position = pos;
    }

    public void LookAtPlayer()
    {
        transform.position = PlayerManager.instance.player.transform.position;
        mapCamera.orthographicSize = minZoom;
    }
}