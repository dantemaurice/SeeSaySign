using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SeeSaySign.Controls
{
    public class SightWord
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SeeStage2Question { get; set; }
        public string SeeStage3Question { get; set; }
        public string SayVideoUrl { get; set; }
        public string SignVideoUrl { get; set; }
        public string ImageUrl { get; set; }
        public string SoundUrl { get; set; }
        public string RecordableSayVideoUrl { get; set; }
        public string RecordableSignVideoUrl { get; set; }
        public bool? Enabled { get; set; }
        public SightWordType WordType { get; set; }

    }
}
