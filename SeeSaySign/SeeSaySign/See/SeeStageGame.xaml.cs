using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SeeSaySign.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SeeSaySign.See
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SeeStageGame : ContentPage
    {
        private SightWord _correctWord;
        private SightWord[] _words = new SightWord[3];
        private ImageButton[] _randomOrderedButtons;

        private int _currentNumberOfGuesses = 0;
        

        private double _widthHolder;
        private double _heightHolder;

        private readonly SeeGameMode _currentMode;
        private double _numberOfWordsInGame;
        private double _numberOfCorrectAnswers;
        private double _numberOfIncorrectAnswers;

        public SeeStageGame(SightWord Selected, SeeGameMode mode)
        {
            InitializeComponent();
            
            _widthHolder = Width;
            _heightHolder = Height;

            //determine GameMode and set label
            _currentMode = mode;
            switch (_currentMode)
            {
                case SeeGameMode.Stage2:
                    TitleLabel.Text = "Stage 2: Find the Picture!";
                    _numberOfWordsInGame = SessionScores.SeeStage2Score.AllGameWords.Count;
                    _numberOfCorrectAnswers = SessionScores.SeeStage2Score.Correct.Count;
                    _numberOfIncorrectAnswers = SessionScores.SeeStage2Score.Incorrect.Count;
                    break;
                case SeeGameMode.Stage3:
                    TitleLabel.Text = "Stage 3: What Does the Picture Do?";
                    _numberOfWordsInGame = SessionScores.SeeStage3Score.AllGameWords.Count;
                    _numberOfCorrectAnswers = SessionScores.SeeStage3Score.Correct.Count;
                    _numberOfIncorrectAnswers = SessionScores.SeeStage3Score.Incorrect.Count;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            //update score
            CorrectScoreLabel.Text = _numberOfCorrectAnswers.ToString();
            WrongScoreLabel.Text = _numberOfIncorrectAnswers.ToString();

            LoadWord(Selected);

            CorrectScoreImage.Source = ImageSource.FromResource("SeeSaySign.Images.Smiley-Green.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
            WrongScoreImage.Source = ImageSource.FromResource("SeeSaySign.Images.Smiley-Red.png", typeof(ImageResourceExtension).GetTypeInfo().Assembly);
        }

	    protected async override void OnAppearing()
	    {
		    base.OnAppearing();
		    switch (_currentMode)
		    {
			    case SeeGameMode.Stage2:
				    await Voice.SpeakWithCancelOption($"Touch The {_correctWord.Name}");
				    break;
			    case SeeGameMode.Stage3:
				    await Voice.SpeakWithCancelOption($"{_correctWord.SeeStage3Question}");
				    break;
			    default:
				    throw new ArgumentOutOfRangeException();
		    }
		}

	    protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (_widthHolder != width || _heightHolder != height)
            {
                _widthHolder = Width;
                _heightHolder = Height;
                DetermineImageLayout();
            }
            
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
			    await DisplayAlert("Something went wrong", e.Message, "Dismiss");
		    }
	    }

		private void LoadWord(SightWord word)
        {
            //Reset number of guesses because new word
            _currentNumberOfGuesses = 0;
            //set Correct word and display the correct text on the page
            _correctWord = word;

            //Determine GameMode and enable the appropriate word from that SessionScore
            if (_currentMode == SeeGameMode.Stage2)
            {
                //enable word so it is selectable in the wordlistGameMode page
                SessionScores.SeeStage2Score.AllGameWords.FirstOrDefault(w => w == word).Enabled = true;
                // todo after stage questions are populated are populated
                //Directions.Text = $"{_correctWord.SeeStage2Question} \n(Select the correct picture)";
                Directions.Text = $"Touch The {_correctWord.Name}";
            }
            else
            {
                SessionScores.SeeStage3Score.AllGameWords.FirstOrDefault(w => w == word).Enabled = true;
                // todo after stage questions are populated are populated
               // Directions.Text = $"{_correctWord.SeeStage3Question} \n(Select the correct picture)";
                Directions.Text = $"{_correctWord.SeeStage3Question}";
            }
            
            //Create command for linking to image buttons
            Command ImageClick = new Command(ImageButton_OnClicked);
            
            //Get two random words and combine them with the correct one into a list
            List<SightWord> currentGameWords = WordManager.GetWords().Where(w => w.Id != _correctWord.Id).ToList();
            currentGameWords = currentGameWords.GetRandom(2).ToList();
            currentGameWords.Add(_correctWord);
            _words = currentGameWords.ToArray();

            //for good measure we are randomizing the order of the ImageButtons before we put the words into them
            _randomOrderedButtons = DetermineRandomOrderOfButtons();
            for (int i = 0; i <= 2; i++)
            {
                _randomOrderedButtons[i].Command = ImageClick;
                _randomOrderedButtons[i].CommandParameter = _words[i];
                _randomOrderedButtons[i].Source = ImageSource.FromResource(_words[i].ImageUrl, typeof(ImageResourceExtension).GetTypeInfo().Assembly);
                _randomOrderedButtons[i].BorderColor = Color.White;
            }
           
        }
        
        private async void ImageButton_OnClicked(object o)
        {
			if ((SightWord) o == _correctWord)
            {
                //Answer was correct, so we set number of current guesses back to 0
                _currentNumberOfGuesses = 0;

                //find correct picture and makes the border green
                for (int i = 0; i <= 2; i++)
                {
                    ImageButton current = _randomOrderedButtons[i];
                    if (current.CommandParameter == o)
                    {
                        current.BorderColor = Color.Green;
                    }
                }
                
                switch (_currentMode)
                {
                    case SeeGameMode.Stage2:
                        //if word is in the incorrect list, which is always true unless a double click is registered
                        SessionScores.SeeStage2Score.Correct.Add(_correctWord);
                        _numberOfCorrectAnswers = SessionScores.SeeStage2Score.Correct.Count;
                        break;
                    case SeeGameMode.Stage3:
                        //if word is in the incorrect list, which is always true unless a double click is registered
                        SessionScores.SeeStage3Score.Correct.Add(_correctWord);
                        _numberOfCorrectAnswers = SessionScores.SeeStage3Score.Correct.Count;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //display correct message with score
                Voice.SpeakNow($"Good job!");
                await DisplayAlert($"GOOD JOB!", $"That was the {((SightWord)o).Name}!", "OK");
            }
            else
            {
                _currentNumberOfGuesses++;

                if (_currentNumberOfGuesses > 1)
                {
                    //Add word to Incorrect answers list in the appropriate GameMode
                    switch (_currentMode)
                    {
                        case SeeGameMode.Stage2:
                            SessionScores.SeeStage2Score.Incorrect.Add(_correctWord);
                            _numberOfIncorrectAnswers = SessionScores.SeeStage2Score.Incorrect.Count;
                            break;
                        case SeeGameMode.Stage3:
                            SessionScores.SeeStage3Score.Incorrect.Add(_correctWord);
                            _numberOfIncorrectAnswers = SessionScores.SeeStage3Score.Incorrect.Count;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    Voice.SpeakNow($"Maybe next time");
                    await DisplayAlert("Good try", $"That was a: {((SightWord)o).Name}", "OK");
                }
                else
                {
                    Voice.SpeakNow($"Try again.");
                    await DisplayAlert("Try Again", $"That was a: {((SightWord)o).Name}", "OK");
                }
               
                //Set image border to Red as they answered wrong
                for (int i = 0; i <= 2; i++)
                {
                    ImageButton current = _randomOrderedButtons[i];
                    if (current.CommandParameter == o)
                    {
                        current.BorderColor = Color.Red;
                    }
                }
            }
            //whether the number of guesses is 0 or 2 we go on to next word.. basically they either got it right or spent both guesses
            if (_currentNumberOfGuesses != 1)
            {
                //go on to next word
                switch (_currentMode)
                {
                    case SeeGameMode.Stage2:
                        //checks if all words the the "AllGameWords" are in either the Incorrect or Correct lists.. if they are the game is over
                        if (SessionScores.SeeStage2Score.AllGameWords.All(w => SessionScores.SeeStage2Score.Incorrect.Contains(w) || SessionScores.SeeStage2Score.Correct.Contains(w)))
                        {
                            Voice.SpeakNow($"Game over.");
                            await DisplayAlert("Game Over", $"You got {_numberOfCorrectAnswers} correct!", "OK");
                            await Navigation.PopAsync(true);
                        }
                        //Load next word in incorrect list from the static SessionScore grab next word that isn't in either the incorrect or correct list
                        else
                        {
                            LoadWord(SessionScores.SeeStage2Score.AllGameWords.FirstOrDefault(w => !SessionScores.SeeStage2Score.Incorrect.Contains(w) && !SessionScores.SeeStage2Score.Correct.Contains(w)));
                        }
                        break;
                    case SeeGameMode.Stage3:
                        //checks if all words the the "AllGameWords" are in either the Incorrect or Correct lists.. if they are the game is over
                        if (SessionScores.SeeStage3Score.AllGameWords.All(w => SessionScores.SeeStage3Score.Incorrect.Contains(w) || SessionScores.SeeStage3Score.Correct.Contains(w)))
                        {
                            Voice.SpeakNow($"Game over.");
                            await DisplayAlert("Game Over", $"You got {_numberOfCorrectAnswers} correct!", "OK");
                            await Navigation.PopAsync(true);
                        }
                        //Load next word in incorrect list from the static SessionScore grab next word that isn't in either the incorrect or correct list
                        else
                        {
                            LoadWord(SessionScores.SeeStage3Score.AllGameWords.FirstOrDefault(w => !SessionScores.SeeStage3Score.Incorrect.Contains(w) && !SessionScores.SeeStage3Score.Correct.Contains(w)));
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            //update score
            CorrectScoreLabel.Text = _numberOfCorrectAnswers.ToString();
            WrongScoreLabel.Text = _numberOfIncorrectAnswers.ToString();
        }

        private ImageButton[] DetermineRandomOrderOfButtons()
        {
            ImageButton[] buttonsOrder = new ImageButton[3];
            Random r = new Random();
            int random = r.Next(1, 3);

            switch (random)
            {
                case 1:
                    buttonsOrder[0] = Image1;
                    buttonsOrder[1] = Image2;
                    buttonsOrder[2] = Image3;
                    break;
                case 2:
                    buttonsOrder[0] = Image2;
                    buttonsOrder[1] = Image3;
                    buttonsOrder[2] = Image1;
                    break;
                case 3:
                    buttonsOrder[0] = Image3;
                    buttonsOrder[1] = Image1;
                    buttonsOrder[2] = Image2;
                    break;
            }
            return buttonsOrder;
        }
        
        async void SpeakButton_OnClicked(object sender, EventArgs e)
        {

	        TryCancelSpeach();
			switch (_currentMode)
            {
                case SeeGameMode.Stage2:
                    await Voice.SpeakWithCancelOption($"Touch The {_correctWord.Name}");
                    break;
                case SeeGameMode.Stage3:
                    await Voice.SpeakWithCancelOption($"{_correctWord.SeeStage3Question}");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            //Below used to test the pronunciation of each  word
            //foreach (SightWord v in WordManager.GetWords())         {
            //    await Voice.SpeakNow(v.Name);
            //}
        }
        
        #region UpdateLayout

        private void DetermineImageLayout()
        {
            if (Height > Width)
            {
                TitleLabel.Margin = new Thickness(25, 50, 25, 25);
                //Directions.Margin = new Thickness(0, 25, 75, 25);
                //Speaker.Margin = new Thickness(25, 25, 5, 25);
                double heightPixels = Height;
                double imageHeight = (heightPixels - ScoreGrid.Height - (TitleLabel.Height + 125) - (Speaker.Height)) / 3;
                PictureGrid.RowDefinitions = new RowDefinitionCollection();
                for (int i = 0; i <= 2; i++)
                {
                    PictureGrid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });
                }
                for (int i = 0; i <= 2; i++)
                {
                    _randomOrderedButtons[i].SetValue(Grid.RowProperty, i);
                    _randomOrderedButtons[i].SetValue(Grid.ColumnProperty, 0);
                    _randomOrderedButtons[i].HeightRequest = imageHeight;
                }
                PictureGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition()
                    {
                        Width = GridLength.Star
                    }
                };
            }
            else
            {
                if (Width > Height)
                {
                    double heightPixels = Height;
                    double perElement = heightPixels / 7;
                    TitleLabel.Margin = new Thickness(25, 0, 25, 25);
                    //Directions.Margin = new Thickness(0, 25, 75, 10);
                    //Speaker.Margin = new Thickness(25,25,5,10);
                }

                PictureGrid.RowDefinitions = new RowDefinitionCollection();
                for (int i = 0; i < 1; i++)
                {
                    PictureGrid.RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Star
                    });
                }
                for (int i = 0; i <= 2; i++)
                {
                    _randomOrderedButtons[i].SetValue(Grid.RowProperty, 0);
                    _randomOrderedButtons[i].SetValue(Grid.ColumnProperty, i);
                    _randomOrderedButtons[i].HeightRequest = -1;
                    //_randomOrderedButtons[i].HeightRequest = Height - TitleLabel.Height - 25 - (Height/7) - CurrentScore.Height - Directions.Height - 35;
                    _randomOrderedButtons[i].VerticalOptions = LayoutOptions.CenterAndExpand;
                }
                PictureGrid.ColumnDefinitions = new ColumnDefinitionCollection()
                {
                    new ColumnDefinition()
                    {
                        Width = GridLength.Star
                    },
                    new ColumnDefinition()
                    {
                        Width = GridLength.Star
                    },
                    new ColumnDefinition()
                    {
                        Width = GridLength.Star
                    }
                };
            }

        }

        #endregion

    }
}