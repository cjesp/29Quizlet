using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models
{
    [DataContract]
    public class Term : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember(Name = "id")]
        public Int64 Id { get; set; }

        [DataMember(Name = "term")]
        public string TermText { get; set; }

        [DataMember(Name = "definition")]
        public string Definition { get; set; }

        [DataMember]
        public Image Image { get; set; }


        // non-api member
        [DataMember]
        private bool _favorite;
        public bool Favorite
        {
            get { return this._favorite; }
            set
            {
                if (this._favorite != value)
                {
                    _favorite = value;
                    this.NotifyPropertyChanged("Favorite");
                }
            }
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Favorite = false;
        }
    }
}
