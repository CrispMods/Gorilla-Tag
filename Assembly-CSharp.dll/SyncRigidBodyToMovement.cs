using System;
using BoingKit;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class SyncRigidBodyToMovement : MonoBehaviour
{
	// Token: 0x060027CA RID: 10186 RVA: 0x0004A393 File Offset: 0x00048593
	private void Awake()
	{
		this.targetParent = this.targetRigidbody.transform.parent;
		this.targetRigidbody.transform.parent = null;
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x0010C0C4 File Offset: 0x0010A2C4
	private void OnEnable()
	{
		this.targetRigidbody.gameObject.SetActive(true);
		this.targetRigidbody.transform.position = base.transform.position;
		this.targetRigidbody.transform.rotation = base.transform.rotation;
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x0004A3CD File Offset: 0x000485CD
	private void OnDisable()
	{
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x0010C118 File Offset: 0x0010A318
	private void FixedUpdate()
	{
		this.targetRigidbody.velocity = (base.transform.position - this.targetRigidbody.position) / Time.fixedDeltaTime;
		this.targetRigidbody.angularVelocity = QuaternionUtil.ToAngularVector(Quaternion.Inverse(this.targetRigidbody.rotation) * base.transform.rotation) / Time.fixedDeltaTime;
	}

	// Token: 0x04002B9C RID: 11164
	[SerializeField]
	private Rigidbody targetRigidbody;

	// Token: 0x04002B9D RID: 11165
	private Transform targetParent;
}
