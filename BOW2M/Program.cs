using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOW2M
{
    class Program
    {
        static int Main(string[] args)
        {
            var cmdline = ParseCommandLineToSettingsAndArgs(args);
            if (!cmdline.Slashed.ContainsKey("/WAV") || !cmdline.Slashed.ContainsKey("/MP3"))
            {
                Instructions();
                return 1;
            }

            var bitrate = 218;
            if (cmdline.Slashed.ContainsKey("/BITRATE"))
            {
                if (!int.TryParse(cmdline.Slashed["/BITRATE"].ToString(), out bitrate))
                {
                    Console.WriteLine("/BITRATE: must be a number");
                    return 3;
                }
            }

            var incomingWavFile = cmdline.Slashed["/WAV"].ToString();
            if (!File.Exists(incomingWavFile))
            {
                Console.WriteLine($"{incomingWavFile} not found.");
                return 2;
            }

            var outgoingMP3File = cmdline.Slashed["/MP3"].ToString();
            
            WaveToMP3(incomingWavFile, outgoingMP3File, bitrate);
            
            return 0;
        }


        internal static void Instructions()
        {
            Console.WriteLine("Syntax:\n\tBOW2M.EXE /WAV:\"wavefile\" /MP3:\"mp3file\" [/BITRATE:bitrate]");
            Console.WriteLine("\tbitrate defaults to 128.");
        }

        internal static void WaveToMP3(string inFile, string outFile, int bitrate)
        {
            Codec.WaveToMP3(inFile, outFile, bitrate);
        }

        internal static CommandLine ParseCommandLineToSettingsAndArgs(string[] args)
        {
            var results = new CommandLine
            {
                Slashed = new Dictionary<string, object>(),
                Args = new List<string>()
            };

            foreach (string arg in args)
            {
                if (arg.StartsWith("/"))
                {
                    var colonPos = arg.IndexOf(":");
                    if (colonPos > -1)
                    {
                        results.Slashed[arg.Substring(0, colonPos)] = arg.Substring(colonPos + 1);
                    }
                    else
                    {
                        results.Slashed[arg] = true;
                    }
                }
                else
                {
                    results.Args.Add(arg);
                }
            }
            return results;
        }
    }
    internal class CommandLine
    {
        public Dictionary<string, object> Slashed { get; set; }
        public List<string> Args { get; set; }
    }

}