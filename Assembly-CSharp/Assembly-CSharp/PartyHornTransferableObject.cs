using System;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000CC RID: 204
public class PartyHornTransferableObject : TransferrableObject
{
	// Token: 0x0600055A RID: 1370 RVA: 0x0001FEDA File Offset: 0x0001E0DA
	internal override void OnEnable()
	{
		base.OnEnable();
		this.localHead = GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform;
		this.InitToDefault();
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001FF07 File Offset: 0x0001E107
	internal override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0001FF0F File Offset: 0x0001E10F
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001FF20 File Offset: 0x0001E120
	protected Vector3 CalcMouthPiecePos()
	{
		Transform transform = base.transform;
		Vector3 vector = transform.position;
		if (this.mouthPiece)
		{
			vector += transform.InverseTransformPoint(this.mouthPiece.position);
		}
		else
		{
			vector += transform.forward * this.mouthPieceZOffset;
		}
		return vector;
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001FF7C File Offset: 0x0001E17C
	protected override void LateUpdateLocal()
	{
		base.LateUpdateLocal();
		if (!base.InHand())
		{
			return;
		}
		if (this.itemState != TransferrableObject.ItemStates.State0)
		{
			return;
		}
		if (!GorillaParent.hasInstance)
		{
			return;
		}
		Transform transform = base.transform;
		Vector3 b = this.CalcMouthPiecePos();
		float num = this.mouthPieceRadius * this.mouthPieceRadius * GTPlayer.Instance.scale * GTPlayer.Instance.scale;
		bool flag = (this.localHead.TransformPoint(this.mouthOffset) - b).sqrMagnitude < num;
		if (this.soundActivated && PhotonNetwork.InRoom)
		{
			bool flag2;
			if (flag)
			{
				GorillaTagger instance = GorillaTagger.Instance;
				if (instance == null)
				{
					flag2 = false;
				}
				else
				{
					Recorder myRecorder = instance.myRecorder;
					bool? flag3 = (myRecorder != null) ? new bool?(myRecorder.IsCurrentlyTransmitting) : null;
					bool flag4 = true;
					flag2 = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
				}
			}
			else
			{
				flag2 = false;
			}
			flag = flag2;
		}
		for (int i = 0; i < GorillaParent.instance.vrrigs.Count; i++)
		{
			VRRig vrrig = GorillaParent.instance.vrrigs[i];
			if (vrrig.head == null || vrrig.head.rigTarget == null || flag)
			{
				break;
			}
			flag = ((vrrig.head.rigTarget.transform.TransformPoint(this.mouthOffset) - b).sqrMagnitude < num);
			if (this.soundActivated)
			{
				bool flag5;
				if (flag)
				{
					RigContainer rigContainer = vrrig.rigContainer;
					if (rigContainer == null)
					{
						flag5 = false;
					}
					else
					{
						PhotonVoiceView voice = rigContainer.Voice;
						bool? flag3 = (voice != null) ? new bool?(voice.IsSpeaking) : null;
						bool flag4 = true;
						flag5 = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
					}
				}
				else
				{
					flag5 = false;
				}
				flag = flag5;
			}
		}
		this.itemState = (flag ? TransferrableObject.ItemStates.State1 : this.itemState);
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0002014C File Offset: 0x0001E34C
	protected override void LateUpdateShared()
	{
		base.LateUpdateShared();
		if (TransferrableObject.ItemStates.State1 != this.itemState)
		{
			return;
		}
		if (!this.localWasActivated)
		{
			this.effectsGameObject.SetActive(true);
			this.cooldownRemaining = this.cooldown;
			this.localWasActivated = true;
			UnityEvent onCooldownStart = this.OnCooldownStart;
			if (onCooldownStart != null)
			{
				onCooldownStart.Invoke();
			}
		}
		this.cooldownRemaining -= Time.deltaTime;
		if (this.cooldownRemaining <= 0f)
		{
			this.InitToDefault();
		}
	}

	// Token: 0x06000560 RID: 1376 RVA: 0x000201C6 File Offset: 0x0001E3C6
	private void InitToDefault()
	{
		this.itemState = TransferrableObject.ItemStates.State0;
		this.effectsGameObject.SetActive(false);
		this.cooldownRemaining = this.cooldown;
		this.localWasActivated = false;
		UnityEvent onCooldownReset = this.OnCooldownReset;
		if (onCooldownReset == null)
		{
			return;
		}
		onCooldownReset.Invoke();
	}

	// Token: 0x0400062D RID: 1581
	[Tooltip("This GameObject will activate when held to any gorilla's mouth.")]
	public GameObject effectsGameObject;

	// Token: 0x0400062E RID: 1582
	public float cooldown = 2f;

	// Token: 0x0400062F RID: 1583
	public float mouthPieceZOffset = -0.18f;

	// Token: 0x04000630 RID: 1584
	public float mouthPieceRadius = 0.05f;

	// Token: 0x04000631 RID: 1585
	public Transform mouthPiece;

	// Token: 0x04000632 RID: 1586
	public Vector3 mouthOffset = new Vector3(0f, 0.02f, 0.17f);

	// Token: 0x04000633 RID: 1587
	public bool soundActivated;

	// Token: 0x04000634 RID: 1588
	public UnityEvent OnCooldownStart;

	// Token: 0x04000635 RID: 1589
	public UnityEvent OnCooldownReset;

	// Token: 0x04000636 RID: 1590
	private float cooldownRemaining;

	// Token: 0x04000637 RID: 1591
	private Transform localHead;

	// Token: 0x04000638 RID: 1592
	private PartyHornTransferableObject.PartyHornState partyHornStateLastFrame;

	// Token: 0x04000639 RID: 1593
	private bool localWasActivated;

	// Token: 0x020000CD RID: 205
	private enum PartyHornState
	{
		// Token: 0x0400063B RID: 1595
		None = 1,
		// Token: 0x0400063C RID: 1596
		CoolingDown
	}
}
