using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidade de movimento do personagem
    public GameObject projectilePrefab; // Prefab do projetil
    public Transform shootPoint; // Ponto de onde o projetil ser� disparado
    public float projectileSpeed = 10f; // Velocidade do projetil
    public float shootCooldown = 0.5f; // Tempo de recarga entre os tiros

    private Rigidbody2D rb; // Refer�ncia ao Rigidbody2D
    private Vector2 movement; // Dire��o do movimento
    private float lastShootTime; // Marca o momento do �ltimo tiro

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obt�m o Rigidbody2D
    }

    void Update()
    {
        // Captura a entrada do jogador (teclado para movimento)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        movement = new Vector2(moveX, moveY).normalized;

        // Rota��o em dire��o ao mouse
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
        // Obt�m a posi��o do mouse na tela e converte para o mundo
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calcula a dire��o do mouse em rela��o ao objeto
        Vector2 direction = (Vector2)(mousePosition - transform.position);
        direction.Normalize();

        // Calcula o �ngulo da rota��o em graus
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica a rota��o ao Rigidbody2D
        rb.rotation = angle;
    }

    void Shoot()
    {
        // Marca o momento do �ltimo disparo
        lastShootTime = Time.time;

        // Instancia o projetil na posi��o do ponto de disparo
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        // Obt�m o Rigidbody2D do projetil e aplica uma for�a na dire��o do disparo
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        if (projectileRb != null)
        {
            projectileRb.velocity = shootPoint.right * projectileSpeed;
        }
    }
}
