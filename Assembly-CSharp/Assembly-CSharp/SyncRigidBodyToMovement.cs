using System;
using BoingKit;
using UnityEngine;

// Token: 0x02000642 RID: 1602
public class SyncRigidBodyToMovement : MonoBehaviour
{
	// Token: 0x060027CA RID: 10186 RVA: 0x000C33AE File Offset: 0x000C15AE
	private void Awake()
	{
		this.targetParent = this.targetRigidbody.transform.parent;
		this.targetRigidbody.transform.parent = null;
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027CB RID: 10187 RVA: 0x000C33E8 File Offset: 0x000C15E8
	private void OnEnable()
	{
		this.targetRigidbody.gameObject.SetActive(true);
		this.targetRigidbody.transform.position = base.transform.position;
		this.targetRigidbody.transform.rotation = base.transform.rotation;
	}

	// Token: 0x060027CC RID: 10188 RVA: 0x000C343C File Offset: 0x000C163C
	private void OnDisable()
	{
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027CD RID: 10189 RVA: 0x000C3450 File Offset: 0x000C1650
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
