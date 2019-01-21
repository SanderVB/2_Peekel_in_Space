using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 1;
    private void Start()
    {
        //changes health based on selected difficulty (when difficulty is added)
        /*if (GetComponent<Enemy>() != null)
            health = health + (health * PlayerPrefsController.GetDifficulty());
        else if (GetComponent<PlayerController>() != null)
            health = health - PlayerPrefsController.GetDifficulty());
        else
            return;*/
    }

    public void DamageHandler(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DeathHandler();
            Destroy(gameObject);
        }
    }

    private void DeathHandler()
    {

    }
}
