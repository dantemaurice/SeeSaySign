using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SeeSaySign.Controls;
using SeeSaySign.FormsVideoLibrary;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.SaySign
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VideoMode : ContentPage , ITabbedModePage
	{
		private SightWord _word;
		private string _mode;

	    private double _widthHolder;
	    private double _heightHolder;

	    private ScrollView toAdd;

		public VideoMode(string mode, SightWord word)
		{
			InitializeComponent();
			_mode = mode;
			_word = word;


			switch (_mode)
			{
				case "Say":
					Title = "Hear the Word!";
				TitleLabel.Text = Title;
					Icon = "HearTheWord.png";
					break;
				case "Sign":
					Title = "Sign the Word!";
				TitleLabel.Text = Title;
					Icon = "SignTheWord.png";
					break;
			}

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					Video.Source = new ResourceVideoSource() {Path = $"Videos/{_word.Name}{_mode}.mp4"};
					break;
				case Device.Android:
					Video.Source = new ResourceVideoSource() {Path = $"{_word.Name}{_mode}.mp4"};
					break;

			}

			if (_mode.ToLower() == "say")
			{
				Title = "Hear The Word";
				TitleLabel.Text = Title;
			}
			else if (_mode.ToLower() == "sign")
			{
				Title = "Sign the Word";
				TitleLabel.Text = Title;
			}

			WordLabel.Text = word.Name.ToUpper();
		}


		//protected override void OnSizeAllocated(double width, double height)
	 //   {
	 //       base.OnSizeAllocated(width, height);
	 //       if (_widthHolder != width || _heightHolder != height)
	 //       {
	 //           _widthHolder = Width;
	 //           _heightHolder = Height;
	 //           //DetermineLayout();
	 //       }

	 //   }

	    //private void DetermineLayout()
	    //{
     //       if (Width < 600)
     //       {
     //           //do nothing
     //           if (toAdd != null)
     //           {
     //               //Set Content back to normal..basically remove the scroll view
     //               this.Content = toAdd.Content;

     //               //Setting props of elementts back to the default because its loading on a device the size of a phone
     //               Video.MinimumHeightRequest = 0;
     //               Grid.RowDefinitions[0].Height = GridLength.Star;
     //               Grid.RowDefinitions[1].Height = GridLength.Star;
     //               Grid.RowDefinitions[0].Height = GridLength.Star;
     //               TitleLabel.Margin = new Thickness(25, 75, 25, 25);
     //               TryItButton.Margin = new Thickness(25, 25, 25, 25);
     //           }
     //       }
     //       else
     //       {
     //           //Horozontal
     //           if (Height < 600)
     //           {
     //               //Build scroll view and place all Content inside of it
     //               toAdd = new ScrollView();
     //               toAdd.Content = this.Content;
     //               this.Content = toAdd;

     //               //Change Element Properties to make sure they look good
     //               Video.MinimumHeightRequest = Height;
     //               Grid.RowDefinitions[0].Height = 100;
     //               Grid.RowDefinitions[1].Height = Height;
     //               Grid.RowDefinitions[0].Height = 100;
     //               TitleLabel.Margin = new Thickness(25, 25, 25, 25);
     //               TryItButton.Margin = new Thickness(25, 25, 25, 25);

     //               //Set Grid parent to the scroll view or else the ScrollToAsyn won't work
     //               Grid.Parent = toAdd;
     //               //Scroll to make sure the video is centered
     //               toAdd.ScrollToAsync(toAdd.Children.FirstOrDefault(), ScrollToPosition.Center, true);
     //           }
     //       }
     //   }

	    private void TryItButton_Clicked(object sender, EventArgs e)
	    {
	        Video.Stop();
            Navigation.PushAsync(new RecordMode(_mode, _word));
	    }

	    public void StartVideo()
	    {
	        if (Video.Source != null)
	        {
	            Video.Position = TimeSpan.Zero;
                Video.Play();
	        }
	    }

	    public void StopVideo()
	    {
	        if (Video.Status == VideoStatus.Playing)
	        {
	            Video.Pause();
	        }
	    }

        public void SetAutoPlay()
	    {
	        Video.AutoPlay = true;
	        if (Video.Status != VideoStatus.Playing)
	        {
	            Video.Play();
	        }
	    }

    }
}