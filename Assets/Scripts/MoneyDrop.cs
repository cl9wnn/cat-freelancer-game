using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    private float speed = 0.58f;
    void Update()
    {
        Destroy(gameObject, 2.5f);
        gameObject.transform.Translate(new Vector2(0, -5.5f) * Time.deltaTime * speed);
    }
}
