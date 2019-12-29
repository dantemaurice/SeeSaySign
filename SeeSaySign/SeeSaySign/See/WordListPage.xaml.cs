using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SeeSaySign.Controls;
using SeeSaySign.SaySign;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.See
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WordListPage : ContentPage
    {
        public ObservableCollection<SightWord> Items { get; set; }
	    private string _mode;
	    private int _stage;
		public WordListPage(string mode, int stage)
        {
            InitializeComponent();
            Items = new ObservableCollection<SightWord>(WordManager.GetWords());
			WordList.ItemsSource = Items;
	        _mode = mode;
	        _stage = stage;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
			
            if (e.Item == null)
                return;
	        SightWord word = (SightWord)e.Item;
	        ObservableCollection<Page> pages = new ObservableCollection<Page>()
	        {
		        new VideoMode(_mode, word), new RecordMode(_mode, word)
	        };
			switch (_mode)
	        {
				case "See":
					await Navigation.PushAsync(new SeeStage1(word));
					break;
				case "Say":
					switch (_stage)
					{
						case 1:
							//await Navigation.PushAsync(new VideoMode(_mode, word));
							await Navigation.PushAsync(new MasterTabbedPage(_mode, word, _stage));

							break;
						case 2:
							await Navigation.PushAsync(new MasterTabbedPage(_mode, word, _stage));

							break;
					}
					break;
		        case "Sign":
			        switch (_stage)
			        {
				        case 1:
							await Navigation.PushAsync(new MasterTabbedPage(_mode, word, _stage));

							break;
				        case 2:
					        await Navigation.PushAsync(new MasterTabbedPage(_mode, word, _stage));

							break;
			        }
			        break;

			}
            
           
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
