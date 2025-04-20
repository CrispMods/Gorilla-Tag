using System;
using System.Collections;
using Fusion;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000382 RID: 898
[NetworkBehaviourWeaved(1)]
public class HitTargetNetworkState : NetworkComponent
{
	// Token: 0x060014FE RID: 5374 RVA: 0x000BE940 File Offset: 0x000BCB40
	protected override void Awake()
	{
		base.Awake();
		this.audioPlayer = base.GetComponent<AudioSource>();
		SlingshotProjectileHitNotifier component = base.GetComponent<SlingshotProjectileHitNotifier>();
		if (component != null)
		{
			component.OnProjectileHit += this.ProjectileHitReciever;
			component.OnProjectileCollisionStay += this.ProjectileHitReciever;
			return;
		}
		Debug.LogError("Needs SlingshotProjectileHitNotifier added to this GameObject to increment score");
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x0003E2E9 File Offset: 0x0003C4E9
	protected override void Start()
	{
		base.Start();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x06001500 RID: 5376 RVA: 0x0003E311 File Offset: 0x0003C511
	private void SetInitialState()
	{
		this.networkedScore.Value = 0;
		this.nextHittableTimestamp = 0f;
		this.audioPlayer.GTStop();
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x0003E335 File Offset: 0x0003C535
	public void OnLeftRoom()
	{
		this.SetInitialState();
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x0003E33D File Offset: 0x0003C53D
	internal override void OnEnable()
	{
		NetworkBehaviourUtils.InternalOnEnable(this);
		base.OnEnable();
		if (Application.isEditor)
		{
			base.StartCoroutine(this.TestPressCheck());
		}
		this.SetInitialState();
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x0003E365 File Offset: 0x0003C565
	private IEnumerator TestPressCheck()
	{
		for (;;)
		{
			if (this.testPress)
			{
				this.testPress = false;
				this.TargetHit(Vector3.zero, Vector3.one);
			}
			yield return new WaitForSeconds(1f);
		}
		yield break;
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x0003E374 File Offset: 0x0003C574
	private void ProjectileHitReciever(SlingshotProjectile projectile, Collision collision)
	{
		this.TargetHit(projectile.launchPosition, collision.contacts[0].point);
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x000BE9A0 File Offset: 0x000BCBA0
	public void TargetHit(Vector3 launchPoint, Vector3 impactPoint)
	{
		if (!NetworkSystem.Instance.IsMasterClient)
		{
			return;
		}
		if (Time.time <= this.nextHittableTimestamp)
		{
			return;
		}
		int num = this.networkedScore.Value;
		if (this.scoreIsDistance)
		{
			int num2 = Mathf.RoundToInt((launchPoint - impactPoint).magnitude * 3.28f);
			if (num2 <= num)
			{
				return;
			}
			num = num2;
		}
		else
		{
			num++;
			if (num >= 1000)
			{
				num = 0;
			}
		}
		if (this.resetAfterDuration > 0f && this.resetCoroutine == null)
		{
			this.resetAtTimestamp = Time.time + this.resetAfterDuration;
			this.resetCoroutine = base.StartCoroutine(this.ResetCo());
		}
		this.PlayAudio(this.networkedScore.Value, num);
		this.networkedScore.Value = num;
		this.nextHittableTimestamp = Time.time + (float)this.hitCooldownTime;
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001506 RID: 5382 RVA: 0x0003E393 File Offset: 0x0003C593
	// (set) Token: 0x06001507 RID: 5383 RVA: 0x0003E3B9 File Offset: 0x0003C5B9
	[Networked]
	[NetworkedWeaved(0, 1)]
	public unsafe int Data
	{
		get
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HitTargetNetworkState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			return this.Ptr[0];
		}
		set
		{
			if (this.Ptr == null)
			{
				throw new InvalidOperationException("Error when accessing HitTargetNetworkState.Data. Networked properties can only be accessed when Spawned() has been called.");
			}
			this.Ptr[0] = value;
		}
	}

	// Token: 0x06001508 RID: 5384 RVA: 0x0003E3E0 File Offset: 0x0003C5E0
	public override void WriteDataFusion()
	{
		this.Data = this.networkedScore.Value;
	}

	// Token: 0x06001509 RID: 5385 RVA: 0x000BEA78 File Offset: 0x000BCC78
	public override void ReadDataFusion()
	{
		int data = this.Data;
		if (data != this.networkedScore.Value)
		{
			this.PlayAudio(this.networkedScore.Value, data);
		}
		this.networkedScore.Value = data;
	}

	// Token: 0x0600150A RID: 5386 RVA: 0x0003E3F3 File Offset: 0x0003C5F3
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.networkedScore.Value);
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x000BEAB8 File Offset: 0x000BCCB8
	protected override void ReadDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		int num = (int)stream.ReceiveNext();
		if (num != this.networkedScore.Value)
		{
			this.PlayAudio(this.networkedScore.Value, num);
		}
		this.networkedScore.Value = num;
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0003E419 File Offset: 0x0003C619
	public void PlayAudio(int oldScore, int newScore)
	{
		if (oldScore > newScore && !this.scoreIsDistance)
		{
			this.audioPlayer.GTPlayOneShot(this.audioClips[1], 1f);
			return;
		}
		this.audioPlayer.GTPlayOneShot(this.audioClips[0], 1f);
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x0003E458 File Offset: 0x0003C658
	private IEnumerator ResetCo()
	{
		while (Time.time < this.resetAtTimestamp)
		{
			yield return new WaitForSeconds(this.resetAtTimestamp - Time.time);
		}
		this.networkedScore.Value = 0;
		this.PlayAudio(this.networkedScore.Value, 0);
		this.resetCoroutine = null;
		yield break;
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0003E476 File Offset: 0x0003C676
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0003E48E File Offset: 0x0003C68E
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x04001743 RID: 5955
	[SerializeField]
	private WatchableIntSO networkedScore;

	// Token: 0x04001744 RID: 5956
	[SerializeField]
	private int hitCooldownTime = 1;

	// Token: 0x04001745 RID: 5957
	[SerializeField]
	private bool testPress;

	// Token: 0x04001746 RID: 5958
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x04001747 RID: 5959
	[SerializeField]
	private bool scoreIsDistance;

	// Token: 0x04001748 RID: 5960
	[SerializeField]
	private float resetAfterDuration;

	// Token: 0x04001749 RID: 5961
	private AudioSource audioPlayer;

	// Token: 0x0400174A RID: 5962
	private float nextHittableTimestamp;

	// Token: 0x0400174B RID: 5963
	private float resetAtTimestamp;

	// Token: 0x0400174C RID: 5964
	private Coroutine resetCoroutine;

	// Token: 0x0400174D RID: 5965
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private int _Data;
}
