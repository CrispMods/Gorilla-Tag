using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000322 RID: 802
public class ObjectManipulator : MonoBehaviour
{
	// Token: 0x06001313 RID: 4883 RVA: 0x000B54A8 File Offset: 0x000B36A8
	private void Start()
	{
		if (this.passthrough)
		{
			this.passthrough.colorMapEditorBrightness = -1f;
			this.passthrough.colorMapEditorContrast = -1f;
		}
		base.StartCoroutine(this.StartDemo());
		if (this.objectNameLabel)
		{
			this.objectNameLabel.font.material.renderQueue = 4600;
		}
		if (this.objectInstructionsLabel)
		{
			this.objectInstructionsLabel.font.material.renderQueue = 4600;
		}
		if (this.objectInfoBG)
		{
			this.objectInfoBG.materialForRendering.renderQueue = 4599;
		}
	}

	// Token: 0x06001314 RID: 4884 RVA: 0x000B5560 File Offset: 0x000B3760
	private void Update()
	{
		Vector3 localControllerPosition = OVRInput.GetLocalControllerPosition(this.controller);
		Quaternion localControllerRotation = OVRInput.GetLocalControllerRotation(this.controller);
		this.FindHoverObject(localControllerPosition, localControllerRotation);
		if (this.hoverObject && OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, this.controller))
		{
			this.grabObject = this.hoverObject;
			this.GrabHoverObject(this.grabObject, localControllerPosition, localControllerRotation);
		}
		if (this.grabObject)
		{
			this.grabTime += Time.deltaTime * 5f;
			this.grabTime = Mathf.Clamp01(this.grabTime);
			this.ManipulateObject(this.grabObject, localControllerPosition, localControllerRotation);
			if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, this.controller))
			{
				this.ReleaseObject();
				return;
			}
		}
		else
		{
			this.grabTime -= Time.deltaTime * 5f;
			this.grabTime = Mathf.Clamp01(this.grabTime);
		}
	}

	// Token: 0x06001315 RID: 4885 RVA: 0x000B564C File Offset: 0x000B384C
	private void GrabHoverObject(GameObject grbObj, Vector3 controllerPos, Quaternion controllerRot)
	{
		this.localGrabOffset = Quaternion.Inverse(controllerRot) * (this.grabObject.transform.position - controllerPos);
		this.localGrabRotation = Quaternion.Inverse(controllerRot) * this.grabObject.transform.rotation;
		this.rotationOffset = 0f;
		if (this.grabObject.GetComponent<GrabObject>())
		{
			this.grabObject.GetComponent<GrabObject>().Grab(this.controller);
			this.grabObject.GetComponent<GrabObject>().grabbedRotation = this.grabObject.transform.rotation;
			this.AssignInstructions(this.grabObject.GetComponent<GrabObject>());
		}
		this.handGrabPosition = controllerPos;
		this.handGrabRotation = controllerRot;
		this.camGrabPosition = Camera.main.transform.position;
		this.camGrabRotation = Camera.main.transform.rotation;
	}

	// Token: 0x06001316 RID: 4886 RVA: 0x000B5740 File Offset: 0x000B3940
	private void ReleaseObject()
	{
		if (this.grabObject.GetComponent<GrabObject>())
		{
			if (this.grabObject.GetComponent<GrabObject>().objectManipulationType == GrabObject.ManipulationType.Menu)
			{
				this.grabObject.transform.position = this.handGrabPosition + this.handGrabRotation * this.localGrabOffset;
				this.grabObject.transform.rotation = this.handGrabRotation * this.localGrabRotation;
			}
			this.grabObject.GetComponent<GrabObject>().Release();
		}
		this.grabObject = null;
	}

	// Token: 0x06001317 RID: 4887 RVA: 0x0003D05C File Offset: 0x0003B25C
	private IEnumerator StartDemo()
	{
		this.demoObjects.SetActive(false);
		float timer = 0f;
		float fadeTime = 1f;
		while (timer <= fadeTime)
		{
			timer += Time.deltaTime;
			float t = Mathf.Clamp01(timer / fadeTime);
			if (this.passthrough)
			{
				this.passthrough.colorMapEditorBrightness = Mathf.Lerp(-1f, 0f, t);
				this.passthrough.colorMapEditorContrast = Mathf.Lerp(-1f, 0f, t);
			}
			yield return null;
		}
		this.demoObjects.SetActive(true);
		Vector3 normalized = new Vector3(Camera.main.transform.forward.x, 0f, Camera.main.transform.forward.z).normalized;
		this.demoObjects.transform.position = Camera.main.transform.position + normalized;
		this.demoObjects.transform.rotation = Quaternion.LookRotation(normalized);
		yield break;
	}

	// Token: 0x06001318 RID: 4888 RVA: 0x000B57D8 File Offset: 0x000B39D8
	private void FindHoverObject(Vector3 controllerPos, Quaternion controllerRot)
	{
		RaycastHit[] array = Physics.RaycastAll(controllerPos, controllerRot * Vector3.forward);
		float num = float.PositiveInfinity;
		float d = 2f;
		bool enabled = true;
		Vector3 vector = Vector3.zero;
		foreach (RaycastHit raycastHit in array)
		{
			float num2 = Vector3.Distance(raycastHit.point, controllerPos);
			if (num2 < num)
			{
				this.hoverObject = raycastHit.collider.gameObject;
				num = num2;
				d = (this.grabObject ? num2 : (num2 - 0.1f));
				vector = raycastHit.point;
			}
		}
		if (array.Length == 0)
		{
			this.hoverObject = null;
		}
		foreach (Collider collider in Physics.OverlapSphere(controllerPos, 0.05f))
		{
			this.hoverObject = collider.gameObject;
			enabled = false;
			vector = collider.ClosestPoint(controllerPos);
			vector += (Camera.main.transform.position - vector).normalized * 0.05f;
		}
		if (this.objectInfo && this.objectInstructionsLabel)
		{
			bool active = this.hoverObject || this.grabObject;
			this.objectInfo.gameObject.SetActive(active);
			Vector3 forward = vector - Camera.main.transform.position;
			if (this.hoverObject && !this.grabObject)
			{
				Vector3 a = vector - forward.normalized * 0.05f;
				this.objectInfo.position = Vector3.Lerp(a, this.objectInfo.position, this.grabTime);
				this.objectInfo.rotation = Quaternion.LookRotation(forward);
				this.objectInfo.localScale = Vector3.one * forward.magnitude * 2f;
				if (this.hoverObject.GetComponent<GrabObject>())
				{
					this.AssignInstructions(this.hoverObject.GetComponent<GrabObject>());
				}
			}
			else if (this.grabObject)
			{
				Vector3 b = controllerPos + (Camera.main.transform.position - controllerPos).normalized * 0.1f;
				this.objectInfo.position = Vector3.Lerp(this.objectInfo.position, b, this.grabTime);
				this.objectInfo.rotation = Quaternion.LookRotation(this.objectInfo.position - Camera.main.transform.position);
				this.objectInfo.localScale = Vector3.one;
				if (this.grabObject.GetComponent<GrabObject>())
				{
					enabled = this.grabObject.GetComponent<GrabObject>().showLaserWhileGrabbed;
				}
			}
		}
		if (this.laser)
		{
			this.laser.positionCount = 2;
			Vector3 position = controllerPos + controllerRot * (Vector3.forward * 0.05f);
			this.cursorPosition = controllerPos + controllerRot * (Vector3.forward * d);
			this.laser.SetPosition(0, position);
			this.laser.SetPosition(1, this.cursorPosition);
			this.laser.enabled = enabled;
			if (this.grabObject && this.grabObject.GetComponent<GrabObject>())
			{
				this.grabObject.GetComponent<GrabObject>().CursorPos(this.cursorPosition);
			}
		}
	}

	// Token: 0x06001319 RID: 4889 RVA: 0x000B5B9C File Offset: 0x000B3D9C
	private void ManipulateObject(GameObject obj, Vector3 controllerPos, Quaternion controllerRot)
	{
		bool flag = true;
		Vector2 vector = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, this.controller);
		if (obj.GetComponent<GrabObject>())
		{
			flag = false;
			switch (obj.GetComponent<GrabObject>().objectManipulationType)
			{
			case GrabObject.ManipulationType.Default:
				flag = true;
				break;
			case GrabObject.ManipulationType.ForcedHand:
				obj.transform.position = Vector3.Lerp(obj.transform.position, controllerPos, this.grabTime);
				obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, controllerRot, this.grabTime);
				break;
			case GrabObject.ManipulationType.DollyHand:
			{
				float num = this.localGrabOffset.z + vector.y * 0.01f;
				num = Mathf.Clamp(num, 0.1f, 2f);
				this.localGrabOffset = Vector3.forward * num;
				obj.transform.position = Vector3.Lerp(obj.transform.position, controllerPos + controllerRot * this.localGrabOffset, this.grabTime);
				obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, controllerRot, this.grabTime);
				break;
			}
			case GrabObject.ManipulationType.DollyAttached:
				obj.transform.position = controllerPos + controllerRot * this.localGrabOffset;
				obj.transform.rotation = controllerRot * this.localGrabRotation;
				this.ClampGrabOffset(ref this.localGrabOffset, vector.y);
				break;
			case GrabObject.ManipulationType.HorizontalScaled:
			{
				obj.transform.position = controllerPos + controllerRot * this.localGrabOffset;
				if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, this.controller))
				{
					obj.transform.localScale = this.ClampScale(obj.transform.localScale, vector);
				}
				else
				{
					this.rotationOffset -= vector.x;
					this.ClampGrabOffset(ref this.localGrabOffset, vector.y);
				}
				Vector3 vector2 = obj.GetComponent<GrabObject>().grabbedRotation * Vector3.forward;
				vector2 = new Vector3(vector2.x, 0f, vector2.z);
				obj.transform.rotation = Quaternion.Euler(0f, this.rotationOffset, 0f) * Quaternion.LookRotation(vector2);
				break;
			}
			case GrabObject.ManipulationType.VerticalScaled:
			{
				obj.transform.position = controllerPos + controllerRot * this.localGrabOffset;
				if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, this.controller))
				{
					obj.transform.localScale = this.ClampScale(obj.transform.localScale, vector);
				}
				else
				{
					this.rotationOffset -= vector.x;
					this.ClampGrabOffset(ref this.localGrabOffset, vector.y);
				}
				Vector3 vector3 = obj.GetComponent<GrabObject>().grabbedRotation * Vector3.up;
				vector3 = new Vector3(vector3.x, 0f, vector3.z);
				obj.transform.rotation = Quaternion.LookRotation(Vector3.up, Quaternion.Euler(0f, this.rotationOffset, 0f) * vector3);
				break;
			}
			case GrabObject.ManipulationType.Menu:
			{
				Vector3 vector4 = this.handGrabPosition + this.handGrabRotation * Vector3.forward * 0.4f;
				Quaternion b = Quaternion.LookRotation(vector4 - this.camGrabPosition);
				obj.transform.position = Vector3.Lerp(obj.transform.position, vector4, this.grabTime);
				obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation, b, this.grabTime);
				break;
			}
			default:
				flag = true;
				break;
			}
		}
		if (flag)
		{
			obj.transform.position = controllerPos + controllerRot * this.localGrabOffset;
			obj.transform.Rotate(Vector3.up * -vector.x);
			this.ClampGrabOffset(ref this.localGrabOffset, vector.y);
		}
	}

	// Token: 0x0600131A RID: 4890 RVA: 0x000B5FB0 File Offset: 0x000B41B0
	private void ClampGrabOffset(ref Vector3 localOffset, float thumbY)
	{
		Vector3 vector = localOffset + Vector3.forward * thumbY * 0.01f;
		if (vector.z > 0.1f)
		{
			localOffset = vector;
		}
	}

	// Token: 0x0600131B RID: 4891 RVA: 0x000B5FF4 File Offset: 0x000B41F4
	private Vector3 ClampScale(Vector3 localScale, Vector2 thumb)
	{
		float num = localScale.x + thumb.x * 0.01f;
		if (num <= 0.1f)
		{
			num = 0.1f;
		}
		float num2 = localScale.z + thumb.y * 0.01f;
		if (num2 <= 0.1f)
		{
			num2 = 0.1f;
		}
		return new Vector3(num, 0f, num2);
	}

	// Token: 0x0600131C RID: 4892 RVA: 0x000B6054 File Offset: 0x000B4254
	private void CheckForDominantHand()
	{
		if (this.hoverObject || this.grabObject)
		{
			return;
		}
		if (this.controller == OVRInput.Controller.RTouch)
		{
			if (OVRInput.Get(OVRInput.RawButton.LHandTrigger, OVRInput.Controller.Active))
			{
				this.controller = OVRInput.Controller.LTouch;
				return;
			}
		}
		else if (OVRInput.Get(OVRInput.RawButton.RHandTrigger, OVRInput.Controller.Active))
		{
			this.controller = OVRInput.Controller.RTouch;
		}
	}

	// Token: 0x0600131D RID: 4893 RVA: 0x000B60B8 File Offset: 0x000B42B8
	private void AssignInstructions(GrabObject targetObject)
	{
		if (this.objectNameLabel)
		{
			this.objectNameLabel.text = targetObject.ObjectName;
		}
		if (this.objectInstructionsLabel)
		{
			if (this.grabObject)
			{
				this.objectInstructionsLabel.text = targetObject.ObjectInstructions;
				return;
			}
			this.objectInstructionsLabel.text = "Grip Trigger to grab";
		}
	}

	// Token: 0x0400150A RID: 5386
	private OVRInput.Controller controller = OVRInput.Controller.RTouch;

	// Token: 0x0400150B RID: 5387
	private GameObject hoverObject;

	// Token: 0x0400150C RID: 5388
	private GameObject grabObject;

	// Token: 0x0400150D RID: 5389
	private float grabTime;

	// Token: 0x0400150E RID: 5390
	private Vector3 localGrabOffset = Vector3.zero;

	// Token: 0x0400150F RID: 5391
	private Quaternion localGrabRotation = Quaternion.identity;

	// Token: 0x04001510 RID: 5392
	private Vector3 camGrabPosition = Vector3.zero;

	// Token: 0x04001511 RID: 5393
	private Quaternion camGrabRotation = Quaternion.identity;

	// Token: 0x04001512 RID: 5394
	private Vector3 handGrabPosition = Vector3.zero;

	// Token: 0x04001513 RID: 5395
	private Quaternion handGrabRotation = Quaternion.identity;

	// Token: 0x04001514 RID: 5396
	private Vector3 cursorPosition = Vector3.zero;

	// Token: 0x04001515 RID: 5397
	private float rotationOffset;

	// Token: 0x04001516 RID: 5398
	public LineRenderer laser;

	// Token: 0x04001517 RID: 5399
	public Transform objectInfo;

	// Token: 0x04001518 RID: 5400
	public TextMesh objectNameLabel;

	// Token: 0x04001519 RID: 5401
	public TextMesh objectInstructionsLabel;

	// Token: 0x0400151A RID: 5402
	public Image objectInfoBG;

	// Token: 0x0400151B RID: 5403
	public GameObject demoObjects;

	// Token: 0x0400151C RID: 5404
	public OVRPassthroughLayer passthrough;
}
