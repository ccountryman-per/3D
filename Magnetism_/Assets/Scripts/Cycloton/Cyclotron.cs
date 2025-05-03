using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cyclotron : MonoBehaviour
{
    [Header("Physics Objects")]
    [SerializeField] private GameObject charge;
    [SerializeField] private float mass = 1f;
    [SerializeField] private float q = 1f;

    [SerializeField] private Vector3 totalVelocity;     // Net velocity of the charge
    [SerializeField] private Vector3 B;                 // Magnetic field vector
    private Vector3 effectiveE = Vector3.zero;          // Active electric field vector

    [Header("Electric Field")]
    [SerializeField] private Collider electricFieldTriggerZone;
    [SerializeField] private Vector3 electricFieldStrength;
    private bool isInElectricField = true;
    private int eFieldEntryCount = 0;

    [Header("UI Controls")]
    [SerializeField] private Slider BFieldSlider;
    //[SerializeField] private Slider VelocitySlider;
    [SerializeField] private Slider EFieldSlider;

    [SerializeField] private TextMeshProUGUI BFieldValue;
    [SerializeField] private TextMeshProUGUI QValue;
    [SerializeField] private TextMeshProUGUI VelocityValue;
    [SerializeField] private TextMeshProUGUI EFieldValue;

    [SerializeField] public List<GameObject> EFieldArrow;

    [Header("Velocity")]
    [SerializeField] public float VelocityMagnitude;

    public bool isInside = true;

    void Start()
    {
        // Set slider ranges
        BFieldSlider.minValue = 0f;
        BFieldSlider.maxValue = 10f;
        EFieldSlider.minValue = 0f;
        EFieldSlider.maxValue = 10f;
        //VelocitySlider.minValue = -5f;
        //VelocitySlider.maxValue = 5f;

        // Initial values
        totalVelocity = new Vector3(10,0,0); 
        VelocityValue.text = $"v: {totalVelocity.magnitude:F2}";
        B = new Vector3(0, 2, 0);
        effectiveE = new Vector3(0, 0, 3);

        BFieldSlider.value = B.y;
        BFieldSlider.interactable = false;
        EFieldSlider.value = effectiveE.z;
        EFieldSlider.interactable = false;
        //VelocitySlider.value = totalVelocity.magnitude;

        // Initialize field values
        UpdateBField(BFieldSlider.value);
        UpdateEField(EFieldSlider.value);
        //UpdateVelocity(VelocitySlider.value);

        // Add slider listeners
        BFieldSlider.onValueChanged.AddListener(UpdateBField);
        EFieldSlider.onValueChanged.AddListener(UpdateEField);
        //VelocitySlider.onValueChanged.AddListener(UpdateVelocity);
    }

    void FixedUpdate()
    {
        

        ApplyForces();
        charge.transform.position += totalVelocity * Time.deltaTime;
        VelocityMagnitude = totalVelocity.magnitude;



        //Debug.Log($"Velocity: {totalVelocity.magnitude}, Direction: {totalVelocity.normalized}");
        VelocityValue.text = $"v: {totalVelocity.magnitude:F2}";

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == electricFieldTriggerZone)
        {
            isInElectricField = true;
            eFieldEntryCount++;

            electricFieldStrength = -electricFieldStrength;
            effectiveE = electricFieldStrength;

            int idx = (eFieldEntryCount - 1) % EFieldArrow.Count;
            for (int i = 0; i < EFieldArrow.Count; i++)
            {
                bool show = (i == idx);
                EFieldArrow[i].SetActive(show);
                if (show)
                    EFieldArrow[i].transform.forward = effectiveE.normalized;
            }
        }

        if (other == electricFieldTriggerZone || other.CompareTag("Dees"))
        {
            B = new Vector3(0, BFieldSlider.value, 0);
        }
        else
        {
            B = new Vector3(0, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == electricFieldTriggerZone)
        {
            isInElectricField = false;
            effectiveE = Vector3.zero;
            B = new Vector3(0, 0, 0);

            foreach (var arrow in EFieldArrow)
                arrow.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other == electricFieldTriggerZone)
        {
            isInElectricField = true;
        }

        if (other == electricFieldTriggerZone || other.CompareTag("Dees"))
        {
            B = new Vector3(0, BFieldSlider.value, 0);
        }
        else
        {
            B = new Vector3(0, 0, 0);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isInside = true;

    }

    void ApplyForces()
    {
        // Store initial speed - magnetic fields shouldn't change speed
        float initialSpeed = totalVelocity.magnitude;

        // Lorentz force due to magnetic field: F = q(v × B)
        Vector3 magneticForce = q * Vector3.Cross(totalVelocity, B);
        Vector3 magneticAcceleration = magneticForce / mass;
        Vector3 magneticVelocityDelta = magneticAcceleration * Time.deltaTime;

        // Apply magnetic force (changes direction only)
        totalVelocity += magneticVelocityDelta;

        // Restore original speed (magnetic field should only change direction, not speed)
        totalVelocity = totalVelocity.normalized * initialSpeed;

        // Electric field force: F = qE (only if inside field)
        if (isInElectricField && effectiveE.magnitude > 0.001f)
        {
            Vector3 electricForce = q * effectiveE;
            Vector3 electricAcceleration = electricForce / mass;
            Vector3 electricVelocityDelta = electricAcceleration * Time.deltaTime;

            totalVelocity += electricVelocityDelta;
        }

        Debug.Log($"Speed: {totalVelocity.magnitude}, B: {B.magnitude}, E: {effectiveE.magnitude}");
    }


    void UpdateBField(float newValue)
    {
        B = new Vector3(0, newValue, 0);
        if (BFieldValue != null)
            BFieldValue.text = $"B: {newValue:F2}";
    }

    void UpdateEField(float newValue)
    {
        electricFieldStrength = new Vector3(0, 0, -newValue);
        if (EFieldValue != null)
            EFieldValue.text = $"E: {newValue:F2}";
    }

    void UpdateCharge(float newValue)
    {
        q = newValue;
        if (QValue != null)
            QValue.text = $"q: {newValue:F2}";
    }

    void UpdateVelocity(float newValue)
    {
        totalVelocity = totalVelocity.normalized * newValue;
        if (VelocityValue != null)
            VelocityValue.text = $"v: {newValue:F2}";
    }

    // Optional external calls (not used in current logic)
    public void HandleChargeTriggerEnter(Collider other)
    {
        if (other == electricFieldTriggerZone)
        {
            isInElectricField = true;
            eFieldEntryCount++;
            electricFieldStrength = -electricFieldStrength;
            effectiveE = electricFieldStrength;

            foreach (var arrow in EFieldArrow)
            {
                arrow.transform.forward = electricFieldStrength.normalized;
                arrow.SetActive(true);
            }
        }
    }

    public void HandleChargeTriggerExit(Collider other)
    {
        if (other == electricFieldTriggerZone)
        {
            isInElectricField = false;
            effectiveE = Vector3.zero;

            foreach (var arrow in EFieldArrow)
                arrow.SetActive(false);
        }
    }

    public void HandleChargeCollisionStay(Collision collision)
    {
        isInside = true;
    }
}
