using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x0200094F RID: 2383
	internal class User
	{
		// Token: 0x06003966 RID: 14694 RVA: 0x00055B1E File Offset: 0x00053D1E
		static User()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x06003967 RID: 14695
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06003968 RID: 14696
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_IsReady")]
		internal static extern int IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x06003969 RID: 14697
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserID")]
		internal static extern int GetUserID(StringBuilder userId, int size);

		// Token: 0x0600396A RID: 14698
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserID")]
		internal static extern int GetUserID_64(StringBuilder userId, int size);

		// Token: 0x0600396B RID: 14699
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserName")]
		internal static extern int GetUserName(StringBuilder userName, int size);

		// Token: 0x0600396C RID: 14700
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserName")]
		internal static extern int GetUserName_64(StringBuilder userName, int size);

		// Token: 0x0600396D RID: 14701
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserAvatarUrl")]
		internal static extern int GetUserAvatarUrl(StringBuilder userAvatarUrl, int size);

		// Token: 0x0600396E RID: 14702
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserAvatarUrl")]
		internal static extern int GetUserAvatarUrl_64(StringBuilder userAvatarUrl, int size);
	}
}
