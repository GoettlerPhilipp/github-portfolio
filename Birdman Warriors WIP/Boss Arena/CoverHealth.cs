using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverHealth : MonoBehaviour
{
    public int health;
    private bool invincible;
    [SerializeField] private float invincibleTimer = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (invincible)
            invincibleTimer -= Time.deltaTime;
        if (invincibleTimer < 0)
            invincible = false;
        if(health <= 0)
            Destroy(gameObject);
    }

    public void GetDamage()
    {
        if (!invincible)
        {
            health--;
            invincible = true;
        }
    }
}
