using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamTrack : MonoBehaviour
{
    [Header("Airwing")]
    public GameObject Airwing = null;

    [Header("Camera")]
    public GameObject Cam;

    [Header("Cam Z Offset")]
    public float CamZOffset = -35f;
    public float CamYOffset = 10f;
    float CamYOffsetDir = 1f;
    float ZoomZOffset = 1f;

    [Header("Cam Z Rotation Parent")]
    public GameObject ZRotate;

    [Header("Cam Near Far")]
    public float CamNearLimit =  10f;
    public float CamFarLimit =  2500f;

    [Header("AimPosObject")]
    public GameObject AimPosObject;
    Vector3 AimPos;
    float AimZAngle = 0;
    float AimZDir = 1f;
    bool SpecifiedRoll = false;
    float ManualRoll = 0;

    [Header("TargetPos Object")]
    public GameObject TargetPosObject;
    Ray crossRay;
    float crossRayDist = 20000f;
    RaycastHit crossHit;


    [Header("AimScreen")]
    public GameObject AimScreen;
    public RectTransform CrossHair;
    public RectTransform AutoPilotCrossHair;
    public RectTransform AutoPilotBar;
    Vector2 CrossPos = new Vector2(0, 0);
    Vector2 CrossRayPos = new Vector2(0, 0);
    float lastScreenW;
    float lastScreenH;
    float origScreenW;
    float origScreenH;
    float frustumScaleW;
    float frustumScaleH;
    float frustumWidth;
    float frustumHeight;
    float limitTop;
    float limitBottom;
    float limitLeft;
    float limitRight;
    float AimDistance;

    [Header("Mobile Control Buttons")]
    public bool LeftSideControls = true;
    public GameObject ControlCanvas;
    public GameObject ButtonX;
    public GameObject ButtonXArea;
    public GameObject ButtonDL;
    public GameObject ButtonDR;
    public GameObject ButtonDU;
    public GameObject ButtonDD;
    public GameObject ButtonDRR;
    public GameObject ButtonDLL;
    public GameObject ButtonDArea;

    // X Button
    Vector2 SizeButtonX;
    Vector2 PosButtonX;
    Vector2 MinButtonX;
    Vector2 MaxButtonX;
    // XArea Button
    Vector2 SizeButtonXArea;
    Vector2 PosButtonXArea;
    Vector2 MinButtonXArea;
    Vector2 MaxButtonXArea;


    [Header("GroundRayDistance")]
    public float GroundRayDistance = 300f;
    bool OnTheGround = false;
    bool NearTheGround = false;
    bool MovingOnUp = false;

    // Boost
    bool  BoostOn = false;
    float BoostTime = 30f;
    float BoostMaxTime = 30f;
    float BoostMinTime = 3f;

    // Rails
    Vector3 RailDirection = new Vector3(0, 0, 1f);
    Quaternion RailRotation = Quaternion.identity;


    void Start()
    {
        AimDistance = AimScreen.transform.localPosition.z + (CamZOffset + ZoomZOffset);

        origScreenW = Screen.width;
        origScreenH = Screen.height;
        //CheckFrustum();

        Application.backgroundLoadingPriority = ThreadPriority.Low;
    }

    void Update()
    {
        if( Airwing == null || !Game.I.IsPlayingGame() )  return;

        PerformInputs();
        FollowTarget();
    }

    void FixedUpdate()
    {

    }

    public void SetCamPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void SetCamRot(Quaternion rot)
    {
        transform.rotation = rot;
    }

    public Vector3 AimPosition()        { return AimPos - new Vector3(0, CamYOffset * CamYOffsetDir, 0);   }
    public Vector3 TargetPosition()        { return TargetPosObject.transform.position;   }
    public float AimZ()                 { return AimZAngle;   }
    public float AimZDirection()        { return AimZDir;   }

    public bool IsOnTheGround()         { return OnTheGround;   }
    public bool IsNearTheGround()       { return NearTheGround;   }
    public bool IsMovingOnUp()          { return MovingOnUp;   }
    public bool IsKeyboardRoll()        { return SpecifiedRoll;   }
    public bool IsManualRoll()          { return ManualRoll != 0f;   }
    public float ManualRollVal()        { return ManualRoll;   }

    public void OnFirePressed() {
        if( Airwing == null)  return;
        Airwing.GetComponent<Airwing>().Shoot();
    }

    public void SetAirwing(GameObject crft) {
        Airwing = crft;
    }

    public void SetRailDir(Transform dir) {
        //print("SetRailDir " + dir.transform.rotation.eulerAngles);

        // RailDirection = dir.transform.forward;
        // RailRotation = dir.transform.rotation;
        //
        // transform.rotation = RailRotation;
    }

    public Quaternion GetRailDir() {
        return RailRotation;
    }

    public Vector3 GetRailForward() {
        return RailDirection;
    }

    public void SetRailStartPos(GameObject rejuvinator)
    {
        //print("SetRailStartPos " + rejuvinator.transform.position);

        SetRailDir(rejuvinator.transform);

        CrossPos = new Vector2(0, 0);
    }


    public void FollowTarget()
    {
        //CheckFrustum();



        transform.position = Airwing.transform.position + ((-Cam.transform.forward).normalized * CamZOffset);

        transform.rotation = Airwing.transform.rotation;

      //  Vector3 newPos = Airwing.transform.position + (RailDirection * (CamZOffset + ZoomZOffset));

        // Based on Rail direction speed
        // Adjust Cam Distance(not used)
        // Vector3 AirwingVector = Airwing.transform.position + (RailDirection * (CamZOffset + ZoomZOffset)) + ((Airwing.GetComponent<Airwing>().rigid.velocity) * Time.deltaTime);
        // Vector3 CamToAirwing = AirwingVector - newPos;
        //
        // float CamAdjust = 0;
        // if( CamToAirwing.magnitude > CamFarLimit)
        // {
        //     CamAdjust = (CamFarLimit - CamToAirwing.magnitude);
        //     print("TOOFAR by " + CamAdjust);
        // }
        // else if( CamToAirwing.magnitude < CamNearLimit)
        // {
        //     CamAdjust = (CamNearLimit - CamToAirwing.magnitude);
        //     print("TOONEAR by " + CamAdjust);
        // }
        //
        // print(CamToAirwing.magnitude);
        // Vector3 NearFarAdjusted = (-CamToAirwing.normalized * CamAdjust);



        // transform.position = newPos + new Vector3(0, CamYOffset * CamYOffsetDir, 0); // + NearFarAdjusted;



         // NEAR GROUND CHECK
        if (Airwing.transform.position.y <= GroundRayDistance + 100f)
            NearTheGround = true;
        else
            NearTheGround = false;

        //RaycastHit hit;
        if (Airwing.transform.position.y <= GroundRayDistance) //Physics.Raycast(Airwing.transform.position, Vector3.down, out hit, GroundRayDistance))
        {
            // Vector3 incomingVec = hit.point - Airwing.transform.position;
            // Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            // reflectVec.Normalize();
            // reflectVec *= Airwing.GetComponent<Airwing>().rigid.velocity.magnitude;
            //
            // Vector3 AirwingToHit = Airwing.transform.position - hit.point;
            // float GroundRayDiff = GroundRayDistance - AirwingToHit.magnitude;
            //
            // // print("AirwingToHit " + AirwingToHit);
            // // print("GroundRayDiff " + GroundRayDiff);
            //
            //
            // float GroundRayMid = GroundRayDistance / 2;
            // Vector3 SurfaceTarget = hit.point + (RailDirection * AimDistance);
            // Vector3 AutoTarget = SurfaceTarget + (Vector3.up * GroundRayMid) + (RailDirection * 100f);
            //
            // Debug.DrawLine(hit.point, hit.point + (Vector3.up * GroundRayDistance), Color.green);
            // Debug.DrawLine(Airwing.transform.position, hit.point, Color.red);
            // Debug.DrawLine(Airwing.transform.position, SurfaceTarget, Color.yellow);
            // Debug.DrawLine(hit.point + (Vector3.up * GroundRayMid), AutoTarget, Color.blue);


            // GO ON THE GROUND
            if( !OnTheGround && !MovingOnUp)
            {
                Airwing.GetComponent<Airwing>().rigid.useGravity = false;
                Airwing.GetComponent<Airwing>().rigid.constraints = RigidbodyConstraints.FreezePositionY;

                VectorRigidBody( Airwing.GetComponent<Airwing>().rigid );

                OnTheGround = true;

                print("##### LANDED ON THE GROUND #####");
            }

            // Airwing.GetComponent<Airwing>().elevatorWing.WingOff();
            // Airwing.GetComponent<Airwing>().aileronLeftWing.WingOff();
            // Airwing.GetComponent<Airwing>().aileronRightWing.WingOff();
            // Airwing.GetComponent<Airwing>().rudderWing.WingOff();


            // if( !AutoPilotCrossHair.gameObject.active)
            //     AutoPilotCrossHair.gameObject.SetActive(true);
            //Debug.Break();


        }
        else
        {
            //Time.timeScale = 1f;
            //OnTheGround = false;
            //Airwing.GetComponent<Airwing>().rigid.constraints = RigidbodyConstraints.None;
        }


        // AUTOPILOT
        float AutoPilotUpBreakDist = 1f;
        if( MovingOnUp)
        {
            float UpAim = CrossPos.y;
            if( UpAim < 0) UpAim = 0;
            UpAim = UpAim + AutoPilotUpBreakDist;
            AutoPilotCrossHair.anchoredPosition = new Vector3(CrossPos.x, UpAim, 0);
        }
        else
            AutoPilotCrossHair.anchoredPosition = new Vector3(CrossPos.x, 0, 0);
        AutoPilotBar.anchoredPosition = new Vector3(0, AutoPilotUpBreakDist, 0);



        float RelativeAdjust = 1f;
        Vector2 CrossRelative = new Vector2(CrossPos.x/frustumWidth, CrossPos.y/frustumHeight);

        if(CrossRelative.magnitude < 0.1f)
            RelativeAdjust = 80f;
        else if(CrossRelative.magnitude < 0.2f)
            RelativeAdjust = 40f;
        else if(CrossRelative.magnitude < 0.3f)
            RelativeAdjust = 20f;

        if(CrossRelative.magnitude > 0.45f)
            RelativeAdjust = -25f;

        // float CrossRelativeX = CrossPos.x/frustumWidth;
        // if(CrossRelativeX < 0) CrossRelativeX *= -1f;
        // float CrossRelativeY = CrossPos.y/frustumHeight;
        // if(CrossRelativeY < 0) CrossRelativeY *= -1f;
        //
        // if(CrossRelativeX);
        //
        //print("RelativeAdjust " + RelativeAdjust + "  " + CrossRelative + " " + CrossRelative.magnitude);


        // Height Limit
        if( Airwing.transform.position.y > 50000f && CrossPos.y > 0)
        {
            print ("***** HEIGHT LIMIT *****");
            AimPos = AutoPilotCrossHair.transform.position;
        }
        // Ground Limit
        else if( OnTheGround)
            AimPos = AutoPilotCrossHair.transform.position;
        else if( MovingOnUp)
            AimPos = AutoPilotCrossHair.transform.position;
        else
            AimPos = CrossHair.transform.position + (RelativeAdjust * RailDirection);

        AimPosObject.transform.position = AimPos;
        AimPosObject.transform.rotation = Quaternion.Euler(0, 0, AimZAngle);



        // GO OFF THE GROUND
        if( OnTheGround && CrossPos.y > AutoPilotUpBreakDist)
        {
            print("^___^  AUTOPILOT UP BREAK  ^____^");
            Airwing.GetComponent<Airwing>().rigid.useGravity = true;
            Airwing.GetComponent<Airwing>().rigid.constraints = RigidbodyConstraints.None;

            OnTheGround = false;
            MovingOnUp = true;
        }

        // ONCE OFF THE GROUND
        if( MovingOnUp)
            if (Airwing.transform.position.y > GroundRayDistance)
            {
                print("^^^^^ LIFTED UP OFF GROUND ^^^^^");
                MovingOnUp = false;
            }

    }


    void PerformInputs()
    {

        AimZAngle = 0;
        AimZDir = 1f;
        SpecifiedRoll = false;
        ManualRoll = 0f;


        // PC INPUT
        if( !Game.I.MobileMode())
        {
            // MOUSE
            //CrossPos += new Vector2(Input.GetAxis("Mouse X") * AimSpeed, Input.GetAxis("Mouse Y") * AimSpeed);
            float mX = (Input.mousePosition.x - (Screen.width/2f)) * frustumScaleW;
            float mY = (Input.mousePosition.y - (Screen.height/2f)) * frustumScaleH;
            CrossPos = new Vector2(mX, mY);

            CrossRayPos = Input.mousePosition;


            // MANUAL ROLL
            // Q
            if( Input.GetKey(KeyCode.Q))
            {
                ManualRoll = -1.5f;
            }
            // E
            else if( Input.GetKey(KeyCode.E))
            {
                ManualRoll = 1.5f;
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



            // BOOST
            if( Input.GetKey(KeyCode.Space) && BoostTime > BoostMinTime)
                if( !BoostOn)
                    OnBoostPressed();

            if( Input.GetKeyUp(KeyCode.Space))
                OffBoost();

            // FIRE
            if ( Input.GetButton("Fire1") || Input.GetButtonDown("Fire1") )
            {
                Game.I.LockCursor();
                OnFirePressed();
            }

            if (Input.GetButtonUp("Fire1"))
                BoomBox.I.SoundWeapon.Stop();

            // Zoom
            if (Input.GetButtonDown("Fire2"))
            {
                print("ZOOM");
                if(ZoomZOffset != 1f)
                    ZoomZOffset = 1f;
                else
                    ZoomZOffset = -35f;
            }


        }
        else
        {
            // ANDROID MULTITOUCH
            Vector2 NewCrossPos = new Vector2(CrossPos.x, CrossPos.y);
            int Touches = Input.touchCount;
            for ( int i = 0 ; i < Touches ; i++ ) {

                Vector2 TheTouch = Input.GetTouch(i).position;


                // X Button
                if( TheTouch.x > MinButtonX.x && TheTouch.x <= MaxButtonX.x && TheTouch.y > MinButtonX.y && TheTouch.y <= MaxButtonX.y)
                    OnFirePressed();
                // XArea Button
                else if( TheTouch.x > MinButtonXArea.x && TheTouch.x <= MaxButtonXArea.x && TheTouch.y > MinButtonXArea.y && TheTouch.y <= MaxButtonXArea.y) {
                    /*no event*/   }
                else
                {
                    // AIM TOUCH, update cross pos
                    if(LeftSideControls)
                        TheTouch.x -= 300f;
                    else
                        TheTouch.x += 300f;

                    CrossRayPos = TheTouch;

                    NewCrossPos.x = (((TheTouch.x) - Screen.width/2) / Screen.width) * frustumWidth;
                    NewCrossPos.y = (((TheTouch.y + 50f) - Screen.height/2) / Screen.height) * frustumHeight;
                }
            }


            CrossPos = NewCrossPos;
            CrossRayPos = NewCrossPos;

        }

        // Boost
        if( BoostOn)
            BoostTime -= Time.deltaTime;
        else
        {
            BoostTime += Time.deltaTime / 1.5f;
            if( BoostTime > BoostMaxTime)
                BoostTime = BoostMaxTime;
        }

        if( BoostTime <= 0)
            OffBoost();


        // Limits
        if( CrossPos.y > limitTop)          CrossPos.y = limitTop;
        else if( CrossPos.y < limitBottom)  CrossPos.y = limitBottom;
        if( CrossPos.x > limitRight)        CrossPos.x = limitRight;
        else if( CrossPos.x < limitLeft)    CrossPos.x = limitLeft;

        // Place cross on scaled frustum canvas
        CrossHair.anchoredPosition = new Vector3(CrossPos.x, CrossPos.y, 0);


        // Aim Ray
        crossRay = Camera.main.ScreenPointToRay( CrossRayPos );
        Vector3 start = Camera.main.transform.position;
        Vector3 end = crossRay.GetPoint(crossRayDist);

        int layerMask = 1 << 0; // Default layer
        if (Physics.Raycast( crossRay, out crossHit, crossRayDist, layerMask))
        {
            TargetPosObject.transform.position = crossHit.point;
            //Debug.DrawLine(start, crossHit.point, Color.green);
        }
        else
        {
            TargetPosObject.transform.position = end;
            //Debug.DrawLine(start, end, Color.green);
        }
    }

    void OnBoostPressed()
    {
        print("#####  BOOST ON  #####");
        Airwing.GetComponent<Airwing>().EngineHigh();
        BoostOn = true;
    }

    void OffBoost()
    {
        print("#####  BOOST OFF  #####");
        Airwing.GetComponent<Airwing>().EngineMid();
        BoostOn = false;
    }




    void VectorRigidBody(Rigidbody rigid)
	{
            Vector3 SaveVelocity = rigid.velocity;
            Vector3 SaveAngularVelocity = rigid.angularVelocity;
            //print("BeforeVelocity: " + rigid.velocity);
            rigid.isKinematic = true;

            // "Surface Vectoring"
            SaveVelocity.y = 0f;
            rigid.position = new Vector3(rigid.position.x, GroundRayDistance, rigid.position.z);
            rigid.rotation = Quaternion.LookRotation(SaveVelocity);

            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            Airwing.GetComponent<Airwing>().elevator.Deflection = 0;
            Airwing.GetComponent<Airwing>().aileronLeft.Deflection = 0;
            Airwing.GetComponent<Airwing>().aileronRight.Deflection = 0;
            Airwing.GetComponent<Airwing>().rudder.Deflection = 0;

            //print("AfterVelocity: " + rigid.velocity);
            rigid.isKinematic = false;
            rigid.AddForce( SaveVelocity, ForceMode.VelocityChange);
             // zero the torque
            //rigid.AddTorque( SaveAngularVelocity, ForceMode.VelocityChange);
	}

	void MoveRigidBodyToHeight(Rigidbody rigid, float height)
	{
		Vector3 SaveVelocity = rigid.velocity;
		Vector3 SaveAngularVelocity = rigid.angularVelocity;
		rigid.isKinematic = true;

        rigid.position = new Vector3(rigid.position.x, height, rigid.position.z);

		rigid.velocity = Vector3.zero;
		rigid.angularVelocity = Vector3.zero;

		rigid.isKinematic = false;
		rigid.AddForce( SaveVelocity, ForceMode.VelocityChange);
		rigid.AddTorque( SaveAngularVelocity, ForceMode.VelocityChange);
	}

    void SetButtonBoundaries()
    {
        float leftX = 0;
        RectTransform rect;
        // X Button
        rect = ButtonX.GetComponent<RectTransform>();
        leftX = Screen.width + rect.anchoredPosition.x;
        SizeButtonX = rect.sizeDelta;
        if(LeftSideControls) rect.anchoredPosition = new Vector2(-leftX + SizeButtonX.x, rect.anchoredPosition.y);
        PosButtonX = rect.anchoredPosition;
        MinButtonX = new Vector2( Screen.width + PosButtonX.x - (SizeButtonX.x),   (PosButtonX.y) );
        MaxButtonX = new Vector2( Screen.width + PosButtonX.x,                      PosButtonX.y + (SizeButtonX.y) );
        // XArea Button
        rect = ButtonXArea.GetComponent<RectTransform>();
        leftX = Screen.width + rect.anchoredPosition.x;
        SizeButtonXArea = rect.sizeDelta;
        if(LeftSideControls) rect.anchoredPosition = new Vector2(-leftX + SizeButtonXArea.x, rect.anchoredPosition.y);
        PosButtonXArea = rect.anchoredPosition;
        MinButtonXArea = new Vector2( Screen.width + PosButtonXArea.x - (SizeButtonXArea.x),   (PosButtonXArea.y) );
        MaxButtonXArea = new Vector2( Screen.width + PosButtonXArea.x,                      PosButtonXArea.y + (SizeButtonXArea.y) );
    }


    public void CheckFrustum()
    {
        // if (lastScreenW != Screen.width || lastScreenH != Screen.height)
        // {
            SetButtonBoundaries();

            var frustumShrink = 1f; // % of screen

            lastScreenW = Screen.width;
            lastScreenH = Screen.height;

            frustumHeight = 2.0f * AimScreen.transform.localPosition.z * Mathf.Tan(Cam.GetComponent<Camera>().fieldOfView * 0.5f * Mathf.Deg2Rad);
            frustumWidth = frustumHeight * Cam.GetComponent<Camera>().aspect;

            frustumScaleH = frustumHeight / origScreenH;
            frustumScaleW = frustumWidth / origScreenW;

            AimScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(frustumWidth * frustumShrink, frustumHeight * frustumShrink);

            limitTop = (frustumHeight * frustumShrink) / 2;
            limitBottom = -limitTop;

            limitRight = (frustumWidth * frustumShrink) / 2;
            limitLeft = -limitRight;

        // }
    }


    void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 1);
	}


}
