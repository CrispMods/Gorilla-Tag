﻿using System;
using UnityEngine;

// Token: 0x02000306 RID: 774
[RequireComponent(typeof(Canvas))]
public class OVROverlayCanvas : MonoBehaviour
{
	// Token: 0x06001276 RID: 4726 RVA: 0x0005864C File Offset: 0x0005684C
	private void Start()
	{
		this._canvas = base.GetComponent<Canvas>();
		this._rectTransform = this._canvas.GetComponent<RectTransform>();
		float width = this._rectTransform.rect.width;
		float height = this._rectTransform.rect.height;
		float num = (width >= height) ? 1f : (width / height);
		float num2 = (height >= width) ? 1f : (height / width);
		int num3 = this.ScaleViewport ? 0 : 8;
		int num4 = Mathf.CeilToInt(num * (float)(this.MaxTextureSize - num3 * 2));
		int num5 = Mathf.CeilToInt(num2 * (float)(this.MaxTextureSize - num3 * 2));
		int num6 = num4 + num3 * 2;
		int num7 = num5 + num3 * 2;
		float x = width * ((float)num6 / (float)num4);
		float num8 = height * ((float)num7 / (float)num5);
		float num9 = (float)num4 / (float)num6;
		float num10 = (float)num5 / (float)num7;
		Vector2 vector = (this.Opacity == OVROverlayCanvas.DrawMode.Opaque) ? new Vector2(0.005f / this._rectTransform.lossyScale.x, 0.005f / this._rectTransform.lossyScale.y) : Vector2.zero;
		this._renderTexture = new RenderTexture(num6, num7, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
		this._renderTexture.useMipMap = !this.ScaleViewport;
		GameObject gameObject = new GameObject(base.name + " Overlay Camera")
		{
			hideFlags = (HideFlags.HideInHierarchy | HideFlags.NotEditable)
		};
		gameObject.transform.SetParent(base.transform, false);
		this._camera = gameObject.AddComponent<Camera>();
		this._camera.stereoTargetEye = StereoTargetEyeMask.None;
		this._camera.transform.position = base.transform.position - base.transform.forward;
		this._camera.orthographic = true;
		this._camera.enabled = false;
		this._camera.targetTexture = this._renderTexture;
		this._camera.cullingMask = 1 << base.gameObject.layer;
		this._camera.clearFlags = CameraClearFlags.Color;
		this._camera.backgroundColor = Color.clear;
		this._camera.orthographicSize = 0.5f * num8 * this._rectTransform.localScale.y;
		this._camera.nearClipPlane = 0.99f;
		this._camera.farClipPlane = 1.01f;
		this._quad = new Mesh
		{
			name = base.name + " Overlay Quad",
			hideFlags = HideFlags.HideAndDontSave
		};
		this._quad.vertices = new Vector3[]
		{
			new Vector3(-0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f),
			new Vector3(0.5f, 0.5f),
			new Vector3(0.5f, -0.5f)
		};
		this._quad.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		this._quad.triangles = new int[]
		{
			0,
			1,
			2,
			2,
			3,
			0
		};
		this._quad.bounds = new Bounds(Vector3.zero, Vector3.one);
		this._quad.UploadMeshData(true);
		switch (this.Opacity)
		{
		case OVROverlayCanvas.DrawMode.Opaque:
			this._defaultMat = new Material(this._opaqueShader);
			break;
		case OVROverlayCanvas.DrawMode.OpaqueWithClip:
			this._defaultMat = new Material(this._opaqueShader);
			this._defaultMat.EnableKeyword("WITH_CLIP");
			break;
		case OVROverlayCanvas.DrawMode.TransparentDefaultAlpha:
			this._defaultMat = new Material(this._transparentShader);
			this._defaultMat.EnableKeyword("ALPHA_SQUARED");
			break;
		case OVROverlayCanvas.DrawMode.TransparentCorrectAlpha:
			this._defaultMat = new Material(this._transparentShader);
			break;
		}
		this._defaultMat.mainTexture = this._renderTexture;
		this._defaultMat.color = Color.black;
		this._defaultMat.mainTextureOffset = new Vector2(0.5f - 0.5f * num9, 0.5f - 0.5f * num10);
		this._defaultMat.mainTextureScale = new Vector2(num9, num10);
		GameObject gameObject2 = new GameObject(base.name + " MeshRenderer")
		{
			hideFlags = (HideFlags.HideInHierarchy | HideFlags.NotEditable)
		};
		gameObject2.transform.SetParent(base.transform, false);
		gameObject2.AddComponent<MeshFilter>().sharedMesh = this._quad;
		this._meshRenderer = gameObject2.AddComponent<MeshRenderer>();
		this._meshRenderer.sharedMaterial = this._defaultMat;
		gameObject2.layer = this.Layer;
		gameObject2.transform.localScale = new Vector3(width - vector.x, height - vector.y, 1f);
		GameObject gameObject3 = new GameObject(base.name + " Overlay")
		{
			hideFlags = (HideFlags.HideInHierarchy | HideFlags.NotEditable)
		};
		gameObject3.transform.SetParent(base.transform, false);
		this._overlay = gameObject3.AddComponent<OVROverlay>();
		this._overlay.isDynamic = true;
		this._overlay.noDepthBufferTesting = true;
		this._overlay.isAlphaPremultiplied = !Application.isMobilePlatform;
		this._overlay.textures[0] = this._renderTexture;
		this._overlay.currentOverlayType = OVROverlay.OverlayType.Underlay;
		this._overlay.transform.localScale = new Vector3(x, num8, 1f);
		this._overlay.useExpensiveSuperSample = this.Expensive;
	}

	// Token: 0x06001277 RID: 4727 RVA: 0x00058C23 File Offset: 0x00056E23
	private void OnDestroy()
	{
		Object.Destroy(this._defaultMat);
		Object.Destroy(this._quad);
		Object.Destroy(this._renderTexture);
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00058C46 File Offset: 0x00056E46
	private void OnEnable()
	{
		if (this._overlay)
		{
			this._meshRenderer.enabled = true;
			this._overlay.enabled = true;
		}
		if (this._camera)
		{
			this._camera.enabled = true;
		}
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x00058C86 File Offset: 0x00056E86
	private void OnDisable()
	{
		if (this._overlay)
		{
			this._overlay.enabled = false;
			this._meshRenderer.enabled = false;
		}
		if (this._camera)
		{
			this._camera.enabled = false;
		}
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00058CC8 File Offset: 0x00056EC8
	protected virtual bool ShouldRender()
	{
		if (this.DrawRate > 1 && Time.frameCount % this.DrawRate != this.DrawFrameOffset % this.DrawRate)
		{
			return false;
		}
		if (Camera.main != null)
		{
			for (int i = 0; i < 2; i++)
			{
				Camera.StereoscopicEye eye = (Camera.StereoscopicEye)i;
				GeometryUtility.CalculateFrustumPlanes(Camera.main.GetStereoProjectionMatrix(eye) * Camera.main.GetStereoViewMatrix(eye), OVROverlayCanvas._FrustumPlanes);
				if (GeometryUtility.TestPlanesAABB(OVROverlayCanvas._FrustumPlanes, this._meshRenderer.bounds))
				{
					return true;
				}
			}
			return false;
		}
		return true;
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00058D58 File Offset: 0x00056F58
	private void Update()
	{
		if (this.ShouldRender())
		{
			if (this.ScaleViewport && Camera.main != null)
			{
				float magnitude = (Camera.main.transform.position - base.transform.position).magnitude;
				float num = Mathf.Ceil(this.PixelsPerUnit * Mathf.Max(this._rectTransform.rect.width * base.transform.lossyScale.x, this._rectTransform.rect.height * base.transform.lossyScale.y) / magnitude / 8f * (float)this._renderTexture.height) * 8f;
				num = Mathf.Clamp(num, (float)this.MinTextureSize, (float)this._renderTexture.height);
				float num2 = num - 2f;
				this._camera.orthographicSize = 0.5f * this._rectTransform.rect.height * this._rectTransform.localScale.y * num / num2;
				float num3 = this._rectTransform.rect.width / this._rectTransform.rect.height;
				float num4 = num2 * num3;
				float num5 = Mathf.Ceil((num4 + 2f) * 0.5f) * 2f / (float)this._renderTexture.width;
				float num6 = num / (float)this._renderTexture.height;
				float num7 = (this.Opacity == OVROverlayCanvas.DrawMode.Opaque) ? 1.001f : 0f;
				float num8 = (num4 - num7) / (float)this._renderTexture.width;
				float num9 = (num2 - num7) / (float)this._renderTexture.height;
				this._camera.rect = new Rect((1f - num5) / 2f, (1f - num6) / 2f, num5, num6);
				Rect rect = new Rect(0.5f - 0.5f * num8, 0.5f - 0.5f * num9, num8, num9);
				this._defaultMat.mainTextureOffset = rect.min;
				this._defaultMat.mainTextureScale = rect.size;
				this._overlay.overrideTextureRectMatrix = true;
				rect.y = 1f - rect.height - rect.y;
				Rect rect2 = new Rect(0f, 0f, 1f, 1f);
				this._overlay.SetSrcDestRects(rect, rect, rect2, rect2);
			}
			this._camera.Render();
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x0600127C RID: 4732 RVA: 0x00059002 File Offset: 0x00057202
	// (set) Token: 0x0600127D RID: 4733 RVA: 0x0005901E File Offset: 0x0005721E
	public bool overlayEnabled
	{
		get
		{
			return this._overlay && this._overlay.enabled;
		}
		set
		{
			if (this._overlay)
			{
				this._overlay.enabled = value;
				this._defaultMat.color = (value ? Color.black : Color.white);
			}
		}
	}

	// Token: 0x04001457 RID: 5207
	[SerializeField]
	[HideInInspector]
	private Shader _transparentShader;

	// Token: 0x04001458 RID: 5208
	[SerializeField]
	[HideInInspector]
	private Shader _opaqueShader;

	// Token: 0x04001459 RID: 5209
	private RectTransform _rectTransform;

	// Token: 0x0400145A RID: 5210
	private Canvas _canvas;

	// Token: 0x0400145B RID: 5211
	private Camera _camera;

	// Token: 0x0400145C RID: 5212
	private OVROverlay _overlay;

	// Token: 0x0400145D RID: 5213
	private RenderTexture _renderTexture;

	// Token: 0x0400145E RID: 5214
	private MeshRenderer _meshRenderer;

	// Token: 0x0400145F RID: 5215
	private Mesh _quad;

	// Token: 0x04001460 RID: 5216
	private Material _defaultMat;

	// Token: 0x04001461 RID: 5217
	public int MaxTextureSize = 1600;

	// Token: 0x04001462 RID: 5218
	public int MinTextureSize = 200;

	// Token: 0x04001463 RID: 5219
	public float PixelsPerUnit = 1f;

	// Token: 0x04001464 RID: 5220
	public int DrawRate = 1;

	// Token: 0x04001465 RID: 5221
	public int DrawFrameOffset;

	// Token: 0x04001466 RID: 5222
	public bool Expensive;

	// Token: 0x04001467 RID: 5223
	public int Layer;

	// Token: 0x04001468 RID: 5224
	public OVROverlayCanvas.DrawMode Opacity = OVROverlayCanvas.DrawMode.OpaqueWithClip;

	// Token: 0x04001469 RID: 5225
	private bool ScaleViewport = Application.isMobilePlatform;

	// Token: 0x0400146A RID: 5226
	private static readonly Plane[] _FrustumPlanes = new Plane[6];

	// Token: 0x02000307 RID: 775
	public enum DrawMode
	{
		// Token: 0x0400146C RID: 5228
		Opaque,
		// Token: 0x0400146D RID: 5229
		OpaqueWithClip,
		// Token: 0x0400146E RID: 5230
		TransparentDefaultAlpha,
		// Token: 0x0400146F RID: 5231
		TransparentCorrectAlpha
	}
}
