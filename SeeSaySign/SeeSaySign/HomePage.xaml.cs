using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeeSaySign.SaySign;
using SeeSaySign.See;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
		public HomePage ()
		{
			InitializeComponent ();
		}
        
	    private async void SeeButton_OnClicked(object sender, EventArgs e)
	    {
	        await Navigation.PushAsync(new SeeModePage("See"));
	    }

	    async void SayButton_OnClicked(object sender, EventArgs e)
	    {
		    await Navigation.PushAsync(new SaySignModePage("Say"));
	    }

	    async void SignButton_OnClicked(object sender, EventArgs e)
	    {
		    await Navigation.PushAsync(new SaySignModePage("Sign"));
		}
	}
}