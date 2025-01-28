using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerBehavior : MonoBehaviour
{
    private Transform player;
    public float moveSpeed = 3f; // Velocidade de movimento do inimigo
    public float stopDistance = 1f; // Distância mínima para parar de perseguir o jogador

    private Rigidbody2D rb; // Referência ao Rigidbody2D
    private Vector2 movement; // Direção do movimento

    private int vida = 3;

    void Start()
    {

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
           player = playerObject.transform;
        }
        // Obtém o Rigidbody2D do inimigo
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null)
        {
            // Calcula a direção do inimigo em relação ao jogador
            Vector2 direction = (player.position - transform.position).normalized;

            // Calcula a distância entre o inimigo e o jogador
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Atualiza o movimento somente se estiver fora da distância mínima
            if (distanceToPlayer > stopDistance)
            {
                movement = direction;
            }
            else
            {
                movement = Vector2.zero; // Para o movimento quando estiver perto
            }

            // Rotaciona o inimigo para olhar para o jogador
            RotateTowardsPlayer(direction);
        }

        if(vida <= 0)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // Aplica o movimento ao Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }

    void RotateTowardsPlayer(Vector2 direction)
    {
        // Calcula o ângulo de rotação em direção ao jogador
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica a rotação ao Rigidbody2D
        rb.rotation = angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            vida--;
        }
    }
}
