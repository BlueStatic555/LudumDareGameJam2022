using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rat_spawn : MonoBehaviour
{
    public float timer;
    private float remaining;
    public GameObject rat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        remaining -= dt;
        if (remaining <= 0)
        {
            GameObject my_rat = Instantiate(rat);
            my_rat.transform.position = transform.position;
            remaining = timer;
        }
    }
}
