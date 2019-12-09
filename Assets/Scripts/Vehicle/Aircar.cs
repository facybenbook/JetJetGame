using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aircar : MonoBehaviour {

    public Rigidbody Target;

    protected float TargetDistance = Mathf.Infinity;

    public string team;

    public enum S {
        Moving,
        MovingToRoad,
        MovingToRoadEnd,
        Landed,
        Landing,
        TakingOff,
        FoundTargetRoad,
        FoundTargetRoadEnd,
        Idle,
        Parked,
        Crashing
    }

    [HideInInspector]
    public S CarState;

    [HideInInspector]
    public Vector3 targetPos; // position where the plane is moving towards


    [HideInInspector]
    public float currentMovementSpeed;
    public float movementSpeed = 50;
    public float rotationSpeed = 50;

    [HideInInspector]
    public float currentRotation;
    [HideInInspector]
    public Vector3 previousPosition;
    [HideInInspector]
    public float previousYRot;

    public Transform planeModel;

    [HideInInspector]
    float zRot;  // tilt the plane when it turns


    [HideInInspector]
    public float gettingDistanceTimer;
    [HideInInspector]
    public bool gettingDistanceFromTarget; // to get some distance when we go too close to the target, so we can approach it again
    [HideInInspector]
    private Vector3 gettingDistancePosition; // randomized position we're moving towards to get some distance

    public float tooCloseDistance = 30; // distance to target closer than this triggers the gettingDistance variable

    [HideInInspector]
    public int landingStep;
    [HideInInspector]
    public bool hasLandingSpot;
    [HideInInspector]
    public int takingOffStep;
    [HideInInspector]
    public Port currentPort;
    [HideInInspector]
    public Port.ParkingSpot landingSpot;


    [HideInInspector]
    public Health bodyHealth;
    [HideInInspector]
    private float currentTerrainY;
    [HideInInspector]
    private Vector3 idlePosition;


    //

    public void Awake() {
        bodyHealth = GetComponent<Health>();
        targetPos = transform.position;

        InvokeRepeating("Observe", 0, 0.5f);
    }

    public virtual void Update() { }

    public void UpdateTick() {
        float dist = 11000;
        RaycastHit hit;
        Vector3 dir = new Vector3(0, -1, 0);
        if( Physics.Raycast(transform.position + new Vector3(0, 100, 0), dir, out hit, dist)) { // raycast from above to detect terrain collisions
            // if( hit.collider.tag == "Terrain") {
                currentTerrainY = hit.point.y;
            // }
        }
        else {
            currentTerrainY = 0;
        }


        if( CarState == S.Crashing)
        {
            if( currentTerrainY < 20 && currentTerrainY != 0 )
            {
                Crash();
            }
            MoveForward(currentMovementSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), 0.25f * Time.deltaTime);
        }
        else if( CarState == S.Landing) {
            Landing();
            MoveTowardsPosition();
        }
        else if( CarState == S.Idle)
        {
            currentMovementSpeed = movementSpeed; //Mathf.MoveTowards(currentMovementSpeed, movementSpeed, 10f * Time.deltaTime);
            SetTargetPosition(idlePosition);
            //MoveTowardsPosition();
        }
        else if( CarState == S.TakingOff)
        {
            TakingOff();
            MoveTowardsPosition();
        }
        else if( CarState == S.Moving || CarState == S.MovingToRoad || CarState == S.MovingToRoadEnd )
        {
            currentMovementSpeed = movementSpeed; //Mathf.MoveTowards(currentMovementSpeed, movementSpeed, 10f * Time.deltaTime);
            MoveTowardsPosition();
        }
        else if( CarState == S.Landed)
        {
            if( Vector3.Distance(transform.position, landingSpot.spot.position) > 2) {
                MoveForward(10);
                RotateTowardsPosition(landingSpot.spot.position); // slowly roll towards parking spot
            } else {
                SetState(S.Parked); // reached parking spot
            }
        }
    }

    //

    void Observe() {

        TargetDistance = Vector3.Distance(transform.position, Target.transform.position);

    }

    void Landing() {

        targetPos = currentPort.landingSpots[landingStep].position; // fly towards the port

        if( Vector3.Distance(transform.position, targetPos) < tooCloseDistance) { // reached the target, next landing step
            if( hasLandingSpot) {
                landingStep++;
                if( landingStep > 4) { // reached the last landing step
                    Landed();
                }
            } else {
                if( gettingDistanceFromTarget == false) {
                    landingSpot = currentPort.GetFreeSpot(this); // try to reserve a landing spot from the port
                    if( landingSpot != null) {
                        hasLandingSpot = true; // found a free landing spot
                    } else {

                        //Port FULL, Find Random New Port
                        //print("Port FULL, Find Random New Port");

                        Port newPort = PortAuthority.I.RandomNewPort(currentPort.transform.position);
                        if(newPort != null)
                            currentPort = newPort;

                        landingSpot = currentPort.GetFreeSpot(this); // try to reserve a landing spot from the port
                        if( landingSpot != null) {
                            hasLandingSpot = true; // found a free landing spot
                        }
                        else
                        {
                            GetDistance(); // port is full, wait and fly in circles
                        }

                    }
                }
            }
        }
        if( landingStep >= 4) {
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, 10, 5f * Time.deltaTime); // slow down
        } else {
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, movementSpeed, 10f * Time.deltaTime);
        }
    }
    void Landed() {
        SetState(S.Landed);
        landingStep = 0;
        transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //make sure the plane is straight
    }

    void GetDistance() { // got close to the target, fly in a random distance for a while to get some distance
        //print("GetDistance");
        gettingDistanceTimer = UnityEngine.Random.Range(1f, 5f);

        if( landingStep < 3) {
            gettingDistanceFromTarget = true;

            if( transform.position.y < 70) {// limit height not to hit the ground
                gettingDistancePosition = transform.position + new Vector3(UnityEngine.Random.Range(-tooCloseDistance, tooCloseDistance), 50, UnityEngine.Random.Range(-tooCloseDistance, tooCloseDistance));
            } else {
                gettingDistancePosition = transform.position + new Vector3(UnityEngine.Random.Range(-tooCloseDistance, tooCloseDistance), UnityEngine.Random.Range(-50, 50), UnityEngine.Random.Range(-tooCloseDistance, tooCloseDistance));
            }
        }
    }
    void TakingOff() {
        if( takingOffStep > 0) {
            currentMovementSpeed = Mathf.MoveTowards(currentMovementSpeed, movementSpeed, 10f * Time.deltaTime); // on runway or in the air, increase speed
        } else {
            currentMovementSpeed = 10;// on ground, slow speed
        }
        targetPos = currentPort.landingSpots[4 - takingOffStep].position;
        if( (Vector3.Distance(transform.position, targetPos) < tooCloseDistance && takingOffStep > 1) || (takingOffStep <= 1 && Vector3.Distance(transform.position, targetPos) < 5)) {
            takingOffStep++;
            if( takingOffStep > 4) {
                TakeOffFinished();
            }
        }
    }

    void TakeOffFinished() {
        takingOffStep = 0;
        SetStateIdle(transform.position);
    }


    public void Death() { // start crashing

        Crash(); // Immediately crash for now

        // print("Death");
        // SetState(S.Crashing);

        // GameObject explosionObject = (GameObject)Instantiate(Resources.Load("Explosion/1Explosion"), transform.position, transform.rotation);
        // explosionObject.transform.parent = gameObject.transform;
        // GameObject.Destroy(explosionObject, 6f);

    }

    void Crash() { // remove plane
        //print("Crash");

        GameObject explosionObject = (GameObject)Instantiate(Resources.Load("Explosion/1Explosion"), transform.position, transform.rotation);
        GameObject.Destroy(explosionObject, 6f);

        GameObject.Destroy(gameObject);
    }

    public void StartLanding() {
        StartLanding(PortAuthority.I.ClosestPort(transform.position));
    }

    public void StartLanding(Port port) {

        if( port != null) {
            if( CarState != S.Landed && CarState != S.Landing) {
                currentPort = port;
                SetState(S.Landing);
                landingStep = 0;
            }
        } else {
            SetStateIdle(transform.position); //port not found
        }
    }

    public void StartTakeoff() {
        StartTakeoff(PortAuthority.I.ClosestPort(transform.position));
    }

    public void StartTakeoff(Port port) {
        if( CarState == S.Parked) {

            currentPort = port;
            SetState(S.TakingOff);
            takingOffStep = 0;
        } else {
            Debug.Log("The plane must be landed in order to take off!");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        //print(" " + collision.tag);

        if(collision.tag == "AimHUD_Cull")
        {
            print("Cull " + gameObject.name);
            GameObject.Destroy(gameObject);
        }
    }

    public void SetStateIdle(Vector3 pos)
    {
        CarState = S.Idle;
        idlePosition = pos;
    }

    public void SetState(S b)
    {
        //print("SetState " + b.ToString());
        CarState = b;
        if( CarState == S.TakingOff)
        {
            landingSpot = null;
            hasLandingSpot = false;
            currentPort.ClearParkingSpot(this); // clear the current parking spot so other planes can land
        }
    }


    void MoveForward(float speed)
    {
        //print("MoveForward " + speed);
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void RotateTowardsPosition(Vector3 pos)
    {

        Quaternion oldRot = transform.rotation;
        transform.LookAt(pos);
        Quaternion desiredRot = transform.rotation;
        if( CarState == S.Landed || (CarState == S.TakingOff && takingOffStep <= 1)) {
            transform.rotation = Quaternion.RotateTowards(oldRot, desiredRot, 300 * Time.deltaTime);
        } else {
            transform.rotation = Quaternion.RotateTowards(oldRot, desiredRot, rotationSpeed * Time.deltaTime);
        }
    }

    public void SetTargetPosition(Vector3 pos)
    {
        //print("SetTargetPosition " + pos);
        targetPos = pos;
    }

    void MoveTowardsPosition()
    {
        MoveForward(currentMovementSpeed);
        //print("MoveTowardsPosition " + currentMovementSpeed);

        float dist = Vector3.Distance(transform.position, targetPos);

        if( gettingDistanceTimer > 0) {
            gettingDistanceTimer -= Time.deltaTime;

            if( gettingDistanceTimer <= 0) {
                gettingDistanceFromTarget = false;
                if( dist < tooCloseDistance) {
                    GetDistance();
                }
            }
        }



        if( CarState == S.MovingToRoad && dist < tooCloseDistance) {
            transform.position = targetPos;
            CarState = S.FoundTargetRoad;
            //print("FoundTargetRoad");
        }

        if( CarState == S.MovingToRoadEnd && dist < tooCloseDistance) {
            transform.position = targetPos;
            CarState = S.FoundTargetRoadEnd;
            //print("FoundTargetRoadEnd");
        }

        if( CarState == S.Moving && dist < tooCloseDistance && gettingDistanceFromTarget == false) {
            GetDistance(); // got close to target, get some distance and reapproach
            //print("GetDistance");
        }

        if( gettingDistanceFromTarget == false) {
            RotateTowardsPosition(targetPos);
        } else {
            RotateTowardsPosition(gettingDistancePosition);
        }


        if( CarState == S.Moving || CarState == S.MovingToRoad || CarState == S.MovingToRoadEnd || (CarState == S.Landing && landingStep < 2) || (CarState == S.TakingOff && takingOffStep > 1)) {
            RotateZ(); // tilt the plane
        } else {
            zRot = 0; // on ground, no tilt
        }

        planeModel.localRotation = Quaternion.Euler(0, 0, zRot);


        previousPosition = transform.position;
        previousYRot = transform.eulerAngles.y;
    }

    void RotateZ()
    {
        float r = Mathf.Clamp(previousYRot - transform.eulerAngles.y, -2, 2);
        if( previousYRot - transform.eulerAngles.y > 0) {
            zRot = Mathf.MoveTowards(zRot, Mathf.Abs(r) * 50, 50 * Time.deltaTime);
        } else {
            zRot = Mathf.MoveTowards(zRot, Mathf.Abs(r) * (-50), 50 * Time.deltaTime);
        }
        planeModel.localRotation = Quaternion.Euler(0, 0, zRot);
    }

    // void OnGUI()
    // {
    //
    //     guistyle.normal.textColor = Color.red;
    //
    //     Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
    //     int yplus = 8;  int ypos = yplus;
    //
    //     if( TargetDistance < 2000)
    //         guistyle.fontSize = 14;
    //     else if( TargetDistance < 5000)
    //         guistyle.fontSize = 12;
    //     else if( TargetDistance < 8000)
    //         guistyle.fontSize = 10;
    //     else if( TargetDistance < 100000)
    //         guistyle.fontSize = 8;
    //     else if( TargetDistance < 200000)
    //         guistyle.fontSize = 7;
    //     else
    //         guistyle.fontSize = 6;
    //
    //     GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y - 30, 80, 25), CarState.ToString() + " " + currentMovementSpeed, guistyle ); // + " " + idlePosition
    //
    //
    //     if(currentPort != null)
    //     {
    //         if(landingStep != 0)
    //         {
    //             GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "LandingStep " + landingStep, guistyle ); ypos+=yplus; // + landingSpot.spot.ToString()
    //         }
    //
    //         if(landingSpot != null && landingSpot.spot != null)
    //         {
    //             GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "LandingSpot " + currentPort.name, guistyle ); ypos+=yplus; // + landingSpot.spot.ToString()
    //         }
    //         if(gettingDistanceFromTarget)
    //         {
    //             GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Getting Distance " + currentPort.name, guistyle );  ypos+=yplus; // + gettingDistanceTimer.ToString("f1")
    //         }
    //
    //         // if(currentPort != null)
    //         // {
    //         //     GUI.Label(new Rect(pos.x - 40, Screen.height - pos.y + ypos, 80, 25), "Haz CurrentPort ", guistyle );  ypos+=yplus; // + currentPort.transform.position.ToString()
    //         // }
    //     }
    // }

}
