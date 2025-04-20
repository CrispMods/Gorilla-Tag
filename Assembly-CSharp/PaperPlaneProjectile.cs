using System;
using GorillaTag.Reactions;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200044B RID: 1099
public class PaperPlaneProjectile : MonoBehaviour
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x06001B12 RID: 6930 RVA: 0x000D8834 File Offset: 0x000D6A34
	// (remove) Token: 0x06001B13 RID: 6931 RVA: 0x000D886C File Offset: 0x000D6A6C
	public event PaperPlaneProjectile.PaperPlaneHit OnHit;

	// Token: 0x170002F2 RID: 754
	// (get) Token: 0x06001B14 RID: 6932 RVA: 0x00042653 File Offset: 0x00040853
	public new Transform transform
	{
		get
		{
			return this._tCached;
		}
	}

	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06001B15 RID: 6933 RVA: 0x0004265B File Offset: 0x0004085B
	public VRRig MyRig
	{
		get
		{
			return this.myRig;
		}
	}

	// Token: 0x06001B16 RID: 6934 RVA: 0x00042663 File Offset: 0x00040863
	private void Awake()
	{
		this._tCached = base.transform;
		this.spawnWorldEffects = base.GetComponent<SpawnWorldEffects>();
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x0004267D File Offset: 0x0004087D
	private void Start()
	{
		this.ResetProjectile();
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x00042685 File Offset: 0x00040885
	public void ResetProjectile()
	{
		this._timeElapsed = 0f;
		this.flyingObject.SetActive(true);
		this.crashingObject.SetActive(false);
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000D88A4 File Offset: 0x000D6AA4
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

	// Token: 0x06001B1A RID: 6938 RVA: 0x000D8968 File Offset: 0x000D6B68
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

	// Token: 0x06001B1B RID: 6939 RVA: 0x000426AA File Offset: 0x000408AA
	internal void SetVRRig(VRRig rig)
	{
		this.myRig = rig;
	}

	// Token: 0x04001DE0 RID: 7648
	private const float speedScaleRatio = 0.7f;

	// Token: 0x04001DE1 RID: 7649
	public Vector3 startPos;

	// Token: 0x04001DE2 RID: 7650
	public Vector3 endPos;

	// Token: 0x04001DE3 RID: 7651
	[FormerlySerializedAs("_flyTimeOut")]
	[Range(1f, 128f)]
	public float flyTimeOut = 32f;

	// Token: 0x04001DE5 RID: 7653
	[Space]
	public float curveTime = 0.4f;

	// Token: 0x04001DE6 RID: 7654
	[Space]
	public Vector3 curveDirection;

	// Token: 0x04001DE7 RID: 7655
	public float curveDistance = 9.8f;

	// Token: 0x04001DE8 RID: 7656
	[Space]
	[NonSerialized]
	private float _timeElapsed;

	// Token: 0x04001DE9 RID: 7657
	[NonSerialized]
	private float _speed;

	// Token: 0x04001DEA RID: 7658
	[NonSerialized]
	private Vector3 _direction;

	// Token: 0x04001DEB RID: 7659
	[NonSerialized]
	private bool _stopped;

	// Token: 0x04001DEC RID: 7660
	private Transform _tCached;

	// Token: 0x04001DED RID: 7661
	private SpawnWorldEffects spawnWorldEffects;

	// Token: 0x04001DEE RID: 7662
	private Vector3 nextPos;

	// Token: 0x04001DEF RID: 7663
	private RaycastHit[] results = new RaycastHit[1];

	// Token: 0x04001DF0 RID: 7664
	[SerializeField]
	private float maxFlightTime = 7.5f;

	// Token: 0x04001DF1 RID: 7665
	[SerializeField]
	private float minFlightTime = 0.5f;

	// Token: 0x04001DF2 RID: 7666
	[SerializeField]
	private float maxSpeed = 10f;

	// Token: 0x04001DF3 RID: 7667
	[SerializeField]
	private float minSpeed = 1f;

	// Token: 0x04001DF4 RID: 7668
	[SerializeField]
	private bool enableRotation;

	// Token: 0x04001DF5 RID: 7669
	[SerializeField]
	private GameObject flyingObject;

	// Token: 0x04001DF6 RID: 7670
	[SerializeField]
	private GameObject crashingObject;

	// Token: 0x04001DF7 RID: 7671
	[SerializeField]
	private LayerMask layerMask;

	// Token: 0x04001DF8 RID: 7672
	private VRRig myRig;

	// Token: 0x04001DF9 RID: 7673
	private float scaleFactor;

	// Token: 0x0200044C RID: 1100
	// (Invoke) Token: 0x06001B1E RID: 6942
	public delegate void PaperPlaneHit(Vector3 endPoint);
}
