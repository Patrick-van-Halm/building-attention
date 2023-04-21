using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineRenderer : Graphic
{
    [SerializeField] private float thickness = 10f;
    [SerializeField] private List<Vector2> positions = new List<Vector2>();

    ///<summary>Set the positions of all vertices in the line.</summary>
    public void SetPositions(List<Vector2> positions)
    {
        this.positions = positions;
        SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (positions.Count < 2) return;

        float angle = 0;

        for (int i = 0; i < positions.Count - 1; i++)
        {
            Vector2 point = positions[i];
            Vector2 point2 = positions[i + 1];

            if (i < positions.Count - 1)
            {
                angle = GetAngle(positions[i], positions[i + 1]) + 90f;
            }

            DrawVerticesForPosition(point, point2, vh, angle);
        }

        for (int i = 0; i < positions.Count - 1; i++)
        {
            int index = i * 4;
            vh.AddTriangle(index + 0, index + 1, index + 2);
            vh.AddTriangle(index + 1, index + 2, index + 3);
        }
    }

    private float GetAngle(Vector2 me, Vector2 target)
    {
        return (float)(Mathf.Atan2(target.y - me.y, target.x -me.x) * (180 / Mathf.PI));
    }

    private void DrawVerticesForPosition(Vector2 point, Vector2 point2, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(rectTransform.rect.width * point.x, rectTransform.rect.height * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(rectTransform.rect.width * point.x, rectTransform.rect.height * point.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(rectTransform.rect.width * point2.x, rectTransform.rect.height * point2.y);
        vh.AddVert(vertex);

        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(rectTransform.rect.width * point2.x, rectTransform.rect.height * point2.y);
        vh.AddVert(vertex);
    }
}
