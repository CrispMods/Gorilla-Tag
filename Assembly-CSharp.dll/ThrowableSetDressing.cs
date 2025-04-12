using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CF RID: 2255
[RequireComponent(typeof(NetworkView))]
public class ThrowableSetDressing : TransferrableObject
{
	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06003676 RID: 13942 RVA: 0x000531E5 File Offset: 0x000513E5
	// (set) Token: 0x06003677 RID: 13943 RVA: 0x000531ED File Offset: 0x000513ED
	public bool inInitialPose { get; private set; } = true;

	// Token: 0x06003678 RID: 13944 RVA: 0x000531F6 File Offset: 0x000513F6
	public override bool ShouldBeKinematic()
	{
		return this.inInitialPose || base.ShouldBeKinematic();
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x00053208 File Offset: 0x00051408
	protected override void Awake()
	{
		base.Awake();
		this.netView = base.GetComponent<NetworkView>();
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x0005321C File Offset: 0x0005141C
	protected override void Start()
	{
		base.Start();
		this.respawnAtPos = base.transform.position;
		this.respawnAtRot = base.transform.rotation;
		this.currentState = TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00053251 File Offset: 0x00051451
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.inInitialPose = false;
		this.StopRespawnTimer();
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00053268 File Offset: 0x00051468
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.StartRespawnTimer(-1f);
		return true;
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00053282 File Offset: 0x00051482
	public override void DropItem()
	{
		base.DropItem();
		this.StartRespawnTimer(-1f);
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x00053295 File Offset: 0x00051495
	private void StopRespawnTimer()
	{
		if (this.respawnTimer != null)
		{
			base.StopCoroutine(this.respawnTimer);
			this.respawnTimer = null;
		}
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x000532B2 File Offset: 0x000514B2
	public void SetWillTeleport()
	{
		this.worldShareableInstance.SetWillTeleport();
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x001414C4 File Offset: 0x0013F6C4
	public void StartRespawnTimer(float overrideTimer = -1f)
	{
		float timerDuration = (overrideTimer != -1f) ? overrideTimer : this.respawnTimerDuration;
		this.StopRespawnTimer();
		if (this.respawnTimerDuration != 0f && (!this.netView.IsValid || this.netView.IsMine))
		{
			this.respawnTimer = base.StartCoroutine(this.RespawnTimerCoroutine(timerDuration));
		}
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x000532BF File Offset: 0x000514BF
	private IEnumerator RespawnTimerCoroutine(float timerDuration)
	{
		yield return new WaitForSeconds(timerDuration);
		if (base.InHand())
		{
			yield break;
		}
		this.SetWillTeleport();
		base.transform.position = this.respawnAtPos;
		base.transform.rotation = this.respawnAtRot;
		this.inInitialPose = true;
		this.rigidbodyInstance.isKinematic = true;
		yield break;
	}

	// Token: 0x040038BA RID: 14522
	public float respawnTimerDuration;

	// Token: 0x040038BC RID: 14524
	[Tooltip("set this only if this set dressing is using as an ingredient for the magic cauldron - Halloween")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x040038BD RID: 14525
	private float _respawnTimestamp;

	// Token: 0x040038BE RID: 14526
	[SerializeField]
	private CapsuleCollider capsuleCollider;

	// Token: 0x040038BF RID: 14527
	private NetworkView netView;

	// Token: 0x040038C0 RID: 14528
	private Vector3 respawnAtPos;

	// Token: 0x040038C1 RID: 14529
	private Quaternion respawnAtRot;

	// Token: 0x040038C2 RID: 14530
	private Coroutine respawnTimer;
}
