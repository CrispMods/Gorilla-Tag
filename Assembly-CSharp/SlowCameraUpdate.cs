using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000614 RID: 1556
public class SlowCameraUpdate : MonoBehaviour
{
	// Token: 0x060026A4 RID: 9892 RVA: 0x0004A5DB File Offset: 0x000487DB
	public void Awake()
	{
		this.frameRate = 30f;
		this.timeToNextFrame = 1f / this.frameRate;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x060026A5 RID: 9893 RVA: 0x0004A606 File Offset: 0x00048806
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdateMirror());
	}

	// Token: 0x060026A6 RID: 9894 RVA: 0x00033636 File Offset: 0x00031836
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x060026A7 RID: 9895 RVA: 0x0004A615 File Offset: 0x00048815
	public IEnumerator UpdateMirror()
	{
		for (;;)
		{
			if (base.gameObject.activeSelf)
			{
				Debug.Log("rendering camera!");
				this.myCamera.Render();
			}
			yield return new WaitForSeconds(this.timeToNextFrame);
		}
		yield break;
	}

	// Token: 0x04002AA2 RID: 10914
	private Camera myCamera;

	// Token: 0x04002AA3 RID: 10915
	private float frameRate;

	// Token: 0x04002AA4 RID: 10916
	private float timeToNextFrame;
}
