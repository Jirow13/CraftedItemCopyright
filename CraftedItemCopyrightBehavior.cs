using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace CraftedItemCopyright
{
	internal class CraftedItemCopyrightBehavior : CampaignBehaviorBase
	{
		public override void RegisterEvents()
		{
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.OnNewItemCraftedEvent.AddNonSerializedListener(this, new Action<ItemObject, Crafting.OverrideData>(this.OnNewItemCrafted));
			CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnSessionLaunched));
		}

		private void OnSettlementEntered( MobileParty party, Settlement settlement, Hero hero)
        {
			if (CraftedItemCopyrightSettings.Instance is { } settings && settings.CleanItemsOnSettlementEntry)
			{
				if ((party != null && hero != null) && party.LeaderHero != null && party.LeaderHero == hero && party.IsMainParty)
				{
					// Debug
					//InformationManager.DisplayMessage(new InformationMessage($"OnSettlementEntered Triggered by "+hero.Name+ "\nMobileParty = " + party.Name + "\nSettlement = " + settlement.Name + "\n"));
					//InformationManager.DisplayMessage(new InformationMessage($"Looking through settlement inventory\n"));

					List<ItemRosterElement> to_remove = new List<ItemRosterElement>();
					if (settlement.ItemRoster != null)
					{
						// Debug
						//int i = 0;
						foreach (ItemRosterElement element in settlement.ItemRoster)
						{
							if (element.EquipmentElement.Item != null && element.EquipmentElement.Item.NotMerchandise && element.EquipmentElement.Item.IsCraftedWeapon &&
								( settings.IgnoreUniqueItems == true && element.EquipmentElement.Item.IsUniqueItem != true))
							{
								to_remove.Add(element);
								// Debug
								//i++;
								//InformationManager.DisplayMessage(new InformationMessage($"Added item to remove: "+element+"\n"));
							}
						}

						foreach (ItemRosterElement element2 in to_remove)
						{
							settlement.ItemRoster.Remove(element2);
							//Debug
							//InformationManager.DisplayMessage(new InformationMessage($"Removed "+element2+".\n"));
						}
					}
				}
			}
		}

		private void OnNewItemCrafted(ItemObject itemObject, Crafting.OverrideData overrideData)
		{
			Traverse.Create(itemObject).Property<bool>("NotMerchandise", null).Value = true;
			// Debug
			//InformationManager.DisplayMessage(new InformationMessage($"Triggered OnNewItemCrafted:"+itemObject.Name+"\n"));
		}

		public void OnSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			if (CraftedItemCopyrightSettings.Instance is { } settings && settings.CleanItemsOnSessionLaunched)
			{
				//Debug
				//MessageBox.Show($"Session Launched. Checking first.run:");
				//Fix: Changing to !first.run didn't do anything as the bool is declared. Changed to == false and it started to work.
				if (this.first_run == false)
				{
					//Debug
					//MessageBox.Show($"this.first_run is False");

					this.first_run = true;
					//Debug
					//MessageBox.Show($"Cycling Through Settlement Inventory for Crafted/NoMerchandise Items\n");
					//int i = 0;
					foreach (Settlement settlement in Settlement.All)
					{
						List<ItemRosterElement> to_remove = new List<ItemRosterElement>();
						if (settlement.ItemRoster != null)
						{
							foreach (ItemRosterElement element in settlement.ItemRoster)
							{
								if (element.EquipmentElement.Item != null && element.EquipmentElement.Item.NotMerchandise && element.EquipmentElement.Item.IsCraftedWeapon &&
								(settings.IgnoreUniqueItems == true && element.EquipmentElement.Item.IsUniqueItem != true))
								{
									to_remove.Add(element);
									// Debug
									//i++;
									//InformationManager.DisplayMessage(new InformationMessage($"Added item to remove: "+element+"\n"));
								}
							}

							foreach (ItemRosterElement element2 in to_remove)
							{
								settlement.ItemRoster.Remove(element2);
								//Debug
								//InformationManager.DisplayMessage(new InformationMessage($"Removed "+element2+".\n"));
							}
						}
					}
					//Debug
					//InformationManager.DisplayMessage(new InformationMessage($"Removed " + i + " Crafted Items.\n"));
					//InformationManager.DisplayMessage(new InformationMessage($"first_run final disposition is "+first_run+".\n"));
				}
			}
		}

		public override void SyncData(IDataStore dataStore)
		{
			if (dataStore.IsLoading)
			{
				this.first_run = false;
			}
		}

		public static readonly CraftedItemCopyrightBehavior Instance = new CraftedItemCopyrightBehavior();

		public bool first_run = true;
	}
}
