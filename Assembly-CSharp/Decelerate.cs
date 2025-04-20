using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200051E RID: 1310
public class Decelerate : MonoBehaviour
{
	// Token: 0x06001FD6 RID: 8150 RVA: 0x00045A25 File Offset: 0x00043C25
	public void Restart()
	{
		base.enabled = true;
	}

	// Token: 0x06001FD7 RID: 8151 RVA: 0x000F04DC File Offset: 0x000EE6DC
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

	// Token: 0x0400239D RID: 9117
	[SerializeField]
	private Rigidbody _rigidbody;

	// Token: 0x0400239E RID: 9118
	[SerializeField]
	private float _friction = 0.875f;

	// Token: 0x0400239F RID: 9119
	[SerializeField]
	private bool _resetOrientationOnRelease;

	// Token: 0x040023A0 RID: 9120
	public UnityEvent onStop;
}
