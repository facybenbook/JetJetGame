using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoomBox : MonoBehaviour
{

    public static BoomBox I;
    protected BoomBox() { }
    private void Awake() { I = this; }

    public AudioSource Radio;
    public AudioSource SoundGeneral;
    public AudioSource SoundWeapon;

    public bool EnginesOn = false;

    public AudioSource SoundCubeA;
    public AudioSource SoundCubeB;
    public AudioSource SoundCubeC;

    public AudioSource SoundCubeTR;
    public AudioSource SoundCubeBR;
    public AudioSource SoundCubeBL;
    public AudioSource SoundCubeTL;

    public AudioClip[] clips = null;


    //

    void Start()
    {
        StartCoroutine( PlayRadio() );
    }

    IEnumerator PlayRadio()
    {
        yield return 0;

        Radio.Play();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if(!EnginesOn) return;

        float pitch = Game.I.Airwing.GetSpeedToPitch();

        print(pitch);

        SoundCubeA.pitch = pitch;
        SoundCubeB.pitch = pitch;
        SoundCubeC.pitch = pitch;

        SoundCubeTR.pitch = pitch;
        SoundCubeBR.pitch = pitch;
        SoundCubeBL.pitch = pitch;
        SoundCubeTL.pitch = pitch;
    }

    //

    public void PlayEngines()
    {
        print("PlayEngines");

        EnginesOn = true;
        //
        SoundCubeA.Play();
        SoundCubeB.Play();
        SoundCubeC.Play();

        SoundCubeTR.time = Random.Range(0, SoundCubeTR.clip.length);
        SoundCubeBR.time = Random.Range(0, SoundCubeBR.clip.length);
        SoundCubeBL.time = Random.Range(0, SoundCubeBL.clip.length);
        SoundCubeTL.time = Random.Range(0, SoundCubeTL.clip.length);

        SoundCubeTR.Play();
        SoundCubeBR.Play();
        SoundCubeBL.Play();
        SoundCubeTL.Play();
    }

    public void PauseEngines()
    {
        print("PauseEngines");

        EnginesOn = false;

        SoundCubeA.Pause();
        SoundCubeB.Pause();
        SoundCubeC.Pause();

        SoundCubeTR.Pause();
        SoundCubeBR.Pause();
        SoundCubeBL.Pause();
        SoundCubeTL.Pause();
    }


    //


    // IEnumerator RandomStart(AudioSource source)
    // {
    //     while (EnginesOn) {
    //         AudioClip clip = EngineSounds[Random.Range(0, EngineSounds.Length - 1)];
    //         source.PlayOneShot(  clip  );
    //         yield return new WaitForSeconds(  clip.length  );
    //
    //     }
    // }

    public void PlayOnce(AudioClip clip)
    {
        SoundGeneral.PlayOneShot(clip);//, 0.25f);
    }

    public void PlaySoundWeapon(AudioClip clip)
    {
        SoundWeapon.PlayOneShot(clip);//, 0.25f);
    }

    public void Play(AudioClip clip, Vector3 position)
    {
        StartCoroutine(PlayAudio(clip, position));
    }

    IEnumerator PlayAudio(AudioClip clip, Vector3 position)
    {
        AudioSource.PlayClipAtPoint(clip, position);
        yield break;
    }
}
