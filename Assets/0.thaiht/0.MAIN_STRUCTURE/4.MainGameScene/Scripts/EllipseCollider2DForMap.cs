using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace thaiht20183826
{
    public class EllipseCollider2DForMap : MonoBehaviour
    {
        private PolygonCollider2D _collider2D;

        public float radiusX = 2f;  // Bán kính theo trục x
        public float radiusY = 1f;  // Bán kính theo trục y
        public int numPoints = 30;  // Số điểm xấp xỉ

        private void Awake()
        {
            _collider2D = GetComponent<PolygonCollider2D>();
        }


        public void UpdateEllipseCollider(float radX, float radY, Vector2 offset)
        {
            radiusX = radX;
            radiusY = radY;
            _collider2D.offset = offset;
            Vector2[] points = new Vector2[numPoints];

            float angleIncrement = (2f * Mathf.PI) / numPoints;
            float angle = 0f;

            for (int i = 0; i < numPoints; i++)
            {
                float x = Mathf.Sin(angle) * radiusX;
                float y = Mathf.Cos(angle) * radiusY;

                points[i] = new Vector2(x, y);
                angle += angleIncrement;
            }

            _collider2D.points = points;
        }





    }
}
