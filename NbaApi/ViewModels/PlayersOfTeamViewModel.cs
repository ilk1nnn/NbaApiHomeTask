using NbaApi.ApiEntities.Teams;
using NbaApi.Models;
using NbaApi.Services.NBAApiService;
using NbaApi.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NbaApi.ViewModels
{
    public class PlayersOfTeamViewModel : BaseViewModel
    {

        private ObservableCollection<Player> allPlayers;

        public ObservableCollection<Player> AllPlayers
        {
            get { return allPlayers; }
            set { allPlayers = value; OnPropertyChanged(); }
        }


        private ObservableCollection<Response> allTeams;

        public ObservableCollection<Response> AllTeams
        {
            get { return allTeams; }
            set { allTeams = value;OnPropertyChanged(); }
        }

        List<Response> result = null;

        List<Player> playerResult = null;

        public async void Load(Response team)
        {

            var service = new NbaApiService();
            result = await service.GetTeamsAsync();
            AllTeams = new ObservableCollection<Response>(result);

            for (int i = 0; i < AllTeams.Count; i++)
            {
                if (AllTeams[i].id == team.id)
                {
                    playerResult = await service.GetPlayersByTeamIdAsync(team.id);
                    AllPlayers = new ObservableCollection<Player>(playerResult);

                }
            }




        }

        public PlayersOfTeamViewModel(Response team)
        {
            Load(team);
        }


    }
}
