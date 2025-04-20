using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x0200015A RID: 346
public class DJScratchSoundPlayer : MonoBehaviour, ISpawnable
{
	// Token: 0x170000CF RID: 207
	// (get) Token: 0x060008CD RID: 2253 RVA: 0x00036444 File Offset: 0x00034644
	// (set) Token: 0x060008CE RID: 2254 RVA: 0x0003644C File Offset: 0x0003464C
	public bool IsSpawned { get; set; }

	// Token: 0x170000D0 RID: 208
	// (get) Token: 0x060008CF RID: 2255 RVA: 0x00036455 File Offset: 0x00034655
	// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0003645D File Offset: 0x0003465D
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x060008D1 RID: 2257 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnDespawn()
	{
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x0008F9F0 File Offset: 0x0008DBF0
	private void OnEnable()
	{
		if (this._events.IsNull())
		{
			this._events = base.gameObject.GetOrAddComponent<RubberDuckEvents>();
			NetPlayer netPlayer = (this.myRig != null) ? ((this.myRig.creator != null) ? this.myRig.creator : NetworkSystem.Instance.LocalPlayer) : null;
			if (netPlayer != null)
			{
				this._events.Init(netPlayer);
			}
		}
		this._events.Activate += this.OnPlayEvent;
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00036466 File Offset: 0x00034666
	private void OnDisable()
	{
		if (this._events.IsNotNull())
		{
			this._events.Activate -= this.OnPlayEvent;
			this._events.Dispose();
		}
	}

	// Token: 0x060008D4 RID: 2260 RVA: 0x000364A2 File Offset: 0x000346A2
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		if (!rig.isLocal)
		{
			this.scratchTableLeft.enabled = false;
			this.scratchTableRight.enabled = false;
		}
	}

	// Token: 0x060008D5 RID: 2261 RVA: 0x000364CB File Offset: 0x000346CB
	public void Play(ScratchSoundType type, bool isLeft)
	{
		if (this.myRig.isLocal)
		{
			this.PlayLocal(type, isLeft);
			this._events.Activate.RaiseOthers(new object[]
			{
				(int)(type + (isLeft ? 100 : 0))
			});
		}
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x0008FA84 File Offset: 0x0008DC84
	public void OnPlayEvent(int sender, int target, object[] args, PhotonMessageInfoWrapped info)
	{
		if (sender != target)
		{
			return;
		}
		if (info.senderID != this.myRig.creator.ActorNumber)
		{
			return;
		}
		if (args.Length != 1)
		{
			Debug.LogError(string.Format("Invalid DJ Scratch Event - expected 1 arg, got {0}", args.Length));
			return;
		}
		int num = (int)args[0];
		bool flag = num >= 100;
		if (flag)
		{
			num -= 100;
		}
		ScratchSoundType scratchSoundType = (ScratchSoundType)num;
		if (scratchSoundType < ScratchSoundType.Pause || scratchSoundType > ScratchSoundType.Back)
		{
			return;
		}
		this.PlayLocal(scratchSoundType, flag);
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x0008FAFC File Offset: 0x0008DCFC
	public void PlayLocal(ScratchSoundType type, bool isLeft)
	{
		switch (type)
		{
		case ScratchSoundType.Pause:
			(isLeft ? this.scratchTableLeft : this.scratchTableRight).PauseTrack();
			this.scratchPause.Play();
			return;
		case ScratchSoundType.Resume:
			(isLeft ? this.scratchTableLeft : this.scratchTableRight).ResumeTrack();
			this.scratchResume.Play();
			return;
		case ScratchSoundType.Forward:
			this.scratchForward.Play();
			(isLeft ? this.scratchTableLeft : this.scratchTableRight).PauseTrack();
			return;
		case ScratchSoundType.Back:
			this.scratchBack.Play();
			(isLeft ? this.scratchTableLeft : this.scratchTableRight).PauseTrack();
			return;
		default:
			return;
		}
	}

	// Token: 0x04000A74 RID: 2676
	[SerializeField]
	private SoundBankPlayer scratchForward;

	// Token: 0x04000A75 RID: 2677
	[SerializeField]
	private SoundBankPlayer scratchBack;

	// Token: 0x04000A76 RID: 2678
	[SerializeField]
	private SoundBankPlayer scratchPause;

	// Token: 0x04000A77 RID: 2679
	[SerializeField]
	private SoundBankPlayer scratchResume;

	// Token: 0x04000A78 RID: 2680
	[SerializeField]
	private DJScratchtable scratchTableLeft;

	// Token: 0x04000A79 RID: 2681
	[SerializeField]
	private DJScratchtable scratchTableRight;

	// Token: 0x04000A7A RID: 2682
	private RubberDuckEvents _events;

	// Token: 0x04000A7B RID: 2683
	private VRRig myRig;
}
