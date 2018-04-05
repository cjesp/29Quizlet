using _29Quizlet.Models.QuizletTypes.Uploading;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Unity;
using _29Quizlet.Helpers.Enums;

namespace _29Quizlet.ViewModels
{
    public class NewClassPageViewModel : ViewModelBase
    {
        private IQuizletRESTApi _quizletApi;
        private readonly ISetsLocalStorage _localSets;
        string _ClassName = default(string);
        public string ClassName { get { return _ClassName; } set { Set(ref _ClassName, value); } }

        string _ClassDescription = default(string);
        public string ClassDescription { get { return _ClassDescription; } set { Set(ref _ClassDescription, value); } }

        bool _ShowClassNameError = default(bool);
        public bool ShowClassNameError { get { return _ShowClassNameError; } set { Set(ref _ShowClassNameError, value); } }

        bool _ShowClassDescriptionError = default(bool);
        public bool ShowClassDescriptionError { get { return _ShowClassDescriptionError; } set { Set(ref _ShowClassDescriptionError, value); } }

        bool _ShowModal = default(bool);
        public bool ShowModal { get { return _ShowModal; } set { Set(ref _ShowModal, value); } }

        public ObservableCollection<ClassSetsViewModel> ClassSets { get; set; }
        public ObservableCollection<ClassSetsViewModel> MySets { get; set; }


        public NewClassPageViewModel()
        {
            _quizletApi = App.Container.Resolve<IQuizletRESTApi>();
            _localSets = App.Container.Resolve<ISetsLocalStorage>();
            ClassSets = new ObservableCollection<ClassSetsViewModel>();
            MySets = new ObservableCollection<ClassSetsViewModel>();

            ShowClassDescriptionError = false;
            ShowClassNameError = false;
            ShowModal = true;
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var localSets = await _localSets.GetAllSets();

            if (localSets.Any())
            {
                foreach (var set in localSets)
                {
                    var setVm = new ClassSetsViewModel()
                    {
                        Id = set.Id,
                        Terms = $"{set.TermCount} terms",
                        Title = set.Title,
                    };

                    MySets.Add(setVm);
                }
            }
            else
            {
                var sets = await _quizletApi.GetCreatedSets();
                foreach (var set in sets.Items)
                {
                    var setVm = new ClassSetsViewModel()
                    {
                        Id = set.ItemData.Id,
                        Terms = $"{set.ItemData.TermCount} terms",
                        Title = set.ItemData.Title,
                    };

                    MySets.Add(setVm);
                }
            }


            await Task.CompletedTask;
        }



        public void ClassNameChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                if (!string.IsNullOrEmpty(txtBox.Text))
                {
                    ShowClassNameError = false;
                }

            }
        }

        public void ClassDescriptionChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                if (!string.IsNullOrEmpty(txtBox.Text))
                {
                    ShowClassDescriptionError = false;
                }

            }
        }

        public void RemoveSets(object sender, RoutedEventArgs e)
        {
            var selectedSets = ClassSets.Where(x => x.Selected).ToList();

            if (selectedSets == null || selectedSets.Count() == 0)
                return;

            foreach (var set in selectedSets)
            {
                ClassSets.Remove(set);
            }
        }

        public void AddSets(object sender, RoutedEventArgs e)
        {
            var selectedSets = MySets.Where(x => x.Selected);

            if (selectedSets == null || selectedSets.Count() == 0)
                return;

            foreach (var set in selectedSets)
            {
                if (ClassSets.Where(x => x.Id == set.Id).Any())
                    continue;
                else
                    ClassSets.Add(set.Copy(set));
            }

            ShowModal = !ShowModal;

        }

        public void FlipModal(object sender, RoutedEventArgs e)
        {
            ShowModal = !ShowModal;
        }

        public async void Save(object sender, RoutedEventArgs e)
        {
            bool error = false;
            if (string.IsNullOrEmpty(ClassDescription))
            {
                error = true;
                ShowClassDescriptionError = true;
            }
            if (string.IsNullOrEmpty(ClassName))
            {
                error = true;
                ShowClassNameError = true;
            }

            // if we encoutered an error we stop here
            if (error)
                return;

            var clazz = new CreateClass()
            {
                name = ClassName,
                description = ClassDescription
            };

            try
            {
                Views.Busy.SetBusy(true, "Creating class...");
                var result = await _quizletApi.AddClass(clazz);

                if (result != null)
                {
                    var classId = (int)result;
                    var classSetIds = ClassSets.Select(x => x.Id);
                    var res = await _quizletApi.AddSetsToClass(classId, classSetIds.ToArray());

                    if (res)
                    {
                        NavigationService.Navigate(typeof(Views.MyClassesPage), NewClassNavigation.UpdatedOrNewClass);
                    }
                    else
                    {
                        var dialog = new MessageDialog($"Error: The Class ({clazz.name}) was created but the sets wasn't added to the class.");
                        await dialog.ShowAsync();
                        NavigationService.Navigate(typeof(Views.MyClassesPage), NewClassNavigation.None);
                    }
                }
                else
                {
                    var dialog = new MessageDialog($"Error: The Class ({clazz.name}) wasn't created.");
                    await dialog.ShowAsync();
                    NavigationService.Navigate(typeof(Views.MyClassesPage), NewClassNavigation.UpdatedOrNewClass);
                }

            }
            catch (Exception)
            {
                var dialog = new MessageDialog("Something went wrong!");
                await dialog.ShowAsync();
            }
            finally
            {
                Views.Busy.SetBusy(false, null);
            }


        }

    }

    public class ClassSetsViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Terms { get; set; }
        public bool Selected { get; set; } = false;

        public ClassSetsViewModel Copy(ClassSetsViewModel vm)
        {
            var copy = new ClassSetsViewModel()
            {
                Id = vm.Id,
                Selected = false,
                Title = vm.Title,
                Terms = vm.Terms
            };

            return copy;
        }
    }
}
