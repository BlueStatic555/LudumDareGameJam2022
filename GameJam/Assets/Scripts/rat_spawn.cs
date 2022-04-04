using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rat_spawn : MonoBehaviour
{
    public float timer;
    private float remaining;
    public GameObject rat;
    public GameObject fast_rat;


    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        remaining -= dt;
        if (remaining <= 0)
        {
            int num = Random.Range(0, 10);
            if (num == 0)  //10% chance for fast rat
            {
                GameObject my_rat = Instantiate(fast_rat);
                my_rat.transform.position = transform.position;
            }
            else 
            {
                GameObject my_rat = Instantiate(rat);
                my_rat.transform.position = transform.position;
            }
            if(timer > 1.0f)
                timer -= 0.5f;
            remaining = timer;
        }
    }
}
