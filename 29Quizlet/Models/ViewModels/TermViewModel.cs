using _29Quizlet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.ViewModels
{
    public class TermViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Int64 Id { get; set; }
        public string TermText { get; set; }
        public string Definition { get; set; }
        public Image Image { get; set; }

        private bool favorite;
        public bool Favorite
        {
            get { return this.favorite; }
            set
            {
                if (this.favorite != value)
                {
                    favorite = value;
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

        public static TermViewModel Create(Term term)
        {
            var termViewModel = new TermViewModel();
            termViewModel.Id = term.Id;
            termViewModel.TermText = term.TermText;
            termViewModel.Definition = term.Definition;
            termViewModel.Image = term.Image;
            termViewModel.Favorite = false;

            return termViewModel;
        }

        public void FavoriteButtonClick()
        {
            Favorite = !Favorite;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var tvm = obj as TermViewModel;
            if (tvm == null)
            {
                return false;
            }
            else
            {
                return tvm.TermText == this.TermText && tvm.Definition == this.Definition;
            }
        }

        // to kill the warning!
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
