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
	// Token: 0x060014B2 RID: 5298 RVA: 0x00065B88 File Offset: 0x00063D88
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

	// Token: 0x060014B3 RID: 5299 RVA: 0x00065BE6 File Offset: 0x00063DE6
	protected override void Start()
	{
		base.Start();
		RoomSystem.LeftRoomEvent = (Action)Delegate.Combine(RoomSystem.LeftRoomEvent, new Action(this.OnLeftRoom));
	}

	// Token: 0x060014B4 RID: 5300 RVA: 0x00065C0E File Offset: 0x00063E0E
	private void SetInitialState()
	{
		this.networkedScore.Value = 0;
		this.nextHittableTimestamp = 0f;
		this.audioPlayer.GTStop();
	}

	// Token: 0x060014B5 RID: 5301 RVA: 0x00065C32 File Offset: 0x00063E32
	public void OnLeftRoom()
	{
		this.SetInitialState();
	}

	// Token: 0x060014B6 RID: 5302 RVA: 0x00065C3A File Offset: 0x00063E3A
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

	// Token: 0x060014B7 RID: 5303 RVA: 0x00065C62 File Offset: 0x00063E62
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

	// Token: 0x060014B8 RID: 5304 RVA: 0x00065C71 File Offset: 0x00063E71
	private void ProjectileHitReciever(SlingshotProjectile projectile, Collision collision)
	{
		this.TargetHit(projectile.launchPosition, collision.contacts[0].point);
	}

	// Token: 0x060014B9 RID: 5305 RVA: 0x00065C90 File Offset: 0x00063E90
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
	// (get) Token: 0x060014BA RID: 5306 RVA: 0x00065D68 File Offset: 0x00063F68
	// (set) Token: 0x060014BB RID: 5307 RVA: 0x00065D8E File Offset: 0x00063F8E
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

	// Token: 0x060014BC RID: 5308 RVA: 0x00065DB5 File Offset: 0x00063FB5
	public override void WriteDataFusion()
	{
		this.Data = this.networkedScore.Value;
	}

	// Token: 0x060014BD RID: 5309 RVA: 0x00065DC8 File Offset: 0x00063FC8
	public override void ReadDataFusion()
	{
		int data = this.Data;
		if (data != this.networkedScore.Value)
		{
			this.PlayAudio(this.networkedScore.Value, data);
		}
		this.networkedScore.Value = data;
	}

	// Token: 0x060014BE RID: 5310 RVA: 0x00065E08 File Offset: 0x00064008
	protected override void WriteDataPUN(PhotonStream stream, PhotonMessageInfo info)
	{
		if (!info.Sender.IsMasterClient)
		{
			return;
		}
		stream.SendNext(this.networkedScore.Value);
	}

	// Token: 0x060014BF RID: 5311 RVA: 0x00065E30 File Offset: 0x00064030
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

	// Token: 0x060014C0 RID: 5312 RVA: 0x00065E83 File Offset: 0x00064083
	public void PlayAudio(int oldScore, int newScore)
	{
		if (oldScore > newScore && !this.scoreIsDistance)
		{
			this.audioPlayer.GTPlayOneShot(this.audioClips[1], 1f);
			return;
		}
		this.audioPlayer.GTPlayOneShot(this.audioClips[0], 1f);
	}

	// Token: 0x060014C1 RID: 5313 RVA: 0x00065EC2 File Offset: 0x000640C2
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

	// Token: 0x060014C3 RID: 5315 RVA: 0x00065EE0 File Offset: 0x000640E0
	[WeaverGenerated]
	public override void CopyBackingFieldsToState(bool A_1)
	{
		base.CopyBackingFieldsToState(A_1);
		this.Data = this._Data;
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x00065EF8 File Offset: 0x000640F8
	[WeaverGenerated]
	public override void CopyStateToBackingFields()
	{
		base.CopyStateToBackingFields();
		this._Data = this.Data;
	}

	// Token: 0x040016FB RID: 5883
	[SerializeField]
	private WatchableIntSO networkedScore;

	// Token: 0x040016FC RID: 5884
	[SerializeField]
	private int hitCooldownTime = 1;

	// Token: 0x040016FD RID: 5885
	[SerializeField]
	private bool testPress;

	// Token: 0x040016FE RID: 5886
	[SerializeField]
	private AudioClip[] audioClips;

	// Token: 0x040016FF RID: 5887
	[SerializeField]
	private bool scoreIsDistance;

	// Token: 0x04001700 RID: 5888
	[SerializeField]
	private float resetAfterDuration;

	// Token: 0x04001701 RID: 5889
	private AudioSource audioPlayer;

	// Token: 0x04001702 RID: 5890
	private float nextHittableTimestamp;

	// Token: 0x04001703 RID: 5891
	private float resetAtTimestamp;

	// Token: 0x04001704 RID: 5892
	private Coroutine resetCoroutine;

	// Token: 0x04001705 RID: 5893
	[WeaverGenerated]
	[SerializeField]
	[DefaultForProperty("Data", 0, 1)]
	[DrawIf("IsEditorWritable", true, CompareOperator.Equal, DrawIfMode.ReadOnly)]
	private int _Data;
}
