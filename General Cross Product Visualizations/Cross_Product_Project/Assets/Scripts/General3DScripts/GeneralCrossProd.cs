using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GeneralCrossProd : MonoBehaviour
{
    // Objects
    public Transform resultantModel;
    public Transform modelA;
    public Transform modelB;
    public GameObject conePrefab;
    private GameObject coneA, coneB, coneRes;
    // Visuals
    public LineRenderer lineRenderer;
    public TextMeshProUGUI angleText;
    public TextMeshProUGUI vectorALiveLabel, vectorBLiveLabel, vectorResultLiveLabel;
    public Toggle liveLabelToggle, zToggle;
    // Controls
    [SerializeField] private Slider AXSlider, AYSlider, AZSlider, BXSlider, BYSlider, BZSlider;
    [SerializeField] private Vector3 vectorA = new Vector3(1, 0, 0);
    [SerializeField] private Vector3 vectorB = new Vector3(0, 1, 0);
    private Vector3 vectorResult = new Vector3(0, 0, 1);
    // Floats 
    public float arrowScale = 10.0f;
    [SerializeField] float arcWidth = 0.5f;
    public float coneSize = 1f;
    private float screenOffset = 50f;

    // Booleans
    private bool isLabelLive = false;
    private bool isZToggled = false;

    // Other
    public Material materialA, materialB, materialRes;
    public ArrowController arrowAController;
    public ArrowController arrowBController;


    // Start is called before the first frame update
    void Start()
    {
        coneA = Instantiate(conePrefab, modelA.transform.position, Quaternion.identity);
        coneB = Instantiate(conePrefab, modelB.transform.position, Quaternion.identity);
        coneRes = Instantiate(conePrefab, resultantModel.transform.position, Quaternion.identity);

        coneA.transform.localScale = coneA.transform.localScale * arrowScale;

        AssignColor(coneA, materialA);
        AssignColor(coneB, materialB);
        AssignColor(coneRes, materialRes);

        AXSlider.onValueChanged.AddListener(UpdateVectorA);
        AYSlider.onValueChanged.AddListener(UpdateVectorA);
        AZSlider.onValueChanged.AddListener(UpdateVectorA);

        BXSlider.onValueChanged.AddListener(UpdateVectorB);
        BYSlider.onValueChanged.AddListener(UpdateVectorB);
        BZSlider.onValueChanged.AddListener(UpdateVectorB);

        liveLabelToggle.onValueChanged.AddListener(OnLabelToggleChanged);
        zToggle.onValueChanged.AddListener(OnZChanged);
    }

    void UpdateVectorA(float value)
    {
        float xmag = AXSlider.value;
        float ymag = AYSlider.value;
        float zmag = AZSlider.value;
        vectorA = new Vector3(xmag, ymag, zmag);
    }

    void UpdateVectorB(float value)
    {
        float xmag = BXSlider.value;
        float ymag = BYSlider.value;
        float zmag = BZSlider.value;
        vectorB = new Vector3(xmag, ymag, zmag);
    }


    // Update is called once per frame
    void UpdateVectors()
    {
        coneA.transform.localScale = new Vector3(arrowScale * coneSize, arrowScale * coneSize, arrowScale * coneSize); // Scale down the cone by a factor of 10
        coneB.transform.localScale = new Vector3(arrowScale * coneSize, arrowScale * coneSize, arrowScale * coneSize); // Same for the second cone
        coneRes.transform.localScale = new Vector3(arrowScale * coneSize, arrowScale * coneSize, arrowScale * coneSize); // Same for the resultant cone

        arrowAController.ArrowRescale(vectorA, arrowScale);
        arrowBController.ArrowRescale(vectorB, arrowScale);


        Vector3 cross = Vector3.Cross(vectorA, vectorB);
        vectorResult = cross;
        float magnitude = cross.magnitude;

        resultantModel.rotation = Quaternion.LookRotation(cross);
        resultantModel.localScale = new Vector3(1, 1, -magnitude);
        resultantModel.localScale = resultantModel.localScale * arrowScale;
        float angle = Vector3.Angle(vectorA, vectorB);
        DisplayAngle(angle);
        Debug.Log(angle);

        coneA.transform.position = modelA.transform.position + modelA.transform.forward * modelA.transform.localScale.z * 0.02f; // Set cone at the end of vector A
        coneA.transform.rotation = Quaternion.LookRotation(vectorA); 

        coneB.transform.position = modelB.transform.position + modelB.transform.forward * modelB.transform.localScale.z * 0.02f; // Set cone at the end of vector A
        coneB.transform.rotation = Quaternion.LookRotation(vectorB);

        coneRes.transform.position = resultantModel.transform.position + resultantModel.transform.forward * resultantModel.transform.localScale.z * .02f; // Set cone at the end of vector A
        coneRes.transform.rotation = Quaternion.LookRotation(-cross);
        DrawArc(vectorA, vectorB, 100, lineRenderer);
        LiveLabel();
       
    }

    // main update recognized by unity and runs once a frame
    void Update()
    { 
        ZAdjustments();
        UpdateVectors();
    }

    public void DrawArc(Vector3 A, Vector3 B, int segmentCount, LineRenderer lineRenderer)
    {
        A.Normalize();
        B.Normalize();

        lineRenderer.positionCount = segmentCount;
        lineRenderer.widthMultiplier = arcWidth;
        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (float)(segmentCount - 1);
            Vector3 pointOnArc = Vector3.Slerp(A, B, t);
            lineRenderer.SetPosition(i, pointOnArc);
        }
    }

    public void DisplayAngle(float angleValue)
    {
        angleText.text = $"Angle: {angleValue:F1}°";
    }

    public void LiveLabel()
    {
        if (!isLabelLive)
        {
            vectorALiveLabel.gameObject.SetActive(false);
            vectorBLiveLabel.gameObject.SetActive(false);
            vectorResultLiveLabel.gameObject.SetActive(false);
            return;
        }
        Vector3 screenPosA = Camera.main.WorldToScreenPoint(vectorA);
        Vector3 screenPosB = Camera.main.WorldToScreenPoint(vectorB);
        Vector3 screenPosResult = Camera.main.WorldToScreenPoint(-vectorResult);

        screenPosA.x = Mathf.Clamp(screenPosA.x, 10f, Screen.width * 0.75f);
        screenPosA.y = Mathf.Clamp(screenPosA.y, 10f, Screen.height * 0.75f);

        screenPosB.x = Mathf.Clamp(screenPosB.x, 10f, Screen.width * 0.75f);
        screenPosB.y = Mathf.Clamp(screenPosB.y, 10f, Screen.height * 0.75f);

        screenPosResult.x = Mathf.Clamp(screenPosResult.x, 10f, Screen.width * 0.75f);
        screenPosResult.y = Mathf.Clamp(screenPosResult.y, 10f, Screen.height * 0.75f);

        vectorALiveLabel.gameObject.SetActive(true);
        vectorBLiveLabel.gameObject.SetActive(true);
        vectorResultLiveLabel.gameObject.SetActive(true);

        vectorALiveLabel.transform.position = screenPosA;
        vectorBLiveLabel.transform.position = screenPosB;
        vectorResultLiveLabel.transform.position = screenPosResult;

        vectorALiveLabel.text = $"A: ({vectorA.x:F2}, {vectorA.y:F2}, {vectorA.z:F2})";
        vectorBLiveLabel.text = $"B: ({vectorB.x:F2}, {vectorB.y:F2}, {vectorB.z:F2})";
        vectorResultLiveLabel.text = $"Cross: ({vectorResult.x:F2}, {vectorResult.y:F2}, {vectorResult.z:F2})";
    }

    void OnZChanged(bool value)
    {
        isZToggled = value;
    }
    public void ZAdjustments()
    {
        if (!isZToggled)
        {
            AZSlider.interactable = false;
            BZSlider.interactable = false;
            vectorA = new Vector3(vectorA.x, vectorA.y, 0);
            vectorB = new Vector3(vectorB.x, vectorB.y, 0);
        }
        else
        {
            AZSlider.interactable = true;
            BZSlider.interactable = true;
        }
    }

    void OnLabelToggleChanged(bool value)
    {
        isLabelLive = value; 
    }

    void AssignColor(GameObject cone, Material material)
    {
        Renderer rend = cone.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = material;
        }
    }
}
