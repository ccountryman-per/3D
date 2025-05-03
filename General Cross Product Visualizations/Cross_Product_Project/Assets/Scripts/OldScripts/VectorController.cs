using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorController : MonoBehaviour
{

    public Transform arrowModel;
    public Vector3 direction = new Vector3(1, 0, 0);
    private Vector3 baseScale;


    // Start is called before the first frame update
    void Start()
    {
        baseScale = arrowModel.localScale.normalized;
    }

    public void SetScale(float magnitude)
    {
        arrowModel.localScale = new Vector3(1, 1, magnitude + 48.6f);
        transform.forward = direction.normalized;
        arrowModel.localPosition = new Vector3(0, 0, 0);
    }
}
