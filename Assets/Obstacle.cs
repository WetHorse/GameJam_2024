using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private void LateUpdate()
    {
        if (PlayerHealthSystem.Instance == null) return;
        if (PlayerHealthSystem.Instance.transform.position.y > transform.position.y + 10f)
        {
            Destroy(gameObject);
        }
    }

    public void Damage ()
    {
        if (particleSystem != null)
        {
            Instantiate(particleSystem,transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
