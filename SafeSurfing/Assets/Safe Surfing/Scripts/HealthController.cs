using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    public int Lives = 3;

    public UnityEvent LifeLost;

    public bool IsDead => Lives == 0;
    public bool IsIgnoringBullets { get; private set; }

    public void SetIgnoreBullets(float ignoreTime)
    {
        StartCoroutine(IgnoreBullets(ignoreTime));
    }

    private IEnumerator IgnoreBullets(float time)
    {
        IsIgnoringBullets = true;
        yield return new WaitForSeconds(time);
        IsIgnoringBullets = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !IsIgnoringBullets)
        {
            if (!IsDead)
            {
                Lives--;
                LifeLost?.Invoke();
            }
                
        }
    }
}
