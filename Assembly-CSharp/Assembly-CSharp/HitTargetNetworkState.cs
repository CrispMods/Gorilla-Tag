using System;
using System.Collections;
using Fusion;
using GorillaTag;
using Photon.Pun;
using UnityEngine;

// Token: 0x02000377 RID: 887
[NetworkBehaviourWeaved(1)]
public class HitTargetNetworkState : NetworkComponent
{
	// Token: 0x060014B5 RID: 5301 RVA: 0x00065F0C File Offset: 0x0006410C
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

	// Token: 0x060014B6 RID: 5302 RVA: 0x00065F6A File Offset: 0x0006416A
	protected override void Start()
	{
		base.Start();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060014B7 RID: 5303 RVA: 0x00065F92 File Offset: 0x00064192
	private void SetInitialState()
	{
		this.networkedScore.Value = 0;
		this.nextHittableTimestamp = 0f;
		this.audioPlayer.GTStop();
	}

	// Token: 0x060014B8 RID: 5304 RVA: 0x00065FB6 File Offset: 0x000641B6
	public void OnLeftRoom()
	{
		this.SetInitialState();
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x00065FBE File Offset: 0x000641BE
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

	// Token: 0x060014BA RID: 5306 RVA: 0x00065FE6 File Offset: 0x000641E6
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

	// Token: 0x060014BB RID: 5307 RVA: 0x00065FF5 File Offset: 0x000641F5
	private void ProjectileHitReciever(SlingshotProjectile projectile, Collision collision)
	{
		this.TargetHit(projectile.launchPosition, collision.contacts[0].point);
	}

	// Token: 0x060014BC RID: 5308 RVA: 0x00066014 File Offset: 0x00064214
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

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x060014BD RID: 5309 RVA: 0x000660EC File Offset: 0x000642EC
	// (set) Token: 0x060014BE RID: 5310 RVA: 0x00066112 File Offset: 0x00064312
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

	// Token: 0x060014BF RID: 5311 RVA: 0x00066139 File Offset: 0x00064339
	public override void WriteDataFusion()
	{
		this.Data = this.networkedScore.Value;
	}

	// Token: 0x060014C0 RID: 5312 RVA: 0x0006614C File Offset: 0x0006434C
	public override void ReadDataFusion()
	{
		int data = this.Data;
		if (data != this.networkedScore.Value)
		{
			this.PlayAudio(this.networkedScore.Value, data);
		}
		this.networkedScore.Value = data;
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x0006618C File Offset: 0x0006438C
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.networkedScore.Value);
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x000661B4 File Offset: 0x000643B4
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

	// Token: 0x060014C3 RID: 5315 RVA: 0x00066207 File Offset: 0x00064407
	public void PlayAudio(int oldScore, int newScore)
	{
		if (oldScore > newScore && !this.scoreIsDistance)
		{
			this.audioPlayer.GTPlayOneShot(this.audioClips[1], 1f);
			return;
		}
		this.audioPlayer.GTPlayOneShot(this.audioClips[0], 1f);
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x00066246 File Offset: 0x00064446
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

	// Token: 0x060014C6 RID: 5318 RVA: 0x00066264 File Offset: 0x00064464
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x0006627C File Offset: 0x0006447C
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040016FC RID: 5884
	[SerializeField]
	private WatchableIntSO networkedScore;

	// Token: 0x040016FD RID: 5885
	[SerializeField]
	private int hitCooldownTime = 1;

	// Token: 0x040016FE RID: 5886
	[SerializeField]
	private bool testPress;

	// Token: 0x040016FF RID: 5887
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x04001700 RID: 5888
	[SerializeField]
	private bool scoreIsDistance;

	// Token: 0x04001701 RID: 5889
	[SerializeField]
	private float resetAfterDuration;

	// Token: 0x04001702 RID: 5890
	private AudioSource audioPlayer;

	// Token: 0x04001703 RID: 5891
	private float nextHittableTimestamp;

	// Token: 0x04001704 RID: 5892
	private float resetAtTimestamp;

	// Token: 0x04001705 RID: 5893
	private Coroutine resetCoroutine;

	// Token: 0x04001706 RID: 5894
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private int _Data;
}
