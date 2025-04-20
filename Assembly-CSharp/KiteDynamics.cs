using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class KiteDynamics : MonoBehaviour, ITetheredObjectBehavior
{
	// Token: 0x06001542 RID: 5442 RVA: 0x000BF428 File Offset: 0x000BD628
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.knotRb = this.knot.GetComponent<Rigidbody>();
		this.balloonCollider = base.GetComponent<Collider>();
		this.grabPtPosition = this.grabPt.position;
		this.grabPtInitParent = this.grabPt.transform.parent;
	}

	// Token: 0x06001543 RID: 5443 RVA: 0x0003E5DA File Offset: 0x0003C7DA
	private void Start()
	{
		this.airResistance = Mathf.Clamp(this.airResistance, 0f, 1f);
		this.balloonCollider.enabled = false;
	}

	// Token: 0x06001544 RID: 5444 RVA: 0x000BF488 File Offset: 0x000BD688
	public void ReParent()
	{
		if (this.grabPt != null)
		{
			this.grabPt.transform.parent = this.grabPtInitParent.transform;
		}
		this.bouyancyActualHeight = UnityEngine.Random.Range(this.bouyancyMinHeight, this.bouyancyMaxHeight);
	}

	// Token: 0x06001545 RID: 5445 RVA: 0x000BF4D8 File Offset: 0x000BD6D8
	public void EnableDynamics(bool enable, bool collider, bool kinematic)
	{
		this.enableDynamics = enable;
		if (this.balloonCollider)
		{
			this.balloonCollider.enabled = collider;
		}
		if (this.rb != null)
		{
			this.rb.isKinematic = kinematic;
			if (!enable)
			{
				this.rb.velocity = Vector3.zero;
				this.rb.angularVelocity = Vector3.zero;
			}
		}
	}

	// Token: 0x06001546 RID: 5446 RVA: 0x0003E603 File Offset: 0x0003C803
	public void EnableDistanceConstraints(bool enable, float scale = 1f)
	{
		this.rb.useGravity = !enable;
		this.balloonScale = scale;
		this.grabPtPosition = this.grabPt.position;
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06001547 RID: 5447 RVA: 0x0003E62C File Offset: 0x0003C82C
	public bool ColliderEnabled
	{
		get
		{
			return this.balloonCollider && this.balloonCollider.enabled;
		}
	}

	// Token: 0x06001548 RID: 5448 RVA: 0x000BF544 File Offset: 0x000BD744
	private void FixedUpdate()
	{
		if (this.rb.isKinematic || this.rb.useGravity)
		{
			return;
		}
		if (this.enableDynamics)
		{
			Vector3 vector = (this.grabPt.position - this.grabPtPosition) * 100f;
			vector = Matrix4x4.Rotate(this.ctrlRotation).MultiplyVector(vector);
			this.rb.AddForce(vector, ForceMode.Force);
			Vector3 velocity = this.rb.velocity;
			float magnitude = velocity.magnitude;
			this.rb.velocity = velocity.normalized * Mathf.Min(magnitude, this.maximumVelocity * this.balloonScale);
			base.transform.LookAt(base.transform.position - this.rb.velocity);
		}
	}

	// Token: 0x06001549 RID: 5449 RVA: 0x000306DC File Offset: 0x0002E8DC
	void ITetheredObjectBehavior.DbgClear()
	{
		throw new NotImplementedException();
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x0003DFA6 File Offset: 0x0003C1A6
	bool ITetheredObjectBehavior.IsEnabled()
	{
		return base.enabled;
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x0003E648 File Offset: 0x0003C848
	void ITetheredObjectBehavior.TriggerEnter(Collider other, ref Vector3 force, ref Vector3 collisionPt, ref bool transferOwnership)
	{
		transferOwnership = false;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x000BF620 File Offset: 0x000BD820
	public bool ReturnStep()
	{
		this.rb.isKinematic = true;
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.grabPt.position, Time.deltaTime * this.returnSpeed);
		return base.transform.position == this.grabPt.position;
	}

	// Token: 0x0400177B RID: 6011
	private Rigidbody rb;

	// Token: 0x0400177C RID: 6012
	private Collider balloonCollider;

	// Token: 0x0400177D RID: 6013
	private Bounds bounds;

	// Token: 0x0400177E RID: 6014
	[SerializeField]
	private float bouyancyMinHeight = 10f;

	// Token: 0x0400177F RID: 6015
	[SerializeField]
	private float bouyancyMaxHeight = 20f;

	// Token: 0x04001780 RID: 6016
	private float bouyancyActualHeight = 20f;

	// Token: 0x04001781 RID: 6017
	[SerializeField]
	private float airResistance = 0.01f;

	// Token: 0x04001782 RID: 6018
	public GameObject knot;

	// Token: 0x04001783 RID: 6019
	private Rigidbody knotRb;

	// Token: 0x04001784 RID: 6020
	public Transform grabPt;

	// Token: 0x04001785 RID: 6021
	private Transform grabPtInitParent;

	// Token: 0x04001786 RID: 6022
	[SerializeField]
	private float maximumVelocity = 2f;

	// Token: 0x04001787 RID: 6023
	private bool enableDynamics;

	// Token: 0x04001788 RID: 6024
	[SerializeField]
	private float balloonScale = 1f;

	// Token: 0x04001789 RID: 6025
	private Vector3 grabPtPosition;

	// Token: 0x0400178A RID: 6026
	[SerializeField]
	private Quaternion ctrlRotation;

	// Token: 0x0400178B RID: 6027
	[SerializeField]
	private float returnSpeed = 50f;
}
