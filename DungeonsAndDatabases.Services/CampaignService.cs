﻿using DungeonsAndDatabases.Data;
using DungeonsAndDatabases.Models.CampaignModels;
using DungeonsAndDatabases.Models.LogModels;
using DungeonsAndDatabases.Models.Loot;
using DungeonsAndDatabases.Models.MembershipModels;
using DungeonsAndDatabases.Models.PlayerModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDatabases.Services
{
    public class CampaignService
    {
        //GUID
        private readonly Guid _userId;

        public CampaignService(Guid userId)
        {
            _userId = userId;
        }

        //Create a new Campaign
        public async Task<bool> CreateCampaign(CampaignCreate model)
        {
            var entity =
                new Campaign()
                {
                    CampaignName = model.CampaignName,
                    GameSystem = model.GameSystem,
                    DmGuid = _userId
                };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Campaigns.Add(entity);
                return await ctx.SaveChangesAsync() == 1;
            }
        }

        //Get all Campaigns
        public async Task<IEnumerable<CampaignListView>> GetCampaigns()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = await
                   ctx
                       .Campaigns
                       .Where(e => e.DmGuid == _userId || e.Memberships.Any(x=> x.Character.PlayerID == _userId))
                       .Select(
                           e =>
                               new CampaignListView
                               {
                                   CampaignID = e.CampaignID,
                                   CampaignName = e.CampaignName,
                                   GameSystem = e.GameSystem,
                                   DmGuid = e.DmGuid
                               }).ToListAsync();
               
                return query;
            }
        }
        //Get a specific Campaign by ID
        public async Task<CampaignDetail> GetCampaignById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx.Campaigns
                    .Where(
                        x => x.CampaignID == id).FirstOrDefaultAsync();
                        
                    
                    //ctx
                    //    .Campaigns
                    //    .Single(e => e.CampaignID == id);
                
                    var campaignDetail =  new CampaignDetail
                    {
                        CampaignID = entity.CampaignID,
                        CampaignName = entity.CampaignName,
                        GameSystem = entity.GameSystem,
                        DmGuid = entity.DmGuid,
                        DmName = entity.DungeonMaster.PlayerName,
                        Memberships = entity.Memberships.Select(
                            e =>  //Shows the campaign membership
                            new MembershipWithPlayerName { 
                                CampaignId = e.CampaignId,
                                CampaignName = e.Campaign.CampaignName,
                                CharacterId = e.CharacterID,
                                CharacterName = e.Character.CharacterName,
                                PlayerName = e.Character.Player.PlayerName
                            }
                            ).ToList(),
                        CampaignLoot = entity.CampaignLoot.Select(
                            e => //Populates the campaign loot
                            new LootDetails
                            {
                                LootID = e.LootID,
                                Name = e.Name,
                                Description = e.Description,
                                ValueInGP = e.ValueInGP,
                                CampaignID = e.CampaignID
                            }
                            ).ToList(),
                        CampaignLog = entity.CampaignLog.Select(
                            e => //Populates the Log entries the user has on the campaign
                            new LogEntryListItem
                            {
                                LogID = e.LogID,
                                DateCreated = e.DateCreated,
                                DateUpdated = e.DateUpdated,
                                CampaignID = e.CampaignID,
                                CampaignName = e.Campaign.CampaignName,
                                Message = e.Message
                            }
                            ).ToList()
                    };
                  
                return campaignDetail;

            }
        }
        //Update a campaign 
        public async Task<bool> UpdateCampaign(int id,CampaignUpdate model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx.Campaigns
                    .Where(
                        e => e.CampaignID == id).FirstOrDefaultAsync();
                entity.CampaignName = model.CampaignName;
                entity.GameSystem = model.GameSystem;
                //entity.DmGuid = model.DmGuid;
                return await ctx.SaveChangesAsync() == 1;
            }
        }
        //Delete a campaign
        public async Task<bool> DeleteCampaign(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx.Campaigns
                    .Where(
                        e => e.CampaignID == id).FirstOrDefaultAsync();
                ctx.Campaigns.Remove(entity);
                return await ctx.SaveChangesAsync() == 1;
            }
        }
        //Check the user to see if they are the DM of the campaign
        public async Task<bool> CheckDMCredentials(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx.Campaigns
                    .Where(e => e.CampaignID == id).FirstOrDefaultAsync();
                if (entity == null || entity.DmGuid != _userId)
                    return false;
                return true;
            }
        }
    }
}
