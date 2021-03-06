//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                                               ''
//''    Copyright (C) 2008-2012  Stavros Skamagkis                               ''
//''                                                                             ''
//''    This program is free software: you can redistribute it and/or modify     ''
//''    it under the terms of the GNU General Public License as published by     ''
//''    the Free Software Foundation, either version 3 of the License, or        ''
//''    (at your option) any later version.                                      ''
//''                                                                             ''
//''    This program is distributed in the hope that it will be useful,          ''
//''    but WITHOUT ANY WARRANTY; without even the implied warranty of           ''
//''    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the            ''
//''    GNU General Public License for more details.                             ''
//''                                                                             ''
//''    You should have received a copy of the GNU General Public License        ''
//''    along with this program.  If not, see <http://www.gnu.org/licenses/>.    ''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


using System;
using System.Collections.Generic;
using System.IO;


namespace EMA
{




    internal static class VideoFilenameCleaner
    {




        internal static string CleanVideoFilename
            (string filename, IEnumerable<string> combinedSceneTags)
        {


            Debugger.LogMessageToFile
                    ("[Video filename cleaner]" +
                     " Cleaning filename from common tags...");

            return RemoveTagsFromFilename
                (combinedSceneTags, filename);
        
        }





        //TODO: Add exception handling
        private static string RemoveTagsFromFilename
            (IEnumerable<string> combinedTags,
            string filename)
        {



            int releaseIndex = 0;

            foreach (string tag in  combinedTags)
            {

                int tempIndex = filename.
                    IndexOf(tag, 0, filename.Length,
                    StringComparison.Ordinal);


                //TODO: This change should make the video filename cleaner not stop until it removes all unwanted tags. I'll have to test this though.
                if (tempIndex > 0 && releaseIndex == 0)
                {
                    //if (tempIndex > 0 && releaseIndex > tempIndex)
                    releaseIndex = tempIndex;
                }


            }



            if (releaseIndex > 0)
                filename = filename.Substring
                    (0, releaseIndex);



            filename = filename.Trim();



            return filename;
        }





                public static IEnumerable<string>
                    ReadDictionaryReturnAllTags()
                {

                    Helpers.UpdateProgress
                        ("","Retrieving plugin path...");

                    string pluginPath 
                        = Debugger.GetPluginPath();




                    Helpers.UpdateProgress
                        ("","Loading filename " +
                            "cleaner dictionaries...");


                    Debugger.LogMessageToFile
                        ("Loading filename " +
                         "cleaner dictionaries...");


                    string dictionaryPath 
                        = pluginPath +
                        @"Video filename cleaner dictionary\";



                    string resolutionFile = dictionaryPath + "Resolution.txt";
                    string languageFile = dictionaryPath + "Language.txt";
                    string releaseFile = dictionaryPath + "Release type.txt";
                    string codecFile = dictionaryPath + "Codec.txt";
                    string otherFile = dictionaryPath + "Other.txt";



                    string[] resolutionTags;
                    string[] languageTags;
                    string[] releaseTags;
                    string[] codecTags;
                    string[] otherTags;

                    try
                    {
                       resolutionTags = File.ReadAllLines(resolutionFile);
                       languageTags = File.ReadAllLines(languageFile);
                       releaseTags = File.ReadAllLines(releaseFile);
                       codecTags = File.ReadAllLines(codecFile);
                       otherTags = File.ReadAllLines(otherFile);


                    }
                    catch (Exception e)
                    
                    {

                        Helpers.UpdateProgress
                            ("","Filename cleaner dictionaries not found." +
                                " Import sequence terminated.");

                        Debugger.LogMessageToFile
                            ("Filename cleaner dictionary not found." +
                             " E.F.I. was unable to open and read" +
                             " a dictionary file for video filename cleaning usage.  " +
                             "Please make sure all needed filename cleaner dictionary files" +
                             " are present in the plugin's directory and try again." +
                             "  Missing dictionary file might seriously affect" +
                             " the filename cleaner's performance.");


                        return new[] {""};
                    }





                    var combinedTags = new List<string>
                        (resolutionTags.Length +
                         languageTags.Length + 
                         releaseTags.Length + 
                         codecTags.Length +
                         otherTags.Length);


                    //combinedTags.AddRange(otherTags);
                    //combinedTags.AddRange(languageTags);
                    //combinedTags.AddRange(resolutionTags);
                    //combinedTags.AddRange(codecTags);
                    //combinedTags.AddRange(releaseTags);

                    combinedTags.AddRange
                        (releaseTags);

                    combinedTags.AddRange
                        (codecTags);

                    combinedTags.AddRange
                        (resolutionTags);

                    combinedTags.AddRange
                        (languageTags);

                    combinedTags.AddRange
                        (otherTags);


                    return combinedTags;

                }







    }//endof Class


}//endof Namespace