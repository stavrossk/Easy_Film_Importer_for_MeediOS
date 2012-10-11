using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EMA.ImportingEngine;
using EMA.MediaAnalyzer;
using MeediOS;




namespace EMA.MediaSnapshotEngine
{


    class MediaSectionPopulator
    {






                internal static bool PopulateMediaSection
                (FileInfo file,
                 IMLSection moviesSection, 
                 IEnumerable<string> videoexts,
                 DirectoryInfo parent,
                 bool isVideo,
                 bool isAudio,
                 string fileName, 
                 string parentName,
                 IEnumerable<string> combinedSceneTags)
                {


                    if (!isVideo && !isAudio)
                        return true;

            
                    if (Helpers.UserCancels
                        (MainImportingEngine.SpecialStatus))
                        return false;


                    ImportVideo
                        (file, moviesSection, videoexts,
                         parent, isVideo, fileName,
                         parentName, combinedSceneTags);


                    return true;
                }







        private static void ImportVideo
            (FileInfo file,
             IMLSection moviesSection,
             IEnumerable<string> videoexts,
             DirectoryInfo parent,
             bool isVideo,
             string fileName,
             string parentName,
             IEnumerable<string> combinedSceneTags)
        {


            if (!isVideo)
                return;


            Debugger.LogMessageToFile
                ("This file is a video file.");





            if (Helpers.UserCancels
                (MainImportingEngine.SpecialStatus))
                return;


            if ( MediaSectionPopulatorHelpers
                .SkipTrailer(file) )
                return;


            if (Helpers.UserCancels
                (MainImportingEngine
                .SpecialStatus))
                return;



            if (AdhereToVideoFilesizeThreshold(file)) 
                return;



            if (Helpers.UserCancels
                (MainImportingEngine
                .SpecialStatus))
                return;




            ImportMovie
                (file, moviesSection,
                videoexts, parent,
                fileName, parentName,
                combinedSceneTags);


           
        }




        private static bool AdhereToVideoFilesizeThreshold
            (FileInfo file)
        {


            return file.Length < 52428800;


        }





        private static void ImportMovie
            (FileInfo file,
             IMLSection moviesSection,
             IEnumerable<string> videoexts, 
             DirectoryInfo parent,
             string fileName,
             string parentName,
             IEnumerable<string> combinedSceneTags)
        {



            Debugger.LogMessageToFile
                ("[Media File Importers]" +
                 " Movie File Importer is enabled.");



            if (!MediaSectionPopulatorHelpers
                .ProceedToImportMovie(file)) 
                return;



            LocateTitle_AddToMoviesSection_ImportMediaInfo
                (file, moviesSection, videoexts,
                 parent, fileName, parentName,
                 combinedSceneTags );
     

        
        }




        // ReSharper disable InconsistentNaming
        private static void LocateTitle_AddToMoviesSection_ImportMediaInfo
            (FileSystemInfo file, IMLSection moviesSection,
        // ReSharper restore InconsistentNaming
                            IEnumerable<string> videoexts,
                            DirectoryInfo parent,
                            string fileName, string parentName,
                            IEnumerable<string> combinedSceneTags)
        {


            Application.DoEvents();

            Debugger.LogMessageToFile
                ("This video file is considered to be a film");


            MainImportingEngine.ThisProgress.Progress
                (MainImportingEngine.CurrentProgress, 
                String.Format("Importing film {0}...", fileName));
           


            Debugger.LogMessageToFile
                (String.Format
                ("Importing film {0}...", 
                fileName));



            Application.DoEvents();



            var useParentFolder 
                = MovieTitleLocatingEngine
                .MovieTitleLocator
                (file, videoexts,
                parent, fileName, 
                ref parentName,
                combinedSceneTags );


            Application.DoEvents();

            AddToMoviesSectionAndImportMediaInfo
                (file, moviesSection, fileName,
                parentName, useParentFolder);

        }





        private static void AddToMoviesSectionAndImportMediaInfo
            (FileSystemInfo file, IMLSection moviesSection,
             string fileName, string parentName, bool useParentFolder)
        {
        
            Application.DoEvents();

            IMLItem item;

            AddToMoviesSection
                (file, moviesSection,
                fileName, parentName,
                useParentFolder, out item);

        
        }





        private static void AddToMoviesSection
            (FileSystemInfo file, IMLSection moviesSection,
            string fileName, string parentName,
            bool useParentFolder, out IMLItem item)
        {


            Application.DoEvents();



            if (useParentFolder)
            {

                Debugger.LogMessageToFile
                    ("Will use the parent directory's" +
                     " name as Item's name.");

                Debugger.LogMessageToFile
                    (String.Format
                    ("Adding file {0} to Films section...",
                    file.FullName));


                AddFileToSection
                    (out item, moviesSection,
                    parentName, file.FullName,
                    file.FullName);

          
            }
            else
            {

                Debugger.LogMessageToFile
                    ("Will use the file's" +
                     " name as Item's name.");

                Debugger.LogMessageToFile
                    (String.Format
                    ("Adding file {0}" +
                     " to Films section...",
                     file.FullName));


                AddFileToSection
                    (out item, moviesSection,
                    fileName, file.FullName, 
                    file.FullName);
            
            
            }

        }


        internal static bool AnalyzeFileAndAddToMediaSection
            (FileInfo file,
             IMLSection section,
             IList extensionsToIgnore,
             IEnumerable<string> videoExtensions,
             IEnumerable<string> combinedSceneTags,
             string fileExtension,
             bool isAudio,
             bool isVideo,
             DirectoryInfo parent,
             string parentName,
             string fileName)
        {
            Application.DoEvents();


            MediaTypeDetector.AddFileToIgnoredExtensions
                (file, extensionsToIgnore, fileName,
                 isAudio, isVideo, fileExtension);

            Application.DoEvents();



            if (!PopulateMediaSection
                (file, section,
                 videoExtensions, parent,
                 isVideo, isAudio, fileName, parentName,
                 combinedSceneTags))
                return false;


            Application.DoEvents();

            return true;

        }




        internal static void AddFileToSection
            (out IMLItem item, IMLSection section,
            string itemName, string itemLocation,
            string externalID)
        {


            //TODO: Implement option to not update the Media Section on each imported item.

             if (Settings.UpdateMediaSectionOnEachImportedItem)
                section.BeginUpdate();


            item = section.AddNewItem
                (itemName, itemLocation);

            item.ExternalID = externalID;
            item.SaveTags();



            if (Settings.UpdateMediaSectionOnEachImportedItem)
                section.EndUpdate();


      
        }
    
    
    
    }


}
