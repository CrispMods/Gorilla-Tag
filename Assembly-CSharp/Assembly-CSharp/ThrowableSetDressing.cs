using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CF RID: 2255
[RequireComponent(typeof(NetworkView))]
public class ThrowableSetDressing : TransferrableObject
{
	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06003676 RID: 13942 RVA: 0x00101A34 File Offset: 0x000FFC34
	// (set) Token: 0x06003677 RID: 13943 RVA: 0x00101A3C File Offset: 0x000FFC3C
	public bool inInitialPose { get; private set; } = true;

	// Token: 0x06003678 RID: 13944 RVA: 0x00101A45 File Offset: 0x000FFC45
	public override bool ShouldBeKinematic()
	{
		return this.inInitialPose || base.ShouldBeKinematic();
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x00101A57 File Offset: 0x000FFC57
	protected override void Awake()
	{
		base.Awake();
		this.netView = base.GetComponent<NetworkView>();
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x00101A6B File Offset: 0x000FFC6B
	protected override void Start()
	{
		base.Start();
		this.respawnAtPos = base.transform.position;
		this.respawnAtRot = base.transform.rotation;
		this.currentState = TransferrableObject.PositionState.Dropped;
	}

	// Token: 0x0600367B RID: 13947 RVA: 0x00101AA0 File Offset: 0x000FFCA0
	public override void OnGrab(InteractionPoint pointGrabbed, GameObject grabbingHand)
	{
		base.OnGrab(pointGrabbed, grabbingHand);
		this.inInitialPose = false;
		this.StopRespawnTimer();
	}

	// Token: 0x0600367C RID: 13948 RVA: 0x00101AB7 File Offset: 0x000FFCB7
	public override bool OnRelease(DropZone zoneReleased, GameObject releasingHand)
	{
		if (!base.OnRelease(zoneReleased, releasingHand))
		{
			return false;
		}
		this.StartRespawnTimer(-1f);
		return true;
	}

	// Token: 0x0600367D RID: 13949 RVA: 0x00101AD1 File Offset: 0x000FFCD1
	public override void DropItem()
	{
		base.DropItem();
		this.StartRespawnTimer(-1f);
	}

	// Token: 0x0600367E RID: 13950 RVA: 0x00101AE4 File Offset: 0x000FFCE4
	private void StopRespawnTimer()
	{
		if (this.respawnTimer != null)
		{
			base.StopCoroutine(this.respawnTimer);
			this.respawnTimer = null;
		}
	}

	// Token: 0x0600367F RID: 13951 RVA: 0x00101B01 File Offset: 0x000FFD01
	public void SetWillTeleport()
	{
		this.worldShareableInstance.SetWillTeleport();
	}

	// Token: 0x06003680 RID: 13952 RVA: 0x00101B10 File Offset: 0x000FFD10
	public void StartRespawnTimer(float overrideTimer = -1f)
	{
		float timerDuration = (overrideTimer != -1f) ? overrideTimer : this.respawnTimerDuration;
		this.StopRespawnTimer();
		if (this.respawnTimerDuration != 0f && (!this.netView.IsValid || this.netView.IsMine))
		{
			this.respawnTimer = base.StartCoroutine(this.RespawnTimerCoroutine(timerDuration));
		}
	}

	// Token: 0x06003681 RID: 13953 RVA: 0x00101B6F File Offset: 0x000FFD6F
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
