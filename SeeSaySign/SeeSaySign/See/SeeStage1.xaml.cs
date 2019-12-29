using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SeeSaySign.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.See
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SeeStage1 : CarouselPage
	{
	    public ObservableCollection<SightWord> Items { get; set; }
	    public int currentPageIndex { get; set; }
		public int StartPageIndex { get; set; }

        public SeeStage1 (SightWord selectedWord )
        {
			InitializeComponent ();
            Items = new ObservableCollection<SightWord>(WordManager.GetWords());
            ItemsSource = Items;
	        StartPageIndex = Items.FirstOrDefault(p => p.Id == selectedWord.Id).Id - 1;
            if (StartPageIndex > 0) CurrentPage = Children[StartPageIndex];
            Appearing += (sender, args) => SayStart(sender);
		    CurrentPageChanged += (sender, args) => SayNext(sender);
        }

		protected override async void OnDisappearing()
		{
			base.OnDisappearing();
			
			TryCancelSpeach();
		}

		public async void TryCancelSpeach()
		{
			try
			{
				Voice._cts.Cancel();
			}
			catch (Exception e)
			{
				//await DisplayAlert("Something went wrong", e.Message, "Dismiss");
			}
		}

		//  when the page is loaded, the word will be said
		async void SayStart(object sender)
		{
			string word;
			word = (sender as SeeStage1).Items[StartPageIndex].Name;
			await Voice.SpeakWithCancelOption(word ?? "Something");
			
		}

		async void SayNext(object sender)
		{
			await Voice.SpeakWithCancelOption((CurrentPage.Content as StackLayout).Children.OfType<Label>().Last().Text);
		}

		async void TouchImage_OnClicked(object imageButton, EventArgs eventArgs)
        {
	        SightWord word = WordManager.GetWords().FirstOrDefault(w =>
		        w.Name == (imageButton as ImageButton).CommandParameter.ToString());
	        TryCancelSpeach();
			await Voice.SpeakWithCancelOption("The word is " + word.Name);
	    }
	}
}