using System;
using GorillaExtensions;
using TagEffects;
using UnityEngine;

// Token: 0x02000239 RID: 569
[RequireComponent(typeof(Collider))]
public class HandEffectsTester : MonoBehaviour, IHandEffectsTrigger
{
	// Token: 0x1700013D RID: 317
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0003923B File Offset: 0x0003743B
	public bool Static
	{
		get
		{
			return this.isStatic;
		}
	}

	// Token: 0x1700013E RID: 318
	// (get) Token: 0x06000CFF RID: 3327 RVA: 0x00039243 File Offset: 0x00037443
	Transform IHandEffectsTrigger.Transform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x1700013F RID: 319
	// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0003924B File Offset: 0x0003744B
	VRRig IHandEffectsTrigger.Rig
	{
		get
		{
			return null;
		}
	}

	// Token: 0x17000140 RID: 320
	// (get) Token: 0x06000D01 RID: 3329 RVA: 0x0003924E File Offset: 0x0003744E
	IHandEffectsTrigger.Mode IHandEffectsTrigger.EffectMode
	{
		get
		{
			return this.mode;
		}
	}

	// Token: 0x17000141 RID: 321
	// (get) Token: 0x06000D02 RID: 3330 RVA: 0x00039256 File Offset: 0x00037456
	bool IHandEffectsTrigger.FingersDown
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.FistBump || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000D03 RID: 3331 RVA: 0x0003926D File Offset: 0x0003746D
	bool IHandEffectsTrigger.FingersUp
	{
		get
		{
			return this.mode == IHandEffectsTrigger.Mode.HighFive || this.mode == IHandEffectsTrigger.Mode.HighFive_And_FistBump;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000D04 RID: 3332 RVA: 0x00039283 File Offset: 0x00037483
	public bool RightHand { get; }

	// Token: 0x06000D05 RID: 3333 RVA: 0x0003928B File Offset: 0x0003748B
	private void Awake()
	{
		this.triggerZone = base.GetComponent<Collider>();
	}

	// Token: 0x06000D06 RID: 3334 RVA: 0x00039299 File Offset: 0x00037499
	private void OnEnable()
	{
		if (!HandEffectsTriggerRegistry.HasInstance)
		{
			HandEffectsTriggerRegistry.FindInstance();
		}
		HandEffectsTriggerRegistry.Instance.Register(this);
	}

	// Token: 0x06000D07 RID: 3335 RVA: 0x000392B2 File Offset: 0x000374B2
	private void OnDisable()
	{
		HandEffectsTriggerRegistry.Instance.Unregister(this);
	}

	// Token: 0x17000144 RID: 324
	// (get) Token: 0x06000D08 RID: 3336 RVA: 0x000392BF File Offset: 0x000374BF
	Vector3 IHandEffectsTrigger.Velocity
	{
		get
		{
			if (this.mode == IHandEffectsTrigger.Mode.HighFive)
			{
				return Vector3.zero;
			}
			IHandEffectsTrigger.Mode mode = this.mode;
			return Vector3.zero;
		}
	}

	// Token: 0x17000145 RID: 325
	// (get) Token: 0x06000D09 RID: 3337 RVA: 0x000392DD File Offset: 0x000374DD
	TagEffectPack IHandEffectsTrigger.CosmeticEffectPack
	{
		get
		{
			return this.cosmeticEffectPack;
		}
	}

	// Token: 0x06000D0A RID: 3338 RVA: 0x00030607 File Offset: 0x0002E807
	public void OnTriggerEntered(IHandEffectsTrigger other)
	{
	}

	// Token: 0x06000D0B RID: 3339 RVA: 0x000A0D58 File Offset: 0x0009EF58
	public bool InTriggerZone(IHandEffectsTrigger t)
	{
		if (!(base.transform.position - t.Transform.position).IsShorterThan(this.triggerZone.bounds.size))
		{
			return false;
		}
		RaycastHit raycastHit;
		switch (this.mode)
		{
		case IHandEffectsTrigger.Mode.HighFive:
			return t.FingersUp && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.up), out raycastHit, this.triggerRadius);
		case IHandEffectsTrigger.Mode.FistBump:
			return t.FingersDown && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.right), out raycastHit, this.triggerRadius);
		case IHandEffectsTrigger.Mode.HighFive_And_FistBump:
			return (t.FingersUp && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.up), out raycastHit, this.triggerRadius)) || (t.FingersDown && this.triggerZone.Raycast(new Ray(t.Transform.position, t.Transform.right), out raycastHit, this.triggerRadius));
		}
		return this.triggerZone.Raycast(new Ray(t.Transform.position, this.triggerZone.bounds.center - t.Transform.position), out raycastHit, this.triggerRadius);
	}

	// Token: 0x0400106A RID: 4202
	[SerializeField]
	private TagEffectPack cosmeticEffectPack;

	// Token: 0x0400106B RID: 4203
	private Collider triggerZone;

	// Token: 0x0400106C RID: 4204
	public IHandEffectsTrigger.Mode mode;

	// Token: 0x0400106D RID: 4205
	[SerializeField]
	private float triggerRadius = 0.07f;

	// Token: 0x0400106E RID: 4206
	[SerializeField]
	private bool isStatic = true;
}
