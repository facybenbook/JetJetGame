using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundCube : MonoBehaviour
{

    public AudioMixerGroup mixerStandard;
    public AudioMixerGroup mixerReverb;

    AudioSource source;



    RaycastHit rayhit;
    int layerMask = 1 << 0; // Default layer

    void Start()
    {
        source = GetComponent<AudioSource>();
    }


    void FixedUpdate()
    {

        // Draw a ray from here to Player
        // if the ray is not clear
        // change the audio source things
        Vector3 to_player = transform.position - Game.I.Airwing.transform.position;

        //Debug.DrawLine(transform.position, transform.position - to_player, Color.yellow);

        if( Physics.Raycast(transform.position, -to_player.normalized, out rayhit, to_player.magnitude, layerMask))
        {
            //Debug.DrawLine(transform.position, rayhit.point, Color.red);
            source.outputAudioMixerGroup = mixerReverb;
            //print(transform.name);
            //Debug.Break();

        }
        else
        {
            source.outputAudioMixerGroup = mixerStandard;
        }



    }

    //

}
