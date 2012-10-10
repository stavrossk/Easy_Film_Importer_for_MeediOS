using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EMA.ImportingEngine;
using EMA.MediaAnalyzer;
using EMA.MediaFileImporters.SingleMediaFileImporter;
using EMA.MediaFileImportingEngine;
using MeediOS;

namespace EMA.MediaSnapshotEngine
{


    class SingleMediaFileImporter
    {

        internal static bool ImportMediaFile
            (FileInfo file,
             IMLSection section,
             ref ArrayList extensionsToIgnore,
             ref string[] filmLocations,
             string[] videoExtensions,
             string[] audioExtensions,
            int totalFiles,
            ref int currentFile, 
            IEnumerable<string> combinedSceneTags,
            string pluginPath,
            IEnumerable<string> videoExtensionsCommon)
        {
            try
            {

                Application.DoEvents();

                #region Init
                
                currentFile++;

                if (file == null)
                    return true;

                #region Set Progress status
                MainImportingEngine.CurrentProgress = ImportingEngineHelpers.ComputeProgress(currentFile, totalFiles);
                MainImportingEngine.GeneralStatus = "Performing media importing...";
                MainImportingEngine.SpecialStatus = String.Format("Checking file {0}...", file.Name);
                #endregion


                if (!File.Exists(file.FullName))
                    return true;



                #region file variables

                string parentName = string.Empty;
                string fileName;
                string fileExtension;
                bool isVideo;
                bool isAudio;


                if (SingleMediaFileImporterHelpers
                    .RemoveExtensionFromFilename
                    (file, out fileName, out fileExtension))
                    return true;



                #region Retrieve Parent Directory
                DirectoryInfo parent = file.Directory;

                if (parent != null && !String.IsNullOrEmpty( parent.Name) )
                parentName = parent.Name;
                #endregion

                #endregion         

                #endregion

                Application.DoEvents();



                if (SingleMediaFileImporterHelpers.ImportAdditionalMediaTypes
                    (file, section,
                    parent, parentName))
                    return true;

                Application.DoEvents();

                if (MediaTypeDetector.FileTypeBelongsToIgnoredExtensions(file, extensionsToIgnore))
                    return true;

                Application.DoEvents();


                if (ExistingMediaItemSeachEngine.SkipAlreadyImportedFiles
                    (file, filmLocations, section))
                    return true;


                Application.DoEvents();


                MediaTypeDetector.FileTypeIsMediaExtension(videoExtensions,
                    audioExtensions, fileExtension, out isVideo, out isAudio,
                    fileName, videoExtensionsCommon);


                Application.DoEvents();


                if (!MediaSectionPopulator
                    .AnalyzeFileAndAddToMediaSection
                    (file, section, extensionsToIgnore,
                    videoExtensions, combinedSceneTags, 
                    fileExtension, isAudio, isVideo,
                    parent, parentName, fileName)) 
                    return false;
           
            
            }
            catch (Exception e)
            {

                if (file != null)
                    Debugger.LogMessageToFile("An error occured while attempting to import a media file " + 
                        // + file.Name + 
                        "." + Environment.NewLine + "The error was: " + e);

            }


            return true;
    
        
        }
   
    
    
    }




}
