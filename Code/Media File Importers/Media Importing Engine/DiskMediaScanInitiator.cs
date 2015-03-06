using System.Collections;
using System.Collections.Generic;
using EMA.Core;
using EMA.MediaSnapshotEngine;
using MeediOS;




namespace EMA.ImportingEngine
{




    class DiskMediaScanInitiator
    {


        protected internal static void ScanDiskForEntertainmentMedia
            (IEnumerable<string> combinedSceneTags,
             string[] videoExtensions,
             string[] audioExtensions,
             string pluginpath,
             string[] filmLocations,
             ArrayList extensionsToIgnore,
             IEnumerable<string> videoExtensionsCommon, 
             Importer importer,
             IMLSection section)
        {


            ImportingEngineHelpers
                .BeginUpdatingSections
                (section);


            extensionsToIgnore
                = ScanDiskForMovies
                (combinedSceneTags, 
                videoExtensions,
                audioExtensions,
                filmLocations,
                extensionsToIgnore,
                videoExtensionsCommon,
                importer,
                section);




            MediaImportingEngineHelpers
                .WriteNonMediaExtensionsToFile
                (extensionsToIgnore, pluginpath);



            ImportingEngineHelpers
                .EndUpdatingSections
                (section);


        }








        private static ArrayList ScanDiskForMovies
            (IEnumerable<string> combinedSceneTags,
             string[] videoExtensions,
             string[] audioExtensions,
             string[] filmLocations,
             ArrayList extensionsToIgnore,
             IEnumerable<string> videoExtensionsCommon,
             Importer importer, IMLSection section)
        {


            DirectoryScanner.ScanMediaDirectories
                (Settings.FilmsFolders,
                 ref extensionsToIgnore,
                 filmLocations,
                 videoExtensions,
                 audioExtensions,
                 combinedSceneTags,
                 videoExtensionsCommon,
                 importer, section);



            return extensionsToIgnore;
        
        
        }
    }

}
