using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilBulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Destruir", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Destruir()
    {
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
