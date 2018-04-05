using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _29Quizlet.Models.Settings
{
    public class SetsSettings
    {
        public IEnumerable<long> SetIDs { get; set; }

        public static SetsSettings GetDefaults()
        {
            return new SetsSettings()
            {
                SetIDs = new List<long>()
                {
                    415,
                    7755348,
                    54267737
                }
            };
        }
    }
}
