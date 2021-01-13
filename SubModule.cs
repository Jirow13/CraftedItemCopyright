using System;
using System.Windows.Forms;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace CraftedItemCopyright
{
	public class SubModule : MBSubModuleBase
	{
		protected override void OnSubModuleLoad()
		{
			base.OnSubModuleLoad();
			try
			{
				new Harmony("mod.CraftedItemCopyright.bannerlord").PatchAll();
			}
			catch (Exception e)
			{
				MessageBox.Show("Couldn't apply Harmony due to: " + e.FlattenException());
			}
		}

		protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
		{
			try
			{
				base.OnGameStart(game, gameStarterObject);
				if (game.GameType is Campaign)
				{
					if (gameStarterObject != null)
					{
						((CampaignGameStarter)gameStarterObject).AddBehavior(CraftedItemCopyrightBehavior.Instance);
					}
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.FlattenException());
			}
		}
	}
}