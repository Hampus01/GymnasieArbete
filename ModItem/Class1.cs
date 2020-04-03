using BepInEx;
using BepInEx.Logging;
using R2API;
using R2API.Utils;
using System.Reflection;
using RoR2;
using UnityEngine;



namespace ModItem
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.TeelTigh.Hurtitem", "Thorns Potion", "0.0.1")]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(ItemDropAPI), nameof(ResourcesAPI))]
    public class Class1 : BaseUnityPlugin
    {
        internal new static ManualLogSource Logger;

        public void Awake()
        {
            Logger = base.Logger;

            Assets.Init();
            Hooks.Init();

        }
    }

    internal static class Assets
    {
        internal static GameObject BiscoLeashPrefab;
        internal static ItemIndex BiscoLeashItemIndex;

        private const string ModPrefix = "@CustomItem:";
        private const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
        private const string IconPath = ModPrefix + "Assets/Import/belt_icon/belt_icon.png";

        internal static void Init()
        {
            //using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CustomItem.rampage"))
            //{
            //    var bundle = AssetBundle.LoadFromStream(stream);
            //    var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
            //    ResourcesAPI.AddProvider(provider);

            //    BiscoLeashPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            //}

            ItemDef itemDef = new ItemDef
            {
                name = "ThornsPotion", 
                tier = ItemTier.Tier2,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Thorn's Potion",
                pickupToken = "Return to sender",
                descriptionToken = "Chande to retrun damage delt to you",
                loreToken = "Empty",
                tags = new[]
                {
                    ItemTag.Utility,
                    ItemTag.Damage
                }
            };

            ItemDisplayRule[] itemDisplayRules = null; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            //itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
            //itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            //itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            //itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            //itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var biscoLeash = new R2API.CustomItem(itemDef, itemDisplayRules);

            BiscoLeashItemIndex = ItemAPI.Add(biscoLeash); // ItemAPI sends back the ItemIndex of your item
        }

    }

    public class Hooks
    {
        internal static void Init()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                DamageInfo damageI = new DamageInfo();
                damageI.damage = 10;
                damageI.procCoefficient = 0;
                damageI.crit = false;
                Chat.AddMessage("Damge");

                if(damageInfo.attacker.GetComponent<HealthComponent>() != null)
                {
                    damageInfo.attacker.GetComponent<HealthComponent>().TakeDamage(damageI);
                }

                orig(self, damageInfo);
            };

        }
    }
}
