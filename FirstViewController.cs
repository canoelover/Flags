using System;
using System.Collections.Generic;

using UIKit;
using Foundation;
using CoreGraphics;
using System.Xml;

namespace Flags
{
	public partial class FirstViewController : UIViewController
	{
		private List <Flag> flags = new List <Flag> ();

		private List <Flag> flagCountryList = new List <Flag> ();

		private List <UIButton> flagButtons;

		private int guessRows;
		private int correctAnswers;
		private string correctCountry;
		private int totalGuesses;
		private bool firstTime = true;

		NSObject observer = null;

		private int numberOfButtons = 3;
		private int numberOfQuestions = 5;

		// Open instance of random number generator
		Random rand = new Random ();

		//		ButtonProcessing bp = new ButtonProcessing ();

		public FirstViewController (IntPtr handle) : base (handle)
		{
			GetFlagsFromXML ("FlagList.xml");

//			ResetFlags ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.


			flagButtons = new List <UIButton> ();

			if (firstTime) {
				ResetFlags ();
				firstTime = false;
			}

			// ================ Settings =================
//			for (int i = 0; i < numberOfButtons; i++) {
//				flagButtons [i].TouchUpInside += delegate {
//					UIButton but = flagButtons [i];
//					ButtonProcessing(but);
//				};
//				flagButtons [i].TouchUpInside += (object sender, EventArgs e) => ButtonProcessing (sender);
//			}

			Guess1Button.TouchUpInside += (object sender, EventArgs e) => {
				
				ButtonProcessing (1);
			};


			Guess2Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (2);
			};


			Guess3Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (3);
			};


			Guess4Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (4);
			};

			Guess5Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (5);
			};

			Guess6Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (6);
			};

			Guess7Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (7);
			};
				
			Guess8Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (8);
			};

			Guess9Button.TouchUpInside += (object sender, EventArgs e) => {

				ButtonProcessing (9);
			};



		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		/// <summary>
		/// This method sets up and starts the flag operation
		/// </summary>
		public void ResetFlags ()
		{
			// Initialize working variables
			correctAnswers = 0;
			totalGuesses = 0;


			// Setup the flags being used in the game based on the number of questions pref
			// utilizing the flagCountryList

			int flagCounter = 1;
			int numberOfFlags = flags.Count;
			flagCountryList.Clear ();


			// =========== Settings ===========

			while (flagCounter <= numberOfQuestions) {

				int randomIdx = RandomFlag (1, numberOfFlags);

				// Get the random flag from the flags list
				var randomFlag = flags [randomIdx];

				// Add the flag if it has not already been added before
				if (!flagCountryList.Contains (randomFlag)) {
					flagCountryList.Add (randomFlag);
					flagCounter++;
				}
			}

			// Start the operation by loading the first flag
			LoadNextFlag ();
		}


		public void LoadNextFlag ()
		{

			//Eliminate the flag at the top of the list after moving the current flag to the next
			var nextFlag = flagCountryList [0];
			flagCountryList.RemoveAt (0);
			correctCountry = nextFlag.Country;


			correctAnswers++;

			// Display the current question number

			QuestionNumberLabel.Text = "Question " + correctAnswers.ToString () + " of " + numberOfQuestions.ToString ();

			// Display the flag
			FlagImageView.Image = UIImage.FromBundle ("Images/" + nextFlag.FileName + ".png");

			//Put the correct answer at the bottom of flag
			int index = flags.FindIndex (
				            (Flag f) => f.Country.Equals (correctCountry, StringComparison.Ordinal));
			var holdVal = flags [index];
			flags.RemoveAt (index);
			flags.Add (holdVal);

			flagButtons.Clear ();

			// Add the correct number of buttons based on settings
			flagButtons.Add (Guess1Button);
			flagButtons.Add (Guess2Button);
			flagButtons.Add (Guess3Button);
			flagButtons.Add (Guess4Button);
			flagButtons.Add (Guess5Button);
			flagButtons.Add (Guess6Button);
			flagButtons.Add (Guess7Button);
			flagButtons.Add (Guess8Button);
			flagButtons.Add (Guess9Button);


			for (int i = 0; i < 9; i++) {
				flagButtons [i].Hidden = true;
			}

			//============== Settings ================
			for (int i = 0; i < numberOfButtons; i++) {

				flagButtons [i].Hidden = false;
				var countryName = flags [i].Country;
				flagButtons [i].SetTitle (countryName, UIControlState.Normal);
				flagButtons [i].TitleLabel.Text = countryName;
			}

			// Randomly replace one of the buttons with the right answer
			int index2 = rand.Next (numberOfButtons); 
			flagButtons [index2].SetTitle (correctCountry, UIControlState.Normal);
			flagButtons [index2].TitleLabel.Text = correctCountry;


		}

		/// <summary>
		/// This method determins a random number for a series of numbers
		/// </summary>
		/// <returns>The flag.</returns>
		/// <param name="lowerLimit">Lower limit.</param>
		/// <param name="upperLimit">Upper limit.</param>
		public int RandomFlag (int lowerLimit, int upperLimit)
		{
			int randomNum;

			randomNum = rand.Next (lowerLimit, upperLimit);

			return randomNum;

		}

		/// <summary>
		/// This method parses the xml file flaglist.xml
		/// </summary>
		/// <param name="path">Path.</param>
		private void GetFlagsFromXML (string path)
		{
			XmlTextReader reader = new XmlTextReader (path);
			Flag currentFlag = null;
			while (reader.Read ()) {
				if (reader.NodeType == XmlNodeType.Element && reader.Name == "FLAG") {
					currentFlag = new Flag ();
				} else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "FLAG") {
					flags.Add (currentFlag);
				} else {
					switch (reader.Name) {
					case "FILENAME":
						currentFlag.FileName = reader.ReadElementContentAsString ();
						break;
					case "CONTINIENT":
						currentFlag.Continient = reader.ReadElementContentAsString ();
						break;
					case "COUNTRY":
						currentFlag.Country = reader.ReadElementContentAsString ();
						break;
					}
				}
			}

		}

		/// <summary>
		/// This method handles normal button processing
		/// </summary>
		/// <param name="button">Button.</param>
		public void ButtonProcessing (int button)
		{
			var guess = flagButtons [button - 1];
			totalGuesses++;

			// Correct guess
			if (guess.CurrentTitle.Equals (correctCountry)) {

				// Display correct answer in green
				AnswerLabel.Text = "CORRECT....." + correctCountry + "!";
				AnswerLabel.TextColor = UIColor.Green;


				// End of game process
				if (correctAnswers == numberOfQuestions) {
					var endOfGameActionSheet = new UIActionSheet ("You finished the game.  Is it OK to continue!");
					endOfGameActionSheet.AddButton ("Yes");
					endOfGameActionSheet.DestructiveButtonIndex = 0;
//					taxoffActionSheet.AddButton ("Cancel");
//					taxoffActionSheet.CancelButtonIndex = 1;
					endOfGameActionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
						if (b.ButtonIndex == 0)
						{
							ResetFlags();
						} 
					};

					endOfGameActionSheet.ShowInView (View);	


				} else {
					LoadNextFlag ();
				}
			} else {
				
				// Guess was incorrect and display in red
				AnswerLabel.Text = "Sorry......it's wrong!";
				AnswerLabel.TextColor = UIColor.Red;
				guess.Hidden = true;
			}
					
		}
	}
}

