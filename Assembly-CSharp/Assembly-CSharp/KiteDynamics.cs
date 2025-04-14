using System;
using UnityEngine;

// Token: 0x0200037E RID: 894
public class KiteDynamics : MonoBehaviour, ITetheredObjectBehavior
{
	// Token: 0x060014F9 RID: 5369 RVA: 0x00066CE4 File Offset: 0x00064EE4
	private void Awake()
	{
		this.rb = base.GetComponent<Rigidbody>();
		this.knotRb = this.knot.GetComponent<Rigidbody>();
		this.balloonCollider = base.GetComponent<Collider>();
		this.grabPtPosition = this.grabPt.position;
		this.grabPtInitParent = this.grabPt.transform.parent;
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x00066D41 File Offset: 0x00064F41
	private void Start()
	{
		this.airResistance = Mathf.Clamp(this.airResistance, 0f, 1f);
		this.balloonCollider.enabled = false;
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x00066D6C File Offset: 0x00064F6C
	public void ReParent()
	{
		if (this.grabPt != null)
		{
			this.grabPt.transform.parent = this.grabPtInitParent.transform;
		}
		this.bouyancyActualHeight = Random.Range(this.bouyancyMinHeight, this.bouyancyMaxHeight);
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x00066DBC File Offset: 0x00064FBC
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

	// Token: 0x060014FD RID: 5373 RVA: 0x00066E26 File Offset: 0x00065026
	public void EnableDistanceConstraints(bool enable, float scale = 1f)
	{
		this.rb.useGravity = !enable;
		this.balloonScale = scale;
		this.grabPtPosition = this.grabPt.position;
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060014FE RID: 5374 RVA: 0x00066E4F File Offset: 0x0006504F
	public bool ColliderEnabled
	{
		get
		{
			return this.balloonCollider && this.balloonCollider.enabled;
		}
	}

	// Token: 0x060014FF RID: 5375 RVA: 0x00066E6C File Offset: 0x0006506C
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

	// Token: 0x06001500 RID: 5376 RVA: 0x00002628 File Offset: 0x00000828
	void ITetheredObjectBehavior.DbgClear()
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x00064883 File Offset: 0x00062A83
	bool ITetheredObjectBehavior.IsEnabled()
	{
		return base.enabled;
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x00066F46 File Offset: 0x00065146
	void ITetheredObjectBehavior.TriggerEnter(Collider other, ref Vector3 force, ref Vector3 collisionPt, ref bool transferOwnership)
	{
		transferOwnership = false;
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x00066F4C File Offset: 0x0006514C
	public bool ReturnStep()
	{
		this.rb.isKinematic = true;
		base.transform.position = Vector3.MoveTowards(base.transform.position, this.grabPt.position, Time.deltaTime * this.returnSpeed);
		return base.transform.position == this.grabPt.position;
	}

	// Token: 0x04001734 RID: 5940
	private Rigidbody rb;

	// Token: 0x04001735 RID: 5941
	private Collider balloonCollider;

	// Token: 0x04001736 RID: 5942
	private Bounds bounds;

	// Token: 0x04001737 RID: 5943
	[SerializeField]
	private float bouyancyMinHeight = 10f;

	// Token: 0x04001738 RID: 5944
	[SerializeField]
	private float bouyancyMaxHeight = 20f;

	// Token: 0x04001739 RID: 5945
	private float bouyancyActualHeight = 20f;

	// Token: 0x0400173A RID: 5946
	[SerializeField]
	private float airResistance = 0.01f;

	// Token: 0x0400173B RID: 5947
	public GameObject knot;

	// Token: 0x0400173C RID: 5948
	private Rigidbody knotRb;

	// Token: 0x0400173D RID: 5949
	public Transform grabPt;

	// Token: 0x0400173E RID: 5950
	private Transform grabPtInitParent;

	// Token: 0x0400173F RID: 5951
	[SerializeField]
	private float maximumVelocity = 2f;

	// Token: 0x04001740 RID: 5952
	private bool enableDynamics;

	// Token: 0x04001741 RID: 5953
	[SerializeField]
	private float balloonScale = 1f;

	// Token: 0x04001742 RID: 5954
	private Vector3 grabPtPosition;

	// Token: 0x04001743 RID: 5955
	[SerializeField]
	private Quaternion ctrlRotation;

	// Token: 0x04001744 RID: 5956
	[SerializeField]
	private float returnSpeed = 50f;
}
