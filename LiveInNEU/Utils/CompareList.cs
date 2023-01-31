using System;
using System.Collections.Generic;
using System.Text;
using LiveInNEU.Services.implementations;

namespace LiveInNEU.Utils
{
    /// <author>赵全</author>
    class LessonEqualityComparer : IEqualityComparer<LessonData>
    {
        public bool Equals(LessonData x, LessonData y) => x.KKXND == y.KKXND && x.KKXQM == y.KKXQM;
        public int GetHashCode(LessonData obj) => obj.KKXND.GetHashCode();
    }

   
}
