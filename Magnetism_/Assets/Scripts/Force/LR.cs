using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LR : MonoBehaviour
{
    [SerializeField] private GameObject charge;       
    [SerializeField] private float trailInterval = 1f;  
    [SerializeField] private int maxLength = 300;          // Maximum segments before older ones are removed
    [SerializeField] private Material lineMaterial;       
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private GameObject CameraSystem;

    private Queue<LineRenderer> lineRenderers;    // Queue to hold our line renderers
    private float timeSinceLastTrail = 0f;
    private Vector3 previousPosition;
    [SerializeField] private Button resetButton;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = charge.transform.position;
        DontDestroyOnLoad(CameraSystem);
    }

    private void Start()
    {
        // Instantiate the queue.
        lineRenderers = new Queue<LineRenderer>();
        // Record the starting position.
        previousPosition = charge.transform.position;

        resetButton.onClick.AddListener(OnResetClicked);
    }

    public void ResetGame()
    {
        
        // Reload the active scene, effectively resetting the game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnResetClicked()
    {
        ResetGame();
    }

    private void Update()
    {
        Vector3 p = charge.transform.position; 
        if (Mathf.Abs(originalPos.x - p.x) >= 80f ||
            Mathf.Abs(originalPos.y - p.y) >= 80f ||
            Mathf.Abs(originalPos.z - p.z) >= 80f)
        {
            ResetGame();
        }

        // Increase timer based on frame time.
        timeSinceLastTrail += Time.deltaTime*5;

        if (timeSinceLastTrail >= trailInterval)
        {
            // Reset the timer.
            timeSinceLastTrail = 0f;

            // Create a new GameObject for the LineRenderer.
            GameObject lrObj = new GameObject("LineRenderer");
            // Optionally set its parent to keep the hierarchy clean.
            lrObj.transform.parent = transform;

            // Add a LineRenderer component.
            LineRenderer lr = lrObj.AddComponent<LineRenderer>();

            // Configure the LineRenderer.
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.SetPosition(0, previousPosition);                // Set the start point.
            lr.SetPosition(1, charge.transform.position);         // Set the end point.

            // Enqueue the new LineRenderer.
            lineRenderers.Enqueue(lr);

            // Update previous position.
            previousPosition = charge.transform.position;

            // If the queue exceeds the maxLength, dequeue the oldest segment.
            if (lineRenderers.Count > maxLength)
            {
                LineRenderer oldLR = lineRenderers.Dequeue();
                Destroy(oldLR.gameObject);
            }
        }
    }
}
