using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    public enum MagneticPole
    {
        North,
        South
    }

    [SerializeField]
    private MagneticPole pole = MagneticPole.North;
    
    [SerializeField]
    private float fieldStrength = 1f;
    
    [SerializeField]
    private float fieldRadius = 5f;
    
    [SerializeField]
    private bool showFieldLines = true;
    
    private Color northColor = Color.red;
    private Color southColor = Color.blue;
    private Color currentColor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateFieldColor();
    }

    private void UpdateFieldColor()
    {
        currentColor = (pole == MagneticPole.North) ? northColor : southColor;
    }

    private void OnValidate()
    {
        UpdateFieldColor();
    }

    private void OnDrawGizmos()
    {
        if (!showFieldLines) return;

        Gizmos.color = currentColor;

        // Draw main field radius
        Gizmos.DrawWireSphere(transform.position, fieldRadius);

        // Draw field lines
        int lineCount = 8;
        float angleStep = 360f / lineCount;
        
        for (int i = 0; i < lineCount; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            
            // Draw field line
            Vector3 lineStart = transform.position;
            Vector3 lineEnd = transform.position + (direction * fieldRadius);
            
            Gizmos.DrawLine(lineStart, lineEnd);
            
            // Draw arrow at the end
            float arrowSize = fieldRadius * 0.1f;
            Vector3 right = Quaternion.Euler(0, 30, 0) * -direction;
            Vector3 left = Quaternion.Euler(0, -30, 0) * -direction;
            
            if (pole == MagneticPole.North)
            {
                // Arrows pointing outward for North pole
                Gizmos.DrawLine(lineEnd, lineEnd + (right * arrowSize));
                Gizmos.DrawLine(lineEnd, lineEnd + (left * arrowSize));
            }
            else
            {
                // Arrows pointing inward for South pole
                Gizmos.DrawLine(lineStart, lineStart + (right * arrowSize));
                Gizmos.DrawLine(lineStart, lineStart + (left * arrowSize));
            }
        }
    }

    public Vector3 GetMagneticForce(Vector3 position, float magneticCharge)
    {
        Vector3 direction = (position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, position);
        
        if (distance > fieldRadius) return Vector3.zero;
        
        float forceMagnitude = (fieldStrength * magneticCharge) / (distance * distance);
        Vector3 force = direction * forceMagnitude;
        
        // Invert force for South pole
        if (pole == MagneticPole.South)
            force *= -1;
            
        return force;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
