using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHeight : MonoBehaviour
{
    [Range(0.1f, 10f)] public float height = 1f;
     
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(transform.localScale.x, height, transform.localScale.z);
    }
}
