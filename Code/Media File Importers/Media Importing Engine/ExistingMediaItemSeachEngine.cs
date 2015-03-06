using System.Collections.Generic;
using System.IO;
using MeediOS;

namespace EMA.MediaFileImportingEngine
{



    class ExistingMediaItemSeachEngine
    {


        internal static bool SkipAlreadyImportedFiles
            (FileSystemInfo file, 
             IEnumerable<string> filmLocations, 
             IMLSection section)
        {

            Helpers.UpdateProgress("Importing Media Files...",
                "Searching media library for existing file entry...");


            return SearchForExistingItemInMoviesSection
                (file, filmLocations, section);


        }





        private static bool SearchForExistingItemInMoviesSection
            (FileSystemInfo file, IEnumerable<string> filmLocations,
             IMLSection section)
        {



            if (filmLocations == null)
                return false;


            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var movieFolder in Settings.FilmsFolders)
                // ReSharper restore LoopCanBeConvertedToQuery
            {


                if (!file.FullName.Contains(movieFolder))
                    continue;

                //if (filmLocations.Any(location => location == file.FullName))
                //    return true;

                IMLItem item =
                    section.FindItemByExternalID
                        (file.FullName);


                if (item != null)
                    return true;

            }


            return false;

        }




    }



}
