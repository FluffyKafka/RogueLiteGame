using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ObjectController
{
    internal class APopUpText : AObjectController
    {
        [SerializeField] protected TextMeshPro textMesh;

        [Header("PopUp Info")]
        [SerializeField] private float popInSpeed;
        [SerializeField] private float popOutSpeed;
        [SerializeField] private float popOutBeginAlpha;
        [SerializeField] private float colorFadeSpeed;
        [SerializeField] private float lifeDuration;

        private float textTimer;

        public void Setup(FCPopUpTextFactory _factory, string _text)
        {
            factory = _factory;

            textMesh.text = _text;
            textTimer = lifeDuration;
        }

        private void Update()
        {
            textTimer -= Time.deltaTime;
            if (textTimer < 0)
            {
                float alpha = textMesh.color.a - colorFadeSpeed * Time.deltaTime;
                textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);

                if (alpha < popOutBeginAlpha)
                {
                    transform.position =
                        Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), popOutSpeed * Time.deltaTime);
                }
                if (alpha <= 0)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                transform.position =
                        Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), popInSpeed * Time.deltaTime);
            }
        }
    }
}

