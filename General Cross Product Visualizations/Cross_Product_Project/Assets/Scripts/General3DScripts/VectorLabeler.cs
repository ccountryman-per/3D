using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VectorLabeler : MonoBehaviour
{
    public Transform arrowModel;
    public TextMeshProUGUI uiText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(arrowModel.position + arrowModel.transform.localScale);
        uiText.transform.position = screenPos;
        uiText.text = $"({arrowModel.forward.x:F1}, {arrowModel.forward.y:F1}, {arrowModel.forward.z:F1})";
    }
}
