using _29Quizlet.Models;
using _29Quizlet.Models.QuizletTypes.Uploading;
using _29Quizlet.Models.ViewModels;
using _29Quizlet.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Unity;

namespace _29Quizlet.ViewModels
{
    public class NewSetPageViewModel : ViewModelBase
    {
        private IQuizletRESTApi _quizletApi { get; set; }
        private ISetsLocalStorage _setsLocalStorage;
        public ObservableCollection<LanguageViewModel> LanguageOptions { get; set; }
        public ObservableCollection<TermDefinitionViewModel> TermsDefinitions { get; set; }
        public List<long> TermIds { get; set; }
        public bool EditMode { get; set; }
        public long SetId { get; set; }

        string _Title = default(string);
        public string Title { get { return _Title; } set { Set(ref _Title, value); } }
        
        bool _ShowTitleError = default(bool);
        public bool ShowTitleError { get { return _ShowTitleError; } set { Set(ref _ShowTitleError, value); } }

        LanguageViewModel _SelectedTermLanguage = default(LanguageViewModel);
        public LanguageViewModel SelectedTermLanguage { get { return _SelectedTermLanguage; } set { Set(ref _SelectedTermLanguage, value); } }
        
        LanguageViewModel _SelectedDefinitionLanguage = default(LanguageViewModel);
        public LanguageViewModel SelectedDefinitionLanguage { get { return _SelectedDefinitionLanguage; } set { Set(ref _SelectedDefinitionLanguage, value); } }

        bool _EveryoneOrMe = default(bool);
        public bool EveryoneOrMe { get { return _EveryoneOrMe; } set { Set(ref _EveryoneOrMe, value); } }

        public NewSetPageViewModel()
        {
            _quizletApi = App.Container.Resolve<IQuizletRESTApi>();
            _setsLocalStorage = App.Container.Resolve<ISetsLocalStorage>();

            TermsDefinitions = new ObservableCollection<TermDefinitionViewModel>()
            {
                new TermDefinitionViewModel() { Valid = true },
                new TermDefinitionViewModel() { Valid = true }
            };

            LanguageOptions = new ObservableCollection<LanguageViewModel>()
            {
                new LanguageViewModel() { CountryCode =  "af", Language = " Afrikaans" },
                new LanguageViewModel() { CountryCode =  "ak", Language = " Akan" },
                new LanguageViewModel() { CountryCode =  "akk", Language = " Akkadian" },
                new LanguageViewModel() { CountryCode =  "sq", Language = " Albanian" },
                new LanguageViewModel() { CountryCode =  "ase", Language = " American Sign Language" },
                new LanguageViewModel() { CountryCode =  "am", Language = " Amharic" },
                new LanguageViewModel() { CountryCode =  "ang", Language = " Anglo-Saxon" },
                new LanguageViewModel() { CountryCode =  "ar", Language = " Arabic" },
                new LanguageViewModel() { CountryCode =  "hy", Language = " Armenian" },
                new LanguageViewModel() { CountryCode =  "az", Language = " Azerbaijani" },
                new LanguageViewModel() { CountryCode =  "eu", Language = " Basque" },
                new LanguageViewModel() { CountryCode =  "be", Language = " Belarusian" },
                new LanguageViewModel() { CountryCode =  "bn", Language = " Bengali" },
                new LanguageViewModel() { CountryCode =  "bh", Language = " Bihari" },
                new LanguageViewModel() { CountryCode =  "bs", Language = " Bosnian" },
                new LanguageViewModel() { CountryCode =  "bg", Language = " Bulgarian" },
                new LanguageViewModel() { CountryCode =  "br", Language = " Breton" },
                new LanguageViewModel() { CountryCode =  "my", Language = " Burmese" },
                new LanguageViewModel() { CountryCode =  "ca", Language = " Catalan" },
                new LanguageViewModel() { CountryCode =  "ceb", Language = " Cebuano" },
                new LanguageViewModel() { CountryCode =  "ch", Language = " Chamorro" },
                new LanguageViewModel() { CountryCode =  "chem", Language = " Chemistry" },
                new LanguageViewModel() { CountryCode =  "chr", Language = " Cherokee" },
                new LanguageViewModel() { CountryCode =  "zh-CN", Language = " Chinese (Simplified)" },
                new LanguageViewModel() { CountryCode =  "zh-pi", Language = " Chinese (Pinyin)" },
                new LanguageViewModel() { CountryCode =  "zh-TW", Language = " Chinese (Traditional)" },
                new LanguageViewModel() { CountryCode =  "co", Language = " Corsican" },
                new LanguageViewModel() { CountryCode =  "cho", Language = " Choctaw" },
                new LanguageViewModel() { CountryCode =  "cop", Language = " Coptic" },
                new LanguageViewModel() { CountryCode =  "hr", Language = " Croatian" },
                new LanguageViewModel() { CountryCode =  "cs", Language = " Czech" },
                new LanguageViewModel() { CountryCode =  "da", Language = " Danish" },
                new LanguageViewModel() { CountryCode =  "den", Language = " Dene" },
                new LanguageViewModel() { CountryCode =  "dv", Language = " Dhivehi" },
                new LanguageViewModel() { CountryCode =  "luo", Language = " Dholuo" },
                new LanguageViewModel() { CountryCode =  "nl", Language = " Dutch" },
                new LanguageViewModel() { CountryCode =  "en", Language = " English" },
                new LanguageViewModel() { CountryCode =  "eo", Language = " Esperanto" },
                new LanguageViewModel() { CountryCode =  "et", Language = " Estonian" },
                new LanguageViewModel() { CountryCode =  "fo", Language = " Faroese" },
                new LanguageViewModel() { CountryCode =  "fi", Language = " Finnish" },
                new LanguageViewModel() { CountryCode =  "dyo", Language = " Jola-Fonyi" },
                new LanguageViewModel() { CountryCode =  "fr", Language = " French" },
                new LanguageViewModel() { CountryCode =  "ff", Language = " Fula" },
                new LanguageViewModel() { CountryCode =  "gd", Language = " Gaelic" },
                new LanguageViewModel() { CountryCode =  "gl", Language = " Galician" },
                new LanguageViewModel() { CountryCode =  "ka", Language = " Georgian" },
                new LanguageViewModel() { CountryCode =  "de", Language = " German" },
                new LanguageViewModel() { CountryCode =  "got", Language = " Gothic" },
                new LanguageViewModel() { CountryCode =  "el", Language = " Greek" },
                new LanguageViewModel() { CountryCode =  "gn", Language = " Guarani" },
                new LanguageViewModel() { CountryCode =  "gu", Language = " Gujarati" },
                new LanguageViewModel() { CountryCode =  "hai", Language = " Haida" },
                new LanguageViewModel() { CountryCode =  "ht", Language = " Haitian Creole" },
                new LanguageViewModel() { CountryCode =  "ha", Language = " Hausa" },
                new LanguageViewModel() { CountryCode =  "haw", Language = " Hawaiian" },
                new LanguageViewModel() { CountryCode =  "iw", Language = " Hebrew" },
                new LanguageViewModel() { CountryCode =  "hi", Language = " Hindi" },
                new LanguageViewModel() { CountryCode =  "hmv", Language = " Hmong" },
                new LanguageViewModel() { CountryCode =  "hu", Language = " Hungarian" },
                new LanguageViewModel() { CountryCode =  "is", Language = " Icelandic" },
                new LanguageViewModel() { CountryCode =  "ig", Language = " Igbo" },
                new LanguageViewModel() { CountryCode =  "hil", Language = " Ilonggo" },
                new LanguageViewModel() { CountryCode =  "id", Language = " Indonesian" },
                new LanguageViewModel() { CountryCode =  "ipa", Language = " International Phonetic Alphabet (IPA)" },
                new LanguageViewModel() { CountryCode =  "iu", Language = " Inuktitut" },
                new LanguageViewModel() { CountryCode =  "ga", Language = " Irish" },
                new LanguageViewModel() { CountryCode =  "it", Language = " Italian" },
                new LanguageViewModel() { CountryCode =  "ja", Language = " Japanese" },
                new LanguageViewModel() { CountryCode =  "ja-ro", Language = " Japanese (Romaji)" },
                new LanguageViewModel() { CountryCode =  "jv", Language = " Javanese" },
                new LanguageViewModel() { CountryCode =  "kg", Language = " KiKongo" },
                new LanguageViewModel() { CountryCode =  "kin", Language = " Kinyarwanda" },
                new LanguageViewModel() { CountryCode =  "kio", Language = " Kiowa" },
                new LanguageViewModel() { CountryCode =  "kn", Language = " Kannada" },
                new LanguageViewModel() { CountryCode =  "kk", Language = " Kazakh" },
                new LanguageViewModel() { CountryCode =  "km", Language = " Khmer" },
                new LanguageViewModel() { CountryCode =  "mjd", Language = " Konkow" },
                new LanguageViewModel() { CountryCode =  "ko", Language = " Korean" },
                new LanguageViewModel() { CountryCode =  "ksw", Language = " Sgaw Karen" },
                new LanguageViewModel() { CountryCode =  "ku", Language = " Kurdish" },
                new LanguageViewModel() { CountryCode =  "ky", Language = " Kyrgyz" },
                new LanguageViewModel() { CountryCode =  "lkt", Language = " Lakota" },
                new LanguageViewModel() { CountryCode =  "lo", Language = " Lao" },
                new LanguageViewModel() { CountryCode =  "la", Language = " Latin" },
                new LanguageViewModel() { CountryCode =  "lv", Language = " Latvian" },
                new LanguageViewModel() { CountryCode =  "unm", Language = " Lenape" },
                new LanguageViewModel() { CountryCode =  "ln", Language = " Lingala" },
                new LanguageViewModel() { CountryCode =  "lt", Language = " Lithuanian" },
                new LanguageViewModel() { CountryCode =  "lua", Language = " Luba-Kasai" },
                new LanguageViewModel() { CountryCode =  "lb", Language = " Luxembourgish" },
                new LanguageViewModel() { CountryCode =  "mk", Language = " Macedonian" },
                new LanguageViewModel() { CountryCode =  "ms", Language = " Malay" },
                new LanguageViewModel() { CountryCode =  "mg", Language = " Malagasy" },
                new LanguageViewModel() { CountryCode =  "ml", Language = " Malayalam" },
                new LanguageViewModel() { CountryCode =  "mt", Language = " Maltese" },
                new LanguageViewModel() { CountryCode =  "mnk", Language = " Mandinka" },
                new LanguageViewModel() { CountryCode =  "mi", Language = " Maori" },
                new LanguageViewModel() { CountryCode =  "rar", Language = " Maori (Cook Islands)" },
                new LanguageViewModel() { CountryCode =  "mr", Language = " Marathi" },
                new LanguageViewModel() { CountryCode =  "mh", Language = " Marshallese" },
                new LanguageViewModel() { CountryCode =  "math", Language = " Math / Symbols" },
                new LanguageViewModel() { CountryCode =  "moh", Language = " Mohawk" },
                new LanguageViewModel() { CountryCode =  "mn", Language = " Mongolian" },
                new LanguageViewModel() { CountryCode =  "nah", Language = " Nahuatl" },
                new LanguageViewModel() { CountryCode =  "nv", Language = " Navajo" },
                new LanguageViewModel() { CountryCode =  "ne", Language = " Nepali" },
                new LanguageViewModel() { CountryCode =  "no", Language = " Norwegian" },
                new LanguageViewModel() { CountryCode =  "oc", Language = " Occitan" },
                new LanguageViewModel() { CountryCode =  "or", Language = " Oriya" },
                new LanguageViewModel() { CountryCode =  "om", Language = " Oromo" },
                new LanguageViewModel() { CountryCode =  "oj", Language = " Ojibwe" },
                new LanguageViewModel() { CountryCode =  "pi", Language = " Pāli" },
                new LanguageViewModel() { CountryCode =  "ps", Language = " Pashto" },
                new LanguageViewModel() { CountryCode =  "fa", Language = " Persian" },
                new LanguageViewModel() { CountryCode =  "fil", Language = " Filipino" },
                new LanguageViewModel() { CountryCode =  "pl", Language = " Polish" },
                new LanguageViewModel() { CountryCode =  "pt", Language = " Portuguese" },
                new LanguageViewModel() { CountryCode =  "pa", Language = " Punjabi" },
                new LanguageViewModel() { CountryCode =  "qu", Language = " Quechua" },
                new LanguageViewModel() { CountryCode =  "ro", Language = " Romanian" },
                new LanguageViewModel() { CountryCode =  "rm", Language = " Romansh" },
                new LanguageViewModel() { CountryCode =  "ru", Language = " Russian" },
                new LanguageViewModel() { CountryCode =  "sm", Language = " Samoan" },
                new LanguageViewModel() { CountryCode =  "sa", Language = " Sanskrit" },
                new LanguageViewModel() { CountryCode =  "see", Language = " Seneca" },
                new LanguageViewModel() { CountryCode =  "sr", Language = " Serbian" },
                new LanguageViewModel() { CountryCode =  "shn", Language = " Shan" },
                new LanguageViewModel() { CountryCode =  "sd", Language = " Sindhi" },
                new LanguageViewModel() { CountryCode =  "si", Language = " Sinhalese" },
                new LanguageViewModel() { CountryCode =  "sk", Language = " Slovak" },
                new LanguageViewModel() { CountryCode =  "sl", Language = " Slovenian" },
                new LanguageViewModel() { CountryCode =  "so", Language = " Somali" },
                new LanguageViewModel() { CountryCode =  "es", Language = " Spanish" },
                new LanguageViewModel() { CountryCode =  "su", Language = " Sundanese" },
                new LanguageViewModel() { CountryCode =  "sw", Language = " Swahili" },
                new LanguageViewModel() { CountryCode =  "sv", Language = " Swedish" },
                new LanguageViewModel() { CountryCode =  "syc", Language = " Syriac" },
                new LanguageViewModel() { CountryCode =  "tl", Language = " Tagalog" },
                new LanguageViewModel() { CountryCode =  "tg", Language = " Tajik" },
                new LanguageViewModel() { CountryCode =  "ta", Language = " Tamil" },
                new LanguageViewModel() { CountryCode =  "tt", Language = " Tatar" },
                new LanguageViewModel() { CountryCode =  "te", Language = " Telugu" },
                new LanguageViewModel() { CountryCode =  "tet", Language = " Tetum" },
                new LanguageViewModel() { CountryCode =  "th", Language = " Thai" },
                new LanguageViewModel() { CountryCode =  "bo", Language = " Tibetan" },
                new LanguageViewModel() { CountryCode =  "ti", Language = " Tigrinya" },
                new LanguageViewModel() { CountryCode =  "to", Language = " Tonga" },
                new LanguageViewModel() { CountryCode =  "ood", Language = " Tohono O'odham" },
                new LanguageViewModel() { CountryCode =  "trc", Language = " Triki" },
                new LanguageViewModel() { CountryCode =  "tr", Language = " Turkish" },
                new LanguageViewModel() { CountryCode =  "tyv", Language = " Tuvan" },
                new LanguageViewModel() { CountryCode =  "uk", Language = " Ukrainian" },
                new LanguageViewModel() { CountryCode =  "ur", Language = " Urdu" },
                new LanguageViewModel() { CountryCode =  "uz", Language = " Uzbek" },
                new LanguageViewModel() { CountryCode =  "ug", Language = " Uighur" },
                new LanguageViewModel() { CountryCode =  "vi", Language = " Vietnamese" },
                new LanguageViewModel() { CountryCode =  "cy", Language = " Welsh" },
                new LanguageViewModel() { CountryCode =  "fy", Language = " Western Frisian" },
                new LanguageViewModel() { CountryCode =  "win", Language = " Winnebago" },
                new LanguageViewModel() { CountryCode =  "wo", Language = " Wolof" },
                new LanguageViewModel() { CountryCode =  "xh", Language = " Xhosa" },
                new LanguageViewModel() { CountryCode =  "yi", Language = " Yiddish" },
                new LanguageViewModel() { CountryCode =  "yo", Language = " Yoruba" },
                new LanguageViewModel() { CountryCode =  "zu", Language = " Zulu" },
                new LanguageViewModel() { CountryCode =  "??", Language = " Other / Unknown" },
            };

            // the defaults
            SelectedTermLanguage = LanguageOptions[36];
            SelectedDefinitionLanguage = LanguageOptions[36];
            ShowTitleError = false;
            EditMode = false;
            TermIds = new List<long>();
            
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            var set = parameter as Set;

            if (parameter != null)
            {
                EditMode = true;
                TermsDefinitions.Clear();

                foreach (var term in set.Terms)
                {
                    var termAndDef = new TermDefinitionViewModel()
                    {
                        Definition = term.Definition,
                        Term = term.TermText,
                        Valid = true,
                    };

                    TermsDefinitions.Add(termAndDef);
                    TermIds.Add(term.Id);
                }

                Title = set.Title;

                if (set.Visibility == "public")
                    EveryoneOrMe = true;
                else
                    EveryoneOrMe = false;

                SetId = set.Id;

                var termLang = LanguageOptions.
                    Where(x => x.CountryCode == set.LangTerms)
                    .SingleOrDefault();

                var defLang = LanguageOptions.
                    Where(x => x.CountryCode == set.LangDefinitions)
                    .SingleOrDefault();

                SelectedDefinitionLanguage = defLang;
                SelectedTermLanguage = termLang;

            }

            await Task.CompletedTask;
        }

        public void TitleTextChanged(object sender, TextChangedEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null)
            {
                if (!string.IsNullOrEmpty(txtBox.Text))
                {
                    ShowTitleError = false;
                }

            }
        }

        public async void Save(object sender, RoutedEventArgs e)
        {
            var error = false;

            if (string.IsNullOrEmpty(Title))
            {
                error = true;
                ShowTitleError = true;
            }

            foreach (var termDef in TermsDefinitions)
            {
                if (string.IsNullOrEmpty(termDef.Term) || string.IsNullOrEmpty(termDef.Definition))
                {
                    termDef.Valid = false;
                    error = true;
                }
            }

            if (!error && !EditMode)
            {
                // save the set...
                var terms = new List<string>();
                var definitions = new List<string>();

                foreach (var termDef in TermsDefinitions)
                {
                    terms.Add(termDef.Term);
                    definitions.Add(termDef.Definition);
                }

                var setToCreate = new CreateSet()
                {
                    Title = Title,
                    LangDefinitions = SelectedDefinitionLanguage.CountryCode,
                    LangTerms = SelectedTermLanguage.CountryCode,
                    Definitions = definitions.ToArray(),
                    Terms = terms.ToArray(),
                    Visibility = EveryoneOrMe ? Models.QuizletTypes.Uploading.Visibility.Everyone : Models.QuizletTypes.Uploading.Visibility.OnlyMe,
                };

                var result = await _quizletApi.CreateNewSet(setToCreate);
                if (result)
                {
                    //Get new updated sets and save them locally
                    await SaveCreatedOrUpdatedSets();

                    //Navigate back
                    NavigationService.Navigate(typeof(Views.MySetsPage));
                }
                else
                {
                    var dialog = new MessageDialog("Error: and error occured while creating the set.");
                    await dialog.ShowAsync();
                }
            }
            if (!error && EditMode)
            {
                var terms = new List<string>();
                var definitions = new List<string>();

                foreach (var termDef in TermsDefinitions)
                {
                    terms.Add(termDef.Term);
                    definitions.Add(termDef.Definition);
                }

                var setToUpdate = new UpdateSet()
                {
                    Definitions = definitions.ToArray(),
                    Terms = terms.ToArray(),
                    TermIds = TermIds.ToArray(),
                    Title = Title,
                    LangDefinitions = SelectedDefinitionLanguage.CountryCode,
                    LangTerms = SelectedTermLanguage.CountryCode,
                    Visibility = EveryoneOrMe ? Models.QuizletTypes.Uploading.Visibility.Everyone : Models.QuizletTypes.Uploading.Visibility.OnlyMe,
                };

                var result = await _quizletApi.UpdateSet(setToUpdate, SetId);

                if (result)
                {
                    await SaveCreatedOrUpdatedSets();
                    NavigationService.Navigate(typeof(Views.MySetsPage));
                }
                else
                {
                    var dialog = new MessageDialog("Error: and error occured while creating the set.");
                    await dialog.ShowAsync();
                }
            }


        }

        private async Task SaveCreatedOrUpdatedSets()
        {
            //Get new updated sets and save them locally
            var allSets = await _quizletApi.GetCreatedSets();
            var newSets = allSets.Items.Select(x => x.ItemData);
            await _setsLocalStorage.PurgeAndAllNewSets(newSets);
        } 

        public void AddRow(object sender, RoutedEventArgs e)
        {
            TermsDefinitions.Add(new TermDefinitionViewModel() { Valid = true });
            TermIds.Add(0);
        }

        public async void DeleteRow(object sender, RoutedEventArgs e)
        {
            if (TermsDefinitions.Count <= 2)
            {
                var dialog = new MessageDialog("Error: you must have at least two terms and definitions in a set.");
                await dialog.ShowAsync();
            }
            else
            {
                var row = TermsDefinitions.Last();
                TermsDefinitions.Remove(row);
                var id = TermIds.Last();
                TermIds.Remove(id);
            }

        }

    }
}
