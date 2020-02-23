using System;
using BepInEx;
using RoR2;


namespace ModItem
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.TeelTigh.Hurtitem", "Thorns Potion", "0.0.1")]
    public class Class1 : BaseUnityPlugin
    {
        public void Awake()
        {
            ItemDef itemDef = new ItemDef
            {
                name = "Thorns Potion",
                canRemove = true,
                pickupIconPath = "Thorns Potion",
                tier = ItemTier.Tier2
                
            };
            ItemTag[] tagArray = new ItemTag[1];
            ItemDef itemDef2 = itemDef;
            itemDef2.tags = tagArray;
            itemDef.unlockableName = "sss";

        }
    }
}
