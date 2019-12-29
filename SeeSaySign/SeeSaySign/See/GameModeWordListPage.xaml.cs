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
	public partial class GameModeWordListPage : ContentPage
	{
	    private List<SightWord> _currentWords;
	    private double _currentScore;
	    private SeeGameMode _modeSelected;
		public GameModeWordListPage (SeeGameMode mode)
		{
		    _modeSelected = mode;
			InitializeComponent ();
		    
		    Appearing += (sender, args) => Load();
            
        }

	    private void Load()
	    {
	        switch (_modeSelected)
	        {
	            case SeeGameMode.Stage2:
	                //Initialize Score for game mode..
	                if (SessionScores.SeeStage2Score == null || SessionScores.SeeStage2Score.AllGameWords.Count == 0)
	                {
	                    SessionScores.ResetStage2();
	                }
	                //ENd Initialize gamemode score
                    _currentWords = SessionScores.SeeStage2Score.AllGameWords;
                    CurrentScoreLabel.Text = CalculateScoreString(SessionScores.SeeStage2Score.Correct.Count);

	                break;
	            case SeeGameMode.Stage3:
	                //Initialize Score for game mode..
	                if (SessionScores.SeeStage3Score == null || SessionScores.SeeStage3Score.AllGameWords.Count == 0)
	                {
	                    SessionScores.ResetStage3();
	                }
                    _currentWords = SessionScores.SeeStage3Score.AllGameWords;
                    CurrentScoreLabel.Text = CalculateScoreString(SessionScores.SeeStage3Score.Correct.Count);

                    break;
	            default:
	                //Should not be able to happen
	                break;
	        }
            //have to set item source back to null or the dataTrigger wont update.
	        WordList.ItemsSource = null;

            //Set ItemSource of WordList
	        WordList.ItemsSource = _currentWords;

	        switch (_modeSelected)
	        {
	            case SeeGameMode.Stage2:
	                Header.Text = "Stage 2: Find the Picture!";
	                break;
	            case SeeGameMode.Stage3:
	                Header.Text = "Stage 3: What Does it Do?";
                    break;
	            default:
	                throw new ArgumentOutOfRangeException();
	        }
        }

        private string CalculateScoreString(int score)
        {
            string response;
            if (score == 0)
            {
                response = "Play the game to increase your score!";
            }
            else
               response = $"You have gotten {score} right so far!";
            return response;
            
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
	    {
	        SightWord Question = (SightWord) e.Item;

	        if (Question.Enabled == false)
	        {
	            await DisplayAlert("Locked", "This question will unlock once yuou complete the prior words", "OK");
            }
	        else
	        {
	            List<SightWord> correctAnswers;
	            List<SightWord> incorrectAnswers;
                switch (_modeSelected)
	            {
	                case SeeGameMode.Stage2:
	                    correctAnswers = SessionScores.SeeStage2Score.Correct;
	                    incorrectAnswers = SessionScores.SeeStage2Score.Incorrect;
                        break;
	                case SeeGameMode.Stage3:
	                    correctAnswers = SessionScores.SeeStage3Score.Correct;
	                    incorrectAnswers = SessionScores.SeeStage3Score.Incorrect;
                        break;
	                default:
	                    throw new ArgumentOutOfRangeException();
	            }

	            if (correctAnswers.Contains(Question))
	            {
	                await DisplayAlert("Correct Answer", "You got this word correct", "OK");
	            }
	            else if (incorrectAnswers.Contains(Question))
	            {
	                await DisplayAlert("InCorrect Answer", "You got this word wrong", "OK");
	            }
	            else
	            {
	                await Navigation.PushAsync(new SeeStageGame(Question,_modeSelected));
	            }
            }
	        
	    }

	    async void ResetButton_OnClicked(object sender, EventArgs e)
	    {
	        if (await DisplayAlert("Are you sure?",
	            "You current score is " + _currentScore + "%. Are you sure you want to reset the game?", "Reset", "Cancel"))
	        {
	            switch (_modeSelected)
	            {
	                case SeeGameMode.Stage2:
	                    SessionScores.SeeStage2Score.Correct.Clear();
	                    SessionScores.SeeStage2Score.Incorrect.Clear();
	                    SessionScores.SeeStage2Score.AllGameWords.Clear();
                        break;
	                case SeeGameMode.Stage3:
	                    SessionScores.SeeStage3Score.Correct.Clear();
	                    SessionScores.SeeStage3Score.Incorrect.Clear();
	                    SessionScores.SeeStage3Score.AllGameWords.Clear();
                        break;
	                default:
	                    throw new ArgumentOutOfRangeException();
	            }
	            
	            Load();
	        }
	    }
	}
}