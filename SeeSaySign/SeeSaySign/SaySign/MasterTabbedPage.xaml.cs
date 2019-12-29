using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SeeSaySign.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.SaySign
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterTabbedPage : TabbedPage
    {
        public MasterTabbedPage (string mode, SightWord word, int stage)
        {
            InitializeComponent();
			Children.Add(new VideoMode(mode, word));
			Children.Add(new RecordMode(mode, word));

	        CurrentPage = Children[stage-1];

            CurrentPageChanged += MasterTabbedPage_CurrentPageChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //start video
            ((ITabbedModePage) (CurrentPage)).SetAutoPlay();
        }

        private void MasterTabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            ((ITabbedModePage)Children.FirstOrDefault(p => p != CurrentPage)).StopVideo();
            ((ITabbedModePage)(CurrentPage)).StartVideo();
        }
    }
}