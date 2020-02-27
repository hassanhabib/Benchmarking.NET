using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NinjaNye.SearchExtensions.Soundex;

namespace StudentsApp
{
    class Program
    {
        static readonly DateTime now = DateTime.Now;

        static async Task Main(string[] args)
        {
            List<string> names = new List<string> { "Hassan", "Michael" };

            var search = names.SoundexOf<string>(x => x).Matching("Hossan");

            Console.WriteLine("Michelle".ToSoundex());
            Console.WriteLine("David".ToSoundex());
            Console.ReadKey();
        }
    }
}
