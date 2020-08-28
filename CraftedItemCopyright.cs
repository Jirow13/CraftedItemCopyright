using System;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CraftedItemCopyright
{
	internal class CraftedItemCopyrightPatches
	{
		[HarmonyPatch(typeof(Crafting), "InitializePreCraftedWeaponOnLoad")]
		public class InitializePreCraftedWeaponOnLoadPatch
		{
			private static void Postfix(ItemObject itemObject)
			{
				Traverse.Create(itemObject).Property<bool>("NotMerchandise", null).Value = true;
			}

			private static void Finalizer(Exception __exception)
			{
				if (__exception != null)
				{
					MessageBox.Show(__exception.FlattenException());
				}
			}
		}

		[HarmonyPatch(typeof(TournamentGame), "GetTournamentPrize")]
		public class GetTournamentPrizePatch
		{
			private static void Postfix(TournamentGame __instance, ref ItemObject __result)
			{
				if (__result.NotMerchandise && __result.IsCraftedWeapon)
				{
					if (Traverse.Create(__instance).Method("GetTournamentPrize", new Type[0], null).MethodExists())
					{
						__result = Traverse.Create(__instance).Method("GetTournamentPrize", new Type[0], null).GetValue<ItemObject>();
						return;
					}
					MessageBox.Show("Crafted Items No Merchandise: Cannot find method GetTournamentPrize in order to replace crafted item tournament prize!");
				}
			}

			private static void Finalizer(Exception __exception)
			{
				if (__exception != null)
				{
					MessageBox.Show(__exception.FlattenException());
				}
			}
		}
	}
}
