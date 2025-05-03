using UnityEngine;

public class ChargeCollisionDetector : MonoBehaviour
{
    public Cyclotron cyclotron;  // Assign this in the Inspector or dynamically

    private void OnTriggerEnter(Collider other)
    {
        cyclotron?.HandleChargeTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        cyclotron?.HandleChargeTriggerExit(other);
    }

    private void OnCollisionStay(Collision collision)
    {
        cyclotron?.HandleChargeCollisionStay(collision);
    }
}
