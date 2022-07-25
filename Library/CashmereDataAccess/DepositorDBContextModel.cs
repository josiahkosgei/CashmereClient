using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cashmere.Library.CashmereDataAccess
{
    [DbContext(typeof(DepositorDBContext))]
    public partial class DepositorDBContextModel : RuntimeModel
    {
        private static DepositorDBContextModel _instance;
        public static IModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DepositorDBContextModel();
                    _instance.Initialize();
                    _instance.Customize();
                }

                return _instance;
            }
        }

        partial void Initialize();

        partial void Customize();
    }
}
