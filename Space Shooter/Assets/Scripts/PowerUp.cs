using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Shield = 0,
    Speed  = 1,
    Triple = 2,
}

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private PowerUpType type;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y<-6)
        {
            Destroy(gameObject);
        }
    }

    public PowerUpType GetPowerUpType()
    {
        return type;
    }
}
