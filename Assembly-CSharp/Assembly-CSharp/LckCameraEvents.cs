using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

// Token: 0x02000238 RID: 568
public class LckCameraEvents : MonoBehaviour
{
	// Token: 0x06000D0F RID: 3343 RVA: 0x0004446D File Offset: 0x0004266D
	private void OnEnable()
	{
		RenderPipelineManager.beginCameraRendering += this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering += this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x00044491 File Offset: 0x00042691
	private void OnDisable()
	{
		RenderPipelineManager.beginCameraRendering -= this.RenderPipelineManagerOnbeginCameraRendering;
		RenderPipelineManager.endCameraRendering -= this.RenderPipelineManagerOnendCameraRendering;
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x000444B5 File Offset: 0x000426B5
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

	// Token: 0x06000D12 RID: 3346 RVA: 0x000444D6 File Offset: 0x000426D6
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

	// Token: 0x04001061 RID: 4193
	[SerializeField]
	private Camera _camera;

	// Token: 0x04001062 RID: 4194
	public UnityEvent onPreRender;

	// Token: 0x04001063 RID: 4195
	public UnityEvent onPostRender;
}
