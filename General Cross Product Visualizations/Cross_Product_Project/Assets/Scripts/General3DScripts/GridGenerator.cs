using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Axis3D : MonoBehaviour
{
    public Material axisMaterial;  // Material for axis lines
    public float axisLength = 5f;  // Length of each axis line
    public float lineWidth = 0.005f;
    public TMP_FontAsset axisFont;  // Assign in Inspector

    void Start()
    {
        CreateAxis(Vector3.right, Color.red, "X-Axis", "X");
        CreateAxis(Vector3.up, Color.green, "Y-Axis", "Y");
        CreateAxis(Vector3.forward, Color.blue, "Z-Axis", "Z");
    }

    void CreateAxis(Vector3 direction, Color color, string name, string labelText)
    {
        // Create a new GameObject for the axis line
        GameObject axisObj = new GameObject(name);
        axisObj.transform.SetParent(transform);

        // Add LineRenderer
        LineRenderer lineRenderer = axisObj.AddComponent<LineRenderer>();
        lineRenderer.material = axisMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = false;

        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, direction * axisLength);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        // Create label
        GameObject labelObj = new GameObject($"{name}-Label");
        labelObj.transform.SetParent(axisObj.transform);
        labelObj.transform.localPosition = direction * axisLength + direction.normalized * direction.magnitude / 3;

        TextMeshPro text = labelObj.AddComponent<TextMeshPro>();
        text.text = labelText;
        text.fontSize = 10;
        text.color = color;
        text.alignment = TextAlignmentOptions.Center;

        // Optional: Rotate to face camera
        labelObj.transform.LookAt(Camera.main.transform);
        labelObj.transform.Rotate(0, 180f, 0); // Flip to face camera correctly
    }
}
