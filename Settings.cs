using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Settings.Base.Global;


namespace CraftedItemCopyRight
{
    public class CraftedItemCopyrightSettings : AttributeGlobalSettings<CraftedItemCopyrightSettings>
    {
        public override string Id => "CraftedItemCopyrightSettings";
        public override string DisplayName => "Crafted Item Copyright Settings";
        public override string FolderName => "CraftedItemCopyrightSettings";
        public override string Format => "json";


        [SettingPropertyBool("Clean Crafted Items on Game Load (Default Behavior)", Order = 0, RequireRestart = false, HintText = "Remove any crafted items from all settlements when you load the game.")]
        [SettingPropertyGroup("Main")]
        public bool CleanItemsOnSessionLaunched { get; set; } = true;
        
        [SettingPropertyBool( "Clean Crafted Items on Settlement Entry", Order = 1, RequireRestart = false, HintText = "Remove any crafted items from settlement's shop when you enter a settlement.")]
        [SettingPropertyGroup("Main")]
        public bool CleanItemsOnSettlementEntry { get; set; } = false;
     }
}
