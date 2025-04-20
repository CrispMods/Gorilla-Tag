using System;
using GorillaLocomotion;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000D6 RID: 214
public class PartyHornTransferableObject : TransferrableObject
{
	// Token: 0x06000599 RID: 1433 RVA: 0x0003413E File Offset: 0x0003233E
	internal override void OnEnable()
	{
		base.OnEnable();
		this.localHead = GorillaTagger.Instance.offlineVRRig.head.rigTarget.transform;
		this.InitToDefault();
	}

	// Token: 0x0600059A RID: 1434 RVA: 0x0003416B File Offset: 0x0003236B
	internal override void OnDisable()
	{
		base.OnDisable();
	}

	// Token: 0x0600059B RID: 1435 RVA: 0x00034173 File Offset: 0x00032373
	public override void ResetToDefaultState()
	{
		base.ResetToDefaultState();
		this.InitToDefault();
	}

	// Token: 0x0600059C RID: 1436 RVA: 0x00082C14 File Offset: 0x00080E14
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

	// Token: 0x0600059D RID: 1437 RVA: 0x00082C70 File Offset: 0x00080E70
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

	// Token: 0x0600059E RID: 1438 RVA: 0x00082E40 File Offset: 0x00081040
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

	// Token: 0x0600059F RID: 1439 RVA: 0x00034181 File Offset: 0x00032381
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

	// Token: 0x0400066D RID: 1645
	[Tooltip("This GameObject will activate when held to any gorilla's mouth.")]
	public GameObject effectsGameObject;

	// Token: 0x0400066E RID: 1646
	public float cooldown = 2f;

	// Token: 0x0400066F RID: 1647
	public float mouthPieceZOffset = -0.18f;

	// Token: 0x04000670 RID: 1648
	public float mouthPieceRadius = 0.05f;

	// Token: 0x04000671 RID: 1649
	public Transform mouthPiece;

	// Token: 0x04000672 RID: 1650
	public Vector3 mouthOffset = new Vector3(0f, 0.02f, 0.17f);

	// Token: 0x04000673 RID: 1651
	public bool soundActivated;

	// Token: 0x04000674 RID: 1652
	public UnityEvent OnCooldownStart;

	// Token: 0x04000675 RID: 1653
	public UnityEvent OnCooldownReset;

	// Token: 0x04000676 RID: 1654
	private float cooldownRemaining;

	// Token: 0x04000677 RID: 1655
	private Transform localHead;

	// Token: 0x04000678 RID: 1656
	private PartyHornTransferableObject.PartyHornState partyHornStateLastFrame;

	// Token: 0x04000679 RID: 1657
	private bool localWasActivated;

	// Token: 0x020000D7 RID: 215
	private enum PartyHornState
	{
		// Token: 0x0400067B RID: 1659
		None = 1,
		// Token: 0x0400067C RID: 1660
		CoolingDown
	}
}
