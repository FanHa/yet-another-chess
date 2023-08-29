using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float CurrentHealth;
    public float MaxHealth;

    public GameObject HealthBarUI;
    public Slider Slider;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        Slider.value = CalculateHealth();
    }


    // Update is called once per frame
    void Update()
    {
        Slider.value = CalculateHealth();

        if( CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }

    float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }
}
