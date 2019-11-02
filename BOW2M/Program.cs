using System;
using System.Collections.Generic;
using System.IO;

namespace BOW2M
{
    class Program
    {
        static int Main(string[] args)
        {
            var cmdline = ParseCommandLineToSettingsAndArgs(args);
            if (!cmdline.Options.ContainsKey("/WAV") || !cmdline.Options.ContainsKey("/MP3"))
            {
                Instructions();
                return 1;
            }

            var bitrate = 218;
            if (cmdline.Options.ContainsKey("/BITRATE"))
            {
                if (!int.TryParse(cmdline.Options["/BITRATE"].ToString(), out bitrate))
                {
                    Console.WriteLine("/BITRATE: must be a number");
                    return 3;
                }
            }

            var incomingWavFile = cmdline.Options["/WAV"].ToString();
            if (!File.Exists(incomingWavFile))
            {
                Console.WriteLine($"{incomingWavFile} not found.");
                return 2;
            }

            var outgoingMP3File = cmdline.Options["/MP3"].ToString();
            
            ConvertWavToMP3(incomingWavFile, outgoingMP3File, bitrate);
            
            return 0;
        }


        internal static void Instructions()
        {
            Console.WriteLine("Syntax:\n\tBOW2M.EXE /WAV:\"wavefile\" /MP3:\"mp3file\" [/BITRATE:bitrate]");
            Console.WriteLine("\tbitrate defaults to 128.");
        }

        internal static void ConvertWavToMP3(string inFile, string outFile, int bitrate) => 
            Codec.WaveToMP3(inFile, outFile, bitrate);

        internal static CommandLine ParseCommandLineToSettingsAndArgs(string[] args)
        {
            var results = new CommandLine
            {
                Options = new Dictionary<string, object>(),
                Arguments = new List<string>()
            };

            foreach (string arg in args)
            {
                if (arg.StartsWith("/"))
                {
                    var colonPos = arg.IndexOf(":");
                    if (colonPos > -1)
                    {
                        results.Options[arg.Substring(0, colonPos)] = arg.Substring(colonPos + 1);
                    }
                    else
                    {
                        results.Options[arg] = true;
                    }
                }
                else
                {
                    results.Arguments.Add(arg);
                }
            }
            return results;
        }
    }
    internal class CommandLine
    {
        public Dictionary<string, object> Options { get; set; }
        public List<string> Arguments { get; set; }
    }

}