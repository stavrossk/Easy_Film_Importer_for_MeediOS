using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EMA;
using EMA.ImportingEngine;
using EMA.MediaSnapshotEngine;
using MeediOS;



namespace MediaFairy.MediaSnapshotEngine
{


    class MultipartMovieDetectionEngine
    {



        internal static bool DetectAndImportMultipartMovie
            (IMLSection moviesSection, bool isUNC,
             FileSystemInfo dir, string externalID,
             ArrayList filesInDir, 
             string[] videoexts, int videosCount)
        {



            videosCount = CountVideosInDirectory
                (videoexts, videosCount, filesInDir);



            if (!Settings.EnableMediaDetection
                || videosCount <= 1
                || videosCount >= 4) 
                return false;




            MainImportingEngine.
                ThisProgress.Progress
                (MainImportingEngine.CurrentProgress,
                "Detecting film parts...");


            int parts = 0;
            string locationTag = String.Empty;
            int count = 1;

            foreach (FileInfo file in filesInDir)
            {


                var fileIsVideo
                    = DetermineIfFileIsVideoByExtensionMatching
                    (videoexts, file);


                parts = DetectMoviePart
                    (parts, fileIsVideo, 
                    file, isUNC, ref count,
                    ref locationTag);


            }





            return OnMultiplePartsDetectionAddToMoviesSection
                (moviesSection, locationTag,
                externalID, parts, dir);



        }


        internal static int CountVideosInDirectory
            (string[] videoexts, int videosCount, 
            ArrayList filesInDir)
        {


            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (FileInfo file in filesInDir)
                // ReSharper restore LoopCanBeConvertedToQuery
            {

                videosCount += videoexts.Count
                    (videoext => videoext == file.Extension);


            }

            return videosCount;
        }




        internal static bool DetermineIfFileIsVideoByExtensionMatching
            (IEnumerable<string> videoexts, FileSystemInfo file)
        {


            #region Determine if file is video

            bool fileIsVideo = false;
            string ext = file.Extension;

            // ReSharper disable UnusedVariable
            foreach (string videoext in videoexts.Where(videoext => videoext == ext))
            // ReSharper restore UnusedVariable
                fileIsVideo = true;

            #endregion



            return fileIsVideo;
        }


        internal static bool OnMultiplePartsDetectionAddToMoviesSection
            (IMLSection moviesSection, string locationTag,
            string externalID, int parts, FileSystemInfo dir)
        {


            if (parts == 2 || parts == 3)
            {
                Debugger.LogMessageToFile
                    (
                        "Found multi-part video: "
                        + dir.Name +
                        " . Proceeding to add the movie to the Movies section."
                    );

                IMLItem item;


                MediaSectionPopulator
                    .AddFileToSection
                    (out item, moviesSection, 
                    dir.Name, locationTag + "|",
                    externalID);
                
                
                return true;
            }


            return false;


        }





        internal static int DetectMoviePart(int parts, bool isVideo, FileInfo file, bool isUNC,
                                           ref int count, ref string locationTag)
        {
           
            if (!isVideo)
                return parts;

            if (DetectMoviePartByKeywords(file, isUNC, ref parts, ref locationTag)) 
                return parts;


            count++;

            return parts;
        }




        internal static bool DetectMoviePartByKeywords(FileSystemInfo file, bool isUNC, ref int parts, ref string locationTag)
        {

            bool isPart = false;
            int digitIndex;


            bool containsPartKeyword = file.Name.IndexOf("PART", StringComparison.OrdinalIgnoreCase) >= 0;
            bool containsCdKeyword   = file.Name.IndexOf("CD"  , StringComparison.OrdinalIgnoreCase) >= 0;


            if (containsPartKeyword)
            {
                digitIndex = file.Name.IndexOf("part") + 4;

                if (Char.IsDigit(file.Name[digitIndex]) || Char.IsDigit(file.Name[digitIndex + 1]))
                {
                    locationTag = locationTag + "|" + file.FullName;
                    parts++;
                    isPart = true;
                }

            }

            if (containsCdKeyword)
            {
                digitIndex = file.Name.IndexOf("cd") + 2;

                if (Char.IsDigit(file.Name[digitIndex]) || Char.IsDigit(file.Name[digitIndex + 1]))
                {
                    locationTag = locationTag + "|" + file.FullName;
                    parts++;
                    isPart = true;
                }

            }




            return isPart || isUNC;

        }
    }
}
