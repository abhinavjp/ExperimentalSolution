using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.PracticeService.EFConnectionStringHelper
{
    public interface IEntityFrameworkConnectionBuilder
    {
        string CreateEntityFrameworkConnection(string modelName);
    }
}
