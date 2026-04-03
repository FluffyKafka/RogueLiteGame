using ObjectController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    internal class CMap : CUIComponentBase
    {
        [SerializeField] protected LayerMask whatIsDoor;
        [SerializeField] protected Camera mapCamera;
        [SerializeField] protected RawImage mapImage;

        protected IMapUIDeliverPoint targetDeliveryPoint;

        private void Update()
        {
            if (ui.IsMapDragBeginInput())
            {
                Vector3 searchPoint = CalculateImagePosition(ui.CheckMousePosition(true));
                Collider2D hit = Physics2D.OverlapPoint(searchPoint, whatIsDoor);
                if (hit != null && hit.GetComponent<IMapUIDeliverPoint>() != null)
                {
                    IMapUIDeliverPoint targetDeliveryPointTemp = hit.GetComponent<IMapUIDeliverPoint>();
                    if (targetDeliveryPointTemp.IsChosen())
                    {
                        targetDeliveryPoint = targetDeliveryPointTemp;
                        targetDeliveryPoint.Choose(false);
                        targetDeliveryPoint.Deliver();
                        targetDeliveryPoint = null;
                        ui.ChangePageTo(UIData.EUIPageType.InGame);
                    }
                    else
                    {
                        if (targetDeliveryPoint != null)
                        {
                            targetDeliveryPoint.Choose(false);
                        }
                        targetDeliveryPoint = targetDeliveryPointTemp;
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
}

