using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public Button resetButton;
    public Transform vectorA;
    public Transform vectorB;
    public Transform vectorProd;

    public Slider sliderA;
    public Slider sliderB;

    private Vector3 defaultScale = new Vector3(1, 1, 48.6f);

    // Start is called before the first frame update
    void Start()
    {
        resetButton.enabled = true;
        resetButton.onClick.AddListener(ResetVectors);
    }

    // Update is called once per frame
    void ResetVectors()
    {
        vectorA.forward = Vector3.forward;
        vectorA.localScale = defaultScale;
        
        vectorB.forward = Vector3.right;
        vectorB.localScale = defaultScale;
        
        vectorProd.forward = Vector3.up;
        vectorProd.localScale = defaultScale;
        

        sliderA.value = 1;
        sliderB.value = 1;

    }
}
