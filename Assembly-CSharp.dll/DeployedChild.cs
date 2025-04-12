using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class DeployedChild : MonoBehaviour
{
	// Token: 0x0600087E RID: 2174 RVA: 0x0008CE94 File Offset: 0x0008B094
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

	// Token: 0x0600087F RID: 2175 RVA: 0x0003514B File Offset: 0x0003334B
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

	// Token: 0x06000880 RID: 2176 RVA: 0x0003517D File Offset: 0x0003337D
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

	// Token: 0x04000A1D RID: 2589
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04000A1E RID: 2590
	[SerializeReference]
	private DeployableObject _parent;

	// Token: 0x04000A1F RID: 2591
	private bool _isRemote;
}
