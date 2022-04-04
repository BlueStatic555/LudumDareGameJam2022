using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ratAudio : MonoBehaviour
{
    public AudioSource rSource;
    // Start is called before the first frame update
    void Start()
    {
        rSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!rSource.isPlaying)
        {
            Destroy(transform.gameObject);
        }
    }
}
