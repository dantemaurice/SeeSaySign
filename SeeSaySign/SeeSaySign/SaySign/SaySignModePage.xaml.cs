using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeSaySign.See;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.SaySign
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaySignModePage : ContentPage
	{
		private string _mode;
		public SaySignModePage (string mode)
		{
			InitializeComponent ();
			_mode = mode;
			TitleLabel.Text = mode.ToUpper() + "!";

		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (_mode == "Say")
			{
				Stage1.Text = "Stage 1: Hear the Word!";
				Stage2.Text = "Stage 2: Say the Word!";
			}
			else
			{
				Stage1.Text = "Stage 1: Sign the Word!";
				Stage2.Text = "Stage 2: Sign with Me!";
			}


		}
		async void Stage_Selected(object sender, EventArgs e)
		{
			Button stageButton = (sender as Button);
			switch (stageButton?.Text.ToLower())
			{
				case "stage 1: hear the word!":
					await Navigation.PushAsync(new WordListPage(_mode, 1));
					break;
				case "stage 2: say the word!":
					await Navigation.PushAsync(new WordListPage(_mode, 2));
					break;
				case "stage 1: sign the word!":
					await Navigation.PushAsync(new WordListPage(_mode, 1));
					break;
				case "stage 2: sign with me!":
					await Navigation.PushAsync(new WordListPage(_mode, 2));
					break;

				default:
					//Should not be able to happen
					break;
			}




		}
	}
}