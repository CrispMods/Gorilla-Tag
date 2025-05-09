﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;

// Token: 0x020008D8 RID: 2264
public class TextureFromURL : MonoBehaviour
{
	// Token: 0x060036DB RID: 14043 RVA: 0x000543DC File Offset: 0x000525DC
	private void OnEnable()
	{
		if (this.data.Length == 0)
		{
			return;
		}
		if (this.source == TextureFromURL.Source.TitleData)
		{
			this.LoadFromTitleData();
			return;
		}
		this.applyRemoteTexture(this.data);
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x00145884 File Offset: 0x00143A84
	private void LoadFromTitleData()
	{
		TextureFromURL.<LoadFromTitleData>d__7 <LoadFromTitleData>d__;
		<LoadFromTitleData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadFromTitleData>d__.<>4__this = this;
		<LoadFromTitleData>d__.<>1__state = -1;
		<LoadFromTitleData>d__.<>t__builder.Start<TextureFromURL.<LoadFromTitleData>d__7>(ref <LoadFromTitleData>d__);
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x00054407 File Offset: 0x00052607
	private void OnDisable()
	{
		if (this.texture != null)
		{
			UnityEngine.Object.Destroy(this.texture);
			this.texture = null;
		}
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x00030607 File Offset: 0x0002E807
	private void OnPlayFabError(PlayFabError error)
	{
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x001458BC File Offset: 0x00143ABC
	private void OnTitleDataRequestComplete(string imageUrl)
	{
		imageUrl = imageUrl.Replace("\\r", "\r").Replace("\\n", "\n");
		if (imageUrl[0] == '"' && imageUrl[imageUrl.Length - 1] == '"')
		{
			imageUrl = imageUrl.Substring(1, imageUrl.Length - 2);
		}
		this.applyRemoteTexture(imageUrl);
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00145920 File Offset: 0x00143B20
	private void applyRemoteTexture(string imageUrl)
	{
		TextureFromURL.<applyRemoteTexture>d__11 <applyRemoteTexture>d__;
		<applyRemoteTexture>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<applyRemoteTexture>d__.<>4__this = this;
		<applyRemoteTexture>d__.imageUrl = imageUrl;
		<applyRemoteTexture>d__.<>1__state = -1;
		<applyRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<applyRemoteTexture>d__11>(ref <applyRemoteTexture>d__);
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x00145960 File Offset: 0x00143B60
	private Task<Texture2D> GetRemoteTexture(string url)
	{
		TextureFromURL.<GetRemoteTexture>d__12 <GetRemoteTexture>d__;
		<GetRemoteTexture>d__.<>t__builder = AsyncTaskMethodBuilder<Texture2D>.Create();
		<GetRemoteTexture>d__.url = url;
		<GetRemoteTexture>d__.<>1__state = -1;
		<GetRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<GetRemoteTexture>d__12>(ref <GetRemoteTexture>d__);
		return <GetRemoteTexture>d__.<>t__builder.Task;
	}

	// Token: 0x04003902 RID: 14594
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x04003903 RID: 14595
	[SerializeField]
	private TextureFromURL.Source source;

	// Token: 0x04003904 RID: 14596
	[Tooltip("If Source is set to 'TitleData' Data should be the id of the title data entry that defines an image URL. If Source is set to 'URL' Data should be a URL that points to an image.")]
	[SerializeField]
	private string data;

	// Token: 0x04003905 RID: 14597
	private Texture2D texture;

	// Token: 0x04003906 RID: 14598
	private int maxTitleDataAttempts = 10;

	// Token: 0x020008D9 RID: 2265
	private enum Source
	{
		// Token: 0x04003908 RID: 14600
		TitleData,
		// Token: 0x04003909 RID: 14601
		URL
	}
}
