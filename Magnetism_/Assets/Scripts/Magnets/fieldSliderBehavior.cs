using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FieldSliderBehavior : MonoBehaviour
{
    [SerializeField] private GameObject g1;
    [SerializeField] private GameObject g2;
    [SerializeField] private GameObject g3;
    [SerializeField] private GameObject g4;

    [SerializeField] private GameObject u1;
    [SerializeField] private GameObject u2;
    [SerializeField] private GameObject u3;
    [SerializeField] private GameObject u4;

    [SerializeField] private Slider fieldStrength;
    [SerializeField] private TextMeshProUGUI sliderValueText;

    [SerializeField] private Material mat;

    // Separate lists for g and u groups
    private List<GameObject> activeGGroups = new List<GameObject>();
    private List<GameObject> inactiveGGroups = new List<GameObject>();

    private List<GameObject> activeUGroups = new List<GameObject>();
    private List<GameObject> inactiveUGroups = new List<GameObject>();

    private int lastSegment = 0;
    private float oldValue; // To detect whether the slider value is increasing or decreasing

    void Start()
    {
        fieldStrength.maxValue = 10f;
        fieldStrength.minValue = 0f;
        fieldStrength.value = 10f;
        oldValue = fieldStrength.value;

        // Initialize G groups
        InitializeGGroup(g1);
        InitializeGGroup(g2);
        InitializeGGroup(g3);
        InitializeGGroup(g4);

        // Initialize U groups
        InitializeUGroup(u1);
        InitializeUGroup(u2);
        InitializeUGroup(u3);
        InitializeUGroup(u4);

        lastSegment = Mathf.FloorToInt(fieldStrength.value / 2.5f);
        fieldStrength.onValueChanged.AddListener(OnSliderValueChanged);
        UpdateSliderText(fieldStrength.value);
    }

    void InitializeGGroup(GameObject group)
    {
        if (group != null)
        {
            activeGGroups.Add(group);
            ApplyMaterialToChildren(group);
        }
    }

    void InitializeUGroup(GameObject group)
    {
        if (group != null)
        {
            activeUGroups.Add(group);
            ApplyMaterialToChildren(group);
        }
    }

    void ApplyMaterialToChildren(GameObject group)
    {
        if (mat == null) return;

        foreach (Transform child in group.transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = mat;
            }
        }
    }

    void OnSliderValueChanged(float newValue)
    {
        bool isIncreasing = newValue > oldValue;
        int currentSegment = Mathf.FloorToInt(newValue / 2.5f);

        if (currentSegment != lastSegment)
        {
            HandleGroupVisibility(isIncreasing);
            lastSegment = currentSegment;
        }

        UpdateSliderText(newValue);
        oldValue = newValue;
    }

    void HandleGroupVisibility(bool isIncreasing)
    {
        // Toggle one G group
        if (isIncreasing && inactiveGGroups.Count > 0)
        {
            GameObject groupToActivate = inactiveGGroups[inactiveGGroups.Count - 1];
            groupToActivate.SetActive(true);
            activeGGroups.Add(groupToActivate);
            inactiveGGroups.RemoveAt(inactiveGGroups.Count - 1);
        }
        else if (!isIncreasing && activeGGroups.Count > 0)
        {
            GameObject groupToDeactivate = activeGGroups[activeGGroups.Count - 1];
            groupToDeactivate.SetActive(false);
            inactiveGGroups.Add(groupToDeactivate);
            activeGGroups.RemoveAt(activeGGroups.Count - 1);
        }

        // Toggle one U group
        if (isIncreasing && inactiveUGroups.Count > 0)
        {
            GameObject groupToActivate = inactiveUGroups[inactiveUGroups.Count - 1];
            groupToActivate.SetActive(true);
            activeUGroups.Add(groupToActivate);
            inactiveUGroups.RemoveAt(inactiveUGroups.Count - 1);
        }
        else if (!isIncreasing && activeUGroups.Count > 0)
        {
            GameObject groupToDeactivate = activeUGroups[activeUGroups.Count - 1];
            groupToDeactivate.SetActive(false);
            inactiveUGroups.Add(groupToDeactivate);
            activeUGroups.RemoveAt(activeUGroups.Count - 1);
        }
    }

    void UpdateSliderText(float value)
    {
        if (sliderValueText != null)
        {
            sliderValueText.text = $"{value:0.00} T";
        }
    }
}
