using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerHealthSystem))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damage = 35;
    private PlayerHealthSystem playerHealthSystem; 


    private void Awake()
    {
        playerHealthSystem = GetComponent<PlayerHealthSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            Debug.Log("Collided with obstacle!");
            obstacle.Damage();
            playerHealthSystem.Damage(damage);
        }
    }
}
