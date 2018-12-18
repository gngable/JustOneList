using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace JustOneList
{
    public class DelegateCommand : ICommand
    {
        private Action _action = null;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action action)
        {
            _action = action;
        }
    }
}
