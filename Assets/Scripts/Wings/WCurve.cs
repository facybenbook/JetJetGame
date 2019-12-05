using UnityEngine;

[CreateAssetMenu(fileName = "New Wing Curves", menuName = "Wing Curve", order = 99)]
public class WCurve : ScriptableObject
{
	[TextArea]
	public string description;

	[SerializeField]
	[Tooltip("Lift curve by angle of attack. X axis should be from 0 to 180, with the Y axis being lift coeffient.")]
	private AnimationCurve lift = new AnimationCurve(new Keyframe(0.0f, 0.0f),
		new Keyframe(16.0f, 1.1f),
		new Keyframe(20.0f, 0.6f),
		new Keyframe(135.0f, -1.0f),
		new Keyframe(160.0f, -0.6f),
		new Keyframe(164.0f, -1.1f),
		new Keyframe(180.0f, 0.0f));

	[SerializeField]
	[Tooltip("Drag curve by angle of attack. X axis should be from 0 to 180, with the Y axis being drag coeffient.")]
	private AnimationCurve drag = new AnimationCurve(new Keyframe(0.0f, 0.025f),
		new Keyframe(90.0f, 1.0f),
		new Keyframe(180.0f, 0.025f));

	public float GetLiftAtAaoA(float aoa)
	{
		return lift.Evaluate(aoa);
	}

	public float GetDragAtAaoA(float aoa)
	{
		return drag.Evaluate(aoa);
	}

	public void SetLiftCurve(Keyframe[] newCurve)
	{
		lift.keys = newCurve;
	}
}
