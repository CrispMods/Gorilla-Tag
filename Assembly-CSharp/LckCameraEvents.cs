using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000238 RID: 568
public class LckCameraEvents : MonoBehaviour
{
	// Token: 0x06000D0D RID: 3341 RVA: 0x00044129 File Offset: 0x00042329
	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering += this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D0E RID: 3342 RVA: 0x0004414D File Offset: 0x0004234D
	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering -= this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x00044171 File Offset: 0x00042371
	private void RenderPipelineManagerOnbeginCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
	{
		if (this._camera != camera)
		{
			return;
		}
		UnityEvent unityEvent = this.onPreRender;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00044192 File Offset: 0x00042392
	private void RenderPipelineManagerOnendCameraRendering(ScriptableRenderContext scriptableRenderContext, Camera camera)
	{
		if (this._camera != camera)
		{
			return;
		}
		UnityEvent unityEvent = this.onPostRender;
		if (unityEvent == null)
		{
			return;
		}
		unityEvent.Invoke();
	}

	// Token: 0x04001060 RID: 4192
	[SerializeField]
	private Camera _camera;

	// Token: 0x04001061 RID: 4193
	public UnityEvent onPreRender;

	// Token: 0x04001062 RID: 4194
	public UnityEvent onPostRender;
}
