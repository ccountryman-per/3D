using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossProductCalculator : MonoBehaviour
{
    //public Transform vectorA;
    //public Transform vectorB;

    public Slider sliderA;
    public Slider sliderB;

    public Transform vectorC; // Represents cross product visualization

    public Vector3 vectorADir = new Vector3(0, 0, 1);
    public Vector3 vectorBDir = new Vector3(1, 0, 0);
    public Vector3 vectorProduct;

    public VectorController vectorAController;
    public VectorController vectorBController;

    void Start()
    {
        sliderA.onValueChanged.AddListener(UpdateVectors);
        sliderB.onValueChanged.AddListener(UpdateVectors);
    }

    void UpdateVectors(float value)
    {
        float AMag = sliderA.value; // Get magnitude from sliders
        float BMag = sliderB.value;

        vectorAController.SetScale(AMag);
        vectorBController.SetScale(BMag);

        Debug.Log(AMag);
        Debug.Log(BMag);

        Vector3 scaledA = AMag * vectorADir;
        Vector3 scaledB = BMag * vectorBDir;
        vectorProduct = Vector3.Cross(scaledA, scaledB);

        vectorProduct = vectorProduct.normalized * vectorProduct.magnitude;
        vectorC.localScale = new Vector3(1, 1, vectorProduct.z + 48.6f);
    }
}
