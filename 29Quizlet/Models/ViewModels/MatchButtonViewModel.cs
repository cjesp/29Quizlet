using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Core;

namespace _29Quizlet.Models.ViewModels
{
    public enum MatchButtonColorEnum
    {
        Picked,
        Wrong,
        Correct,
        Neutral
    }

    public enum MatchButtonType
    {
        Question,
        Answer
    }

    public class MatchButtonViewModel : ViewModelBase
    {
        public bool IsPressed { get; set; }
        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                Set(ref _isVisible, value);            
            }
        }
        private MatchButtonColorEnum _color;
        public MatchButtonColorEnum Color
        {
            get
            {
                return _color;
            }
            set
            {
                Set(ref _color, value);
            }
        }

        private string _buttonText;
        public string ButtonText
        {
            get
            {
                return _buttonText;
            }
            set
            {
                Set(ref _buttonText, value);
            }
        }

        public TermViewModel Term { get; set; }

        public MatchButtonViewModel(TermViewModel term) : this()
        {
            Term = term;
        }

        public MatchButtonViewModel()
        {
            IsVisible = false;
            Reset();
        }

        public void ResetWithTerm(TermViewModel term, MatchButtonType type)
        {
            Term = term;
            IsVisible = true;
            Reset();
            switch (type)
            {
                case MatchButtonType.Question:
                    ButtonText = term.Definition;
                    break;
                case MatchButtonType.Answer:
                    ButtonText = term.TermText;
                    break;
                default:
                    break;
            }
        }

        private void Reset()
        {
            Color = MatchButtonColorEnum.Neutral;
            IsPressed = false;
        }

        public void Pressed()
        {
            if (IsPressed)
            {
                Color = MatchButtonColorEnum.Neutral;
            }
            else
            {
                Color = MatchButtonColorEnum.Picked;
            }
            IsPressed = !IsPressed;
        }

        public bool CheckForCorrectAnswer(MatchButtonViewModel otherButton)
        {
            var testCorrect = otherButton.Term.Equals(this.Term);
            if (testCorrect)
            {
                this.CorrectAnswer();
                otherButton.CorrectAnswer();
                return true;
            }

            WrongAnswer();
            otherButton.WrongAnswer();
            
            
            //otherButton.Reset();

            return false;
        }

        public async void WrongAnswer()
        {
            Color = MatchButtonColorEnum.Wrong;
            IsPressed = false;
            //dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => 
            //{
            //    await Task.Delay(600);
            //    //.ContinueWith(_ => Reset());
            //    if (!IsPressed)
            //    {
            //        Reset();
            //    }
            //});
            //Dispatcher.DispatchAsync(() => 
            //{
            //    Reset();
            //}, 600);
            //{
            //    await Task.Delay(600);
            //});
            await Task.Delay(600);
            //.ContinueWith(_ => Reset());
            ////IsPressed = false;
            if (!IsPressed)
            {
                Reset();
            }
        }

        public async void CorrectAnswer()
        {
            Color = MatchButtonColorEnum.Correct;
            await Task.Delay(600);
            IsVisible = false;
        }
    }
}
