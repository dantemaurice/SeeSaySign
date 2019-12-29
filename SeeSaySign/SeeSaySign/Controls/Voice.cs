using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace SeeSaySign.Controls
{
    public class Voice
    {
        public static CancellationTokenSource _cts;

        //Default TTS to initiate voice 
        public static async Task SpeakNow(string text)
        {
            await TextToSpeech.SpeakAsync("       " + text);

            // This method will block until utterance finishes.
        }
        //
        public static void SpeakNowAndThen(string text)
        {
            TextToSpeech.SpeakAsync(text).ContinueWith((t) =>
            {
                // Logic that will run after utterance finishes.

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        //This method takes in an optional CancellationToken to stop the utterance once it starts.
        public static async Task SpeakWithCancelOption(string s)
        {
            _cts = new CancellationTokenSource();
            await TextToSpeech.SpeakAsync(s, cancelToken: _cts.Token);
            // This method will block until utterance finishes.
        }
        // stops utterance 
        public static void CancelSpeech()
        {
            if (_cts?.IsCancellationRequested ?? false)
                return;

            _cts.Cancel();
        }
        //allows for more control on how the audio is uttered. Pitch and Volume are the only supported parameters
        //not ready for use
        public static async Task SpeakSpecifc(string text, float volume, float pitch)
        {
            var settings = new SpeechOptions()
            {
                Volume = volume,
                Pitch = pitch,
                
            };

            await TextToSpeech.SpeakAsync(text, settings);
        }
        public static async Task SpeakSpecifc(string text, float volume, float pitch, Locale locale)
        {
            var settings = new SpeechOptions()
            {
                Volume = volume,
                Pitch = pitch,
                Locale = locale
            };

            await TextToSpeech.SpeakAsync(text, settings);
        }
        //gives options to speak with different languages and accents
        //not ready for use

        public static async Task SpeakLanguage(string text)
        {
            var locales = await TextToSpeech.GetLocalesAsync();

            // Grab the first locale
            var locale = locales.FirstOrDefault();

            var settings = new SpeechOptions()
            {
                Volume = (float?) .75,
                Pitch = (float?) 1.0,
                Locale = locale
            };

            await TextToSpeech.SpeakAsync(text, settings);
        }
    }
}
