using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Automate.Core
{
    public class RelayCommand : ICommand
    {
        public Action<object?>? execute {  get; set; }
        public Predicate<object?>? canExecute { get; set; }

        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object?>? exec, Predicate<object?>? canExec)
        {
            execute += exec;
            canExecute += canExec;
        }
        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            if(execute != null) 
            {
                execute(parameter);
            }
        }
    }
}
