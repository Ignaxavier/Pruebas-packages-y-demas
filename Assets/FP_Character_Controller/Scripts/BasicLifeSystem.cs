using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLifeSystem : MonoBehaviour
{
    public      float       _Life = 100f;

    public      float       originalLife;

    private void Awake()
    {
        originalLife = _Life;
    }

    public void GetDamage(float damage)
    {
        if(damage != 0)
        {
            _Life -= damage;
        }
    }

    public void GetHealth(float health)
    {
        if(health != 0)
        {
            _Life += health;

            if(_Life > originalLife)
            {
                _Life = originalLife;
            }
        }
    }

    public void IncreaseBaseLife(float increase)
    {
        if(increase != 0)
        {
            originalLife += increase;

            _Life = originalLife;
        }
    }
}
