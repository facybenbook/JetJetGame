using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Airplane : MonoBehaviour
{
    [Header("Forces")]
    public float thrust = 100f;
    public Vector3 turnTorque = new Vector3(90f, 25f, 45f);
    public float forceMult = 1000f;

    [Header("Agressive Bank")]
    public float aggressiveBankAngle = 10f;

    [Header("Input")]
    float pitch = 0f;
    float yaw = 0f;
    float roll = 0f;

    protected Vector3 AimPos = new Vector3(0, 100000f, 0);

    protected Rigidbody rigid;


    //

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public virtual void Update() { }


    public void UpdateTick()
    {
        RunAutopilot(AimPos, out yaw, out pitch, out roll);
    }

    //

    void RunAutopilot(Vector3 flyTarget, out float yaw, out float pitch, out float roll)
    {
        var localFlyTarget = transform.InverseTransformPoint(flyTarget).normalized;
        var angleOffTarget = Vector3.Angle(transform.forward, flyTarget - transform.position);

        yaw = Mathf.Clamp(localFlyTarget.x, -1f, 1f);
        pitch = -Mathf.Clamp(localFlyTarget.y, -1f, 1f);

        var agressiveRoll = Mathf.Clamp(localFlyTarget.x, -1f, 1f);

        var wingsLevelRoll = transform.right.y;

        var wingsLevelInfluence = Mathf.InverseLerp(0f, aggressiveBankAngle, angleOffTarget);

        roll = Mathf.Lerp(wingsLevelRoll, agressiveRoll, wingsLevelInfluence);
    }

    void FixedUpdate()
    {
        rigid.AddRelativeForce(Vector3.forward * thrust * forceMult, ForceMode.Force);
        rigid.AddRelativeTorque(new Vector3(turnTorque.x * pitch,
                                            turnTorque.y * yaw,
                                            -turnTorque.z * roll) * forceMult, ForceMode.Force);
    }

    void OnTriggerEnter(Collider collision)
    {
        //print(" " + collision.tag);

        // if(collision.tag == "AimHUD_Cull")
        // {
        //     //print("Cull " + gameObject.name);
        //     GameObject.Destroy(gameObject);
        // }
    }

    public void Death() {

        GameObject explosionObject = (GameObject)Instantiate(Resources.Load("Explosion/1Explosion"), transform.position, transform.rotation);
        GameObject.Destroy(explosionObject, 4f);

        GameObject.Destroy(gameObject);
    }


}
