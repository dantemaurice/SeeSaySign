using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SeeSaySign.Controls;
using SeeSaySign.FormsVideoLibrary;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.SaySign
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecordMode : ContentPage, ITabbedModePage
	{
		private SightWord _word;
		private string _mode;
        
		public RecordMode (string mode, SightWord word)
		{
			InitializeComponent ();
			_mode = mode;
			_word = word;

			//set existing source if exists
			SetExistingVideoSource();

			takeVideo.Clicked += TakeVideo_Clicked;
			if (_mode.ToLower() == "say")
			{
				Title = "Say The Word:";
				TitleLabel.Text = Title;
				Icon = "SayTheWord.png";
			}
			else if (_mode.ToLower() == "sign")
			{
				Title = "Sign With Me:";
				TitleLabel.Text = Title;
				Icon = "SignWithMe.png";

			}

			WordLabel.Text = word.Name.ToUpper();
		}

        private void SetExistingVideoSource()
		{
			//Check if video already exists if so play
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					if (File.Exists($"{path}/{_word.Name}{_mode}Record.mp4"))
					{
						Video.IsVisible = true;
						Video.Source = new FileVideoSource()
						{
							File =
								$"{path}/{_word.Name}{_mode}Record.mp4"
						};
					}
					break;
				case Device.Android:
					if (File.Exists($"/storage/emulated/0/Android/data/com.companyname.SeeSaySign/files/Movies/{_word.Name}{_mode}Record.mp4"))
					{
						Video.IsVisible = true;
						Video.Source = new FileVideoSource()
						{
							File =
								$"/storage/emulated/0/Android/data/com.companyname.SeeSaySign/files/Movies/{_word.Name}{_mode}Record.mp4"
						};
					}
					break;
			}
			//End check
		}

		private async void TakeVideo_Clicked(object sender, EventArgs e)
		{
			//Check if file exists and if so delete it
			DeletePreExisting();

			//Check if camera is available
			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakeVideoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
				return;

			}

			//Take New Video
			MediaFile file = await TakeVideo();
			if (file == null)
				return;

			//dispose file so we can access it later
			file.Dispose();


			//try set video source
			try
			{
				SetVideoSource();
			}
			catch (Exception error)
			{
				Video.IsVisible = false;
				await DisplayAlert("Something went wrong", error.Message, "Dismiss");
				//}

				file.Dispose();
			}
		}

		private void SeeIt_OnClicked(object sender, EventArgs e)
		{
			Video.Stop();
			Navigation.PushAsync(new VideoMode(_mode, _word));
		}

		private void DeletePreExisting()
		{
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					if (File.Exists($"{path}/{_word.Name}{_mode}Record.mp4"))
					{
						//Stop video in case its playing
						Video.Stop();
						//Delete
						File.Delete($"{path}/{_word.Name}{_mode}Record.mp4");
					}
					break;
				case Device.Android:
					if (File.Exists($"/storage/emulated/0/Android/data/com.companyname.SeeSaySign/files/Movies/{_word.Name}{_mode}Record.mp4"))
					{
						//Stop video in case its playing
						Video.Stop();
						//Delete
						File.Delete($"/storage/emulated/0/Android/data/com.companyname.SeeSaySign/files/Movies/{_word.Name}{_mode}Record.mp4");
					}
					break;
			}
		}

		private async Task<MediaFile> TakeVideo()
		{
			MediaFile file;
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
					{
						//Changed line 124 in VideoPlayRenderer from asset = AVAsset.FromUrl(new NSUrl(uri)); to asset = AVAsset.FromUrl(new NSUrl(uri,true)); inorder to specify that this is a directory path
						Name = $"{_word.Name}{_mode}Record",
						//SaveToAlbum = true,
					});
					break;
				case Device.Android:
					file = await CrossMedia.Current.TakeVideoAsync(new StoreVideoOptions
					{
						Name = $"{_word.Name}{_mode}Record.mp4",
					});
					break;
				default:
					file = null;
					break;
			}
			return file;
		}

		private void SetVideoSource()
		{
			Video.IsVisible = true;
			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
					Video.IsVisible = true;
					Video.Source = new FileVideoSource()
					{
						File =
							$"{path}/{_word.Name}{_mode}Record.mp4"
					};
					break;
				case Device.Android:
					Video.Source = new FileVideoSource()
					{
						File =
							$"/storage/emulated/0/Android/data/com.companyname.SeeSaySign/files/Movies/{_word.Name}{_mode}Record.mp4"
					};
					break;
			}
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