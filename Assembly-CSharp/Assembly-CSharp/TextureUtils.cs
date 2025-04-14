using System;
using System.IO;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200088A RID: 2186
public static class TextureUtils
{
	// Token: 0x060034F6 RID: 13558 RVA: 0x000FCFF0 File Offset: 0x000FB1F0
	public static Vector4 GetTexelSize(this Texture tex)
	{
		if (tex.AsNull<Texture>() == null)
		{
			return Vector4.zero;
		}
		Vector2 texelSize = tex.texelSize;
		float num = Mathf.Max(texelSize.x, 1f / texelSize.x);
		float num2 = Mathf.Max(texelSize.y, 1f / texelSize.y);
		return new Vector4(1f / num, 1f / num2, num, num2);
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x000FD060 File Offset: 0x000FB260
	public static Color32 CalcAverageColor(Texture2D tex)
	{
		if (tex == null)
		{
			return default(Color32);
		}
		Color32[] pixels = tex.GetPixels32();
		int num = pixels.Length;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < num; i++)
		{
			num2 += (int)pixels[i].r;
			num3 += (int)pixels[i].g;
			num4 += (int)pixels[i].b;
		}
		return new Color32((byte)(num2 / num), (byte)(num3 / num), (byte)(num4 / num), byte.MaxValue);
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x000FD0EC File Offset: 0x000FB2EC
	public static void SaveToFile(Texture source, string filePath, int width, int height, SaveTextureFileFormat fileFormat = SaveTextureFileFormat.PNG, int jpgQuality = 95, bool asynchronous = true, Action<bool> done = null)
	{
		if (source is Texture2D || source is RenderTexture)
		{
			if (width < 0 || height < 0)
			{
				width = source.width;
				height = source.height;
			}
			RenderTexture resizeRT = RenderTexture.GetTemporary(width, height, 0);
			Graphics.Blit(source, resizeRT);
			NativeArray<byte> narray = new NativeArray<byte>(width * height * 4, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			AsyncGPUReadbackRequest asyncGPUReadbackRequest = AsyncGPUReadback.RequestIntoNativeArray<byte>(ref narray, resizeRT, 0, delegate(AsyncGPUReadbackRequest request)
			{
				if (!request.hasError)
				{
					NativeArray<byte> nativeArray;
					switch (fileFormat)
					{
					case SaveTextureFileFormat.EXR:
						nativeArray = ImageConversion.EncodeNativeArrayToEXR<byte>(narray, resizeRT.graphicsFormat, (uint)width, (uint)height, 0U, Texture2D.EXRFlags.None);
						goto IL_C8;
					case SaveTextureFileFormat.JPG:
						nativeArray = ImageConversion.EncodeNativeArrayToJPG<byte>(narray, resizeRT.graphicsFormat, (uint)width, (uint)height, 0U, jpgQuality);
						goto IL_C8;
					case SaveTextureFileFormat.TGA:
						nativeArray = ImageConversion.EncodeNativeArrayToTGA<byte>(narray, resizeRT.graphicsFormat, (uint)width, (uint)height, 0U);
						goto IL_C8;
					}
					nativeArray = ImageConversion.EncodeNativeArrayToPNG<byte>(narray, resizeRT.graphicsFormat, (uint)width, (uint)height, 0U);
					IL_C8:
					File.WriteAllBytes(filePath, nativeArray.ToArray());
					nativeArray.Dispose();
				}
				narray.Dispose();
				Action<bool> done3 = done;
				if (done3 == null)
				{
					return;
				}
				done3(!request.hasError);
			});
			if (!asynchronous)
			{
				asyncGPUReadbackRequest.WaitForCompletion();
			}
			return;
		}
		Action<bool> done2 = done;
		if (done2 == null)
		{
			return;
		}
		done2(false);
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x000FD1E4 File Offset: 0x000FB3E4
	public static Texture2D CreateCopy(Texture2D tex)
	{
		if (tex == null)
		{
			throw new ArgumentNullException("tex");
		}
		RenderTexture temporary = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		Graphics.Blit(tex, temporary);
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = temporary;
		Texture2D texture2D = new Texture2D(tex.width, tex.height);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = active;
		RenderTexture.ReleaseTemporary(temporary);
		return texture2D;
	}
}
