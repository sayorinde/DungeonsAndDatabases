﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDatabases.Data
{
    public class Campaign
    {
        //Campaign Data Class
        [Key]
        public int CampaignID { get; set; } //Campaign ID number
        [Required]
        public string CampaignName { get; set; } //Name of the Campaign
        public string GameSystem { get; set; } // Name of the Game System being played (ie. 5e, 3.5, Pathfinder, Call of Cthullu)
        [ForeignKey(nameof(DungeonMaster))] //Links DungeonMaster Player to the player GUID of the dungeon master
        public Guid DmGuid { get; set; }
        public virtual Player DungeonMaster { get; set; }
        public virtual List<Membership> Memberships { get; set; } = new List<Membership>(); //List of characters who are members of this campaign
        public virtual List<Loot> CampaignLoot { get; set; } = new List<Loot>(); //Campaign specific loot items
        public virtual List<LogEntry> CampaignLog { get; set; } = new List<LogEntry>(); // Campaign Log
    }
}
