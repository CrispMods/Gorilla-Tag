using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000511 RID: 1297
public class Decelerate : MonoBehaviour
{
	// Token: 0x06001F7D RID: 8061 RVA: 0x0009E964 File Offset: 0x0009CB64
	public void Restart()
	{
		base.enabled = true;
	}

	// Token: 0x06001F7E RID: 8062 RVA: 0x0009E970 File Offset: 0x0009CB70
	private void Update()
	{
		if (!this._rigidbody)
		{
			return;
		}
		Vector3 vector = this._rigidbody.velocity;
		vector *= this._friction;
		if (vector.Approx0(0.001f))
		{
			this._rigidbody.velocity = Vector3.zero;
			UnityEvent unityEvent = this.onStop;
			if (unityEvent != null)
			{
				unityEvent.Invoke();
			}
			base.enabled = false;
		}
		else
		{
			this._rigidbody.velocity = vector;
		}
		if (this._resetOrientationOnRelease && !this._rigidbody.rotation.Approx(Quaternion.identity, 1E-06f))
		{
			this._rigidbody.rotation = Quaternion.identity;
		}
	}

	// Token: 0x0400234A RID: 9034
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x0400234B RID: 9035
	[SerializeField]
	private float _friction = 0.875f;

	// Token: 0x0400234C RID: 9036
	[SerializeField]
	private bool _resetOrientationOnRelease;

	// Token: 0x0400234D RID: 9037
	public UnityEvent onStop;
}
