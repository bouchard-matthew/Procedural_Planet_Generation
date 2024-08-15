using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    private Rigidbody rb;
    private GravityAttractor attractor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        attractor = FindObjectOfType<GravityAttractor>();
        if (attractor != null)
        {
            attractor.AddAffectedBody(this);
        }
        else
        {
            Debug.LogWarning("No GravityAttractor found in the scene!");
        }
    }

    void OnDestroy()
    {
        if (attractor != null)
        {
            attractor.RemoveAffectedBody(this);
        }
    }

    void OnDrawGizmos()
    {
        if (rb != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawRay(transform.position, rb.velocity.normalized * 2f);
        }
    }
}
