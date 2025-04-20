using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000156 RID: 342
public class DeployedChild : MonoBehaviour
{
	// Token: 0x060008C0 RID: 2240 RVA: 0x0008F81C File Offset: 0x0008DA1C
	public void Deploy(DeployableObject parent, Vector3 launchPos, Quaternion launchRot, Vector3 releaseVel, bool isRemote = false)
	{
		this._parent = parent;
		this._parent.DeployChild();
		Transform transform = base.transform;
		transform.position = launchPos;
		transform.rotation = launchRot;
		transform.localScale = this._parent.transform.lossyScale;
		this._rigidbody.velocity = releaseVel;
		this._isRemote = isRemote;
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x000363C1 File Offset: 0x000345C1
	public void ReturnToParent(float delay)
	{
		if (delay > 0f)
		{
			base.StartCoroutine(this.ReturnToParentDelayed(delay));
			return;
		}
		if (this._parent != null)
		{
			this._parent.ReturnChild();
		}
	}

	// Token: 0x060008C2 RID: 2242 RVA: 0x000363F3 File Offset: 0x000345F3
	private IEnumerator ReturnToParentDelayed(float delay)
	{
		float start = Time.time;
		while (Time.time < start + delay)
		{
			yield return null;
		}
		if (this._parent != null)
		{
			this._parent.ReturnChild();
		}
		yield break;
	}

	// Token: 0x04000A5F RID: 2655
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04000A60 RID: 2656
	[SerializeReference]
	private DeployableObject _parent;

	// Token: 0x04000A61 RID: 2657
	private bool _isRemote;
}
