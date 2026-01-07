using System;
using System.Windows.Input;

namespace GeneratorSQL.Core
{
    /// <summary>
    /// A command implementation for MVVM pattern
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a new RelayCommand
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">Function to determine if command can execute</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates a new RelayCommand with no parameter
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">Function to determine if command can execute</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
            : this(
                execute != null ? new Action<object>(_ => execute()) : null,
                canExecute != null ? new Func<object, bool>(_ => canExecute()) : null)
        {
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Raises the CanExecuteChanged event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
