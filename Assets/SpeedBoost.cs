using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SpeedBoost : Boost<PlayerMovement>
{
    [SerializeField, Range(0,100)] private int boostTime = 10;
    [SerializeField, Range(0, 100)] private int boostMultiplier = 2;
    [SerializeField, Range(0, 100)] private float accelerationTime = 2f;

    public async override void ApplyBoost(PlayerMovement playerMovement)
    {
        if (playerMovement.TryGetComponent(out PlayerHealthSystem playerHealthSystem))
        {
            playerHealthSystem.GodMod = true;
        }
        float defaultSpeed = playerMovement.GetSpeed();
        float targetSpeed = defaultSpeed*boostMultiplier;
        while (playerMovement.GetSpeed() < targetSpeed)
        {
            playerMovement.SetSpeed(Mathf.Lerp(defaultSpeed, targetSpeed, accelerationTime));
            await Task.Yield();
        }
        playerMovement.SetSpeed(targetSpeed);
        Debug.Log("Speed setted!");
        RollBack(playerMovement,playerHealthSystem);
    }

    public override bool CanBePickedUp(PlayerMovement playerMovementSystem)
    {
        return playerMovementSystem.TryGetComponent(out PlayerHealthSystem playerHealthSystem) && playerHealthSystem.GodMod == false;
    }

    private async void RollBack (PlayerMovement playerMovement, PlayerHealthSystem playerHealthSystem)
    {
        await Task.Delay(boostTime * 1000);
        float defaultSpeed = playerMovement.GetSpeed() / boostMultiplier;
        float targetSpeed = defaultSpeed * boostMultiplier;
        while (playerMovement.GetSpeed() < targetSpeed)
        {
            playerMovement.SetSpeed(Mathf.Lerp(targetSpeed, defaultSpeed, accelerationTime));
            await Task.Yield();
        }
        playerHealthSystem.GodMod = false;
        Debug.Log("Speed setted!");
        playerMovement.SetSpeed(defaultSpeed);
    }
}
