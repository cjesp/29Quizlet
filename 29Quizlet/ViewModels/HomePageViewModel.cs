using Template10.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using _29Quizlet.Repositories;
using System.Collections.ObjectModel;
using _29Quizlet.Models.QuizletTypes.Feeds.Home;
using Windows.UI.Xaml.Controls;
using _29Quizlet.Models.QuizletTypes.Feeds;

namespace _29Quizlet.ViewModels
{
    public class HomePageViewModel : ViewModelBase
    {
        private IQuizletRESTApi _quizApi;
        public ObservableCollection<IHomeItemVM> HomeFeeds { get; set; }

        public HomePageViewModel()
        {
            _quizApi = new QuizletRemoteRESTApi();
            HomeFeeds = new ObservableCollection<IHomeItemVM>();
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await GetFeed();
        }

        public async Task GetFeed()
        {
            HomeFeeds.Clear();

            var feeds = await _quizApi.GetHomeFeed();
            foreach (var feed in feeds.items)
            {
                IHomeItemVM vm = null;

                if (feed is ClassFeed)
                {
                    vm = new ClassFeedVM((ClassFeed)feed);
                }
                if (feed is StudySessionItem)
                {
                    vm = new StudySessionItemVM((StudySessionItem)feed);
                }
                if (feed is Item)
                {
                    vm = new ItemVM((Item)feed);
                }

                if (vm != null)
                {
                    HomeFeeds.Add(vm);
                }
            }
        }

        public async Task Selected(object sender, SelectionChangedEventArgs e)
        {

            var homeFeedItem = (IHomeItemVM)((ListView)sender).SelectedItem;

            if (homeFeedItem is ItemVM)
            {
                var item = homeFeedItem as ItemVM;
                var set = await _quizApi.GetPrivateSet(item.SetId);

                NavigationService.Navigate(typeof(Views.SetDetailPage), set);
            }

            if (homeFeedItem is ClassFeedVM)
            {
                var item = homeFeedItem as ClassFeedVM;
                var theClass = await _quizApi.GetClass(item.ClassId);

                NavigationService.Navigate(typeof(Views.ClassDetailPage), theClass);
            }

            if (homeFeedItem is StudySessionItemVM)
            {
                var item = homeFeedItem as StudySessionItemVM;
                var set = await _quizApi.GetPrivateSet(item.SetId);

                NavigationService.Navigate(typeof(Views.SetDetailPage), set);
            }

        }
           
    }
}

