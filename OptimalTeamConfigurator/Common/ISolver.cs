using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamConfigurator.Interfaces
{
    public interface ISolver
    {
        void Start(BackgroundWorker worker, ProblemResult result);

        Dictionary<int, Dictionary<int, bool>> Relationships
        {
            get; set;
        }
    }
}
