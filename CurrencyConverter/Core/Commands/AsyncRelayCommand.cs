using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CurrencyConverter.Commands
{
    /// <summary>
    /// Implements the ICommand interface for asynchronous operations with execution state tracking.
    /// </summary>
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        /// <summary>
        /// Initializes a new instance of the AsyncRelayCommand class with the specified execute and optional canExecute functions.
        /// </summary>
        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute ?? (() => true);
        }

        /// <summary>
        /// Determines whether the command can execute in its current state. Returns false if currently executing.
        /// </summary>
        public bool CanExecute(object parameter) => !_isExecuting && _canExecute();

        /// <summary>
        /// Executes the asynchronous command if it can be executed, disabling the command during execution.
        /// </summary>
        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChange();
                await _execute();
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChange();
            }
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event to notify command bindings of execution state changes.
        /// </summary>
        public void RaiseCanExecuteChange() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
