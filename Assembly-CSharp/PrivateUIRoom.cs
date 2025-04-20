using System;
using System.Collections.Generic;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000379 RID: 889
public class PrivateUIRoom : MonoBehaviour
{
	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060014A6 RID: 5286 RVA: 0x0003DDE0 File Offset: 0x0003BFE0
	private GTPlayer localPlayer
	{
		get
		{
			return GTPlayer.Instance;
		}
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000BCC8C File Offset: 0x000BAE8C
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

	// Token: 0x060014A8 RID: 5288 RVA: 0x000BCCF0 File Offset: 0x000BAEF0
	private void ToggleLevelVisibility(bool levelShouldBeVisible)
	{
		Camera component = GorillaTagger.Instance.mainCamera.GetComponent<Camera>();
		if (levelShouldBeVisible)
		{
			component.cullingMask = this.savedCullingLayers;
			return;
		}
		this.savedCullingLayers = component.cullingMask;
		component.cullingMask = this.visibleLayers;
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x000BCD3C File Offset: 0x000BAF3C
	private static void StopOverlay()
	{
		PrivateUIRoom.instance.localPlayer.inOverlay = false;
		PrivateUIRoom.instance.inOverlay = false;
		PrivateUIRoom.instance.localPlayer.disableMovement = false;
		PrivateUIRoom.instance.localPlayer.InReportMenu = false;
		PrivateUIRoom.instance.ToggleLevelVisibility(true);
		PrivateUIRoom.instance.occluder.SetActive(false);
		PrivateUIRoom.instance.leftHandObject.SetActive(false);
		PrivateUIRoom.instance.rightHandObject.SetActive(false);
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x000BCDC0 File Offset: 0x000BAFC0
	private void GetIdealScreenPositionRotation(out Vector3 position, out Quaternion rotation, out Vector3 scale)
	{
		GameObject mainCamera = GorillaTagger.Instance.mainCamera;
		rotation = Quaternion.Euler(0f, mainCamera.transform.eulerAngles.y, 0f);
		scale = this.localPlayer.turnParent.transform.localScale;
		position = mainCamera.transform.position + rotation * Vector3.zero * scale.x;
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x000BCE4C File Offset: 0x000BB04C
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

	// Token: 0x060014AC RID: 5292 RVA: 0x000BCF14 File Offset: 0x000BB114
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

	// Token: 0x060014AD RID: 5293 RVA: 0x0003DEC3 File Offset: 0x0003C0C3
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

	// Token: 0x060014AE RID: 5294 RVA: 0x0003DEF0 File Offset: 0x0003C0F0
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

	// Token: 0x060014AF RID: 5295 RVA: 0x000BD010 File Offset: 0x000BB210
	private static void StartOverlay()
	{
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

	// Token: 0x060014B0 RID: 5296 RVA: 0x000BD0E4 File Offset: 0x000BB2E4
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

	// Token: 0x040016D3 RID: 5843
	[SerializeField]
	private GameObject occluder;

	// Token: 0x040016D4 RID: 5844
	[SerializeField]
	private LayerMask visibleLayers;

	// Token: 0x040016D5 RID: 5845
	[SerializeField]
	private GameObject leftHandObject;

	// Token: 0x040016D6 RID: 5846
	[SerializeField]
	private GameObject rightHandObject;

	// Token: 0x040016D7 RID: 5847
	private int savedCullingLayers;

	// Token: 0x040016D8 RID: 5848
	private Transform focusTransform;

	// Token: 0x040016D9 RID: 5849
	private List<Transform> ui;

	// Token: 0x040016DA RID: 5850
	private Dictionary<Transform, Transform> uiParents;

	// Token: 0x040016DB RID: 5851
	private bool inOverlay;

	// Token: 0x040016DC RID: 5852
	private bool overlayForcedActive;

	// Token: 0x040016DD RID: 5853
	private static PrivateUIRoom instance;
}
