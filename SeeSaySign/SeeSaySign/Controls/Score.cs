using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeSaySign.Controls
{
    public class Score
    {
        public List<SightWord> Correct { get; set; }
        public List<SightWord> Incorrect { get; set; }

        public List<SightWord> AllGameWords { get; set; }
    }
}
