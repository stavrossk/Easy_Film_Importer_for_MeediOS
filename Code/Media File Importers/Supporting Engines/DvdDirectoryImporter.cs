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



using System.IO;
using System.Threading;
using System.Windows.Forms;
using EMA.ImportingEngine;
using EMA.MediaSnapshotEngine;
using MeediOS;
using System;

namespace EMA.MediaImporters
{


    class DvdDirectoryImporter
    {



        internal static bool ImportDvdDirectory
            (FileSystemInfo file, IMLSection moviesSection, 
            string parentName, DirectoryInfo parent)
        {



            if (!Settings.ImportDvdFolders)
                return false;


            if (!file.Name.Contains
                ("video_ts.ifo")
                && 
                !file.Name.Contains
                ("VIDEO_TS.IFO"))
                return false;




            string filmTitle;
            IMLItem item;



            if (String.Compare(parentName, "video_ts", 
                StringComparison.OrdinalIgnoreCase) == 0)
            {


                var secondParent = parent.Parent;



                if (secondParent != null)
                {


                    filmTitle = secondParent.Name;

                    Application.DoEvents();



                    MainImportingEngine
                        .ThisProgress.Progress
                        (MainImportingEngine
                        .CurrentProgress, 
                        String.Format
                        ("Importing dvd film {0}...", 
                        filmTitle));


                    Thread.Sleep(1500);
                   


                    MediaSectionPopulator
                        .AddFileToSection
                        (out item, moviesSection,
                        filmTitle, file.FullName,
                        file.FullName);



                    return true;
                }



                return false;
            }





            filmTitle = parent.Name;

            
            Application.DoEvents();
            

            MainImportingEngine
                .ThisProgress.Progress
                (MainImportingEngine
                .CurrentProgress,
                String.Format
                ("Importing dvd film {0}...",
                filmTitle));


            Thread.Sleep(1500);


            MediaSectionPopulator
                .AddFileToSection
                (out item, moviesSection,
                filmTitle, file.FullName,
                file.FullName);

            return true;

        }






    }




}
