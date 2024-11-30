using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxTrigger : MonoBehaviour
{
    [SerializeField] private Material skybox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            RenderSettings.skybox = skybox;
        }
    }
}
