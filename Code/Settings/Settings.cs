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






namespace EMA
{


    public class Settings
    {


                public static string[] RootMediaFolders;

                public static bool UserCancels;

                public static bool ImportingStarted;
                
                public static bool ImportingCompleted;
                
                public static bool WriteDebugLog = true;
        
                public static bool FileInUse;


                internal static bool
                    OverrideAutomatedMovieTitleLocator;

                internal static string
                    MovieTitleLocationInPath 
                    = "Parent folder";
            


                internal static bool
                    ExtractMetadataFromFilenames = true;

                internal static bool 
                    RescanFilesNotFoundInLibrary = true;

                internal static bool
                    ImportDvdFolders = true;


                //TODO: Implement Media Type Detection option.
                internal static bool
                    EnableMediaDetection;



                internal static bool EnableMediaSnapshots;


                internal static string[]
                    FilmsFolders 
                    = new string[1];


                internal static bool
                    UpdateMediaSectionOnEachImportedItem = true;
              



    }



}
