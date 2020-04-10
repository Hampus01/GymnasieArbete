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
        internal static GameObject BiscoLeashPrefab;
        internal static ItemIndex ThornsPotion;

        Hooks hooks;
        Assets assets;

        public void Awake()
        {
            hooks = new Hooks();
            assets = new Assets();

            Hooks();
            Assets();
        }

        internal void Hooks()
        {
            On.RoR2.HealthComponent.TakeDamage += (orig, self, damageInfo) =>
            {
                if (self.body.inventory?.GetItemCount(ThornsPotion) > 0)
                {
                    Chat.AddMessage("Damage");
                    int procChance = Random.Range(1, 10);

                    if (procChance <= 3)
                    {
                        DamageInfo myDamageInfo = new DamageInfo()
                        {
                            damage = damageInfo.damage * self.body.inventory.GetItemCount(ThornsPotion),
                            attacker = self.body.gameObject,
                            position = damageInfo.attacker.transform.position,
                            damageType = DamageType.BypassArmor,
                            damageColorIndex = DamageColorIndex.Item,
                        };

                        damageInfo.attacker.GetComponent<HealthComponent>()?.TakeDamage(myDamageInfo);
                    }
                }

                orig(self, damageInfo);
            };
        }
        public void Assets()
        {

            const string ModPrefix = "@CustomItem:";
            const string PrefabPath = ModPrefix + "Assets/Import/belt/belt.prefab";
            const string IconPath = ModPrefix + "Assets/Import/belt_icon/belt_icon.png";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ModItem.rampage"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);
                var provider = new AssetBundleResourcesProvider(ModPrefix.TrimEnd(':'), bundle);
                ResourcesAPI.AddProvider(provider);

                BiscoLeashPrefab = bundle.LoadAsset<GameObject>("Assets/Import/belt/belt.prefab");
            }

            ItemDef itemDef = new ItemDef
            {
                name = "ThornsPotion",
                tier = ItemTier.Tier2,
                pickupModelPath = PrefabPath,
                pickupIconPath = IconPath,
                nameToken = "Thorn's Potion",
                pickupToken = "Return to sender",
                descriptionToken = "Have a 30% chance to return the damage delt to you (+100%)",
                loreToken = "Empty",
                tags = new[]
                {
                    ItemTag.Utility,
                    ItemTag.Damage
                }
            };

            ItemDisplayRule[] itemDisplayRules = new ItemDisplayRule[1]; // keep this null if you don't want the item to show up on the survivor 3d model. You can also have multiple rules !
            itemDisplayRules[0].followerPrefab = BiscoLeashPrefab; // the prefab that will show up on the survivor
            itemDisplayRules[0].childName = "Chest"; // this will define the starting point for the position of the 3d model, you can see what are the differents name available in the prefab model of the survivors
            itemDisplayRules[0].localScale = new Vector3(0.15f, 0.15f, 0.15f); // scale the model
            itemDisplayRules[0].localAngles = new Vector3(0f, 180f, 0f); // rotate the model
            itemDisplayRules[0].localPos = new Vector3(-0.35f, -0.1f, 0f); // position offset relative to the childName, here the survivor Chest

            var thornsPotion = new CustomItem(itemDef, itemDisplayRules);

            Class1.ThornsPotion = ItemAPI.Add(thornsPotion); // ItemAPI sends back the ItemIndex of your item
        }
    }

    public class Assets
    {

    }


    public class Hooks
    {

    }
}
