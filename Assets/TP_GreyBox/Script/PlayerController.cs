using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public enum CONTROL
	{
		WORLD_AXIS,
		WORLD_ROTATION,
		CAMERA_AXIS,		
	}

	public float speed = 5.0f;
	public float angularSpeed = 360.0f;
	public CONTROL control = CONTROL.WORLD_ROTATION;

	private float rotation = 0.0f;

	Rigidbody _rigidbody = null;
	protected bool IsActive { get; private set; }

	public void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
    {
		Vector3 direction = Vector3.zero;
		switch(control)
		{
			case CONTROL.WORLD_AXIS:
			case CONTROL.CAMERA_AXIS:
				Vector3 forward = Vector3.forward;
				Vector3 right = Vector3.right;
				if(control == CONTROL.CAMERA_AXIS)
				{
					forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
					if (forward.magnitude < 0.01f)
					{
						forward = Camera.main.transform.up;
					}
					forward = forward.normalized;
					right = Vector3.Cross(forward, Vector3.down);
				}
				direction += Input.GetAxisRaw("Horizontal") * right;
				direction += Input.GetAxisRaw("Vertical") * forward;
				direction.Normalize();

				rotation = Vector3.SignedAngle(Vector3.left, direction, Vector3.up);
				break;
			case CONTROL.WORLD_ROTATION:
				rotation += Input.GetAxisRaw("Horizontal") * angularSpeed * Time.deltaTime;
				direction = new Vector3(Mathf.Cos(-(rotation + 180) * Mathf.Deg2Rad), 0, Mathf.Sin(-(rotation + 180) * Mathf.Deg2Rad));
				direction *= Input.GetAxisRaw("Vertical");
				direction.Normalize();
				break;

		}
		_rigidbody.rotation = Quaternion.Euler(0, rotation, 0);
		_rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
	}
}
