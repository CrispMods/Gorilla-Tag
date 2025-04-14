using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;

// Token: 0x020008BC RID: 2236
public class TextureFromURL : MonoBehaviour
{
	// Token: 0x06003613 RID: 13843 RVA: 0x000FFF3E File Offset: 0x000FE13E
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

	// Token: 0x06003614 RID: 13844 RVA: 0x000FFF6C File Offset: 0x000FE16C
	private void LoadFromTitleData()
	{
		TextureFromURL.<LoadFromTitleData>d__7 <LoadFromTitleData>d__;
		<LoadFromTitleData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadFromTitleData>d__.<>4__this = this;
		<LoadFromTitleData>d__.<>1__state = -1;
		<LoadFromTitleData>d__.<>t__builder.Start<TextureFromURL.<LoadFromTitleData>d__7>(ref <LoadFromTitleData>d__);
	}

	// Token: 0x06003615 RID: 13845 RVA: 0x000FFFA3 File Offset: 0x000FE1A3
	private void OnDisable()
	{
		if (this.texture != null)
		{
			Object.Destroy(this.texture);
			this.texture = null;
		}
	}

	// Token: 0x06003616 RID: 13846 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnPlayFabError(PlayFabError error)
	{
	}

	// Token: 0x06003617 RID: 13847 RVA: 0x000FFFC8 File Offset: 0x000FE1C8
	private void OnTitleDataRequestComplete(string imageUrl)
	{
		imageUrl = imageUrl.Replace("\\r", "\r").Replace("\\n", "\n");
		if (imageUrl[0] == '"' && imageUrl[imageUrl.Length - 1] == '"')
		{
			imageUrl = imageUrl.Substring(1, imageUrl.Length - 2);
		}
		this.applyRemoteTexture(imageUrl);
	}

	// Token: 0x06003618 RID: 13848 RVA: 0x0010002C File Offset: 0x000FE22C
	private void applyRemoteTexture(string imageUrl)
	{
		TextureFromURL.<applyRemoteTexture>d__11 <applyRemoteTexture>d__;
		<applyRemoteTexture>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<applyRemoteTexture>d__.<>4__this = this;
		<applyRemoteTexture>d__.imageUrl = imageUrl;
		<applyRemoteTexture>d__.<>1__state = -1;
		<applyRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<applyRemoteTexture>d__11>(ref <applyRemoteTexture>d__);
	}

	// Token: 0x06003619 RID: 13849 RVA: 0x0010006C File Offset: 0x000FE26C
	private Task<Texture2D> GetRemoteTexture(string url)
	{
		TextureFromURL.<GetRemoteTexture>d__12 <GetRemoteTexture>d__;
		<GetRemoteTexture>d__.<>t__builder = AsyncTaskMethodBuilder<Texture2D>.Create();
		<GetRemoteTexture>d__.url = url;
		<GetRemoteTexture>d__.<>1__state = -1;
		<GetRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<GetRemoteTexture>d__12>(ref <GetRemoteTexture>d__);
		return <GetRemoteTexture>d__.<>t__builder.Task;
	}

	// Token: 0x04003841 RID: 14401
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x04003842 RID: 14402
	[SerializeField]
	private TextureFromURL.Source source;

	// Token: 0x04003843 RID: 14403
	[Tooltip("If Source is set to 'TitleData' Data should be the id of the title data entry that defines an image URL. If Source is set to 'URL' Data should be a URL that points to an image.")]
	[SerializeField]
	private string data;

	// Token: 0x04003844 RID: 14404
	private Texture2D texture;

	// Token: 0x04003845 RID: 14405
	private int maxTitleDataAttempts = 10;

	// Token: 0x020008BD RID: 2237
	private enum Source
	{
		// Token: 0x04003847 RID: 14407
		TitleData,
		// Token: 0x04003848 RID: 14408
		URL
	}
}
