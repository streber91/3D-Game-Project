using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Underlord.Logic
{
    class Job
    {
        Entity.Vars_Func.ImpJob jobTyp;

        public Job(Entity.Vars_Func.ImpJob jobTyp)
        {
            this.jobTyp = jobTyp;
        }

        #region Properties

        #endregion

        public Entity.Vars_Func.ImpJob getJobTyp() { return jobTyp; }
    }
}
