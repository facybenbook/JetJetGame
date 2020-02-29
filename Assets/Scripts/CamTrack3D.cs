using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.EventSystems;

public class CamTrack3D : MonoBehaviour
{
    [Header("Camera")]
    public GameObject Cam;

    [Header("Airwing")]
    public GameObject Airwing = null;

    [Header("Options")]
    public float AimDistance = 25f;
    public float CamDistance = 26f;
    public float CamUpDistance = 7f;

    [Header("CrossHaar")]
    public GameObject CrossHaar;
    public GameObject CrossBox;

    public Vector3 BoresightPos
    {get{
        if( Airwing == null) return new Vector3(0, 0, 0);
        else return Airwing.transform.position + (Airwing.transform.forward * AimDistance);
    }}

    public Vector3 HeadAimPos
    {get{
        if( Airwing == null) return new Vector3(0, 0, 0);
        else
        {

            // INTERCEPT HERE TO RESTRICT RANGE OF MOTION TO ONLY FORWARD

            Vector3 dir = new Vector3(0, 0, 0);

            float YawOffStart = transform.rotation.eulerAngles.y;

            //Debug.Log("YawOffStart " + YawOffStart);
            // if( YawOffStart > 90f && YawOffStart <= 180)
            // {
            //     Debug.Log("YawOffStart < 90 && <= 180");
            //     a = (YawOffStart - 90) * Mathf.Deg2Rad;
            //     dir =  new Vector3(0, Mathf.Sin(a), 0).normalized;
            // }
            // else if( YawOffStart > 180f && YawOffStart <= 270)
            // {
            //     Debug.Log("YawOffStart < 270 && > 180");
            //     a = (YawOffStart - 270) * Mathf.Deg2Rad;
            //     dir =  new Vector3(0, Mathf.Sin(a), 0).normalized;
            // }


            return Airwing.transform.position + ((Cam.transform.forward + dir).normalized * AimDistance);
        }
    }}



    private void Start()
    {
        if( Game.I.MobileMode())
        {
            Input.gyro.enabled = true;
            Input.compensateSensors = true;
        }
    }

    private void Update()
    {
        if (!Game.I.IsPlayingGame() || Cam == null || Airwing == null || Airwing.GetComponent<Airwing>() == null)
            return;

        if( Game.I.MobileMode())
            CameraPosMobile();
        else
            CameraPos();

        Cross();
    }

    private void CameraPos()
    {
        // (1:1 ie: 1 head turn = 360 rotation of Airwing)
        // (2:1 ie: 1 head turn = 760 rotation of Airwing)
        // (3:1 ie: 1 head turn = 1080 rotation of Airwing)
        int sensitivity = 1;

        if( sensitivity == 2)
            transform.rotation = Quaternion.AngleAxis( Cam.transform.localRotation.eulerAngles.y, new Vector3(0, 1f, 0) );
        else if( sensitivity == 3)
            transform.rotation = Quaternion.AngleAxis( Cam.transform.localRotation.eulerAngles.y, new Vector3(0, 1f, 0) ) *
                                 Quaternion.AngleAxis( Cam.transform.localRotation.eulerAngles.y, new Vector3(0, 1f, 0) );

        transform.position = Airwing.transform.position + (-Cam.transform.forward * CamDistance) + (Cam.transform.up * CamUpDistance);
    }


    private void CameraPosMobile()
    {
        // https://developers.google.com/vr/develop/unity/guides/magic-window
        transform.rotation =  Quaternion.Euler (90f, 0, 0) * Input.gyro.attitude * Quaternion.Euler (0, 0, 180f);

        transform.position = Airwing.transform.position;
    }

    private void Cross()
    {
        CrossHaar.transform.position = BoresightPos;
        CrossHaar.transform.rotation = Quaternion.LookRotation( -(BoresightPos - Cam.transform.position).normalized );
    }

    public void SetAirwing(GameObject crft) {
        Airwing = crft;
    }


    public void ReCenter()
    {

        // Debug.Log("RECENTER");
        //
        // transform.rotation = Quaternion.identity;
        //
        // #if UNITY_EDITOR || UNITY_STANDALONE
        //     Debug.Log("GvrEditorEmulator.Instance.Recenter()");
        //     GvrEditorEmulator.Instance.Recenter();
        // #else
        //     if( Game.I.MobileMode())
        //         Debug.Log("Recenter not implemented on mobile standard");
        //     else
        //     {
        //         Debug.Log("GvrCardboardHelpers.Recenter()");
        //         GvrCardboardHelpers.Recenter();
        //
        //     }
        // #endif
    }

}
