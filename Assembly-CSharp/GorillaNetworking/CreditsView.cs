using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using LitJson;
using PlayFab;
using UnityEngine;

namespace GorillaNetworking
{
	// Token: 0x02000ACF RID: 2767
	public class CreditsView : MonoBehaviour
	{
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06004547 RID: 17735 RVA: 0x0005D281 File Offset: 0x0005B481
		private int TotalPages
		{
			get
			{
				return this.creditsSections.Sum((CreditsSection section) => this.PagesPerSection(section));
			}
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x00180824 File Offset: 0x0017EA24
		private void Start()
		{
			this.creditsSections = new CreditsSection[]
			{
				new CreditsSection
				{
					Title = "DEV TEAM",
					Entries = new List<string>
					{
						"Anton \"NtsFranz\" Franzluebbers",
						"Carlo Grossi Jr",
						"Cody O'Quinn",
						"David Neubelt",
						"David \"AA_DavidY\" Yee",
						"Derek \"DunkTrain\" Arabian",
						"Elie Arabian",
						"John Sleeper",
						"Haunted Army",
						"Kerestell Smith",
						"Keith \"ElectronicWall\" Taylor",
						"Laura \"Poppy\" Lorian",
						"Lilly Tothill",
						"Matt \"Crimity\" Ostgard",
						"Nick Taylor",
						"Ross Furmidge",
						"Sasha \"Kayze\" Sanders"
					}
				},
				new CreditsSection
				{
					Title = "SPECIAL THANKS",
					Entries = new List<string>
					{
						"The \"Sticks\"",
						"Alpha Squad",
						"Meta",
						"Scout House",
						"Mighty PR",
						"Caroline Arabian",
						"Clarissa & Declan",
						"Calum Haigh",
						"EZ ICE",
						"Gwen"
					}
				},
				new CreditsSection
				{
					Title = "MUSIC BY",
					Entries = new List<string>
					{
						"Stunshine",
						"David Anderson Kirk",
						"Jaguar Jen",
						"Audiopfeil",
						"Owlobe"
					}
				}
			};
			PlayFabTitleDataCache.Instance.GetTitleData("CreditsData", delegate(string result)
			{
				this.creditsSections = JsonMapper.ToObject<CreditsSection[]>(result);
			}, delegate(PlayFabError error)
			{
				Debug.Log("Error fetching credits data: " + error.ErrorMessage);
			});
		}

		// Token: 0x06004549 RID: 17737 RVA: 0x0005D29A File Offset: 0x0005B49A
		private int PagesPerSection(CreditsSection section)
		{
			return (int)Math.Ceiling((double)section.Entries.Count / (double)this.pageSize);
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x0005D2B6 File Offset: 0x0005B4B6
		private IEnumerable<string> PageOfSection(CreditsSection section, int page)
		{
			return section.Entries.Skip(this.pageSize * page).Take(this.pageSize);
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x00180A34 File Offset: 0x0017EC34
		[return: TupleElementNames(new string[]
		{
			"creditsSection",
			"subPage"
		})]
		private ValueTuple<CreditsSection, int> GetPageEntries(int page)
		{
			int num = 0;
			foreach (CreditsSection creditsSection in this.creditsSections)
			{
				int num2 = this.PagesPerSection(creditsSection);
				if (num + num2 > page)
				{
					int item = page - num;
					return new ValueTuple<CreditsSection, int>(creditsSection, item);
				}
				num += num2;
			}
			return new ValueTuple<CreditsSection, int>(this.creditsSections.First<CreditsSection>(), 0);
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x0005D2D6 File Offset: 0x0005B4D6
		public void ProcessButtonPress(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.enter)
			{
				this.currentPage++;
				this.currentPage %= this.TotalPages;
			}
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x0005D2FE File Offset: 0x0005B4FE
		public string GetScreenText()
		{
			return this.GetPage(this.currentPage);
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x00180A90 File Offset: 0x0017EC90
		private string GetPage(int page)
		{
			ValueTuple<CreditsSection, int> pageEntries = this.GetPageEntries(page);
			CreditsSection item = pageEntries.Item1;
			int item2 = pageEntries.Item2;
			IEnumerable<string> enumerable = this.PageOfSection(item, item2);
			string value = "CREDITS - " + ((item2 == 0) ? item.Title : (item.Title + " (CONT)"));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(value);
			stringBuilder.AppendLine();
			foreach (string value2 in enumerable)
			{
				stringBuilder.AppendLine(value2);
			}
			for (int i = 0; i < this.pageSize - enumerable.Count<string>(); i++)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("PRESS ENTER TO CHANGE PAGES");
			return stringBuilder.ToString();
		}

		// Token: 0x04004649 RID: 17993
		private CreditsSection[] creditsSections;

		// Token: 0x0400464A RID: 17994
		public int pageSize = 7;

		// Token: 0x0400464B RID: 17995
		private int currentPage;

		// Token: 0x0400464C RID: 17996
		private const string PlayFabKey = "CreditsData";
	}
}
