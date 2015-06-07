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

		private int guessRows;
		private int correctAnswers;
		private int totalGuesses;

		NSObject observer = null;
		private string continient = "all";
		private int numberOfButtons = 3;
		private int numberOfQuestions = 5;

		// Open instance of random number generator
		Random rand = new Random ();

//		ButtonProcessing bp = new ButtonProcessing ();

		public FirstViewController (IntPtr handle) : base (handle)
		{
			GetFlagsFromXML ("FlagList.xml");

			ResetFlags ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.



			Guess1Button.Hidden = true;
			Guess2Button.Hidden = true;
			Guess3Button.Hidden = true;
			Guess4Button.Hidden = true;
			Guess5Button.Hidden = true;
			Guess6Button.Hidden = true;
			Guess7Button.Hidden = true;
			Guess8Button.Hidden = true;
			Guess9Button.Hidden = true;

			if (numberOfButtons == 3 || numberOfButtons == 6 || numberOfButtons == 9) {

				Guess1Button.Hidden = false;
				Guess2Button.Hidden = false;
				Guess3Button.Hidden = false;

				Guess1Button.TouchUpInside += (object sender, EventArgs e) => {
					

				};


				Guess2Button.TouchUpInside += (object sender, EventArgs e) => {


				};


				Guess3Button.TouchUpInside += (object sender, EventArgs e) => {


				};
			}

			if (numberOfButtons == 6 || numberOfButtons == 9) {

				Guess4Button.Hidden = false;
				Guess5Button.Hidden = false;
				Guess6Button.Hidden = false;

				Guess4Button.TouchUpInside += (object sender, EventArgs e) => {


				};

				Guess5Button.TouchUpInside += (object sender, EventArgs e) => {


				};

				Guess6Button.TouchUpInside += (object sender, EventArgs e) => {


				};
			}

			if (numberOfButtons == 9) {

				Guess7Button.Hidden = false;
				Guess8Button.Hidden = false;
				Guess9Button.Hidden = false;

				Guess7Button.TouchUpInside += (object sender, EventArgs e) => {


				};
				
				Guess8Button.TouchUpInside += (object sender, EventArgs e) => {


				};

				Guess9Button.TouchUpInside += (object sender, EventArgs e) => {


				};
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		/// <summary>
		/// This method sets up and starts the flag operation
		/// </summary>
		public void ResetFlags()
		{
			// Initialize working variables
			correctAnswers = 0;
			totalGuesses = 0;
			flagCountryList.Clear ();

			// Setup the flags being used in the game based on the number of questions pref
			// utilizing the flagCountryList

			int flagCounter = 1;
			int numberOfFlags = flags.Count;


			// =========== Settings ============
			var questions = numberOfQuestions;

			while (flagCounter <= questions) {

				int randomIdx = RandomFlag (1, numberOfFlags);

				// Get the random flag from the flags list
				var randomFlag = flags [randomIdx];

				// Add the flag if it has not already been added before
				if (!flagCountryList.Contains (randomFlag)) {
					flagCountryList.Add (randomFlag);
					flagCounter++;
				}
			}
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
	}
}

