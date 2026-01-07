using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GeneratorSQL.Core
{
    /// <summary>
    /// An async command implementation for MVVM pattern
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object, Task> _execute;
        private readonly Func<object, bool> _canExecute;
        private bool _isExecuting;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Creates a new AsyncRelayCommand
        /// </summary>
        /// <param name="execute">The async action to execute</param>
        /// <param name="canExecute">Function to determine if command can execute</param>
        public AsyncRelayCommand(Func<object, Task> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        /// <summary>
        /// Creates a new AsyncRelayCommand with no parameter
        /// </summary>
        /// <param name="execute">The async action to execute</param>
        /// <param name="canExecute">Function to determine if command can execute</param>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
            : this(
                execute != null ? new Func<object, Task>(_ => execute()) : null,
                canExecute != null ? new Func<object, bool>(_ => canExecute()) : null)
        {
        }

        public bool CanExecute(object parameter)
        {
            return !_isExecuting && (_canExecute == null || _canExecute(parameter));
        }

        public async void Execute(object parameter)
        {
            if (_isExecuting)
                return;

            _isExecuting = true;
            RaiseCanExecuteChanged();

            try
            {
                await _execute(parameter);
            }
            catch (Exception ex)
            {
                // Log error or handle it appropriately
                System.Diagnostics.Debug.WriteLine($"AsyncRelayCommand error: {ex.Message}");
                throw;
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
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
