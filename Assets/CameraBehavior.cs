using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform player;
    public float velocidade = 0.125f;
    public Vector3 offset;

    void Start()
    {
        Camera camera = Camera.main;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 posicaoFinal = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;

            Vector3 movimento = Vector3.Lerp(transform.position, posicaoFinal, velocidade);

            transform.position = movimento;
        }
    }
}
