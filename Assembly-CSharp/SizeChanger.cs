using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005DE RID: 1502
public class SizeChanger : GorillaTriggerBox
{
	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x06002549 RID: 9545 RVA: 0x000B8C64 File Offset: 0x000B6E64
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

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x0600254A RID: 9546 RVA: 0x000B8CA4 File Offset: 0x000B6EA4
	public SizeChanger.ChangerType MyType
	{
		get
		{
			return this.myType;
		}
	}

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600254B RID: 9547 RVA: 0x000B8CAC File Offset: 0x000B6EAC
	public float MaxScale
	{
		get
		{
			return this.maxScale;
		}
	}

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600254C RID: 9548 RVA: 0x000B8CB4 File Offset: 0x000B6EB4
	public float MinScale
	{
		get
		{
			return this.minScale;
		}
	}

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x0600254D RID: 9549 RVA: 0x000B8CBC File Offset: 0x000B6EBC
	public Transform StartPos
	{
		get
		{
			return this.startPos;
		}
	}

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x0600254E RID: 9550 RVA: 0x000B8CC4 File Offset: 0x000B6EC4
	public Transform EndPos
	{
		get
		{
			return this.endPos;
		}
	}

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x0600254F RID: 9551 RVA: 0x000B8CCC File Offset: 0x000B6ECC
	public float StaticEasing
	{
		get
		{
			return this.staticEasing;
		}
	}

	// Token: 0x06002550 RID: 9552 RVA: 0x000B8CD4 File Offset: 0x000B6ED4
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000B8CF8 File Offset: 0x000B6EF8
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

	// Token: 0x06002552 RID: 9554 RVA: 0x000B8D74 File Offset: 0x000B6F74
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

	// Token: 0x06002553 RID: 9555 RVA: 0x000B8DED File Offset: 0x000B6FED
	public void AddEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerEnter;
		}
	}

	// Token: 0x06002554 RID: 9556 RVA: 0x000B8E09 File Offset: 0x000B7009
	public void RemoveEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerEnter;
		}
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x000B8E25 File Offset: 0x000B7025
	public void AddExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerExit;
		}
	}

	// Token: 0x06002556 RID: 9558 RVA: 0x000B8E41 File Offset: 0x000B7041
	public void RemoveExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerExit;
		}
	}

	// Token: 0x06002557 RID: 9559 RVA: 0x000B8E60 File Offset: 0x000B7060
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

	// Token: 0x06002558 RID: 9560 RVA: 0x000B8E9D File Offset: 0x000B709D
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

	// Token: 0x06002559 RID: 9561 RVA: 0x000B8ED4 File Offset: 0x000B70D4
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

	// Token: 0x0600255A RID: 9562 RVA: 0x000B8F11 File Offset: 0x000B7111
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

	// Token: 0x0600255B RID: 9563 RVA: 0x000B8F38 File Offset: 0x000B7138
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

	// Token: 0x0600255C RID: 9564 RVA: 0x000B8FA8 File Offset: 0x000B71A8
	public void SetScaleCenterPoint(Transform centerPoint)
	{
		this.scaleAwayFromPoint = centerPoint;
	}

	// Token: 0x0600255D RID: 9565 RVA: 0x000B8FB1 File Offset: 0x000B71B1
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

	// Token: 0x04002982 RID: 10626
	[SerializeField]
	private SizeChanger.ChangerType myType;

	// Token: 0x04002983 RID: 10627
	[SerializeField]
	private float staticEasing;

	// Token: 0x04002984 RID: 10628
	[SerializeField]
	private float maxScale;

	// Token: 0x04002985 RID: 10629
	[SerializeField]
	private float minScale;

	// Token: 0x04002986 RID: 10630
	private Collider myCollider;

	// Token: 0x04002987 RID: 10631
	[SerializeField]
	private Transform startPos;

	// Token: 0x04002988 RID: 10632
	[SerializeField]
	private Transform endPos;

	// Token: 0x04002989 RID: 10633
	[SerializeField]
	private SizeChangerTrigger enterTrigger;

	// Token: 0x0400298A RID: 10634
	[SerializeField]
	private SizeChangerTrigger exitTrigger;

	// Token: 0x0400298B RID: 10635
	[SerializeField]
	private Transform scaleAwayFromPoint;

	// Token: 0x0400298C RID: 10636
	[SerializeField]
	private SizeChangerTrigger exitOnEnterTrigger;

	// Token: 0x0400298D RID: 10637
	public bool alwaysControlWhenEntered;

	// Token: 0x0400298E RID: 10638
	public int priority;

	// Token: 0x0400298F RID: 10639
	public bool aprilFoolsEnabled;

	// Token: 0x04002990 RID: 10640
	public float startRadius;

	// Token: 0x04002991 RID: 10641
	public float endRadius;

	// Token: 0x04002992 RID: 10642
	public bool affectLayerA = true;

	// Token: 0x04002993 RID: 10643
	public bool affectLayerB = true;

	// Token: 0x04002994 RID: 10644
	public bool affectLayerC = true;

	// Token: 0x04002995 RID: 10645
	public bool affectLayerD = true;

	// Token: 0x04002996 RID: 10646
	public UnityAction OnExit;

	// Token: 0x04002997 RID: 10647
	public UnityAction OnEnter;

	// Token: 0x04002998 RID: 10648
	private HashSet<VRRig> unregisteredPresentRigs;

	// Token: 0x020005DF RID: 1503
	public enum ChangerType
	{
		// Token: 0x0400299A RID: 10650
		Static,
		// Token: 0x0400299B RID: 10651
		Continuous,
		// Token: 0x0400299C RID: 10652
		Radius
	}
}
