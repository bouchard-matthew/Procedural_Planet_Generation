using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    private GravityAttractor attractor;

    private void Start()
    {
        InitializeRigidbody();
        FindAndRegisterWithAttractor();
    }

    private void InitializeRigidbody()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FindAndRegisterWithAttractor()
    {
        attractor = FindObjectOfType<GravityAttractor>();
        attractor?.AddAffectedBody(this);
    }

    private void OnDestroy()
    {
        attractor?.RemoveAffectedBody(this);
    }

    private void OnDrawGizmos()
    {
        if (Rigidbody != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
            Gizmos.DrawRay(transform.position, Rigidbody.velocity.normalized * 2f);
        }
    }
}