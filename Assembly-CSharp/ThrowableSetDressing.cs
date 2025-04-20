using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E8 RID: 2280
[RequireComponent(typeof(NetworkView))]
public class ThrowableSetDressing : TransferrableObject
{
	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06003732 RID: 14130 RVA: 0x00054702 File Offset: 0x00052902
	// (set) Token: 0x06003733 RID: 14131 RVA: 0x0005470A File Offset: 0x0005290A
	public bool inInitialPose { get; private set; } = true;

	// Token: 0x06003734 RID: 14132 RVA: 0x00054713 File Offset: 0x00052913
	public override bool ShouldBeKinematic()
	{
		return this.inInitialPose || base.ShouldBeKinematic();
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x00054725 File Offset: 0x00052925
	protected override void Awake()
	{
		base.Awake();
		this.netView = base.GetComponent<NetworkView>();
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x00054739 File Offset: 0x00052939
	protected override void Start()
	{
		base.Start();
		this.respawnAtPos = base.transform.position;
		this.respawnAtRot = base.transform.rotation;
		this.currentState = TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x0005476E File Offset: 0x0005296E
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.inInitialPose = false;
		this.StopRespawnTimer();
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x00054785 File Offset: 0x00052985
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.StartRespawnTimer(-1f);
		return true;
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x0005479F File Offset: 0x0005299F
	public override void DropItem()
	{
		base.DropItem();
		this.StartRespawnTimer(-1f);
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x000547B2 File Offset: 0x000529B2
	private void StopRespawnTimer()
	{
		if (this.respawnTimer != null)
		{
			base.StopCoroutine(this.respawnTimer);
			this.respawnTimer = null;
		}
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x000547CF File Offset: 0x000529CF
	public void SetWillTeleport()
	{
		this.worldShareableInstance.SetWillTeleport();
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x00146A84 File Offset: 0x00144C84
	public void StartRespawnTimer(float overrideTimer = -1f)
	{
		float timerDuration = (overrideTimer != -1f) ? overrideTimer : this.respawnTimerDuration;
		this.StopRespawnTimer();
		if (this.respawnTimerDuration != 0f && (!this.netView.IsValid || this.netView.IsMine))
		{
			this.respawnTimer = base.StartCoroutine(this.RespawnTimerCoroutine(timerDuration));
		}
	}

	// Token: 0x0600373D RID: 14141 RVA: 0x000547DC File Offset: 0x000529DC
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

	// Token: 0x04003969 RID: 14697
	public float respawnTimerDuration;

	// Token: 0x0400396B RID: 14699
	[Tooltip("set this only if this set dressing is using as an ingredient for the magic cauldron - Halloween")]
	public MagicIngredientType IngredientTypeSO;

	// Token: 0x0400396C RID: 14700
	private float _respawnTimestamp;

	// Token: 0x0400396D RID: 14701
	[SerializeField]
	private CapsuleCollider capsuleCollider;

	// Token: 0x0400396E RID: 14702
	private NetworkView netView;

	// Token: 0x0400396F RID: 14703
	private Vector3 respawnAtPos;

	// Token: 0x04003970 RID: 14704
	private Quaternion respawnAtRot;

	// Token: 0x04003971 RID: 14705
	private Coroutine respawnTimer;
}
