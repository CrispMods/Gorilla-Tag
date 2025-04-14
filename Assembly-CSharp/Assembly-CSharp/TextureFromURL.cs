using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PlayFab;
using UnityEngine;

// Token: 0x020008BF RID: 2239
public class TextureFromURL : MonoBehaviour
{
	// Token: 0x0600361F RID: 13855 RVA: 0x00100506 File Offset: 0x000FE706
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

	// Token: 0x06003620 RID: 13856 RVA: 0x00100534 File Offset: 0x000FE734
	private void LoadFromTitleData()
	{
		TextureFromURL.<LoadFromTitleData>d__7 <LoadFromTitleData>d__;
		<LoadFromTitleData>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<LoadFromTitleData>d__.<>4__this = this;
		<LoadFromTitleData>d__.<>1__state = -1;
		<LoadFromTitleData>d__.<>t__builder.Start<TextureFromURL.<LoadFromTitleData>d__7>(ref <LoadFromTitleData>d__);
	}

	// Token: 0x06003621 RID: 13857 RVA: 0x0010056B File Offset: 0x000FE76B
	private void OnDisable()
	{
		if (this.texture != null)
		{
			Object.Destroy(this.texture);
			this.texture = null;
		}
	}

	// Token: 0x06003622 RID: 13858 RVA: 0x000023F4 File Offset: 0x000005F4
	private void OnPlayFabError(PlayFabError error)
	{
	}

	// Token: 0x06003623 RID: 13859 RVA: 0x00100590 File Offset: 0x000FE790
	private void OnTitleDataRequestComplete(string imageUrl)
	{
		imageUrl = imageUrl.Replace("\\r", "\r").Replace("\\n", "\n");
		if (imageUrl[0] == '"' && imageUrl[imageUrl.Length - 1] == '"')
		{
			imageUrl = imageUrl.Substring(1, imageUrl.Length - 2);
		}
		this.applyRemoteTexture(imageUrl);
	}

	// Token: 0x06003624 RID: 13860 RVA: 0x001005F4 File Offset: 0x000FE7F4
	private void applyRemoteTexture(string imageUrl)
	{
		TextureFromURL.<applyRemoteTexture>d__11 <applyRemoteTexture>d__;
		<applyRemoteTexture>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<applyRemoteTexture>d__.<>4__this = this;
		<applyRemoteTexture>d__.imageUrl = imageUrl;
		<applyRemoteTexture>d__.<>1__state = -1;
		<applyRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<applyRemoteTexture>d__11>(ref <applyRemoteTexture>d__);
	}

	// Token: 0x06003625 RID: 13861 RVA: 0x00100634 File Offset: 0x000FE834
	private Task<Texture2D> GetRemoteTexture(string url)
	{
		TextureFromURL.<GetRemoteTexture>d__12 <GetRemoteTexture>d__;
		<GetRemoteTexture>d__.<>t__builder = AsyncTaskMethodBuilder<Texture2D>.Create();
		<GetRemoteTexture>d__.url = url;
		<GetRemoteTexture>d__.<>1__state = -1;
		<GetRemoteTexture>d__.<>t__builder.Start<TextureFromURL.<GetRemoteTexture>d__12>(ref <GetRemoteTexture>d__);
		return <GetRemoteTexture>d__.<>t__builder.Task;
	}

	// Token: 0x04003853 RID: 14419
	[SerializeField]
	private Renderer _renderer;

	// Token: 0x04003854 RID: 14420
	[SerializeField]
	private TextureFromURL.Source source;

	// Token: 0x04003855 RID: 14421
	[Tooltip("If Source is set to 'TitleData' Data should be the id of the title data entry that defines an image URL. If Source is set to 'URL' Data should be a URL that points to an image.")]
	[SerializeField]
	private string data;

	// Token: 0x04003856 RID: 14422
	private Texture2D texture;

	// Token: 0x04003857 RID: 14423
	private int maxTitleDataAttempts = 10;

	// Token: 0x020008C0 RID: 2240
	private enum Source
	{
		// Token: 0x04003859 RID: 14425
		TitleData,
		// Token: 0x0400385A RID: 14426
		URL
	}
}
