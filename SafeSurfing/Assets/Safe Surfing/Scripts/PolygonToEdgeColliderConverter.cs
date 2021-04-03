using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PolygonToEdgeColliderConverter : MonoBehaviour
{
    private void Awake()
    {
        var polygonCollider = GetComponent<PolygonCollider2D>();

        if (polygonCollider == null)
            polygonCollider = gameObject.AddComponent<PolygonCollider2D>();

        var edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        var points = polygonCollider.points.ToList();
        points.Add(points[0]); //Add first point for final edge

        edgeCollider.points = points.ToArray();
        Destroy(polygonCollider);
    }
}
