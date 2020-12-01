﻿using DungeonsAndDatabases.Models.DND5EAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDatabases.Services
{
    public class DND5eAPI_TestService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _baseUrl = "https://www.dnd5eapi.co/api/";
        private readonly string _dnd5eAPI = "https://www.dnd5eapi.co";
        private readonly string _classes = "classes/";
        private readonly string _races = "races/";
        private readonly string _equipment = "equipment/";
        //    public async Task<Race> GetRaceFromAPIAsync(string race)
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + _races + race);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Race result = await response.Content.ReadAsAsync<Race>();
        //            return result;
        //        }
        //        return null;
        //    }
        //    public async Task<Race_Short> GetShortRaceFromAPIAsync(string race)
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + _races + race);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Race_Short result = await response.Content.ReadAsAsync<Race_Short>();
        //            return result;
        //        }
        //        return null;
        //    }

        //    public async Task<Classes_Short> GetShortClassFromAPIAsync(string classes)
        //    {
        //        HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + _classes + classes);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Classes_Short result = await response.Content.ReadAsAsync<Classes_Short>();
        //            return result;
        //        }
        //        return null;
        //    }
        public async Task<Equipment> GetEquipmentFromAPIAsync(string equipment)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_baseUrl + _classes + equipment);
            if (response.IsSuccessStatusCode)
            {
                Equipment result = await response.Content.ReadAsAsync<Equipment>();
                if (result.Equipment_Category.name == "Weapon")
                    result = await response.Content.ReadAsAsync<Weapon>();
                return result;
            }
            return null;
        }
    }
}
