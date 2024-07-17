using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyDrop : MonoBehaviour
{
    private float speed = 0.58f;

    private void Start()
    {
        Destroy(gameObject, 2.5f);
    }

    void Update()
    {
        gameObject.transform.Translate(new Vector2(0, -5.5f) * Time.deltaTime * speed);
    }
}
