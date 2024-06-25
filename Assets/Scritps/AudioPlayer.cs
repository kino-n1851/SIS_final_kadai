using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField]
    AudioClip hitClip;
    [SerializeField]
    AudioClip deadClip;
    [SerializeField]
    AudioSource HitSource;
    [SerializeField]
    AudioSource DeadSource;
    // Start is called before the first frame update
    void Start()
    {
        HitSource.clip = hitClip;
        DeadSource.clip = deadClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayHit()
    {
        HitSource.Play();
    }

    public void PlayDead(){
        DeadSource.Play();
    }
}
