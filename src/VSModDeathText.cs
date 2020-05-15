using System;
using System.IO;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Newtonsoft.Json;
using Vintagestory.API.Config;
using System.Collections.Generic;

namespace VSModDeathText
{
    public class DeathTexts
    {
        public List<string> Void { get; set; }

        public List<string> Gravity { get; set; }

        public List<string> Fire { get; set; }

        public List<string> BluntAttack { get; set; }

        public List<string> SlashingAttack { get; set; }

        public List<string> PiercingAttack { get; set; }

        public List<string> Suffocation { get; set; }

        public List<string> Heal { get; set; }

        public List<string> Poison { get; set; }

        public List<string> Hunger { get; set; }

        public List<string> Crushing { get; set; }
    }

    public class ModDeathTexts : ModSystem
    {
        ICoreServerAPI SAPI;

        DeathTexts deathTexts;

        Random rand;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            SAPI = api;

            string Path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\VintagestoryData\ModConfig\DeathTexts.json";

            rand = new Random();

            try
            {
                if(!System.IO.File.Exists(Path))
                {
                    using (StreamWriter write = new StreamWriter(Path, false, Encoding.UTF8))
                    {
                        DeathTexts deathTexts = new DeathTexts();
                        deathTexts.Void = new List<string>();
                        deathTexts.Void.Add("it's not worth the effort.\r\n {Name}, isn't it?)");
                        deathTexts.Void.Add("it wasn't worth it!");
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
                        if (deathTexts.Gravity != null)
                        {
                            int size = deathTexts.Gravity.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Gravity[index];
                        }
                        break;
                    case EnumDamageType.Fire:
                        if (deathTexts.Fire != null)
                        {
                            int size = deathTexts.Fire.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Fire[index];
                        }
                        break;
                    case EnumDamageType.BluntAttack:
                        if (deathTexts.BluntAttack != null)
                        {
                            int size = deathTexts.BluntAttack.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.BluntAttack[index];
                        }
                        break;
                    case EnumDamageType.SlashingAttack:
                        if (deathTexts.SlashingAttack != null)
                        {
                            int size = deathTexts.SlashingAttack.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.SlashingAttack[index];
                        }
                        break;
                    case EnumDamageType.PiercingAttack:
                        if (deathTexts.PiercingAttack != null)
                        {
                            int size = deathTexts.PiercingAttack.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.PiercingAttack[index];
                        }
                        break;
                    case EnumDamageType.Suffocation:
                        if (deathTexts.Suffocation != null)
                        {
                            int size = deathTexts.Suffocation.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Suffocation[index];
                        }
                        break;
                    case EnumDamageType.Heal:
                        if (deathTexts.Heal != null)
                        {
                            int size = deathTexts.Heal.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Heal[index];
                        }
                        break;
                    case EnumDamageType.Poison:
                        if (deathTexts.Poison != null)
                        {
                            int size = deathTexts.Poison.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Poison[index];
                        }
                        break;
                    case EnumDamageType.Hunger:
                        if (deathTexts.Hunger != null)
                        {
                            int size = deathTexts.Hunger.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Hunger[index];
                        }
                        break;
                    case EnumDamageType.Crushing:
                        if(deathTexts.Crushing != null)
                        {
                            int size = deathTexts.Crushing.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Crushing[index];
                        }
                        break;
                    default:
                        {
                            int size = deathTexts.Void.Count;
                            int index = rand.Next(size);
                            outstr = deathTexts.Void[index];
                            break;
                        }
                }
            }
            else
            {
                int size = deathTexts.Void.Count;
                int index = rand.Next(size);
                outstr = deathTexts.Void[index];
            }

            if (outstr == null)
                return;

            if (outstr.IndexOf("{Name}") != -1)
                outstr = outstr.Replace("{Name}", byPlayer.PlayerName);

            SAPI.SendMessageToGroup(GlobalConstants.AllChatGroups, outstr, EnumChatType.Notification);
        }
    }
}