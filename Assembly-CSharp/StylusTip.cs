using System;
using UnityEngine;

// Token: 0x02000361 RID: 865
public class StylusTip : MonoBehaviour
{
	// Token: 0x06001416 RID: 5142 RVA: 0x00062608 File Offset: 0x00060808
	private void Awake()
	{
		this.m_controller = ((this.m_handedness == OVRInput.Handedness.LeftHanded) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);
		this.m_breadCrumbContainer = new GameObject(string.Format("BreadCrumbContainer ({0})", this.m_handedness));
		this.m_breadCrumbs = new GameObject[60];
		for (int i = 0; i < this.m_breadCrumbs.Length; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.m_breadCrumbPf, this.m_breadCrumbContainer.transform);
			gameObject.name = string.Format("BreadCrumb ({0})", i);
			gameObject.SetActive(false);
			this.m_breadCrumbs[i] = gameObject;
		}
	}

	// Token: 0x06001417 RID: 5143 RVA: 0x000626A8 File Offset: 0x000608A8
	private void Update()
	{
		Pose pose = new Pose(OVRInput.GetLocalControllerPosition(this.m_controller), OVRInput.GetLocalControllerRotation(this.m_controller));
		Pose transformedBy = pose.GetTransformedBy(this.m_trackingSpace);
		Pose transformedBy2 = StylusTip.GetT_Device_StylusTip(this.m_controller).GetTransformedBy(transformedBy);
		base.transform.SetPositionAndRotation(transformedBy2.position, transformedBy2.rotation);
		float num = OVRInput.Get(OVRInput.Axis1D.PrimaryStylusForce, this.m_controller);
		bool flag = num > 0f;
		GameObject gameObject = this.m_breadCrumbs[this.m_breadCrumbIndexCurr];
		gameObject.transform.position = base.transform.position;
		float num2 = Mathf.Lerp(0.005f, 0.02f, num);
		gameObject.transform.localScale = new Vector3(num2, num2, num2);
		gameObject.GetComponent<MeshRenderer>().material.color = Color.Lerp(Color.white, Color.red, num);
		gameObject.SetActive(flag);
		float num3 = 0f;
		float num4 = float.PositiveInfinity;
		if (this.m_breadCrumbIndexPrev >= 0)
		{
			num4 = (base.transform.position - this.m_breadCrumbs[this.m_breadCrumbIndexPrev].transform.position).magnitude;
			num3 = (num2 + this.m_breadCrumbs[this.m_breadCrumbIndexPrev].transform.localScale.x) * 0.5f;
		}
		if (flag && num4 >= num3)
		{
			this.m_breadCrumbIndexPrev = this.m_breadCrumbIndexCurr;
			this.m_breadCrumbIndexCurr = (this.m_breadCrumbIndexCurr + 1) % this.m_breadCrumbs.Length;
		}
	}

	// Token: 0x06001418 RID: 5144 RVA: 0x00062838 File Offset: 0x00060A38
	private static Pose GetT_Device_StylusTip(OVRInput.Controller controller)
	{
		Pose identity = Pose.identity;
		if (controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.RTouch)
		{
			identity = new Pose(new Vector3(0.0094f, -0.07145f, -0.07565f), Quaternion.Euler(35.305f, 50.988f, 37.901f));
		}
		if (controller == OVRInput.Controller.LTouch)
		{
			identity.position.x = identity.position.x * -1f;
			identity.rotation.y = identity.rotation.y * -1f;
			identity.rotation.z = identity.rotation.z * -1f;
		}
		return identity;
	}

	// Token: 0x04001633 RID: 5683
	private const int MaxBreadCrumbs = 60;

	// Token: 0x04001634 RID: 5684
	private const float BreadCrumbMinSize = 0.005f;

	// Token: 0x04001635 RID: 5685
	private const float BreadCrumbMaxSize = 0.02f;

	// Token: 0x04001636 RID: 5686
	[Header("External")]
	[SerializeField]
	private Transform m_trackingSpace;

	// Token: 0x04001637 RID: 5687
	[Header("Settings")]
	[SerializeField]
	private OVRInput.Handedness m_handedness = OVRInput.Handedness.LeftHanded;

	// Token: 0x04001638 RID: 5688
	[SerializeField]
	private GameObject m_breadCrumbPf;

	// Token: 0x04001639 RID: 5689
	private GameObject m_breadCrumbContainer;

	// Token: 0x0400163A RID: 5690
	private GameObject[] m_breadCrumbs;

	// Token: 0x0400163B RID: 5691
	private int m_breadCrumbIndexPrev = -1;

	// Token: 0x0400163C RID: 5692
	private int m_breadCrumbIndexCurr;

	// Token: 0x0400163D RID: 5693
	private OVRInput.Controller m_controller;
}
