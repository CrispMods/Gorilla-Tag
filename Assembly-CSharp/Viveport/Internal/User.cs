using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Viveport.Internal
{
	// Token: 0x02000932 RID: 2354
	internal class User
	{
		// Token: 0x06003895 RID: 14485 RVA: 0x00108133 File Offset: 0x00106333
		static User()
		{
			Api.LoadLibraryManually("viveport_api");
		}

		// Token: 0x06003896 RID: 14486
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_IsReady")]
		internal static extern int IsReady(StatusCallback IsReadyCallback);

		// Token: 0x06003897 RID: 14487
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_IsReady")]
		internal static extern int IsReady_64(StatusCallback IsReadyCallback);

		// Token: 0x06003898 RID: 14488
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserID")]
		internal static extern int GetUserID(StringBuilder userId, int size);

		// Token: 0x06003899 RID: 14489
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserID")]
		internal static extern int GetUserID_64(StringBuilder userId, int size);

		// Token: 0x0600389A RID: 14490
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserName")]
		internal static extern int GetUserName(StringBuilder userName, int size);

		// Token: 0x0600389B RID: 14491
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserName")]
		internal static extern int GetUserName_64(StringBuilder userName, int size);

		// Token: 0x0600389C RID: 14492
		[DllImport("viveport_api", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserAvatarUrl")]
		internal static extern int GetUserAvatarUrl(StringBuilder userAvatarUrl, int size);

		// Token: 0x0600389D RID: 14493
		[DllImport("viveport_api64", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "IViveportUser_GetUserAvatarUrl")]
		internal static extern int GetUserAvatarUrl_64(StringBuilder userAvatarUrl, int size);
	}
}
