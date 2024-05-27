using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickObj : MonoBehaviour
{
    /*
    private bool move;
    private Vector2 randomVector;
    const float speed = 0.03f;

    private void FixedUpdate()
    {
        if (!move) return;
        transform.Translate(randomVector);
    }

    public void StartMotion(float scoreIncrease)
    {
        transform.localPosition = Vector2.zero;
        GetComponent<Text>().text = "+" +StringMethods.ToShortStr(scoreIncrease);
        gameObject.GetComponent<Text>().color = new Color(255, 255, 255);
        float randomAngle = Random.Range(0, 2 * Mathf.PI);
        randomVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * speed;
        move = true;
        GetComponent<Animation>().Play("clickTextAnim");
    }
      public void StartMotionBoost(float scoreIncrease)
      {
          transform.localPosition = Vector2.zero;
          GetComponent<Text>().text = "+" + StringMethods.ToShortStr(scoreIncrease * 3);
          gameObject.GetComponent<Text>().color = new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
          float randomAngle = Random.Range(0, 2 * Mathf.PI);
          randomVector = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * speed;
          move = true;
         GetComponent<Animation>().Play("clickTextAnim");
      }
    
    public void StopMotion()
    {
        move = false;
    }
    */
}
