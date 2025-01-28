using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//estado do Spawn
public enum SpawnState { SPAWNING, WAITING, COUNTING}

[System.Serializable]
public class Wave
{
    public Transform enemy1, enemy2;
    public int count;
    public float rate;
}

public class Spawner : MonoBehaviour
{
    // para saber se o Player esta dentro da area de trigger do spawner
    public bool playerIn = false;

    //waves e numero da wave
    public Wave[] waves;
    private int nextWave = 0;

    //tempo entre waves e contador
    public float timeBetweenWaves = 5f;
    public float waveCountdown = 0;

    public Transform pointSpawn;


    public SpawnState state = SpawnState.COUNTING;

    //contador de verificador de inimigos
    private float searchCountdown = 1f;

    private void Start()
    {
        //contador acionado
        waveCountdown = timeBetweenWaves;
    }

    private void Update()
    {
        //se o player estiver na area
        if (playerIn)
        {
            //se o Spawner ta esperando o player destruir os inimigos
            if (state == SpawnState.WAITING)
            {
                //verifica se nao tem inimigos vivos
                if (!EnemyAlive())
                {

                    if (nextWave > waves.Length - 1)
                    {
                        nextWave = 0;
                        Debug.Log("Todas Waves finalizadas LOOPANDO...");

                    }
                    state = SpawnState.COUNTING;
                }
            }

            if(state == SpawnState.COUNTING)
            {
                waveCountdown -= Time.deltaTime;
            }

            //tempo do spawner acabou
            if (waveCountdown <= 0)
            {
                //começa a spawnar
                if (state != SpawnState.SPAWNING)
                {
                    StartCoroutine(SpawnWave(waves[nextWave]));
                    waveCountdown = timeBetweenWaves;
                    nextWave++;
                }
            }
 
        }
        else
        {
            //se o PlAYER saiu da area cancela a wave
            state = SpawnState.WAITING;
            waveCountdown = timeBetweenWaves;
            nextWave = 0;
        }
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        //muda o state para invocando
        state = SpawnState.SPAWNING;

        //invoca a quantidade de inimigos o contador manda
        Debug.Log("invocando a wave:" + nextWave);
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave, i);
            yield return new WaitForSeconds(_wave.rate);
        }

        //terminando de invocar os inimigos volta a esperar o player matar;
        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Wave _wave, int count)
    {
        // Verifica se o pointSpawn está atribuído corretamente
        if (pointSpawn != null)
        {
            // Manda inimigos alternadamente
            if (playerIn)
            {
                switch (count % 2)
                {
                    case 0:
                        Instantiate(_wave.enemy1, pointSpawn.position, pointSpawn.rotation);
                        Debug.Log("invocando: " + _wave.enemy1);
                        break;
                    case 1:
                        Debug.Log("invocando: " + _wave.enemy2);
                        Instantiate(_wave.enemy2, pointSpawn.position, pointSpawn.rotation);
                        break;
                }
            }
        }
        else
        {
            Debug.LogError("pointSpawn não está atribuído!");
        }
    }

    //verifica se tem algum inimigo pela tag no mapa
    bool EnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0)
        {
            searchCountdown = 3f;
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entrou");
            playerIn = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player saiu");
            playerIn = false;
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
    }
}
