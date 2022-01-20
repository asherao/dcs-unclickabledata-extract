using System;
using System.IO;
using System.Text.RegularExpressions;

/* Similar to https://github.com/charlestytler/dcs-clickabledata-extract, 
 * but will extract the args from 
 * ...\aircraft\<Aircraft>\Cockpit\Scripts\mainpanel_init.lua instead.
 * The export format will paste into DCS-ExportScript luas.
 * 
 * This only works if the arg has this format:
 * Canopy.arg_number 				= 26
 */


/* Flow:
 * User drags the mainpanel_init.lua onto the exe
 * The exe takes the file and parses it into a specific format
 * the program then prints the info
 * the user copy and pastes it into their application (DCS-ExportScript lua)
 * User will have to manually check for duplicates from clickabledata-extract
 *  good news is that most I have seen in 1 module is 2 duplicated args.
 */

namespace dcs_unclickabledata_extract
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var importedFile = args[0]; //this is the name of the file that the user drops onto the exe
            var importedFileContents = File.ReadLines(importedFile);//this is the file contents

            //we will be adding these to the discovered args to make the putput DCS-ExportScript friendly
            string startbracket = " [";
            string endbracket = "] = \"%0.4f\", --";

            int counter = 0; //a counter for counting the number of args that was found
            foreach (string line in importedFileContents)//this will evaluate one line at a time
            {
                //you can use https://dotnetfiddle.net/ to test bits of code
                //you can use https://regex101.com/ to tst regex
                string[] partsOfLine = Regex.Split(line, @"(\w+).arg_number\s+=\s+(\w+)");
                if (2 < partsOfLine.Length)//this will trigger only if the above regex was good
                {
                    //format the output and print it
                    Console.WriteLine(startbracket + partsOfLine[2] + endbracket + partsOfLine[1]);
                    counter++;//up the counter
                }
            }
            Console.WriteLine("Done. Made " + counter + " args.");//lets the user know how many args were found
            Console.ReadKey();//allows the user to copy/paste into their application before they close the window
        }
    }
}
