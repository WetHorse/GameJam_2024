using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpeedBoost : Boost<PlayerMovement>
{
    [SerializeField, Range(0,100)] private int boostTime = 10;

    public override void ApplyBoost(PlayerMovement playerMovement)
    {
        ///playerMovement.
        if (playerMovement.TryGetComponent(out PlayerHealthSystem playerHealthSystem))
        {
            playerHealthSystem.GodMod = true;
        }
        RollBack(playerMovement,playerHealthSystem);
    }

    public override bool CanBePickedUp(PlayerMovement playerMovementSystem)
    {
        return playerMovementSystem.TryGetComponent(out PlayerHealthSystem playerHealthSystem) && playerHealthSystem.GodMod == false;
    }

    private async void RollBack (PlayerMovement playerMovement, PlayerHealthSystem playerHealthSystem)
    {
        await Task.Delay(boostTime * 1000);
        playerHealthSystem.GodMod = false;
        //playerMovement
    }
}
