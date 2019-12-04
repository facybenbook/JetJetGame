using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public GameObject WeaponProjectile;
    public List<GameObject> ShootPoints;
    public AudioClip ShotSound;

    public float SoundsPerSec = 10f;
    public float ShotsPerSec = 20f;

    private float TimeSinceLastSound = 0f;
    private float TimeSinceLastShot = 0f;

    //

    void Start()
    {

    }

    void Update()
    {
        TimeSinceLastShot += Time.deltaTime;
        TimeSinceLastSound += Time.deltaTime;
    }

    //

    public void Shoot(Vector3 startVelocity, Vector3 startRayPosition, Vector3 forwardRayDirection)
    {
        StartCoroutine( ShootOnFrame(startVelocity, startRayPosition, forwardRayDirection));
    }


    IEnumerator ShootOnFrame(Vector3 startVelocity, Vector3 startRayPosition, Vector3 forwardRayDirection)
    //public void Shoot(Vector3 startVelocity, Vector3 startRayPosition, Vector3 forwardRayDirection)
    {
        yield return 0;

        if( TimeSinceLastShot < (1/ShotsPerSec) )
        {
            // no shot
        }
        else
        {
            foreach (GameObject sp in ShootPoints)
            {

                Vector3 StartPos = sp.transform.position + (startVelocity * Time.deltaTime);

                // ** LASER **
                Projectile projectile = GameObject.Instantiate(WeaponProjectile).GetComponent<Projectile>();
                projectile.team = "Airwing";
                projectile.transform.position = StartPos;
                projectile.transform.rotation = sp.transform.rotation;
                GameObject.Destroy(projectile.gameObject, 6f);

                // rate of fire
                if( TimeSinceLastSound >= (1/SoundsPerSec) )
                {
                    int ran = 0;//  Random.Range(0, 2);
                    if(ran == 0)
                        BoomBox.I.PlaySoundWeapon(ShotSound);
                    else if(ran == 1)
                        BoomBox.I.PlaySoundWeapon(ShotSound);
                    else
                        BoomBox.I.PlaySoundWeapon(ShotSound);

                    TimeSinceLastSound = 0f;
                }
            }
            TimeSinceLastShot = 0f;
        }
    }

}
