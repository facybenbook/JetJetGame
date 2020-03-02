using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;



public class Airwing : MonoBehaviour
{

    [Header("Components")]
    public Rigidbody rigid;

    [Tooltip("Stuff which can be hidden when the craft is exploded")]
    public GameObject ExploderBox;

    [Header("Wings")]
    public WWing Wing;

    public WWing elevatorWing;
    public WSurface elevator;

    public WWing aileronLeftWing;
    public WSurface aileronLeft;

    public WWing aileronRightWing;
    public WSurface aileronRight;

    public WWing rudderWing;
    public WSurface rudder;


    [Header("Physics")]
    [Tooltip("Max thrust")] public float thrustMax = 1000000f;
    [Range(0, 1)] public float throttle = 0f;
    float ThrottleHigh =  1f;
    float ThrottleLow =  1f;

    [Header("Autopilot")]
    [Tooltip("Angle when autopoilot turns completely towards the cam target")]
    public float aggressiveTurnAngle = 90f;
    float pitch = 0f;
    float yaw = 0f;
    float roll = 0f;
    Vector3 FlyTarget;

    float AimZAngle = 0;
    float AimZDir = 1f;
    float ManualRoll = 0;
    bool SpecifiedRoll;

    bool  BoostOn = false;
    float BoostTime = 30f;
    float BoostMaxTime = 30f;
    float BoostMinTime = 3f;


    [Header("Weapons")]
    public List<Weapon> weapons;

    [Header("Effects")]
    float SpeedToPitch = 1f;
    public GameObject trailFX;


    int Health = 0;
    bool IsDead = true;
    [HideInInspector] public double DeadTime = 0;

    [HideInInspector] public bool LowSpeed = true;
    [HideInInspector] public bool HasHitShit = false;

    List<ContactPoint> HasHit;

    public AudioClip RejuvinateSound;
    public GameObject Rejuvinator;

    float TimeSinceLastExplosion = 0f;
    float ExplosionRate = 1f;

    float TimeSinceLastDamage = 0f;
    float DamageRate = 1f;




    void Start()
    {
        TurnOffTrails();
        TurnOffAudio();

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity  = Vector3.zero;
        rigid.isKinematic = true;

        // if( ExploderBox != null )
        //     ExploderBox.SetActive(false);

        //EngineIdle();

    }




    void Update()
    {
        TimeSinceLastExplosion += Time.deltaTime;
        TimeSinceLastDamage += Time.deltaTime;
    }


    void FixedUpdate()
    {
        TimeSinceLastExplosion += Time.deltaTime;
        TimeSinceLastDamage += Time.deltaTime;

        if( !IsDead) {
            SwivelWeapons();
            RunAutopilot();
        }
        else {
            if( Game.I.IsPlayingGame())
            {
                double TimeSinceDeath = Time.time - DeadTime;
                if( TimeSinceDeath > 0 )
                    DoLife();
            }
        }

        if( rigid.velocity.magnitude * 3.6 < 300f)
            LowSpeed = true;
        else
            LowSpeed = false;
    }

    public void EngineHigh()
    {
        throttle = ThrottleHigh;
    }

    public void EngineMid()
    {
        throttle = ThrottleLow;
    }

    public void EngineIdle()
    {
        throttle = 0;
    }


    float GetManualRollInput()
    {

        AimZAngle = 0;
        AimZDir = 1f;
        ManualRoll = 0f;
        SpecifiedRoll = false;

        // MANUAL ROLL
        // Q
        if( Input.GetKey(KeyCode.Q))
        {
            ManualRoll = -1.5f; // continuous roll left
        }
        // E
        else if( Input.GetKey(KeyCode.E))
        {
            ManualRoll = 1.5f; // continuous roll right
        }
        // SPECIFIED ROLL
        else if( Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) )
        {
            // //                W
            // //                0
            // //          315        45
            // //   A   270              90   D
            // //          225       135
            // //               180
            // //                S

            // W
            if( Input.GetKey(KeyCode.W))
            {
                if( !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    AimZAngle = 0f;

                else if( Input.GetKey(KeyCode.A))
                    AimZAngle = 315f;
                else if( Input.GetKey(KeyCode.S))
                    AimZAngle = 0f;
                else if( Input.GetKey(KeyCode.D))
                    AimZAngle = 45f;
            }
            //A
            else if( Input.GetKey(KeyCode.A))
            {
                if( !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
                    AimZAngle = 270f;

                else if( Input.GetKey(KeyCode.W))
                    AimZAngle = 315f;
                else if( Input.GetKey(KeyCode.S))
                    AimZAngle = 225f;
                else if( Input.GetKey(KeyCode.D))
                    AimZAngle = 270f;
            }
            // S
            else if( Input.GetKey(KeyCode.S))
            {
                if( !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                    AimZAngle = 180f;

                else if( Input.GetKey(KeyCode.W))
                    AimZAngle = 180f;
                else if( Input.GetKey(KeyCode.A))
                    AimZAngle = 225f;
                else if( Input.GetKey(KeyCode.D))
                    AimZAngle = 135f;
            }
            // D
            else if( Input.GetKey(KeyCode.D))
            {
                if( !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
                    AimZAngle = 90f;

                else if( Input.GetKey(KeyCode.W))
                    AimZAngle = 45f;
                else if( Input.GetKey(KeyCode.S))
                    AimZAngle = 135f;

                else if( Input.GetKey(KeyCode.A))
                    AimZAngle = 90f;
            }

            AimZDir = AimZAngle < 270f ? 1f : -1f;
            SpecifiedRoll = true;
        }
        

        float finalZRoll = (transform.localRotation.eulerAngles.z + AimZAngle) % 360f;

        finalZRoll %= 360f;


        // turned off roll direction for now
        if( SpecifiedRoll)
        {
            //      Hard Roll with Roll Directionaity
            //
            // //             0 0
            // //       0.25       -0.25 -- halfpower reverse
            // //
            // // 0.5                    1 -- hard roll
            // //
            // //       0.75         1  -- hard roll
            // //             1 1  -- hard roll

            // Invert if rolling left
            if( AimZDir == -1f)
                finalZRoll = 360f - finalZRoll;

            if( finalZRoll >= 350f) // Overshoot (270 -> 360 degrees) "halfpower reverse"
                finalZRoll = -5f;
            else if( finalZRoll >= 340f)
                finalZRoll = -10f;
            else if( finalZRoll >= 330f)
                finalZRoll = -20f;
            else if( finalZRoll >= 320f)
                finalZRoll = -30f;
            else if( finalZRoll >= 315f)
                finalZRoll = -90f;
            else if( finalZRoll > 180f)
                finalZRoll = 360f;
            else
                finalZRoll *= 2; // Range power from 0..1

            // Negate if rolling left
            if( AimZDir == -1f)
                finalZRoll *= AimZDir;

            finalZRoll /= 360f;

        }
        else
        {
            //              Mirror Both Sides
            //          Each side, range power from 0..1
            //
            // //
            // //              0 360
            // //          30        330
            // //      60               300
            // //   90                     270
            // //      120              240
            // //          150      210
            // //               180
            // //              -0 0
            // //          -0.25    0.25
            // //      -0.75           0.75
            // //  -0.5                    0.5
            // //      -0.625           0.75
            // //          -0.75    0.75
            // //              -1 1

            if( finalZRoll > 180f)
            {
                finalZRoll = 360f - finalZRoll;
                finalZRoll = -finalZRoll;
            }
            finalZRoll /= 180f;
        }

        return finalZRoll;
    }


    void RunAutopilot()
    {
        if (elevator == null || aileronLeft == null || aileronRight == null || rudder == null)  return;

        if( !HasHitShit && !IsDead)
        {


            ////
            FlyTarget = Game.I.AirwingControl.MouseAimPos;

            var localFlyTarget = transform.InverseTransformPoint(FlyTarget).normalized;
            var angleOffTarget = Vector3.Angle(transform.forward, FlyTarget - transform.position);

            yaw = localFlyTarget.x;
            pitch = Mathf.Clamp(localFlyTarget.y, -1f, 1f);

            var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);

            float wingsLevelRoll = GetManualRollInput();

            var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveTurnAngle, angleOffTarget);
            ////



            // PITCH
           elevator.Deflection = pitch;


            // YAW
           if( pitch > -0.25f && pitch < 0.25f)
                rudder.Deflection = -(yaw);
            else
                rudder.Deflection = 0;


            // ROLL
            if( ManualRoll != 0f )
                roll = ManualRoll;
            else if( SpecifiedRoll ) 
                roll = wingsLevelRoll;
            else
                roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);

            aileronRight.Deflection = roll;
            aileronLeft.Deflection = -roll;


            // THROTTLE
            if(Input.mouseScrollDelta.y != 0)
                throttle += (Input.mouseScrollDelta.y * 0.1f);

            throttle = Mathf.Clamp01( throttle );
            //if(throttle < 0.3f)  throttle = 0.3f;

            if(Input.mouseScrollDelta.y != 0)
            {
                //print("Throttle adjusted: " + throttle);
            }


            // Final Vehicle Thrust
            rigid.AddRelativeForce(Vector3.forward * thrustMax * throttle, ForceMode.Force);

        }
        else {

            if( !IsDead)
            {
                // force down
                if( !LowSpeed )
                {
                    if( Random.Range(0, 99) == 0 )
                        rigid.AddRelativeForce(Vector3.forward * (thrustMax * 2) * throttle, ForceMode.Force);

                    rigid.AddForce(-Vector3.up * 1200000f, ForceMode.Force); // extra gravity
                }
                else if( rigid.velocity.magnitude > 30 ) // incomplete, tone down gravity when low speed or low altitude
                {
                    //rigid.AddForce(-Vector3.up * 1000000f, ForceMode.Force);
                    rigid.AddForce(-Vector3.up * 1500000f, ForceMode.Force); // extra gravity
                }
            }
            else
            {
                // tootally dead
            }
        }
    }


    

    void SwivelWeapons()
    {
        foreach (Weapon w in weapons) {
            Debug.DrawLine(w.gameObject.transform.position + (rigid.velocity * Time.deltaTime), FlyTarget, Color.green);
            w.gameObject.transform.LookAt( FlyTarget, transform.right );
        }
    }


    public void Shoot()
    {
        if(IsDead)   return;

        Vector3 aimRay = transform.forward;
        Vector3 startRay = transform.position;

        foreach (Weapon w in weapons)
            w.Shoot(rigid.velocity, startRay, aimRay);

    }

    public void TakeDamage(int amount)
    {
        if( IsDead)  return;

        if( TimeSinceLastDamage >= (1/DamageRate) )
        {
            print("TakeDamage");

            Health -= amount;

            if( Health <= 0 )
            {
                Health = 0;
                DoDeath(true);
            }

            TimeSinceLastDamage = 0;
        }
    }



    public void DoDeath(bool withExplosion)
    {
        IsDead = true;
        DeadTime = Time.time;

        //TurnOffTrails();
        TurnOffAudio();


        rigid.velocity = Vector3.zero;
        rigid.angularVelocity  = Vector3.zero;
        rigid.isKinematic = true;


        if( ExploderBox != null )
            ExploderBox.SetActive(false);

        if( withExplosion)
            MakeExplosion("Explosion/1Explosion", transform.position, transform.rotation, true);
    }


    public void SetRejuvinator(GameObject gobj)
    {
        Rejuvinator = gobj;
    }


    public void DoLife()
    {

        print("DoLife");

        rigid.isKinematic = false;

        TurnOnTrails();
        TurnOnAudio();
        EngineMid();

        transform.position = Rejuvinator.transform.position;
        transform.rotation = Rejuvinator.transform.rotation;


        if( ExploderBox != null )
            ExploderBox.SetActive(true);

        IsDead = false;
        HasHitShit = false;
        Health = 100;

        BoomBox.I.PlayOnce(RejuvinateSound);
    }



    public void MakeExplosion(string prefab, Vector3 pos, Quaternion rot, bool parent)
    {
        if( TimeSinceLastExplosion >= (1/ExplosionRate) )
        {
            print("MakeExplosion");

            GameObject explosionObject = (GameObject)Instantiate(Resources.Load( prefab ), pos, rot);
            if( parent) explosionObject.transform.parent = transform.parent;
            Destroy(explosionObject, 5f);

            TimeSinceLastExplosion = 0f;
        }
    }



    void OnCollisionEnter(Collision collision)
    {
        if( IsDead )  return;

        if( !HasHitShit )
        {
            //HasHitShit = true;
        }

        ContactPoint contact = collision.contacts[0];

        foreach (ContactPoint contactPoint in collision.contacts)
        {

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);
            Vector3 pos = contactPoint.thisCollider.transform.position;


            MakeExplosion("Explosion/1Explosion", pos, rot, false);
            TakeDamage(1);


            // if( contactPoint.thisCollider.GetComponent<WSurface>() != null)
            //     contactPoint.thisCollider.GetComponent<WSurface>().Off();
            // if( contactPoint.thisCollider.GetComponent<WWing>() != null)
            //     contactPoint.thisCollider.GetComponent<WWing>().Off();


            // if( contactPoint.otherCollider.tag != "Ground" && rigid.velocity.magnitude > 200)
            // {
            //
            //
            //     //print(contactPoint.thisCollider.name + " hit " + contactPoint.otherCollider.name);
            //     //print("Force Added: " +  rigidMult );
            //
            //     //rigid.AddForce(contactPoint.normal * (rigid.velocity.magnitude * rigid.velocity.magnitude), ForceMode.Impulse);
            // }
            // else
            //     print("Slow hit or Ground hit - No Collider Force");


            if( contactPoint.otherCollider.tag == "Ground")
            {
                //TakeDamage(101);
            }
            else
            {
                //TakeDamage(10);
            }


            // MakeExplosion("1Explosion", pos, rot, false);


        }

    }



    public void TurnOnTrails() {    trailFX.SetActive(true);   }
    public void TurnOffTrails() {   trailFX.SetActive(false);   }

    public void TurnOnAudio() {
        if( !BoomBox.I.EnginesOn)
            BoomBox.I.PlayEngines();
    }

    public void TurnOffAudio() {
        BoomBox.I.PauseEngines();
    }




    public string GetHealth()
    {
        if( IsDead)  return "0";
        else  return Health.ToString();
    }

    public float GetSpeedToPitch()
    {
        // Pitch value from velocity
        SpeedToPitch = ( rigid.velocity.magnitude / 3200f ) ; //2400f is roughly max speed, this is a arbitary
        if( SpeedToPitch > 1.42f) SpeedToPitch += 0.05f;
        if( SpeedToPitch > 1.4f) SpeedToPitch += 0.05f;
        if( SpeedToPitch > 1.35f) SpeedToPitch += 0.05f;
        if( SpeedToPitch > 1.3f) SpeedToPitch += 0.05f;
        if( SpeedToPitch > 0.9f) SpeedToPitch += 0.05f;

        //if( SpeedToPitch < 0.6f) SpeedToPitch = 0.6f;
        // else if( SpeedToPitch < 0.7f) SpeedToPitch = 0.7f; // sharp edge
        // else if( SpeedToPitch < 0.8f) SpeedToPitch = 0.8f; // sharp edge

        return SpeedToPitch;
    }



    float getPitch(Quaternion c) {  return Mathf.Atan2(2*(c.y*c.z + c.w*c.x), c.w*c.w - c.x*c.x - c.y*c.y + c.z*c.z);    }
    float getYaw(Quaternion c) {    return Mathf.Asin(-2*(c.x*c.z - c.w*c.y));    }
    float getRoll(Quaternion c) {   return Mathf.Atan2(2*(c.x*c.y + c.w*c.z), c.w*c.w + c.x*c.x - c.y*c.y - c.z*c.z);    }




    public float ForceG()
	{
		Vector3 localVelocity = transform.InverseTransformDirection(rigid.velocity);
		Vector3 localAngularVel = transform.InverseTransformDirection(rigid.angularVelocity);
		float radius = (Mathf.Approximately(localAngularVel.x, 0.0f)) ? float.MaxValue : localVelocity.z / localAngularVel.x;
		float verticalForce = (Mathf.Approximately(radius, 0.0f)) ? 0.0f : (localVelocity.z * localVelocity.z) / radius;
		float verticalG = verticalForce / -9.81f;
		verticalG += transform.up.y * (Physics.gravity.y / -9.81f);
		return verticalG;
	}



}
