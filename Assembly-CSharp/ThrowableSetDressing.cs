using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CC RID: 2252
[RequireComponent(typeof(NetworkView))]
public class ThrowableSetDressing : TransferrableObject
{
	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x0600366A RID: 13930 RVA: 0x0010146C File Offset: 0x000FF66C
	// (set) Token: 0x0600366B RID: 13931 RVA: 0x00101474 File Offset: 0x000FF674
	public bool inInitialPose { get; private set; } = true;

	// Token: 0x0600366C RID: 13932 RVA: 0x0010147D File Offset: 0x000FF67D
	public override bool ShouldBeKinematic()
	{
		return this.inInitialPose || base.ShouldBeKinematic();
	}

	// Token: 0x0600366D RID: 13933 RVA: 0x0010148F File Offset: 0x000FF68F
	protected override void Awake()
	{
		base.Awake();
		this.netView = base.GetComponent<NetworkView>();
	}

	// Token: 0x0600366E RID: 13934 RVA: 0x001014A3 File Offset: 0x000FF6A3
	protected override void Start()
	{
		base.Start();
		this.respawnAtPos = base.transform.position;
		this.respawnAtRot = base.transform.rotation;
		this.currentState = TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x0600366F RID: 13935 RVA: 0x001014D8 File Offset: 0x000FF6D8
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.inInitialPose = false;
		this.StopRespawnTimer();
	}

	// Token: 0x06003670 RID: 13936 RVA: 0x001014EF File Offset: 0x000FF6EF
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.StartRespawnTimer(-1f);
		return true;
	}

	// Token: 0x06003671 RID: 13937 RVA: 0x00101509 File Offset: 0x000FF709
	public override void DropItem()
	{
		base.DropItem();
		this.StartRespawnTimer(-1f);
	}

	// Token: 0x06003672 RID: 13938 RVA: 0x0010151C File Offset: 0x000FF71C
	private void StopRespawnTimer()
	{
		if (this.respawnTimer != null)
		{
			base.StopCoroutine(this.respawnTimer);
			this.respawnTimer = null;
		}
	}

	// Token: 0x06003673 RID: 13939 RVA: 0x00101539 File Offset: 0x000FF739
	public void SetWillTeleport()
	{
		this.worldShareableInstance.SetWillTeleport();
	}

	// Token: 0x06003674 RID: 13940 RVA: 0x00101548 File Offset: 0x000FF748
	public void StartRespawnTimer(float overrideTimer = -1f)
	{
		float timerDuration = (overrideTimer != -1f) ? overrideTimer : this.respawnTimerDuration;
		this.StopRespawnTimer();
		if (this.respawnTimerDuration != 0f && (!this.netView.IsValid || this.netView.IsMine))
		{
			this.respawnTimer = base.StartCoroutine(this.RespawnTimerCoroutine(timerDuration));
		}
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x001015A7 File Offset: 0x000FF7A7
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

	// Token: 0x040038A8 RID: 14504
	public float respawnTimerDuration;

	// Token: 0x040038AA RID: 14506
	[Tooltip("set this only if this set dressing is using as an ingredient for the magic cauldron - Halloween")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x040038AB RID: 14507
	private float _respawnTimestamp;

	// Token: 0x040038AC RID: 14508
	[SerializeField]
	private CapsuleCollider capsuleCollider;

	// Token: 0x040038AD RID: 14509
	private NetworkView netView;

	// Token: 0x040038AE RID: 14510
	private Vector3 respawnAtPos;

	// Token: 0x040038AF RID: 14511
	private Quaternion respawnAtRot;

	// Token: 0x040038B0 RID: 14512
	private Coroutine respawnTimer;
}
