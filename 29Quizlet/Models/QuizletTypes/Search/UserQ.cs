using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace _29Quizlet.Models.QuizletTypes.Search
{
    [DataContract]
    public class UserQ : ISearchItem
    {
        [DataMember]
        public string username { get; set; }

        [DataMember]
        public string account_type { get; set; }

        [DataMember]
        public string profile_image { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string type { get; set; }
    }

    public class UserQViewModel : ViewModelBase
    {
        public string Username { get; set; }

        Uri _profilePictureSrc = new Uri("ms-appx://29Quizlet/Assets/avatar.png");
        public Uri ProfilePictureSrc
        {
            get { return _profilePictureSrc; }
            set { Set(ref _profilePictureSrc, value); }
        }

        public UserQViewModel(UserQ user)
        {
            Username = user.username;

            if (!string.IsNullOrEmpty(user.profile_image))
            {
                ProfilePictureSrc = new Uri(user.profile_image);
            }
        }
    }
}
