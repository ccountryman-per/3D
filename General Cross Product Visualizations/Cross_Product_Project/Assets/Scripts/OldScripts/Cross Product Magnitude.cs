using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossProductMagnitude : MonoBehaviour {
    public GameObject Arrow1capsule;
    public GameObject Arrow2capsule;
    public GameObject ProductArrow;


    [Range(0.1f, 10f)] public float lengthA = 1f;
    [Range(0.1f, 10f)] public float lengthB = 1f;

    public float vectorProduct;
    // Update is called once per frame
    void Update()
    {
        vectorProduct = lengthA * lengthB;
        transform.localScale = new Vector3(transform.localScale.x, vectorProduct, transform.localScale.z);
    }
}
