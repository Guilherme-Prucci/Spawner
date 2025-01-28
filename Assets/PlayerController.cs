using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do personagem
    public GameObject projectilePrefab; // Prefab do projetil
    public Transform shootPoint; // Ponto de onde o projetil será disparado
    public float projectileSpeed = 10f; // Velocidade do projetil
    public float shootCooldown = 0.5f; // Tempo de recarga entre os tiros

    private Rigidbody2D rb; // Referência ao Rigidbody2D
    private Vector2 movement; // Direção do movimento
    private float lastShootTime; // Marca o momento do último tiro

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D
    }

    void Update()
    {
        // Captura a entrada do jogador (teclado para movimento)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        // Rotação em direção ao mouse
        RotateTowardsMouse();

        // Verifica entrada de disparo
        if (Input.GetButtonDown("Fire1") && Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        // Aplica movimento ao Rigidbody
        rb.velocity = movement * moveSpeed;
    }

    void RotateTowardsMouse()
    {
        // Obtém a posição do mouse na tela e converte para o mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcula a direção do mouse em relação ao objeto
        Vector2 direction = (Vector2)(mousePosition - transform.position);
        direction.Normalize();

        // Calcula o ângulo da rotação em graus
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica a rotação ao Rigidbody2D
        rb.rotation = angle;
    }

    void Shoot()
    {
        // Marca o momento do último disparo
        lastShootTime = Time.time;

        // Instancia o projetil na posição do ponto de disparo
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Obtém o Rigidbody2D do projetil e aplica uma força na direção do disparo
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.velocity = shootPoint.right * projectileSpeed;
        }
    }
}
