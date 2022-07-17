using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Util;

namespace CashmereDeposit.DI
{
    /// <remarks>similar to Autofac.Util.ReleaseAction</remarks>
    public sealed class DisposableAction : Disposable
    {
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is <see langword="null" />.</exception>
        public DisposableAction(System.Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            this.Action = action;
        }

        private System.Action Action { get; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                this.Action.Invoke();
            }
        }
    }
}
