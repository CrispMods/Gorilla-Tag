using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005DF RID: 1503
public class SizeChanger : GorillaTriggerBox
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06002551 RID: 9553 RVA: 0x00103CFC File Offset: 0x00101EFC
	public int SizeLayerMask
	{
		get
		{
			int num = 0;
			if (this.affectLayerA)
			{
				num |= 1;
			}
			if (this.affectLayerB)
			{
				num |= 2;
			}
			if (this.affectLayerC)
			{
				num |= 4;
			}
			if (this.affectLayerD)
			{
				num |= 8;
			}
			return num;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06002552 RID: 9554 RVA: 0x000484B3 File Offset: 0x000466B3
	public SizeChanger.ChangerType MyType
	{
		get
		{
			return this.myType;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06002553 RID: 9555 RVA: 0x000484BB File Offset: 0x000466BB
	public float MaxScale
	{
		get
		{
			return this.maxScale;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06002554 RID: 9556 RVA: 0x000484C3 File Offset: 0x000466C3
	public float MinScale
	{
		get
		{
			return this.minScale;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06002555 RID: 9557 RVA: 0x000484CB File Offset: 0x000466CB
	public Transform StartPos
	{
		get
		{
			return this.startPos;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06002556 RID: 9558 RVA: 0x000484D3 File Offset: 0x000466D3
	public Transform EndPos
	{
		get
		{
			return this.endPos;
		}
	}

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06002557 RID: 9559 RVA: 0x000484DB File Offset: 0x000466DB
	public float StaticEasing
	{
		get
		{
			return this.staticEasing;
		}
	}

	// Token: 0x06002558 RID: 9560 RVA: 0x000484E3 File Offset: 0x000466E3
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x06002559 RID: 9561 RVA: 0x00103D3C File Offset: 0x00101F3C
	public void OnEnable()
	{
		if (this.enterTrigger)
		{
			this.enterTrigger.OnEnter += this.OnTriggerEnter;
		}
		if (this.exitTrigger)
		{
			this.exitTrigger.OnExit += this.OnTriggerExit;
		}
		if (this.exitOnEnterTrigger)
		{
			this.exitOnEnterTrigger.OnEnter += this.OnTriggerExit;
		}
	}

	// Token: 0x0600255A RID: 9562 RVA: 0x00103DB8 File Offset: 0x00101FB8
	public void OnDisable()
	{
		if (this.enterTrigger)
		{
			this.enterTrigger.OnEnter -= this.OnTriggerEnter;
		}
		if (this.exitTrigger)
		{
			this.exitTrigger.OnExit -= this.OnTriggerExit;
		}
		if (this.exitOnEnterTrigger)
		{
			this.exitOnEnterTrigger.OnEnter -= this.OnTriggerExit;
		}
	}

	// Token: 0x0600255B RID: 9563 RVA: 0x00048507 File Offset: 0x00046707
	public void AddEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerEnter;
		}
	}

	// Token: 0x0600255C RID: 9564 RVA: 0x00048523 File Offset: 0x00046723
	public void RemoveEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerEnter;
		}
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x0004853F File Offset: 0x0004673F
	public void AddExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerExit;
		}
	}

	// Token: 0x0600255E RID: 9566 RVA: 0x0004855B File Offset: 0x0004675B
	public void RemoveExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerExit;
		}
	}

	// Token: 0x0600255F RID: 9567 RVA: 0x00103E34 File Offset: 0x00102034
	public void OnTriggerEnter(Collider other)
	{
		if (!other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		this.acceptRig(component);
	}

	// Token: 0x06002560 RID: 9568 RVA: 0x00048577 File Offset: 0x00046777
	public void acceptRig(VRRig rig)
	{
		if (!rig.sizeManager.touchingChangers.Contains(this))
		{
			rig.sizeManager.touchingChangers.Add(this);
		}
		UnityAction onEnter = this.OnEnter;
		if (onEnter == null)
		{
			return;
		}
		onEnter();
	}

	// Token: 0x06002561 RID: 9569 RVA: 0x00103E74 File Offset: 0x00102074
	public void OnTriggerExit(Collider other)
	{
		if (!other.GetComponent<SphereCollider>())
		{
			return;
		}
		VRRig component = other.attachedRigidbody.gameObject.GetComponent<VRRig>();
		if (component == null)
		{
			return;
		}
		this.unacceptRig(component);
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x000485AD File Offset: 0x000467AD
	public void unacceptRig(VRRig rig)
	{
		rig.sizeManager.touchingChangers.Remove(this);
		UnityAction onExit = this.OnExit;
		if (onExit == null)
		{
			return;
		}
		onExit();
	}

	// Token: 0x06002563 RID: 9571 RVA: 0x00103EB4 File Offset: 0x001020B4
	public Vector3 ClosestPoint(Vector3 position)
	{
		if (this.enterTrigger && this.exitTrigger)
		{
			Vector3 vector = this.enterTrigger.ClosestPoint(position);
			Vector3 vector2 = this.exitTrigger.ClosestPoint(position);
			if (Vector3.Distance(position, vector) >= Vector3.Distance(position, vector2))
			{
				return vector2;
			}
			return vector;
		}
		else
		{
			if (this.myCollider)
			{
				return this.myCollider.ClosestPoint(position);
			}
			return position;
		}
	}

	// Token: 0x06002564 RID: 9572 RVA: 0x000485D1 File Offset: 0x000467D1
	public void SetScaleCenterPoint(Transform centerPoint)
	{
		this.scaleAwayFromPoint = centerPoint;
	}

	// Token: 0x06002565 RID: 9573 RVA: 0x000485DA File Offset: 0x000467DA
	public bool TryGetScaleCenterPoint(out Vector3 centerPoint)
	{
		if (this.scaleAwayFromPoint != null)
		{
			centerPoint = this.scaleAwayFromPoint.position;
			return true;
		}
		centerPoint = Vector3.zero;
		return false;
	}

	// Token: 0x04002988 RID: 10632
	[SerializeField]
	private SizeChanger.ChangerType myType;

	// Token: 0x04002989 RID: 10633
	[SerializeField]
	private float staticEasing;

	// Token: 0x0400298A RID: 10634
	[SerializeField]
	private float maxScale;

	// Token: 0x0400298B RID: 10635
	[SerializeField]
	private float minScale;

	// Token: 0x0400298C RID: 10636
	private Collider myCollider;

	// Token: 0x0400298D RID: 10637
	[SerializeField]
	private Transform startPos;

	// Token: 0x0400298E RID: 10638
	[SerializeField]
	private Transform endPos;

	// Token: 0x0400298F RID: 10639
	[SerializeField]
	private SizeChangerTrigger enterTrigger;

	// Token: 0x04002990 RID: 10640
	[SerializeField]
	private SizeChangerTrigger exitTrigger;

	// Token: 0x04002991 RID: 10641
	[SerializeField]
	private Transform scaleAwayFromPoint;

	// Token: 0x04002992 RID: 10642
	[SerializeField]
	private SizeChangerTrigger exitOnEnterTrigger;

	// Token: 0x04002993 RID: 10643
	public bool alwaysControlWhenEntered;

	// Token: 0x04002994 RID: 10644
	public int priority;

	// Token: 0x04002995 RID: 10645
	public bool aprilFoolsEnabled;

	// Token: 0x04002996 RID: 10646
	public float startRadius;

	// Token: 0x04002997 RID: 10647
	public float endRadius;

	// Token: 0x04002998 RID: 10648
	public bool affectLayerA = true;

	// Token: 0x04002999 RID: 10649
	public bool affectLayerB = true;

	// Token: 0x0400299A RID: 10650
	public bool affectLayerC = true;

	// Token: 0x0400299B RID: 10651
	public bool affectLayerD = true;

	// Token: 0x0400299C RID: 10652
	public UnityAction OnExit;

	// Token: 0x0400299D RID: 10653
	public UnityAction OnEnter;

	// Token: 0x0400299E RID: 10654
	private HashSet<VRRig> unregisteredPresentRigs;

	// Token: 0x020005E0 RID: 1504
	public enum ChangerType
	{
		// Token: 0x040029A0 RID: 10656
		Static,
		// Token: 0x040029A1 RID: 10657
		Continuous,
		// Token: 0x040029A2 RID: 10658
		Radius
	}
}
