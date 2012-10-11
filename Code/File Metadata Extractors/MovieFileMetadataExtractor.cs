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
using System.Globalization;
using MeediOS;

namespace EMA
{

    public static class MovieFileMetadataExtractor
    {



        //TODO: Test if the overloads for this function are working correctly.
        //Used fy the Film Updater
        public static bool ExtractMetadataFromMovieFilename
            ( IMLItem item, IEnumerable<string> combinedSceneTags)
        {


            if (!Settings.ExtractMetadataFromFilenames)
                return false;


            if (String.IsNullOrEmpty
                (item.Name))
                return false;


           

            Helpers.UpdateProgress
                ("Performing diagnostic operations",
                "Performing local metadata extraction...");
                
            Debugger.LogMessageToFile
                ("Performing local metadata extraction...");



            try
            {

                 ExtractImdbIdFromFilename
                    (item, combinedSceneTags);



                item.Name = VideoFilenameCleaner
                    .CleanVideoFilename
                    (item.Name, combinedSceneTags );

                item.SaveTags();

                //TODO: The change I made to the video filename cleaner should avoid the need of calling this function more than once.
            
                item.Name = VideoFilenameCleaner
                    .CleanVideoFilename
                    (item.Name, combinedSceneTags).Trim();

                item.SaveTags();

                item.Name = VideoFilenameCleaner
                    .CleanVideoFilename
                    (item.Name, combinedSceneTags).Trim();


                item.SaveTags(); 


                ExtractYearFromFilename(item);



            }
            catch (Exception e)
            {

                
                Debugger.LogMessageToFile
                    ("An unexpected error occured" +
                     " in the filename parsing method. " +
                     "The error was: " + e );

            }

            return false;
        }






        //TODO: This function should be changed to use regural expressions. 
        private static string ExtractImdbIdFromFilename
            (IMLItem item, IEnumerable<string> combinedSceneTags)
        {


            string imdbid = String.Empty;

            if (combinedSceneTags == null)
                return imdbid;

            
            if (!item.Name.Contains("tt"))
                return imdbid;


            int imdbidIndex
                = item.Name.IndexOf
                ("tt", StringComparison.Ordinal);


            if ((item.Name.Length - imdbidIndex) <= 2)
                return imdbid;


            Char firstDigit = item.Name[imdbidIndex + 2];
            if (imdbidIndex >= 0 && Char.IsNumber(firstDigit))
            {
                int imdbIndexStart = imdbidIndex;
                int imdbIndexEnd = 0;
                imdbidIndex = imdbidIndex + 2;
                //string substring = item.Name.Substring(imdbid_index, item.Name.Length - imdbid_index - 1);


                for (int i = imdbidIndex; i <= item.Name.Length; i++)
                {
                    if (Char.IsNumber(item.Name[i]))
                    {
                        imdbIndexEnd = i;
                    }
                    else break;
                }

                int imdbidLength = imdbIndexEnd - imdbidIndex + 1;
                imdbid = item.Name.Substring(imdbidIndex, imdbidLength);
                imdbid = "tt" + imdbid;


                item.Tags["ImdbID"] = imdbid;

                string leftNamepart = item.Name.Substring(0, imdbIndexStart);

                string rightNamepart = item.Name.Substring(imdbIndexEnd + 1,
                                                           item.Name.Length - imdbIndexEnd - 1);

                item.Name = leftNamepart + rightNamepart;
                item.SaveTags();
            }


            Debugger.LogMessageToFile
                (String.Format
                ("ImdbID {0} was extracted" +
                 " from item's name...", imdbid));



            return imdbid;
        }





        //TODO: This function should be changed to use regural expressions. 
        private static void ExtractYearFromFilename(IMLItem item)
        {


            #region First Try: Year in parenthesis
            int tempIndex = item.Name.IndexOf("[19");

            int yearIndex = tempIndex >= 0
                ? tempIndex + 1
                : 0;

            tempIndex = item.Name.IndexOf("(19");
            if (tempIndex >= 0) yearIndex = tempIndex + 1;

            tempIndex = item.Name.IndexOf(".19");
            if (tempIndex >= 0) yearIndex = tempIndex + 1;

            tempIndex = item.Name.IndexOf("[20");
            if (tempIndex >= 0) yearIndex = tempIndex + 1;

            tempIndex = item.Name.IndexOf("(20");
            if (tempIndex >= 0) yearIndex = tempIndex + 1;

            tempIndex = item.Name.IndexOf(".20");
            if (tempIndex >= 0) yearIndex = tempIndex + 1;
            #endregion

            #region Second Try: Year without parenthesis
            if (yearIndex == 0)
            {
                int lastChar = item.Name.Length - 1;
                int firstYearChar = lastChar - 3;
                string numberWord = item.Name.Substring(firstYearChar, 4);

                if (!numberWord.Contains("x") && !numberWord.Contains("E") 
                    && !numberWord.Contains("S") )
                    //the number extracted must Not belong to episode numbers
                {
                    if (item.Name.Length >= 8)
                    {
                        if (Char.IsDigit(item.Name[lastChar]))
                        {
                            if (Char.IsDigit(item.Name[firstYearChar]))
                            {
                                #region Second Try: Year without parenthesis
                                tempIndex = item.Name.IndexOf("19");
                                if (tempIndex >= 0) yearIndex = tempIndex;

                                tempIndex = item.Name.IndexOf("20");
                                if (tempIndex >= 0) yearIndex = tempIndex;
                                #endregion
                            }
                        }
                    }
                }
            }
            #endregion




            #region Extract Year and store it in item's tag

            if (yearIndex <= 0)
                return;

            string year = item
                .Name.Substring
                (yearIndex, 4);


            item.Tags["Year"] = year;
            item.SaveTags();


            item.Name = item.Name.Trim();
                       
            Char yearPrevChar = item.Name[yearIndex -1];



            if (String.CompareOrdinal(yearPrevChar.ToString
                (CultureInfo.InvariantCulture),
                '('.ToString(CultureInfo.InvariantCulture)) == 0 
                || String.CompareOrdinal(yearPrevChar.ToString
                (CultureInfo.InvariantCulture),
                '['.ToString(CultureInfo.InvariantCulture)) == 0 
                || String.CompareOrdinal(yearPrevChar.ToString
                (CultureInfo.InvariantCulture),
                '.'.ToString(CultureInfo.InvariantCulture)) == 0)
                yearIndex = yearIndex - 1;


            item.Name = item.Name.Substring(0, yearIndex);
            item.Name = item.Name.Trim();
            item.SaveTags();



            Debugger.LogMessageToFile
                (String.Format
                ("The film's Year ({0})" +
                 " was extracted from" +
                 " item's name.", year));


            #endregion

        }





    }//endof Class

}//endof Namespace