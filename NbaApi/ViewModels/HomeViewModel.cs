using NbaApi.ApiEntities;
using NbaApi.ApiEntities.Teams;
using NbaApi.Commands;
using NbaApi.Models;
using NbaApi.Services;
using NbaApi.Services.NBAApiService;
using NbaApi.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Response = NbaApi.ApiEntities.Teams.Response;

namespace NbaApi.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public WrapPanel MyPanel { get; set; }

        private ObservableCollection<Response> allTeams;

        public ObservableCollection<Response> AllTeams
        {
            get { return allTeams; }
            set { allTeams = value; OnPropertyChanged(); }
        }
        public RelayCommand SelectedTeamCommand { get; set; }

        private ObservableCollection<PageNo> allPages;

        public ObservableCollection<PageNo> AllPages
        {
            get { return allPages; }
            set { allPages = value; OnPropertyChanged(); }
        }

        private PageNo selectedPageNo;

        public PageNo SelectedPageNo
        {
            get { return selectedPageNo; }
            set
            {
                selectedPageNo = value; OnPropertyChanged();
                var no = SelectedPageNo.No;
                if (result != null)
                    AllTeams = new ObservableCollection<Response>(result.Skip((no - 1) * 10).Take(10));
            }
        }




        private Response selectedTeam;

        public Response SelectedTeam
        {
            get { return selectedTeam; }
            set { selectedTeam = value; OnPropertyChanged(); }
        }




        List<Response> result = null;
        public HomeViewModel()
        {
            LoadData();
        }
        public RelayCommand SelectPageCommand { get; set; }
        public async void LoadData()
        {
            SelectedPageNo = new PageNo
            {
                No = 1
            };
            SelectedTeam = new Response();

            var service = new NbaApiService();

            result = await service.GetTeamsAsync();

            SelectedTeamCommand = new RelayCommand((s) =>
            {
                var window = new PlayersOfTeamWindow();
                window.DataContext = new PlayersOfTeamViewModel(SelectedTeam);
                window.Show();




            });



            allPages = new ObservableCollection<PageNo>();
            var pageCount = decimal.Parse(result.Count.ToString()) / 10;
            int count = (int)Math.Ceiling(pageCount);

            for (int i = 0; i < count; i++)
            {
                allPages.Add(new PageNo
                {
                    No = i + 1
                });
            }

            AllTeams = new ObservableCollection<Response>(result.Skip(0).Take(10));


            SelectPageCommand = new RelayCommand((o) =>
            {
                var no = SelectedPageNo.No;
                AllTeams = new ObservableCollection<Response>(result.Skip((no - 1) * 10).Take(10));
            });

        }




    }
}
