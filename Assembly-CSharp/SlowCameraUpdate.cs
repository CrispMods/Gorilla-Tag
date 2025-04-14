using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class SlowCameraUpdate : MonoBehaviour
{
	// Token: 0x06002779 RID: 10105 RVA: 0x000C1537 File Offset: 0x000BF737
	public void Awake()
	{
		this.frameRate = 30f;
		this.timeToNextFrame = 1f / this.frameRate;
		this.myCamera = base.GetComponent<Camera>();
	}

	// Token: 0x0600277A RID: 10106 RVA: 0x000C1562 File Offset: 0x000BF762
	public void OnEnable()
	{
		base.StartCoroutine(this.UpdateMirror());
	}

	// Token: 0x0600277B RID: 10107 RVA: 0x00019935 File Offset: 0x00017B35
	public void OnDisable()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x0600277C RID: 10108 RVA: 0x000C1571 File Offset: 0x000BF771
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

	// Token: 0x04002B3C RID: 11068
	private Camera myCamera;

	// Token: 0x04002B3D RID: 11069
	private float frameRate;

	// Token: 0x04002B3E RID: 11070
	private float timeToNextFrame;
}
