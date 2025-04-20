using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020005EC RID: 1516
public class SizeChanger : GorillaTriggerBox
{
	// Token: 0x170003DF RID: 991
	// (get) Token: 0x060025AB RID: 9643 RVA: 0x00106C38 File Offset: 0x00104E38
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

	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060025AC RID: 9644 RVA: 0x0004988A File Offset: 0x00047A8A
	public SizeChanger.ChangerType MyType
	{
		get
		{
			return this.myType;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060025AD RID: 9645 RVA: 0x00049892 File Offset: 0x00047A92
	public float MaxScale
	{
		get
		{
			return this.maxScale;
		}
	}

	// Token: 0x170003E2 RID: 994
	// (get) Token: 0x060025AE RID: 9646 RVA: 0x0004989A File Offset: 0x00047A9A
	public float MinScale
	{
		get
		{
			return this.minScale;
		}
	}

	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x060025AF RID: 9647 RVA: 0x000498A2 File Offset: 0x00047AA2
	public Transform StartPos
	{
		get
		{
			return this.startPos;
		}
	}

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000498AA File Offset: 0x00047AAA
	public Transform EndPos
	{
		get
		{
			return this.endPos;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000498B2 File Offset: 0x00047AB2
	public float StaticEasing
	{
		get
		{
			return this.staticEasing;
		}
	}

	// Token: 0x060025B2 RID: 9650 RVA: 0x000498BA File Offset: 0x00047ABA
	private void Awake()
	{
		this.minScale = Mathf.Max(this.minScale, 0.01f);
		this.myCollider = base.GetComponent<Collider>();
	}

	// Token: 0x060025B3 RID: 9651 RVA: 0x00106C78 File Offset: 0x00104E78
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

	// Token: 0x060025B4 RID: 9652 RVA: 0x00106CF4 File Offset: 0x00104EF4
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

	// Token: 0x060025B5 RID: 9653 RVA: 0x000498DE File Offset: 0x00047ADE
	public void AddEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerEnter;
		}
	}

	// Token: 0x060025B6 RID: 9654 RVA: 0x000498FA File Offset: 0x00047AFA
	public void RemoveEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerEnter;
		}
	}

	// Token: 0x060025B7 RID: 9655 RVA: 0x00049916 File Offset: 0x00047B16
	public void AddExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter += this.OnTriggerExit;
		}
	}

	// Token: 0x060025B8 RID: 9656 RVA: 0x00049932 File Offset: 0x00047B32
	public void RemoveExitOnEnterTrigger(SizeChangerTrigger trigger)
	{
		if (trigger)
		{
			trigger.OnEnter -= this.OnTriggerExit;
		}
	}

	// Token: 0x060025B9 RID: 9657 RVA: 0x00106D70 File Offset: 0x00104F70
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

	// Token: 0x060025BA RID: 9658 RVA: 0x0004994E File Offset: 0x00047B4E
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

	// Token: 0x060025BB RID: 9659 RVA: 0x00106DB0 File Offset: 0x00104FB0
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

	// Token: 0x060025BC RID: 9660 RVA: 0x00049984 File Offset: 0x00047B84
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

	// Token: 0x060025BD RID: 9661 RVA: 0x00106DF0 File Offset: 0x00104FF0
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

	// Token: 0x060025BE RID: 9662 RVA: 0x000499A8 File Offset: 0x00047BA8
	public void SetScaleCenterPoint(Transform centerPoint)
	{
		this.scaleAwayFromPoint = centerPoint;
	}

	// Token: 0x060025BF RID: 9663 RVA: 0x000499B1 File Offset: 0x00047BB1
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

	// Token: 0x040029E1 RID: 10721
	[SerializeField]
	private SizeChanger.ChangerType myType;

	// Token: 0x040029E2 RID: 10722
	[SerializeField]
	private float staticEasing;

	// Token: 0x040029E3 RID: 10723
	[SerializeField]
	private float maxScale;

	// Token: 0x040029E4 RID: 10724
	[SerializeField]
	private float minScale;

	// Token: 0x040029E5 RID: 10725
	private Collider myCollider;

	// Token: 0x040029E6 RID: 10726
	[SerializeField]
	private Transform startPos;

	// Token: 0x040029E7 RID: 10727
	[SerializeField]
	private Transform endPos;

	// Token: 0x040029E8 RID: 10728
	[SerializeField]
	private SizeChangerTrigger enterTrigger;

	// Token: 0x040029E9 RID: 10729
	[SerializeField]
	private SizeChangerTrigger exitTrigger;

	// Token: 0x040029EA RID: 10730
	[SerializeField]
	private Transform scaleAwayFromPoint;

	// Token: 0x040029EB RID: 10731
	[SerializeField]
	private SizeChangerTrigger exitOnEnterTrigger;

	// Token: 0x040029EC RID: 10732
	public bool alwaysControlWhenEntered;

	// Token: 0x040029ED RID: 10733
	public int priority;

	// Token: 0x040029EE RID: 10734
	public bool aprilFoolsEnabled;

	// Token: 0x040029EF RID: 10735
	public float startRadius;

	// Token: 0x040029F0 RID: 10736
	public float endRadius;

	// Token: 0x040029F1 RID: 10737
	public bool affectLayerA = true;

	// Token: 0x040029F2 RID: 10738
	public bool affectLayerB = true;

	// Token: 0x040029F3 RID: 10739
	public bool affectLayerC = true;

	// Token: 0x040029F4 RID: 10740
	public bool affectLayerD = true;

	// Token: 0x040029F5 RID: 10741
	public UnityAction OnExit;

	// Token: 0x040029F6 RID: 10742
	public UnityAction OnEnter;

	// Token: 0x040029F7 RID: 10743
	private HashSet<VRRig> unregisteredPresentRigs;

	// Token: 0x020005ED RID: 1517
	public enum ChangerType
	{
		// Token: 0x040029F9 RID: 10745
		Static,
		// Token: 0x040029FA RID: 10746
		Continuous,
		// Token: 0x040029FB RID: 10747
		Radius
	}
}
