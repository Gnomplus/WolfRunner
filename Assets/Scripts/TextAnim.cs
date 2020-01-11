using UnityEngine.UI;
using UnityEngine;
using System;
using System.Collections;

public class TextAnim : MonoBehaviour
{
    private float a;
    private bool direction = true;
    private bool timer = true;

    private void Start()
    {
        a = 0f ;
    }

    void Update()
    {
        if (timer)
        {
            if (direction)
            {
                a += 0.01f;
            }
            else
            {
                a -= 0.01f;
            }
            if (a > 1 || a < 0)
            {
                direction = !direction;
            }
            StartCoroutine("Timer");
            gameObject.GetComponent<Text>().color = new Color(1, 1, 1, a);
        }      
    }  

    IEnumerator Timer()
    {
        timer = false;
        yield return new WaitForSeconds(0.01f);
        timer = true;
    }
}
