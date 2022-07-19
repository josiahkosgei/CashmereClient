﻿using System;
using Autofac.Util;

namespace CashmereDeposit.DI
{
    /// <remarks>similar to Autofac.Util.ReleaseAction</remarks>
    public sealed class DisposableAction : Disposable
    {
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
        public DisposableAction(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            Action = action;
        }

        private Action Action { get; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                Action.Invoke();
            }
        }
    }
}
