using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MediaFairy.MediaAnalyzer
{
    class MediaTypeDetector
    {


        internal static void AddFileToIgnoredExtensions(FileSystemInfo file, IList extensionsToIgnore,
                                                       string fileName, bool isAudio, bool isVideo,
                                                       string ext)
        {

            if (isVideo || isAudio)
                return;


            if (!Settings.EnableMediaDetection)
                return;


            Debugger.LogMessageToFile("The MediaInfo process detected that the file " + fileName +
                                      " does not contain video nor audio. The file's extension " + ext +
                                      " will be added to the ignored extensions list.");

            extensionsToIgnore.Add(file.Extension);

        }





        internal static void DetectMediaTypeByUsingMediaDurations(string fileName, ref bool isVideo, int audioDuration,
                                                                 int videoDuration, string pluginPath, string ext,
                                                                 ref bool isAudio)
        {

            if (videoDuration >= 15)
            {
                AddFileTypeToKnownVideoExtensions(pluginPath, ext, fileName, isVideo);
                isVideo = true;
            }

            if (videoDuration == 0
                && audioDuration >= 2
                && audioDuration <= 17
                && ext != ".wav" && ext != ".WAV")
            {
                AddFileTypeToKnownAudioExtensions(pluginPath, fileName, isAudio, ext);
                isAudio = true;
            }


        }

        internal static void FileTypeIsMediaExtension(IEnumerable<string> videoExtensions,
                        IEnumerable<string> audioExtensions, string ext,
                        out bool isVideo, out bool isAudio, string fileName,
            IEnumerable<string> videoExtensionsCommon  )
        {

            Helpers.UpdateProgress("Importing Media Files...", 
                String.Format("Detecting media type of file {0}...", fileName));

            isVideo = FileTypeIsVideoExtension(videoExtensionsCommon, ext);

            if (isVideo)
            {
                isAudio = false;
                return;
            }

            isVideo = FileTypeIsVideoExtension(videoExtensions, ext);

            if (isVideo)
            {
                isAudio = false;
                return;
            }

            isAudio = FileTypeIsAudioExtension(audioExtensions, ext);



        }





        internal static bool FileTypeIsAudioExtension(IEnumerable<string> audioExtensions, string ext)
        {
            bool isAudio = false;

            foreach (string audioext in audioExtensions.Where(audioext => String.Compare(audioext, ext, true) == 0))
                isAudio = true;

            return isAudio;
        }

        internal static bool FileTypeIsVideoExtension(IEnumerable<string> videoExtensions, string ext)
        {
            bool isVideo = false;

            foreach (string videoext in videoExtensions.Where(videoext => String.Compare(videoext, ext, true) == 0))
                isVideo = true;

            return isVideo;

        }

        internal static void AddFileTypeToKnownAudioExtensions(string pluginPath, string fileName, bool isAudio, string ext)
        {
            if (!isAudio)
            {
                Debugger.LogMessageToFile(String.Format("The file's extension {0} will be added to the known audio extensions list.", ext));
                var sw = File.AppendText(pluginPath + "Media extensions\\" + "audio_extensions.txt");
                sw.WriteLine(ext);
                sw.Close();
            }

            Debugger.LogMessageToFile(String.Format("The MediaInfo process detected that the file {0} is a music track.", fileName));

            return;
        }

        internal static void AddFileTypeToKnownVideoExtensions(string pluginPath, string ext, string fileName, bool isVideo)
        {
            Debugger.LogMessageToFile(String.Format("The MediaInfo process detected that the file {0} is video.", fileName));

            if (!isVideo)
            {
                Debugger.LogMessageToFile(String.Format("The file's extension {0} will be added to the known video extensions list.", ext));
                var sw = File.AppendText(pluginPath + "Media extensions\\" + "video_extensions.txt");
                sw.WriteLine(ext);
                sw.Close();
            }

            return;
        }

 

        internal static bool FileTypeBelongsToIgnoredExtensions(FileSystemInfo file, ArrayList extensionsToIgnore)
        {

            bool skipThisfile = false;
            //Debugger.LogMessageToFile("Comparing file's extension with ignored extensions list...");

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (string ignoredExtension in (string[]) extensionsToIgnore.ToArray(typeof (string)))
            // ReSharper restore LoopCanBeConvertedToQuery
            {
                if (file.Extension != ignoredExtension)
                    continue;

                skipThisfile = true;
                break;
            }

            return skipThisfile;
        }



        internal static bool FileIsRarPart(string ext)
        {

            if (ext[1] == 'r' || ext[1] == 'R')
            {
                if (Char.IsNumber(ext[2]) && Char.IsNumber(ext[3]))
                {
                    //Debugger.LogMessageToFile("Ignoring multi-part rar archive " + file.Name);
                    return true;
                }
            }

            return false;
        }


    }



}
