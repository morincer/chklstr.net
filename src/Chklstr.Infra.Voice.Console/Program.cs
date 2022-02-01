using System.Speech.Recognition;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Chklstr.Infra.Voice.Console // Note: actual namespace depends on the project name.
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}")
                .CreateLogger();

            var factory = new SerilogLoggerFactory();
            var log = factory.CreateLogger<VoiceCommandDetectionService>();
            var service = new VoiceCommandDetectionService(log);
            service.VoiceCommandDetected += (sender, command) => { Console.Out.WriteLine("Checked"); };

            service.Start();
            /*
            // Create an in-process speech recognizer for the en-US locale.  
            var cultureInfo = new System.Globalization.CultureInfo("en-US");
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine(cultureInfo);

            Choices phrases = new Choices();
            phrases.Add("check");
            phrases.Add("checked");
            phrases.Add("yes");
            phrases.Add("yeah");
            phrases.Add("yeap");

            var gb = new GrammarBuilder();
            gb.Append(phrases);
            gb.Culture = cultureInfo;

            // Create and load a dictation grammar.  
            recognizer.LoadGrammar(new Grammar(gb));

            // Add a handler for the speech recognized event.  
            recognizer.SpeechRecognized += (sender, e) =>
            {
                Console.WriteLine($"Recognized text: {e.Result.Text} @ {e.Result.Confidence}");
            };

            // Configure input to the speech recognizer.  
            recognizer.SetInputToDefaultAudioDevice();

            // Start asynchronous, continuous speech recognition.  
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            */
            // Keep the console window open.  
            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}