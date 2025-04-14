using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000511 RID: 1297
public class Decelerate : MonoBehaviour
{
	// Token: 0x06001F80 RID: 8064 RVA: 0x0009ECE8 File Offset: 0x0009CEE8
	public void Restart()
	{
		base.enabled = true;
	}

	// Token: 0x06001F81 RID: 8065 RVA: 0x0009ECF4 File Offset: 0x0009CEF4
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

	// Token: 0x0400234B RID: 9035
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x0400234C RID: 9036
	[SerializeField]
	private float _friction = 0.875f;

	// Token: 0x0400234D RID: 9037
	[SerializeField]
	private bool _resetOrientationOnRelease;

	// Token: 0x0400234E RID: 9038
	public UnityEvent onStop;
}
