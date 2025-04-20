using System;
using BoingKit;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class SyncRigidBodyToMovement : MonoBehaviour
{
	// Token: 0x060026ED RID: 9965 RVA: 0x0004A928 File Offset: 0x00048B28
	private void Awake()
	{
		this.targetParent = this.targetRigidbody.transform.parent;
		this.targetRigidbody.transform.parent = null;
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060026EE RID: 9966 RVA: 0x0010A4EC File Offset: 0x001086EC
	private void OnEnable()
	{
		this.targetRigidbody.gameObject.SetActive(true);
		this.targetRigidbody.transform.position = base.transform.position;
		this.targetRigidbody.transform.rotation = base.transform.rotation;
	}

	// Token: 0x060026EF RID: 9967 RVA: 0x0004A962 File Offset: 0x00048B62
	private void OnDisable()
	{
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060026F0 RID: 9968 RVA: 0x0010A540 File Offset: 0x00108740
	private void FixedUpdate()
	{
		this.targetRigidbody.velocity = (base.transform.position - this.targetRigidbody.position) / Time.fixedDeltaTime;
		this.targetRigidbody.angularVelocity = QuaternionUtil.ToAngularVector(Quaternion.Inverse(this.targetRigidbody.rotation) * base.transform.rotation) / Time.fixedDeltaTime;
	}

	// Token: 0x04002AFC RID: 11004
	[SerializeField]
	private Rigidbody targetRigidbody;

	// Token: 0x04002AFD RID: 11005
	private Transform targetParent;
}
