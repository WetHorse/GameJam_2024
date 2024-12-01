using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    void Update()
    {
        var playerHP = PlayerHealthSystem.Instance.gameObject;
        if (playerHP == null) return;
        transform.position = new Vector3(0, playerHP.transform.position.y, 0);
    }
}
