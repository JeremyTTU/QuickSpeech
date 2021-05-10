using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;

namespace QuickSpeech
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            while (input != "Q")
            {
                Console.WriteLine("1 - Speech to Text");
                Console.WriteLine("2 - Text to Speech");
                Console.WriteLine();
                Console.WriteLine("Q - Quit");
                Console.Write("? ");
                input = Console.ReadLine().ToUpper();

                switch (input)
                {
                    case "1":
                        SpeechToText();
                        break;
                    case "2":
                        TextToSpeech();
                        break;
                }
            }
        }

        private static void TextToSpeech()
        {
            // Initialize a new instance of the SpeechSynthesizer.  
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output.   5
            synth.SetOutputToDefaultAudioDevice();

            Console.WriteLine("!voice to change voice");
            Console.WriteLine("!quit to quit");
            Console.WriteLine();

            var text = string.Empty;

            while (text != "!quit")
            {
                switch (text.ToUpper())
                {
                    case "!VOICE":
                        foreach (var s in synth.GetInstalledVoices())
                            Console.WriteLine($"{s.VoiceInfo.Name.Split(' ')[1]} - {s.VoiceInfo.Culture.Name} {s.VoiceInfo.Name} ({s.VoiceInfo.Gender} {s.VoiceInfo.Age}) ");
                        Console.Write("Which voice? ");
                        var voice = Console.ReadLine();
                        var correctName = synth.GetInstalledVoices().FirstOrDefault(o => o.VoiceInfo.Name.IndexOf(voice, StringComparison.InvariantCultureIgnoreCase) > -1).VoiceInfo.Name;
                        synth.SelectVoice(correctName);
                        synth.SpeakAsync($"You have now selected ${synth.Voice.Name.Replace("Microsoft", "")}.");
                        break;
                    default:
                        synth.SpeakAsync(text);
                        break;
                }
                text = Console.ReadLine();
            }
        }

        private static void SpeechToText()
        {
            var sr = new SpeechRecognizer();

            // Create an in-process speech recognizer for the en-US locale.  
            using (
            SpeechRecognitionEngine recognizer =
              new SpeechRecognitionEngine(
                new System.Globalization.CultureInfo("en-US")))
            {
                // Create and load a dictation grammar.  
                recognizer.LoadGrammar(new DictationGrammar());

                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                recognizer.SpeechHypothesized +=
                    new EventHandler<SpeechHypothesizedEventArgs>(recognizer_SpeechHypothesized);

                // Configure input to the speech recognizer.  
                recognizer.SetInputToDefaultAudioDevice();

                // Start asynchronous, continuous speech recognition.  
                recognizer.RecognizeAsync(RecognizeMode.Multiple);

                // Keep the console window open.  
                while (true)
                {
                    Console.ReadLine();
                }
            }
        }

        private static void recognizer_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            Console.WriteLine("Hypothesized text: " + e.Result.Text);
        }

        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
        }
    }
}
