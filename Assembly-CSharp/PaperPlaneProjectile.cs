using System;
using GorillaTag.Reactions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200043F RID: 1087
public class PaperPlaneProjectile : MonoBehaviour
{
	// Token: 0x14000049 RID: 73
	// (add) Token: 0x06001ABE RID: 6846 RVA: 0x000839B0 File Offset: 0x00081BB0
	// (remove) Token: 0x06001ABF RID: 6847 RVA: 0x000839E8 File Offset: 0x00081BE8
	public event PaperPlaneProjectile.PaperPlaneHit OnHit;

	// Token: 0x170002EB RID: 747
	// (get) Token: 0x06001AC0 RID: 6848 RVA: 0x00083A1D File Offset: 0x00081C1D
	public new Transform transform
	{
		get
		{
			return this._tCached;
		}
	}

	// Token: 0x170002EC RID: 748
	// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x00083A25 File Offset: 0x00081C25
	public VRRig MyRig
	{
		get
		{
			return this.myRig;
		}
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x00083A2D File Offset: 0x00081C2D
	private void Awake()
	{
		this._tCached = base.transform;
		this.spawnWorldEffects = base.GetComponent<SpawnWorldEffects>();
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x00083A47 File Offset: 0x00081C47
	private void Start()
	{
		this.ResetProjectile();
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x00083A4F File Offset: 0x00081C4F
	public void ResetProjectile()
	{
		this._timeElapsed = 0f;
		this.flyingObject.SetActive(true);
		this.crashingObject.SetActive(false);
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x00083A74 File Offset: 0x00081C74
	public void Launch(Vector3 startPos, Quaternion startRot, Vector3 vel)
	{
		base.gameObject.SetActive(true);
		this.ResetProjectile();
		this.transform.position = startPos;
		if (this.enableRotation)
		{
			this.transform.rotation = startRot;
		}
		else
		{
			this.transform.LookAt(this.transform.position + vel.normalized);
		}
		this._direction = vel.normalized;
		this._speed = Mathf.Clamp(vel.sqrMagnitude / 2f, this.minSpeed, this.maxSpeed);
		this._stopped = false;
		this.scaleFactor = 0.7f * (this.transform.lossyScale.x - 1f + 1.4285715f);
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x00083B38 File Offset: 0x00081D38
	private void Update()
	{
		if (this._stopped)
		{
			if (!this.crashingObject.gameObject.activeSelf)
			{
				if (ObjectPools.instance)
				{
					ObjectPools.instance.Destroy(base.gameObject);
					return;
				}
				base.gameObject.SetActive(false);
			}
			return;
		}
		this._timeElapsed += Time.deltaTime;
		this.nextPos = this.transform.position + this._direction * this._speed * Time.deltaTime * this.scaleFactor;
		if (this._timeElapsed < this.maxFlightTime && (this._timeElapsed < this.minFlightTime || Physics.RaycastNonAlloc(this.transform.position, this.nextPos - this.transform.position, this.results, Vector3.Distance(this.transform.position, this.nextPos), this.layerMask.value) == 0))
		{
			this.transform.position = this.nextPos;
			this.transform.Rotate(Mathf.Sin(this._timeElapsed) * 10f * Time.deltaTime, 0f, 0f);
			return;
		}
		if (this._timeElapsed < this.maxFlightTime)
		{
			SlingshotProjectileHitNotifier slingshotProjectileHitNotifier;
			if (this.results[0].collider.TryGetComponent<SlingshotProjectileHitNotifier>(out slingshotProjectileHitNotifier))
			{
				slingshotProjectileHitNotifier.InvokeHit(this, this.results[0].collider);
			}
			if (this.spawnWorldEffects != null)
			{
				this.spawnWorldEffects.RequestSpawn(this.nextPos);
			}
		}
		this._stopped = true;
		this._timeElapsed = 0f;
		PaperPlaneProjectile.PaperPlaneHit onHit = this.OnHit;
		if (onHit != null)
		{
			onHit(this.nextPos);
		}
		this.OnHit = null;
		this.flyingObject.SetActive(false);
		this.crashingObject.SetActive(true);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x00083D2A File Offset: 0x00081F2A
	internal void SetVRRig(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x04001D91 RID: 7569
	private const float speedScaleRatio = 0.7f;

	// Token: 0x04001D92 RID: 7570
	public Vector3 startPos;

	// Token: 0x04001D93 RID: 7571
	public Vector3 endPos;

	// Token: 0x04001D94 RID: 7572
	[FormerlySerializedAs("_flyTimeOut")]
	[Range(1f, 128f)]
	public float flyTimeOut = 32f;

	// Token: 0x04001D96 RID: 7574
	[Space]
	public float curveTime = 0.4f;

	// Token: 0x04001D97 RID: 7575
	[Space]
	public Vector3 curveDirection;

	// Token: 0x04001D98 RID: 7576
	public float curveDistance = 9.8f;

	// Token: 0x04001D99 RID: 7577
	[Space]
	[NonSerialized]
	private float _timeElapsed;

	// Token: 0x04001D9A RID: 7578
	[NonSerialized]
	private float _speed;

	// Token: 0x04001D9B RID: 7579
	[NonSerialized]
	private Vector3 _direction;

	// Token: 0x04001D9C RID: 7580
	[NonSerialized]
	private bool _stopped;

	// Token: 0x04001D9D RID: 7581
	private Transform _tCached;

	// Token: 0x04001D9E RID: 7582
	private SpawnWorldEffects spawnWorldEffects;

	// Token: 0x04001D9F RID: 7583
	private Vector3 nextPos;

	// Token: 0x04001DA0 RID: 7584
	private RaycastHit[] results = new RaycastHit[1];

	// Token: 0x04001DA1 RID: 7585
	[SerializeField]
	private float maxFlightTime = 7.5f;

	// Token: 0x04001DA2 RID: 7586
	[SerializeField]
	private float minFlightTime = 0.5f;

	// Token: 0x04001DA3 RID: 7587
	[SerializeField]
	private float maxSpeed = 10f;

	// Token: 0x04001DA4 RID: 7588
	[SerializeField]
	private float minSpeed = 1f;

	// Token: 0x04001DA5 RID: 7589
	[SerializeField]
	private bool enableRotation;

	// Token: 0x04001DA6 RID: 7590
	[SerializeField]
	private GameObject flyingObject;

	// Token: 0x04001DA7 RID: 7591
	[SerializeField]
	private GameObject crashingObject;

	// Token: 0x04001DA8 RID: 7592
	[SerializeField]
	private LayerMask layerMask;

	// Token: 0x04001DA9 RID: 7593
	private VRRig myRig;

	// Token: 0x04001DAA RID: 7594
	private float scaleFactor;

	// Token: 0x02000440 RID: 1088
	// (Invoke) Token: 0x06001ACA RID: 6858
	public delegate void PaperPlaneHit(Vector3 endPoint);
}
