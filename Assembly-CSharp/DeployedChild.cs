using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class DeployedChild : MonoBehaviour
{
	// Token: 0x0600087C RID: 2172 RVA: 0x0002E9CC File Offset: 0x0002CBCC
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

	// Token: 0x0600087D RID: 2173 RVA: 0x0002EA29 File Offset: 0x0002CC29
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

	// Token: 0x0600087E RID: 2174 RVA: 0x0002EA5B File Offset: 0x0002CC5B
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

	// Token: 0x04000A1C RID: 2588
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x04000A1D RID: 2589
	[SerializeReference]
	private DeployableObject _parent;

	// Token: 0x04000A1E RID: 2590
	private bool _isRemote;
}
