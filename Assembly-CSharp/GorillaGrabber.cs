using System;
using GorillaLocomotion;
using GorillaLocomotion.Gameplay;
using UnityEngine;
using UnityEngine.XR;

// Token: 0x020005D9 RID: 1497
public class GorillaGrabber : MonoBehaviour
{
	// Token: 0x170003CF RID: 975
	// (get) Token: 0x06002515 RID: 9493 RVA: 0x000B7FEC File Offset: 0x000B61EC
	public XRNode XrNode
	{
		get
		{
			return this.xrNode;
		}
	}

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x06002516 RID: 9494 RVA: 0x000B7FF4 File Offset: 0x000B61F4
	public bool IsLeftHand
	{
		get
		{
			return this.XrNode == XRNode.LeftHand;
		}
	}

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x06002517 RID: 9495 RVA: 0x000B7FFF File Offset: 0x000B61FF
	public bool IsRightHand
	{
		get
		{
			return this.XrNode == XRNode.RightHand;
		}
	}

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x06002518 RID: 9496 RVA: 0x000B800A File Offset: 0x000B620A
	public GTPlayer Player
	{
		get
		{
			return this.player;
		}
	}

	// Token: 0x06002519 RID: 9497 RVA: 0x000B8014 File Offset: 0x000B6214
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

	// Token: 0x0600251A RID: 9498 RVA: 0x000B807C File Offset: 0x000B627C
	private void OnControllerUpdate()
	{
		bool grab = ControllerInputPoller.GetGrab(this.xrNode);
		bool grabMomentary = ControllerInputPoller.GetGrabMomentary(this.xrNode);
		bool grabRelease = ControllerInputPoller.GetGrabRelease(this.xrNode);
		if (this.currentGrabbable != null && (grabRelease || this.GrabDistanceOverCheck()))
		{
			this.Ungrab();
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

	// Token: 0x0600251B RID: 9499 RVA: 0x000B8144 File Offset: 0x000B6344
	private bool GrabDistanceOverCheck()
	{
		return this.currentGrabbedTransform == null || Vector3.Distance(base.transform.position, this.currentGrabbedTransform.TransformPoint(this.localGrabbedPosition)) > this.breakDistance;
	}

	// Token: 0x0600251C RID: 9500 RVA: 0x000B817F File Offset: 0x000B637F
	private void Ungrab()
	{
		this.currentGrabbable.OnGrabReleased(this);
		PlayerGameEvents.DroppedObject(this.currentGrabbable.name);
		this.currentGrabbable = null;
		this.gripEffects.Stop();
		this.hapticStrengthActual = this.hapticStrength;
	}

	// Token: 0x0600251D RID: 9501 RVA: 0x000B81BC File Offset: 0x000B63BC
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

	// Token: 0x0600251E RID: 9502 RVA: 0x000B82CF File Offset: 0x000B64CF
	private Vector3 FindClosestPoint(Collider collider, Vector3 position)
	{
		if (collider is MeshCollider && !(collider as MeshCollider).convex)
		{
			return position;
		}
		return collider.ClosestPoint(position);
	}

	// Token: 0x0600251F RID: 9503 RVA: 0x000B82F0 File Offset: 0x000B64F0
	public void Inject(Transform currentGrabbableTransform, Vector3 localGrabbedPosition)
	{
		if (this.currentGrabbable != null)
		{
			this.Ungrab();
		}
		if (currentGrabbableTransform != null)
		{
			this.currentGrabbable = currentGrabbableTransform.GetComponent<IGorillaGrabable>();
			this.currentGrabbedTransform = currentGrabbableTransform;
			this.localGrabbedPosition = localGrabbedPosition;
			this.currentGrabbable.OnGrabbed(this, out this.currentGrabbedTransform, out localGrabbedPosition);
		}
	}

	// Token: 0x0400293E RID: 10558
	private GTPlayer player;

	// Token: 0x0400293F RID: 10559
	[SerializeField]
	private XRNode xrNode = XRNode.LeftHand;

	// Token: 0x04002940 RID: 10560
	private AudioSource audioSource;

	// Token: 0x04002941 RID: 10561
	private Transform currentGrabbedTransform;

	// Token: 0x04002942 RID: 10562
	private Vector3 localGrabbedPosition;

	// Token: 0x04002943 RID: 10563
	private IGorillaGrabable currentGrabbable;

	// Token: 0x04002944 RID: 10564
	[SerializeField]
	private float grabRadius = 0.015f;

	// Token: 0x04002945 RID: 10565
	[SerializeField]
	private float breakDistance = 0.3f;

	// Token: 0x04002946 RID: 10566
	[SerializeField]
	private float hapticStrength = 0.2f;

	// Token: 0x04002947 RID: 10567
	private float hapticStrengthActual = 0.2f;

	// Token: 0x04002948 RID: 10568
	[SerializeField]
	private float hapticDecay;

	// Token: 0x04002949 RID: 10569
	[SerializeField]
	private ParticleSystem gripEffects;

	// Token: 0x0400294A RID: 10570
	private Collider[] grabCastResults = new Collider[32];

	// Token: 0x0400294B RID: 10571
	private float grabTimeStamp;

	// Token: 0x0400294C RID: 10572
	[SerializeField]
	private float coyoteTimeDuration = 0.25f;
}
