using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform arrow;

    [SerializeField] public Vector3 arrowDirection;
    [SerializeField] float speed = 15f;
    public float baseArrowLength = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        arrow.localPosition = Vector3.zero;
    }

    // Update is called once per frame
    public void ArrowRescale(Vector3 vector, float scale)
    {

        float magnitude = vector.magnitude;
        Quaternion targetRotation = Quaternion.LookRotation(vector);
        arrow.rotation = targetRotation;
        arrow.localScale = new Vector3(scale, scale, scale * magnitude / baseArrowLength);
       // arrow.localScale = arrow.localScale * scale;
    }
}
