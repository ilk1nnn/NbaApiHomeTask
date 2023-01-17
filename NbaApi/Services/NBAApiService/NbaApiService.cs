using NbaApi.ApiEntities;
using NbaApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NbaApi.Services.NBAApiService
{



    public class NbaApiService
    {

        public async Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
        {
            if (ApiKeys.Key != String.Empty)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://api-nba-v1.p.rapidapi.com/players?team={teamId}&season=2021"),
                    Headers =
    {
        { "X-RapidAPI-Key", ApiKeys.Key },
        { "X-RapidAPI-Host", "api-nba-v1.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(body);
                    var players = data.response.ToList();
                    var result = from r in players
                                 select new Player
                                 {
                                     id = r.id,
                                     firstname = r.firstname,
                                     affiliation = r.affiliation,
                                     birth = r.birth,
                                     college = r.college,
                                     height = r.height,
                                     lastname = r.lastname,
                                     leagues = r.leagues,
                                     nba = r.nba,
                                     weight = r.weight
                                 };
                    return result.ToList();
                }
            }
            else
            {
                var result = JsonHelper<Player>.Deserialize("../../Data/players.json");
                return result;
            }
        }


        public async Task<List<Player>> GetPlayersAsync()
        {
            var result = JsonHelper<Player>.Deserialize("../../Data/players.json");
            return result;
        }

        public async Task<List<ApiEntities.Teams.Response>> GetTeamsAsync()
        {
            if (ApiKeys.Key != String.Empty)
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://api-nba-v1.p.rapidapi.com/teams"),
                    Headers =
    {
        { "X-RapidAPI-Key", ApiKeys.Key },
        { "X-RapidAPI-Host", "api-nba-v1.p.rapidapi.com" },
    },
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiEntities.Teams.Rootobject>(body);
                    var teams = data.response.ToList();

                    return teams;
                }
            }
            else
            {
                var result = JsonHelper<ApiEntities.Teams.Response>.Deserialize("../../Data/teams.json");
                return result;
            }

        }

    }
}

