﻿using System;
using System.Windows.Input;

namespace MVVMSlotMachine.Interfaces.Common
{
    /// <summary>
    /// Extends the ICommand interface with
    /// the RaiseCanExecuteChanged method
    /// </summary>
    public abstract class ICommandExtended : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);
    }
}