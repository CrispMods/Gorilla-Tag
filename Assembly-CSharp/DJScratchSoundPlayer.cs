using System;
using GorillaExtensions;
using GorillaTag;
using GorillaTag.CosmeticSystem;
using UnityEngine;

// Token: 0x02000150 RID: 336
public class DJScratchSoundPlayer : MonoBehaviour, ISpawnable
{
	// Token: 0x170000CA RID: 202
	// (get) Token: 0x06000889 RID: 2185 RVA: 0x0002EC1D File Offset: 0x0002CE1D
	// (set) Token: 0x0600088A RID: 2186 RVA: 0x0002EC25 File Offset: 0x0002CE25
	public bool IsSpawned { get; set; }

	// Token: 0x170000CB RID: 203
	// (get) Token: 0x0600088B RID: 2187 RVA: 0x0002EC2E File Offset: 0x0002CE2E
	// (set) Token: 0x0600088C RID: 2188 RVA: 0x0002EC36 File Offset: 0x0002CE36
	public ECosmeticSelectSide CosmeticSelectedSide { get; set; }

	// Token: 0x0600088D RID: 2189 RVA: 0x000023F4 File Offset: 0x000005F4
	public void OnDespawn()
	{
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x0002EC40 File Offset: 0x0002CE40
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

	// Token: 0x0600088F RID: 2191 RVA: 0x0002ECD2 File Offset: 0x0002CED2
	private void OnDisable()
	{
		if (this._events.IsNotNull())
		{
			this._events.Activate -= this.OnPlayEvent;
			this._events.Dispose();
		}
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x0002ED0E File Offset: 0x0002CF0E
	public void OnSpawn(VRRig rig)
	{
		this.myRig = rig;
		if (!rig.isLocal)
		{
			this.scratchTableLeft.enabled = false;
			this.scratchTableRight.enabled = false;
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x0002ED37 File Offset: 0x0002CF37
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

	// Token: 0x06000892 RID: 2194 RVA: 0x0002ED78 File Offset: 0x0002CF78
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

	// Token: 0x06000893 RID: 2195 RVA: 0x0002EDF0 File Offset: 0x0002CFF0
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

	// Token: 0x04000A31 RID: 2609
	[SerializeField]
	private SoundBankPlayer scratchForward;

	// Token: 0x04000A32 RID: 2610
	[SerializeField]
	private SoundBankPlayer scratchBack;

	// Token: 0x04000A33 RID: 2611
	[SerializeField]
	private SoundBankPlayer scratchPause;

	// Token: 0x04000A34 RID: 2612
	[SerializeField]
	private SoundBankPlayer scratchResume;

	// Token: 0x04000A35 RID: 2613
	[SerializeField]
	private DJScratchtable scratchTableLeft;

	// Token: 0x04000A36 RID: 2614
	[SerializeField]
	private DJScratchtable scratchTableRight;

	// Token: 0x04000A37 RID: 2615
	private RubberDuckEvents _events;

	// Token: 0x04000A38 RID: 2616
	private VRRig myRig;
}
