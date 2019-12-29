using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeSaySign.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.See
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeeModePage : ContentPage
	{
		private string _mode;
		public SeeModePage (string mode)
		{
			InitializeComponent ();
			_mode = mode;
		}
        
	    async void Stage_Selected(object sender, EventArgs e)
	    {
	        Button stageButton = (sender as Button);
	            switch (stageButton?.Text)
	            {
	                case "Stage 1: Touch the Picture!":
	                    await Navigation.PushAsync(new WordListPage(_mode, 1));
	                    break;
	                case "Stage 2: Find the Picture!":
                    await Navigation.PushAsync(new GameModeWordListPage(SeeGameMode.Stage2));
                        break;
	                case "Stage 3: What Does it Do?":
                    await Navigation.PushAsync(new GameModeWordListPage(SeeGameMode.Stage3));
                    break;
                default:
                    //Should not be able to happen
                    break;
	            }


           

        }
	}
}