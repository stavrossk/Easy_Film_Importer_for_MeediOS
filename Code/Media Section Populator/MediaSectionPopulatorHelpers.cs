using System;
using System.IO;
using System.Windows.Forms;

namespace EMA.MediaSnapshotEngine
{

    internal class MediaSectionPopulatorHelpers
    {




        internal static bool ProceedToImportMovie(FileInfo file)
        {
            Application.DoEvents();

            bool videoIsLocatedInAMovieFolder = VideoIsLocatedInAMovieFolder(file);

            Application.DoEvents();

            bool proceedToImport = videoIsLocatedInAMovieFolder;


            return proceedToImport;
            
        }






        internal static bool VideoIsLocatedInAMovieFolder(FileSystemInfo file)
        {

            Application.DoEvents();

            bool isFilm = false;

            foreach (string filmsFolder in Settings.FilmsFolders)
            {
                if (String.IsNullOrEmpty(filmsFolder)) continue;

                if (!file.FullName.Contains(filmsFolder)) continue;

                Debugger.LogMessageToFile(
                    "This video file is contained in the specified films root directory and will be considered to be a film.");
                isFilm = true;
            }

            Application.DoEvents();

            return isFilm;
        }

        internal static bool SkipTrailer(FileSystemInfo file)
        {
            string fileNameWoExt = file.Name.Replace(file.Extension, "");

            return fileNameWoExt.EndsWith("-trailer");
        }



    
    }


}
