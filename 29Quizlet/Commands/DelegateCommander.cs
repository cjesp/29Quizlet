using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _29Quizlet.Commands
{
    public class DelegateCommander : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        //private readonly Func<bool> _canExecute2;
        //private readonly Action _execute2;

        public event EventHandler CanExecuteChanged;

        public DelegateCommander(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        //public DelegateCommand(Action execute, Func<bool> canExecute)
        //{
        //    _execute2 = execute;
        //    _ca
        //}

        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
