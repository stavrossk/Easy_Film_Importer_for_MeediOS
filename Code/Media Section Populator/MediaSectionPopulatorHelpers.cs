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



using System;
using System.IO;
using System.Windows.Forms;



namespace EMA.MediaSnapshotEngine
{



    internal class MediaSectionPopulatorHelpers
    {




        internal static bool
            ProceedToImportMovie
            (FileInfo file)
        {


            Application.DoEvents();


            bool videoIsLocatedInAMovieFolder
                = VideoIsLocatedInAMovieFolder(file);


            Application.DoEvents();


            bool proceedToImport
                = videoIsLocatedInAMovieFolder;




            return proceedToImport;
            
        }






        internal static bool VideoIsLocatedInAMovieFolder
            (FileSystemInfo file)
        {

            Application.DoEvents();

            bool isFilm = false;


            foreach (string filmsFolder 
                in Settings.FilmsFolders)
            {


                if (String.IsNullOrEmpty
                    (filmsFolder))
                    continue;



                if (!file.FullName
                    .Contains
                    (filmsFolder))
                    continue;



                Debugger.LogMessageToFile(
                    "This video file is contained" +
                    " in the specified films root directory" +
                    " and will be considered to be a film.");


                isFilm = true;
            
            
            }


            Application.DoEvents();

            return isFilm;
        }




        internal static bool
            SkipTrailer
            (FileSystemInfo file)
        {


            string fileNameWoExt 
                = file.Name.Replace
                (file.Extension, "");



            return fileNameWoExt
                .EndsWith
                ("-trailer");


        }



    
    }


}
