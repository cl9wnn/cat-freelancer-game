using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spez : MonoBehaviour
{
    public float speed;

    void Start()
    {

    }
    public void OnClick()
    {
        Game.Instance.Clicks++; //����� ����� ���� ������������ ���������� ������� ��� ������ 
        SpawnDown.Instance.CaughtCoin.Play();
        Destroy(gameObject);
    }
    void Update()
    {
        Destroy(gameObject, 8f); //���� ������� ����� ���������� 
        gameObject.transform.Translate(new Vector2(0, -5.5f) * Time.deltaTime * speed);
    }
}
