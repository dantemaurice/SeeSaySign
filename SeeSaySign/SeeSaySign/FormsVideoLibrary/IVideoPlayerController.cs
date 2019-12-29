using System;

namespace SeeSaySign.FormsVideoLibrary
{ 
    public interface IVideoPlayerController
    {
        VideoStatus Status { set; get; }

        TimeSpan Duration { set; get; }
    }
}
