using System;
using System.Collections.Generic;
using CjLib;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000815 RID: 2069
public class AutoCatchThrowBall : MonoBehaviour
{
	// Token: 0x060032E2 RID: 13026 RVA: 0x000F3A49 File Offset: 0x000F1C49
	private void Start()
	{
		this.vrRig = base.GetComponent<VRRig>();
	}

	// Token: 0x060032E3 RID: 13027 RVA: 0x000F3A58 File Offset: 0x000F1C58
	private void Update()
	{
		float time = Time.time;
		Vector3 vector = this.vrRig.transform.position + this.vrRig.transform.forward * 0.5f;
		Quaternion quaternion = this.vrRig.transform.rotation * Quaternion.AngleAxis(-this.throwPitch, Vector3.right);
		Vector3 center = vector - quaternion * Vector3.forward * 0.5f;
		int num = Physics.OverlapBoxNonAlloc(center, Vector3.one * 0.5f, this.overlapResults, quaternion);
		DebugUtil.DrawBox(center, quaternion, Vector3.one, Color.green, true, DebugUtil.Style.Wireframe);
		for (int i = 0; i < num; i++)
		{
			Collider collider = this.overlapResults[i];
			TransferrableObject componentInParent = collider.gameObject.GetComponentInParent<TransferrableObject>();
			if (componentInParent != null)
			{
				bool flag = false;
				for (int j = 0; j < this.heldBalls.Count; j++)
				{
					if (componentInParent == this.heldBalls[j].transferrable)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Debug.Log(string.Format("Catching {0} in from collider {1} at position {2}", componentInParent.gameObject.name, collider.gameObject.name, componentInParent.transform.position));
					for (int k = 0; k < this.heldBalls.Count; k++)
					{
					}
					this.heldBalls.Add(new AutoCatchThrowBall.HeldBall
					{
						held = true,
						catchTime = time,
						transferrable = componentInParent
					});
					componentInParent.OnGrab(null, null);
					componentInParent.currentState = TransferrableObject.PositionState.InRightHand;
				}
			}
		}
		for (int l = this.heldBalls.Count - 1; l >= 0; l--)
		{
			AutoCatchThrowBall.HeldBall heldBall = this.heldBalls[l];
			if (heldBall.held)
			{
				heldBall.transferrable.transform.position = vector;
				if (time > heldBall.catchTime + this.throwWaitTime)
				{
					this.Throw(heldBall.transferrable, quaternion * Vector3.forward);
					heldBall.held = false;
					heldBall.throwTime = time;
					this.heldBalls[l] = heldBall;
				}
			}
			else if (time > heldBall.throwTime + this.catchWaitTime)
			{
				Debug.Log("Removing " + heldBall.transferrable.gameObject.name);
				this.heldBalls.RemoveAt(l);
				for (int m = 0; m < this.heldBalls.Count; m++)
				{
				}
			}
		}
		if (Keyboard.current.tKey.wasPressedThisFrame && this.ballPrefab != null)
		{
			TransferrableObject componentInChildren = Object.Instantiate<GameObject>(this.ballPrefab, vector, Quaternion.identity, null).GetComponentInChildren<TransferrableObject>();
			componentInChildren.OnGrab(null, null);
			componentInChildren.currentState = TransferrableObject.PositionState.InRightHand;
			this.Throw(componentInChildren, quaternion * Vector3.forward);
		}
		DebugUtil.DrawRect(vector, quaternion * Quaternion.AngleAxis(-90f, Vector3.right), Vector2.one, Color.green, true, DebugUtil.Style.Wireframe);
	}

	// Token: 0x060032E4 RID: 13028 RVA: 0x000F3D94 File Offset: 0x000F1F94
	private void Throw(TransferrableObject transferrable, Vector3 throwDir)
	{
		Rigidbody componentInChildren = transferrable.GetComponentInChildren<Rigidbody>();
		transferrable.OnRelease(null, null);
		transferrable.currentState = TransferrableObject.PositionState.Dropped;
		componentInChildren.isKinematic = false;
		componentInChildren.velocity = throwDir * this.throwSpeed;
		Debug.Log(string.Format("Throwing {0} in direction {1} at position {2}", transferrable.gameObject.name, throwDir, transferrable.transform.position));
	}

	// Token: 0x04003639 RID: 13881
	public GameObject ballPrefab;

	// Token: 0x0400363A RID: 13882
	public float throwPitch = 20f;

	// Token: 0x0400363B RID: 13883
	public float throwSpeed = 5f;

	// Token: 0x0400363C RID: 13884
	public float throwWaitTime = 1f;

	// Token: 0x0400363D RID: 13885
	public float catchWaitTime = 0.2f;

	// Token: 0x0400363E RID: 13886
	public LayerMask ballLayer;

	// Token: 0x0400363F RID: 13887
	private VRRig vrRig;

	// Token: 0x04003640 RID: 13888
	private Collider[] overlapResults = new Collider[32];

	// Token: 0x04003641 RID: 13889
	private List<AutoCatchThrowBall.HeldBall> heldBalls = new List<AutoCatchThrowBall.HeldBall>();

	// Token: 0x02000816 RID: 2070
	private struct HeldBall
	{
		// Token: 0x04003642 RID: 13890
		public bool held;

		// Token: 0x04003643 RID: 13891
		public float catchTime;

		// Token: 0x04003644 RID: 13892
		public float throwTime;

		// Token: 0x04003645 RID: 13893
		public TransferrableObject transferrable;
	}
}
