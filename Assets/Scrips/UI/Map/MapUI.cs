using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private Camera mapCamera;
    private RawImage mapImage;
    public DeliveryPoint targetDeliveryPoint;
    public DeliveryPoint originDeliveryPoint;
    private bool isDelivering;
    public LayerMask whatIsDoor;


    private void Start()
    {
        mapCamera = UI.instance.mapCamera;
        mapImage = GetComponentInChildren<RawImage>();
    }

    public void SetOriginDeliveryPoint(DeliveryPoint _origin)
    {
        if (GameManager.instance.CanPause())
        {
            UI.instance.SwitchWithKeyTo(UI.instance.mapUI);
            originDeliveryPoint = _origin;
            isDelivering = true;
        }
        else
        {
            PlayerManager.instance.player.fx.CreatePopUpText("战斗中无法传送");
        }
    }

    private void Update()
    {
        if (!isDelivering)
        {
            return;
        }
        else
        {
            if (!UI.instance.mapUI.activeSelf)
            {
                isDelivering = false;
                originDeliveryPoint.isHolding = false;
                originDeliveryPoint = null;
                targetDeliveryPoint = null;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 searchPoint = CalculateImagePosition(Input.mousePosition);   
            Collider2D hit = Physics2D.OverlapPoint(searchPoint, whatIsDoor);
            if (hit != null && hit.GetComponent<DeliveryPoint>() != null)
            {
                if (targetDeliveryPoint != null && targetDeliveryPoint != hit.GetComponent<DeliveryPoint>())
                {
                    targetDeliveryPoint.Choose(false);
                }

                targetDeliveryPoint = hit.GetComponent<DeliveryPoint>();
                if (targetDeliveryPoint.isChosen)
                {
                    originDeliveryPoint.Deliver(targetDeliveryPoint.playerTargetTransform);
                    targetDeliveryPoint.Choose(false);
                    targetDeliveryPoint = null;
                    originDeliveryPoint = null;
                    UI.instance.SwitchTo(UI.instance.inGame);
                }
                else
                {
                    targetDeliveryPoint.Choose(true);
                }
            }
        }
    }

    private Vector3 CalculateImagePosition(Vector3 _mousePosition)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mapImage.rectTransform,
            _mousePosition,
            null,
            out localPoint))
        {
            // 将UI局部坐标转换为UV坐标 (0-1)
            Vector2 uv = ConvertToUV(localPoint);

            // 正交相机直接使用ViewportToWorldPoint
            Vector3 worldPoint = mapCamera.ViewportToWorldPoint(new Vector3(uv.x, uv.y, mapCamera.nearClipPlane));

            // 对于2D正交相机，z坐标通常设为0
            worldPoint.z = 0f;

            return worldPoint;
        }
        return Vector3.zero;
    }

    private Vector2 ConvertToUV(Vector2 localPosition)
    {
        Rect rect = mapImage.rectTransform.rect;

        // 将局部坐标转换为标准化UV坐标 (0-1)
        Vector2 uv = new Vector2(
            (localPosition.x - rect.x) / rect.width,
            (localPosition.y - rect.y) / rect.height
        );

        return uv;
    }
}

