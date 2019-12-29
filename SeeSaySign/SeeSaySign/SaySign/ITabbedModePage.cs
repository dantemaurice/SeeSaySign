using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SeeSaySign.SaySign
{
    public interface ITabbedModePage
    {
        void StartVideo();
        void StopVideo();
        void SetAutoPlay();
    }
}
