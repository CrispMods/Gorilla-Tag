using System;
using GorillaLocomotion;
using GorillaLocomotion.Gameplay;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020005E7 RID: 1511
public class GorillaGrabber : MonoBehaviour
{
	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06002577 RID: 9591 RVA: 0x00049661 File Offset: 0x00047861
	public XRNode XrNode
	{
		get
		{
			return this.xrNode;
		}
	}

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06002578 RID: 9592 RVA: 0x00049669 File Offset: 0x00047869
	public bool IsLeftHand
	{
		get
		{
			return this.XrNode == XRNode.LeftHand;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06002579 RID: 9593 RVA: 0x00049674 File Offset: 0x00047874
	public bool IsRightHand
	{
		get
		{
			return this.XrNode == XRNode.RightHand;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600257A RID: 9594 RVA: 0x0004967F File Offset: 0x0004787F
	public GTPlayer Player
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x0600257B RID: 9595 RVA: 0x001061CC File Offset: 0x001043CC
	private void Start()
	{
		ControllerInputPoller.AddUpdateCallback(new Action(this.OnControllerUpdate));
		this.hapticStrengthActual = this.hapticStrength;
		this.audioSource = base.GetComponent<AudioSource>();
		this.player = base.GetComponentInParent<GTPlayer>();
		if (!this.player)
		{
			Debug.LogWarning("Gorilla Grabber Component has no player in hierarchy. Disabling this Gorilla Grabber");
			base.GetComponent<GorillaGrabber>().enabled = false;
		}
	}

	// Token: 0x0600257C RID: 9596 RVA: 0x00106234 File Offset: 0x00104434
	private void OnControllerUpdate()
	{
		bool grab = ControllerInputPoller.GetGrab(this.xrNode);
		bool grabMomentary = ControllerInputPoller.GetGrabMomentary(this.xrNode);
		bool grabRelease = ControllerInputPoller.GetGrabRelease(this.xrNode);
		if (this.currentGrabbable != null && (grabRelease || this.GrabDistanceOverCheck()))
		{
			this.Ungrab(null);
		}
		if (grabMomentary)
		{
			this.grabTimeStamp = Time.time;
		}
		if (grab && this.currentGrabbable == null)
		{
			this.currentGrabbable = this.TryGrab(Time.time - this.grabTimeStamp < this.coyoteTimeDuration);
		}
		if (this.currentGrabbable != null && this.hapticStrengthActual > 0f)
		{
			GorillaTagger.Instance.DoVibration(this.xrNode, this.hapticStrengthActual, Time.deltaTime);
			this.hapticStrengthActual -= this.hapticDecay * Time.deltaTime;
		}
	}

	// Token: 0x0600257D RID: 9597 RVA: 0x00049687 File Offset: 0x00047887
	private bool GrabDistanceOverCheck()
	{
		return this.currentGrabbedTransform == null || Vector3.Distance(base.transform.position, this.currentGrabbedTransform.TransformPoint(this.localGrabbedPosition)) > this.breakDistance;
	}

	// Token: 0x0600257E RID: 9598 RVA: 0x00106300 File Offset: 0x00104500
	internal void Ungrab(IGorillaGrabable specificGrabbable = null)
	{
		if (specificGrabbable != null && specificGrabbable != this.currentGrabbable)
		{
			return;
		}
		this.currentGrabbable.OnGrabReleased(this);
		PlayerGameEvents.DroppedObject(this.currentGrabbable.name);
		this.currentGrabbable = null;
		this.gripEffects.Stop();
		this.hapticStrengthActual = this.hapticStrength;
	}

	// Token: 0x0600257F RID: 9599 RVA: 0x00106354 File Offset: 0x00104554
	private IGorillaGrabable TryGrab(bool momentary)
	{
		IGorillaGrabable gorillaGrabable = null;
		Debug.DrawRay(base.transform.position, base.transform.forward * (this.grabRadius * this.player.scale), Color.blue, 1f);
		int num = Physics.OverlapSphereNonAlloc(base.transform.position, this.grabRadius * this.player.scale, this.grabCastResults);
		float num2 = float.MaxValue;
		for (int i = 0; i < num; i++)
		{
			IGorillaGrabable gorillaGrabable2;
			if (this.grabCastResults[i].TryGetComponent<IGorillaGrabable>(out gorillaGrabable2))
			{
				float num3 = Vector3.Distance(base.transform.position, this.FindClosestPoint(this.grabCastResults[i], base.transform.position));
				if (num3 < num2)
				{
					num2 = num3;
					gorillaGrabable = gorillaGrabable2;
				}
			}
		}
		if (gorillaGrabable != null && (!gorillaGrabable.MomentaryGrabOnly() || momentary) && gorillaGrabable.CanBeGrabbed(this))
		{
			gorillaGrabable.OnGrabbed(this, out this.currentGrabbedTransform, out this.localGrabbedPosition);
			PlayerGameEvents.GrabbedObject(gorillaGrabable.name);
		}
		if (gorillaGrabable != null && !gorillaGrabable.CanBeGrabbed(this))
		{
			gorillaGrabable = null;
		}
		return gorillaGrabable;
	}

	// Token: 0x06002580 RID: 9600 RVA: 0x000496C2 File Offset: 0x000478C2
	private Vector3 FindClosestPoint(Collider collider, Vector3 position)
	{
		if (collider is MeshCollider && !(collider as MeshCollider).convex)
		{
			return position;
		}
		return collider.ClosestPoint(position);
	}

	// Token: 0x06002581 RID: 9601 RVA: 0x00106468 File Offset: 0x00104668
	public void Inject(Transform currentGrabbableTransform, Vector3 localGrabbedPosition)
	{
		if (this.currentGrabbable != null)
		{
			this.Ungrab(null);
		}
		if (currentGrabbableTransform != null)
		{
			this.currentGrabbable = currentGrabbableTransform.GetComponent<IGorillaGrabable>();
			this.currentGrabbedTransform = currentGrabbableTransform;
			this.localGrabbedPosition = localGrabbedPosition;
			this.currentGrabbable.OnGrabbed(this, out this.currentGrabbedTransform, out localGrabbedPosition);
		}
	}

	// Token: 0x0400299D RID: 10653
	private GTPlayer player;

	// Token: 0x0400299E RID: 10654
	[SerializeField]
	private XRNode xrNode = XRNode.LeftHand;

	// Token: 0x0400299F RID: 10655
	private AudioSource audioSource;

	// Token: 0x040029A0 RID: 10656
	private Transform currentGrabbedTransform;

	// Token: 0x040029A1 RID: 10657
	private Vector3 localGrabbedPosition;

	// Token: 0x040029A2 RID: 10658
	private IGorillaGrabable currentGrabbable;

	// Token: 0x040029A3 RID: 10659
	[SerializeField]
	private float grabRadius = 0.015f;

	// Token: 0x040029A4 RID: 10660
	[SerializeField]
	private float breakDistance = 0.3f;

	// Token: 0x040029A5 RID: 10661
	[SerializeField]
	private float hapticStrength = 0.2f;

	// Token: 0x040029A6 RID: 10662
	private float hapticStrengthActual = 0.2f;

	// Token: 0x040029A7 RID: 10663
	[SerializeField]
	private float hapticDecay;

	// Token: 0x040029A8 RID: 10664
	[SerializeField]
	private ParticleSystem gripEffects;

	// Token: 0x040029A9 RID: 10665
	private Collider[] grabCastResults = new Collider[32];

	// Token: 0x040029AA RID: 10666
	private float grabTimeStamp;

	// Token: 0x040029AB RID: 10667
	[SerializeField]
	private float coyoteTimeDuration = 0.25f;
}
