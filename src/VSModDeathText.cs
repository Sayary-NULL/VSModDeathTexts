using System;
using System.IO;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Newtonsoft.Json;
using Vintagestory.API.Config;

namespace VSModDeathText
{
    public class DeathTexts
    {
        public string Void { get; set; }

        public string Gravity { get; set; }

        public string Fire { get; set; }

        public string BluntAttack { get; set; }

        public string SlashingAttack { get; set; }

        public string PiercingAttack { get; set; }

        public string Suffocation { get; set; }

        public string Heal { get; set; }

        public string Poison { get; set; }

        public string Hunger { get; set; }

        public string Crushing { get; set; }
    }

    public class ModDeathTexts : ModSystem
    {
        ICoreServerAPI SAPI;

        DeathTexts deathTexts;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            SAPI = api;

            string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VintagestoryData\ModConfig\DeathTexts.json";

            try
            {
                if(!System.IO.File.Exists(Path))
                {
                    using (StreamWriter write = new StreamWriter(Path, false, Encoding.UTF8))
                    {
                        DeathTexts deathTexts = new DeathTexts();
                        deathTexts.Void = "it's not worth the effort.\r\n {Name}, isn't it?)";
                        string json = JsonConvert.SerializeObject(deathTexts);
                        write.Write(json);
                    }
                }

                using (StreamReader read = new StreamReader(Path, Encoding.UTF8))
                {
                    string json = read.ReadToEnd();
                    deathTexts = JsonConvert.DeserializeObject<DeathTexts>(json);
                    System.Diagnostics.Debug.WriteLine("Loads" + json);
                }

                SAPI.Event.PlayerDeath += Event_PlayerDeath;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ModDeathTexts] Error: " + ex);
            }
        }

        private void Event_PlayerDeath(IServerPlayer byPlayer, DamageSource damageSource)
        {
            string outstr = "";
            if (damageSource != null)
            {
                switch (damageSource.Type)
                {
                    case EnumDamageType.Gravity:
                        outstr = deathTexts.Gravity;
                        break;
                    case EnumDamageType.Fire:
                        outstr = deathTexts.Fire;
                        break;
                    case EnumDamageType.BluntAttack:
                        outstr = deathTexts.BluntAttack;
                        break;
                    case EnumDamageType.SlashingAttack:
                        outstr = deathTexts.SlashingAttack;
                        break;
                    case EnumDamageType.PiercingAttack:
                        outstr = deathTexts.PiercingAttack;
                        break;
                    case EnumDamageType.Suffocation:
                        outstr = deathTexts.Suffocation;
                        break;
                    case EnumDamageType.Heal:
                        outstr = deathTexts.Heal;
                        break;
                    case EnumDamageType.Poison:
                        outstr = deathTexts.Poison;
                        break;
                    case EnumDamageType.Hunger:
                        outstr = deathTexts.Hunger;
                        break;
                    case EnumDamageType.Crushing:
                        outstr = deathTexts.Crushing;
                        break;
                    default:
                        break;
                }
            }
            else outstr = deathTexts.Void;

            if (outstr == null)
                return;

            if (outstr.IndexOf("{Name}") != -1)
                outstr = outstr.Replace("{Name}", byPlayer.PlayerName);

            SAPI.SendMessageToGroup(GlobalConstants.AllChatGroups, outstr, EnumChatType.Notification);
        }
    }
}