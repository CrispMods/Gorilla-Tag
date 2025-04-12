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
	// Token: 0x02000AA6 RID: 2726
	public class CreditsView : MonoBehaviour
	{
		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x0005B8AA File Offset: 0x00059AAA
		private int TotalPages
		{
			get
			{
				return this.creditsSections.Sum((CreditsSection section) => this.PagesPerSection(section));
			}
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x0017994C File Offset: 0x00177B4C
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

		// Token: 0x06004412 RID: 17426 RVA: 0x0005B8C3 File Offset: 0x00059AC3
		private int PagesPerSection(CreditsSection section)
		{
			return (int)Math.Ceiling((double)section.Entries.Count / (double)this.pageSize);
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x0005B8DF File Offset: 0x00059ADF
		private IEnumerable<string> PageOfSection(CreditsSection section, int page)
		{
			return section.Entries.Skip(this.pageSize * page).Take(this.pageSize);
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x00179B5C File Offset: 0x00177D5C
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

		// Token: 0x06004415 RID: 17429 RVA: 0x0005B8FF File Offset: 0x00059AFF
		public void ProcessButtonPress(GorillaKeyboardBindings buttonPressed)
		{
			if (buttonPressed == GorillaKeyboardBindings.enter)
			{
				this.currentPage++;
				this.currentPage %= this.TotalPages;
			}
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x0005B927 File Offset: 0x00059B27
		public string GetScreenText()
		{
			return this.GetPage(this.currentPage);
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x00179BB8 File Offset: 0x00177DB8
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

		// Token: 0x04004564 RID: 17764
		private CreditsSection[] creditsSections;

		// Token: 0x04004565 RID: 17765
		public int pageSize = 7;

		// Token: 0x04004566 RID: 17766
		private int currentPage;

		// Token: 0x04004567 RID: 17767
		private const string PlayFabKey = "CreditsData";
	}
}
