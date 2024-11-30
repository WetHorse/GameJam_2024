using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBoost : Boost<PlayerHealthSystem>
{
    [SerializeField, Range(0,100)] private float healAmount = 25f;

    public override void ApplyBoost(PlayerHealthSystem playerHealthSystem)
    {
        playerHealthSystem.Heal(healAmount);
    }

    public override bool CanBePickedUp(PlayerHealthSystem playerHealthSystem)
    {
        return true;
    }
}
