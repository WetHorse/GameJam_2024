using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class Boost<T> : MonoBehaviour
{
    [SerializeField] protected AnimationCurve acceleration;
    [SerializeField, Range(0, 100)] protected float snappingSpeed;
    [SerializeField] protected ParticleSystem particleSystem;
    protected SphereCollider sphereCollider;

    public virtual void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out T playerHealthSystem))
        {
            if (CanBePickedUp(playerHealthSystem)) transform.position = Vector3.Lerp(transform.position,other.transform.position,snappingSpeed * Time.deltaTime * acceleration.Evaluate(Vector3.Distance(transform.position, other.transform.position) / sphereCollider.radius));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out T playerHealthSystem))
        {
            if (!CanBePickedUp(playerHealthSystem)) return;
            ApplyBoost(playerHealthSystem);
            if (particleSystem != null)
            {
                Instantiate(particleSystem, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    public abstract bool CanBePickedUp(T playerHealthSystem);

    public abstract void ApplyBoost(T playerHealthSystem);
}
