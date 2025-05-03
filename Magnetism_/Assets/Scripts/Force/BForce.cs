using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BForce : MonoBehaviour
{
    [SerializeField] public GameObject charge;
    [SerializeField] public Vector3 B = new Vector3(0, 1, 0); // Default B along Z
    [SerializeField] public Vector3 velocity = new Vector3(1, 0, 0); // Default velocity direction
    [SerializeField] public float q = 1f;
    [SerializeField] public float mass = 1f;

    [Header("UI Controls")]
    [SerializeField] public Slider BFieldSlider;
    [SerializeField] public Slider QSlider;
    [SerializeField] public Slider VelocitySlider;

    [SerializeField] public TextMeshProUGUI BFieldValue;
    [SerializeField] public TextMeshProUGUI QValue;
    [SerializeField] public TextMeshProUGUI VelcityValue;

    private float angularFrequency;
    private Vector3 initialDirection;

    void Start()
    {
        // Set slider max values
        BFieldSlider.maxValue = 10f;
        BFieldSlider.minValue = 0f;

        QSlider.maxValue = 5f;
        QSlider.minValue = 1f;

        VelocitySlider.maxValue = 10f;
        VelocitySlider.minValue = -10f;


        // Set initial reasonable values
        BFieldSlider.value = 1f;
        QSlider.value = 1f;
        VelocitySlider.value = 1f;

        // Trigger initial updates
        UpdateBField(BFieldSlider.value);
        UpdateCharge(QSlider.value);
        UpdateVelocity(VelocitySlider.value);

        // Attach listeners
        BFieldSlider.onValueChanged.AddListener(UpdateBField);
        QSlider.onValueChanged.AddListener(UpdateCharge);
        VelocitySlider.onValueChanged.AddListener(UpdateVelocity);

        initialDirection = velocity.normalized;

    }

    void Update()
    {
        angularFrequency = (q * B.magnitude) / mass;
        float angle = angularFrequency * Time.deltaTime;
        Quaternion rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, B.normalized);
        velocity = rotation * velocity;

        charge.transform.position += velocity * Time.deltaTime;


    }

    void UpdateBField(float newValue)
    {
        B = new Vector3(0, 0, newValue);
        BFieldValue.text = $"B: {newValue:F2}";
    }

    void UpdateCharge(float newValue)
    {
        q = newValue;
        QValue.text = $"q: {newValue:F2}";
    }

    void UpdateVelocity(float newValue)
    {
        velocity = velocity.normalized * newValue;
        VelcityValue.text = $"v: {newValue:F2}";
    }
}
