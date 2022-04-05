using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratAudio : MonoBehaviour
{
    public AudioSource rSource;
    public static int screams;
    // Start is called before the first frame update
    void Start()
    {
        screams++;
        if (screams < 4)
        {
            rSource.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rSource.isPlaying)
        {
            screams--;
            Destroy(transform.gameObject);
        }
    }
}