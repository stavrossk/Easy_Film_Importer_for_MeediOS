//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                    ''
//''    Copyright (C) 2008-2010  Stavros Skamagkis                               ''
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
using EMA.ImportingEngine;
using EMA.MediaSectionUpdaters;
using MeediOS;



#if USE_MEEDIO

#elif USE_MEEDIOS

#endif



namespace EMA
{


    internal class UpdateMediaSectionsParams
    {


        public UpdateMediaSectionsParams
            (IMLSection section,
            IBaseSystem iBaseSystem, 
            IEnumerable<string> combinedSceneTags)
        {

            Section = section;

            Ibs = iBaseSystem;

            CombinedSceneTags 
                = combinedSceneTags;
        
        }




        public IMLSection Section
        { get; private set; }


        public IBaseSystem Ibs 
        { get; private set; }


        public IEnumerable<string>
            CombinedSceneTags 
        { get; private set; }


    }







    internal static class MediaUpdaters
    {




        public static bool UpdateMediaSections
            (IBaseSystem iBaseSystem,
            IMLSection section,
            IEnumerable<string> combinedSceneTags)
        {

            string pluginpath 
                = Debugger.GetPluginPath();



            MainImportingEngine
                .CurrentProgress = 0;
            
            MainImportingEngine.GeneralStatus
                = "Starting media updating process...";
            

            MainImportingEngine.SpecialStatus = "";



            try
            {
  



                if (
                    !MoviesSectionUpdater
                    .UpdateMoviesSection
                    (pluginpath,
                    iBaseSystem,
                    combinedSceneTags,
                    section))
                    return true;




            }
            catch (Exception e)
            {

                Debugger.LogMessageToFile
                    (Environment.NewLine + 
                    "An unexpected error occured" +
                    " in DoImport() method." +
                     " The error was: " +
                    Environment.NewLine + e
                    + Environment.NewLine);


                MainImportingEngine
                    .CurrentProgress = 100;


                MainImportingEngine.ThisProgress.Progress
                    (MainImportingEngine.CurrentProgress,
                    "The import process terminated" +
                    " unexpectidly due to an error.");


            }


            return true;
            
            
       
        }






    }



}



