﻿using DungeonsAndDatabases.Data;
using DungeonsAndDatabases.Models.CampaignModels;
using DungeonsAndDatabases.Models.CharacterModels;
using DungeonsAndDatabases.Models.MembershipModels;
using DungeonsAndDatabases.Models.PlayerModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace DungeonsAndDatabases.Services
{
    public class PlayerService
    {   //Guid
        private readonly Guid _playerId;

        public PlayerService(Guid playerId)
        {
            _playerId = playerId;
        }
        //Create
        public async Task<bool> CreatePlayer(PlayerCreate model)
        {
            var entity = new Player()
            {
                PlayerName = model.PlayerName,
                PlayerID = _playerId
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Players.Add(entity);
                return await ctx.SaveChangesAsync() == 1;
            }
        }
        //Read
        public async Task<IEnumerable<PlayerListItem>> GetPlayers()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = await
                    ctx
                        .Players
                        .Select(
                            e =>
                                new PlayerListItem
                                {
                                    PlayerID = e.PlayerID,
                                    PlayerName = e.PlayerName
                                }
                        ).ToListAsync();
                return query;
            }
        }
        //Method to get a player details by player ID
        public async Task<PlayerDetails> GetPlayerById(Guid id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx.Players.FindAsync(id);
                ctx
                    .Players
                    .Single(e => e.PlayerID == id);
                return
                    new PlayerDetails
                    {
                        PlayerName = entity.PlayerName,
                        PlayerID = entity.PlayerID,

                        Characters = entity.Characters.Select(
                            e =>
                            new CharacterListItem
                            {
                                CharacterId = e.CharacterID,
                                CharacterName = e.CharacterName,
                                Level = e.Level,
                                PlayerID = e.PlayerID


                            }
                            ).ToList(),
                        CharacterCampaigns = entity.Campaigns.Select(

                            e =>
                            new CampaignListViewWithCharacter
                            {
                                CampaignID = e.CampaignID,
                                CampaignName = e.CampaignName,
                                GameSystem = e.GameSystem,
                                DmName = e.DungeonMaster.PlayerName,
                                Characters = e.Memberships.Select(
                                    x => new CharacterDetail
                                    {
                                        PlayerID = x.Character.Player.PlayerID,
                                        CharacterName = x.Character.CharacterName,
                                        Race = x.Character.Race,
                                        Class = x.Character.Class,
                                        Level = x.Character.Level
                                    }
                                    ).ToList()
                            })
                            .ToList()
                    };
            }
        }
        //Update
        public async Task<bool> UpdatePlayer(Guid id, PlayerEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Players
                        .Single(e => e.PlayerID == id);
                entity.PlayerName = model.PlayerName;
                return await ctx.SaveChangesAsync() == 1;
            }
        }
        //Delete
        public async Task<bool> DeletePlayer(Guid id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx
                    .Players
                    .Single(e => e.PlayerID == id);
                ctx.Players.Remove(entity);

                return await ctx.SaveChangesAsync() == 1;
            }
        }

        //Check for duplicate player
        public async Task<bool> CheckForDuplicatePlayer()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = await ctx
                    .Players
                    .FirstOrDefaultAsync(e => e.PlayerID == _playerId);
                if (entity != null)
                    return true;
                return false;
            }
        }
    }
}
