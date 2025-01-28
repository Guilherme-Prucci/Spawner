using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class BoucerBehavior : MonoBehaviour
{
    public float moveSpeed = 15f; // Velocidade do inimigo
    private Vector2 movementDirection; // Direção de movimento
    private Rigidbody2D rb; // Referência ao Rigidbody2D

    public int vida = 3;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D

        // Gera uma direção inicial aleatória
        GenerateRandomDirection();
    }

    void Update()
    {
        // Se o inimigo estiver se movendo, aplica a velocidade na direção
        
            rb.velocity = movementDirection * moveSpeed;
        if(vida <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Bullet") && !collision.gameObject.CompareTag("EvilBullet"))
        {
            ReflectMovement(collision);
        }
        else
        {
            if (collision.gameObject.CompareTag("Bullet"))
            {
                vida--;
            }
        }
        
    }

    void GenerateRandomDirection()
    {
        // Gera uma direção aleatória em 2D
        float randomAngle = Random.Range(0f, 360f); // Ângulo aleatório entre 0 e 360 graus
        movementDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;
    }

    void ReflectMovement(Collision2D collision)
    {
        // Obtém a normal da superfície de colisão (direção da parede)
        Vector2 collisionNormal = collision.contacts[0].normal;

        // Reflete a direção do movimento com base na normal da colisão (simulando o quique)
        movementDirection = Vector2.Reflect(movementDirection, collisionNormal);

    }

    
}
