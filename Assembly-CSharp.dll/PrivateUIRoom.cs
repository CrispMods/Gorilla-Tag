using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200036E RID: 878
public class PrivateUIRoom : MonoBehaviour
{
	// Token: 0x17000243 RID: 579
	// (get) Token: 0x0600145D RID: 5213 RVA: 0x0003CB20 File Offset: 0x0003AD20
	private GTPlayer localPlayer
	{
		get
		{
			return GTPlayer.Instance;
		}
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000BA3F4 File Offset: 0x000B85F4
	private void Awake()
	{
		if (PrivateUIRoom.instance == null)
		{
			PrivateUIRoom.instance = this;
			this.occluder.SetActive(false);
			this.leftHandObject.SetActive(false);
			this.rightHandObject.SetActive(false);
			this.ui = new List<Transform>();
			this.uiParents = new Dictionary<Transform, Transform>();
			return;
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x0600145F RID: 5215 RVA: 0x000BA458 File Offset: 0x000B8658
	private void ToggleLevelVisibility(bool levelShouldBeVisible)
	{
		Debug.Log(string.Format("[PrivateUIRoom::ToggleLevelVisibility] levelShouldBeVisible: {0}", levelShouldBeVisible));
		Camera component = GorillaTagger.Instance.mainCamera.GetComponent<Camera>();
		if (levelShouldBeVisible)
		{
			Debug.Log(string.Format("[PrivateUIRoom::ToggleLevelVisibility] restoring cached culling mask: {0}", this.savedCullingLayers));
			component.cullingMask = this.savedCullingLayers;
			return;
		}
		Debug.Log(string.Format("[PrivateUIRoom::ToggleLevelVisibility] caching current culling mask: {0}", component.cullingMask));
		this.savedCullingLayers = component.cullingMask;
		component.cullingMask = this.visibleLayers;
	}

	// Token: 0x06001460 RID: 5216 RVA: 0x000BA4EC File Offset: 0x000B86EC
	private static void StopOverlay()
	{
		Debug.Log("[PrivateUIRoom::StopOverlay] Stopping overlay...");
		PrivateUIRoom.instance.localPlayer.inOverlay = false;
		PrivateUIRoom.instance.inOverlay = false;
		PrivateUIRoom.instance.localPlayer.disableMovement = false;
		PrivateUIRoom.instance.localPlayer.InReportMenu = false;
		PrivateUIRoom.instance.ToggleLevelVisibility(true);
		PrivateUIRoom.instance.occluder.SetActive(false);
		PrivateUIRoom.instance.leftHandObject.SetActive(false);
		PrivateUIRoom.instance.rightHandObject.SetActive(false);
	}

	// Token: 0x06001461 RID: 5217 RVA: 0x000BA57C File Offset: 0x000B877C
	private void GetIdealScreenPositionRotation(out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		GameObject mainCamera = GorillaTagger.Instance.mainCamera;
		rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
		scale = this.localPlayer.turnParent.transform.localScale;
		position = mainCamera.transform.position + rotation * Vector3.zero * scale.x;
	}

	// Token: 0x06001462 RID: 5218 RVA: 0x000BA608 File Offset: 0x000B8808
	public static void AddUI(Transform focus)
	{
		if (PrivateUIRoom.instance.ui.Contains(focus))
		{
			return;
		}
		PrivateUIRoom.instance.uiParents.Add(focus, focus.parent);
		focus.parent = PrivateUIRoom.instance.transform;
		focus.gameObject.SetActive(false);
		PrivateUIRoom.instance.ui.Add(focus);
		if (PrivateUIRoom.instance.ui.Count == 1 && PrivateUIRoom.instance.focusTransform == null)
		{
			PrivateUIRoom.instance.focusTransform = PrivateUIRoom.instance.ui[0];
			PrivateUIRoom.instance.focusTransform.gameObject.SetActive(true);
			if (!PrivateUIRoom.instance.inOverlay)
			{
				PrivateUIRoom.StartOverlay();
			}
		}
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x000BA6D0 File Offset: 0x000B88D0
	public static void RemoveUI(Transform focus)
	{
		if (!PrivateUIRoom.instance.ui.Contains(focus))
		{
			return;
		}
		focus.gameObject.SetActive(false);
		PrivateUIRoom.instance.ui.Remove(focus);
		if (PrivateUIRoom.instance.focusTransform == focus)
		{
			PrivateUIRoom.instance.focusTransform = null;
		}
		if (PrivateUIRoom.instance.uiParents[focus] != null)
		{
			focus.parent = PrivateUIRoom.instance.uiParents[focus];
			PrivateUIRoom.instance.uiParents.Remove(focus);
		}
		else
		{
			UnityEngine.Object.Destroy(focus.gameObject);
		}
		if (PrivateUIRoom.instance.ui.Count > 0)
		{
			PrivateUIRoom.instance.focusTransform = PrivateUIRoom.instance.ui[0];
			PrivateUIRoom.instance.focusTransform.gameObject.SetActive(true);
			return;
		}
		if (!PrivateUIRoom.instance.overlayForcedActive)
		{
			PrivateUIRoom.StopOverlay();
		}
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x0003CC03 File Offset: 0x0003AE03
	public static void ForceStartOverlay()
	{
		if (PrivateUIRoom.instance == null)
		{
			return;
		}
		PrivateUIRoom.instance.overlayForcedActive = true;
		if (PrivateUIRoom.instance.inOverlay)
		{
			return;
		}
		PrivateUIRoom.StartOverlay();
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x0003CC30 File Offset: 0x0003AE30
	public static void StopForcedOverlay()
	{
		if (PrivateUIRoom.instance == null)
		{
			return;
		}
		PrivateUIRoom.instance.overlayForcedActive = false;
		if (PrivateUIRoom.instance.ui.Count == 0 && PrivateUIRoom.instance.inOverlay)
		{
			PrivateUIRoom.StopOverlay();
		}
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x000BA7CC File Offset: 0x000B89CC
	private static void StartOverlay()
	{
		Debug.Log("[PrivateUIRoom::StartOverlay] Starting overlay...");
		Vector3 vector;
		Quaternion quaternion;
		Vector3 localScale;
		PrivateUIRoom.instance.GetIdealScreenPositionRotation(out vector, out quaternion, out localScale);
		PrivateUIRoom.instance.leftHandObject.transform.localScale = localScale;
		PrivateUIRoom.instance.rightHandObject.transform.localScale = localScale;
		PrivateUIRoom.instance.occluder.transform.localScale = localScale;
		PrivateUIRoom.instance.localPlayer.InReportMenu = true;
		PrivateUIRoom.instance.localPlayer.disableMovement = true;
		PrivateUIRoom.instance.occluder.SetActive(true);
		PrivateUIRoom.instance.rightHandObject.SetActive(true);
		PrivateUIRoom.instance.leftHandObject.SetActive(true);
		PrivateUIRoom.instance.ToggleLevelVisibility(false);
		PrivateUIRoom.instance.localPlayer.inOverlay = true;
		PrivateUIRoom.instance.inOverlay = true;
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x000BA8A8 File Offset: 0x000B8AA8
	private void Update()
	{
		if (!this.localPlayer.InReportMenu)
		{
			return;
		}
		this.occluder.transform.position = GorillaTagger.Instance.mainCamera.transform.position;
		this.rightHandObject.transform.SetPositionAndRotation(this.localPlayer.rightControllerTransform.position, this.localPlayer.rightControllerTransform.rotation);
		this.leftHandObject.transform.SetPositionAndRotation(this.localPlayer.leftControllerTransform.position, this.localPlayer.leftControllerTransform.rotation);
		if (this.focusTransform == null)
		{
			return;
		}
		Vector3 vector = GorillaTagger.Instance.mainCamera.transform.position + GorillaTagger.Instance.mainCamera.transform.forward * 0.02f;
		vector.y = GorillaTagger.Instance.mainCamera.transform.position.y;
		if (Vector3.Distance(vector, this.focusTransform.position) > 0.1f)
		{
			this.focusTransform.position = vector;
			this.focusTransform.rotation = Quaternion.LookRotation(this.focusTransform.position - GorillaTagger.Instance.mainCamera.transform.position);
		}
	}

	// Token: 0x0400168C RID: 5772
	[SerializeField]
	private GameObject occluder;

	// Token: 0x0400168D RID: 5773
	[SerializeField]
	private LayerMask visibleLayers;

	// Token: 0x0400168E RID: 5774
	[SerializeField]
	private GameObject leftHandObject;

	// Token: 0x0400168F RID: 5775
	[SerializeField]
	private GameObject rightHandObject;

	// Token: 0x04001690 RID: 5776
	private int savedCullingLayers;

	// Token: 0x04001691 RID: 5777
	private Transform focusTransform;

	// Token: 0x04001692 RID: 5778
	private List<Transform> ui;

	// Token: 0x04001693 RID: 5779
	private Dictionary<Transform, Transform> uiParents;

	// Token: 0x04001694 RID: 5780
	private bool inOverlay;

	// Token: 0x04001695 RID: 5781
	private bool overlayForcedActive;

	// Token: 0x04001696 RID: 5782
	private static PrivateUIRoom instance;
}
