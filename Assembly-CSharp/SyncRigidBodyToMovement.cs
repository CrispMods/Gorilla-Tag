using System;
using BoingKit;
using UnityEngine;

// Token: 0x02000641 RID: 1601
public class SyncRigidBodyToMovement : MonoBehaviour
{
	// Token: 0x060027C2 RID: 10178 RVA: 0x000C2F2E File Offset: 0x000C112E
	private void Awake()
	{
		this.targetParent = this.targetRigidbody.transform.parent;
		this.targetRigidbody.transform.parent = null;
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027C3 RID: 10179 RVA: 0x000C2F68 File Offset: 0x000C1168
	private void OnEnable()
	{
		this.targetRigidbody.gameObject.SetActive(true);
		this.targetRigidbody.transform.position = base.transform.position;
		this.targetRigidbody.transform.rotation = base.transform.rotation;
	}

	// Token: 0x060027C4 RID: 10180 RVA: 0x000C2FBC File Offset: 0x000C11BC
	private void OnDisable()
	{
		this.targetRigidbody.gameObject.SetActive(false);
	}

	// Token: 0x060027C5 RID: 10181 RVA: 0x000C2FD0 File Offset: 0x000C11D0
	private void FixedUpdate()
	{
		this.targetRigidbody.velocity = (base.transform.position - this.targetRigidbody.position) / Time.fixedDeltaTime;
		this.targetRigidbody.angularVelocity = QuaternionUtil.ToAngularVector(Quaternion.Inverse(this.targetRigidbody.rotation) * base.transform.rotation) / Time.fixedDeltaTime;
	}

	// Token: 0x04002B96 RID: 11158
	[SerializeField]
	private Rigidbody targetRigidbody;

	// Token: 0x04002B97 RID: 11159
	private Transform targetParent;
}
