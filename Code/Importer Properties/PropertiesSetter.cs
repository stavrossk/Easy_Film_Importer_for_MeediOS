//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                    ''
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




using MeediOS;



namespace EMA
{



    public class PropertiesSetter
    {



        internal static void SetProperties
            (IMeedioItem properties)
        {




                //if (properties["MovieTitleLocatorProp"] != null)
                //    Settings.OverrideAutomatedMovieTitleLocator = 
                //        (bool) properties["MovieTitleLocatorProp"];
         

                //if (properties["MovieTitleLocationProp"] != null)
                //    Settings.MovieTitleLocationInPath =
                //        (string) properties["MovieTitleLocationProp"];


            //if (properties["WantToImportFilmsProp"] != null)
            //    Settings.FilmImporterIsEnabled = (bool)properties["WantToImportFilmsProp"];


            //SetMovieDetailsProperties(properties);
 

                //if (properties["ExtractMetadataFromFilenamesProp"] != null)
                //    Settings.ExtractMetadataFromFilenames =
                //        (bool) properties["ExtractMetadataFromFilenamesProp"];




                if (properties["DebugLogProp"] != null)
                    Settings.WriteDebugLog =
                        (bool) properties["DebugLogProp"];




                //if (properties["ImportDvdFoldersProp"] != null)
                //    Settings.ImportDvdFolders =
                //         (bool) properties["ImportDvdFoldersProp"];



        }




    }//endof class


}//endof namespace

