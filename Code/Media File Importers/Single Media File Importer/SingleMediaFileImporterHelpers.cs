using System;
using System.IO;
using EMA.MediaImporters;
using MeediOS;

namespace EMA.MediaFileImporters.SingleMediaFileImporter
{


    internal static class SingleMediaFileImporterHelpers
    {


        internal static bool ImportAdditionalMediaTypes
            (FileSystemInfo file, IMLSection moviesSection,
            DirectoryInfo parent, string parentName)
        {


            return DvdDirectoryImporter.ImportDvdDirectory(file, moviesSection, parentName, parent);


        }




        internal static bool RemoveExtensionFromFilename
            (FileSystemInfo file, 
            out string fileName, out string fileExtension)
        {


            fileName = file.Name;


            fileExtension = file.Extension;



            if (String.IsNullOrEmpty
                (fileExtension))
            {

                Debugger.LogMessageToFile
                    (String.Format
                    ("The file's {0} " +
                     "extension is missing." +
                     " This file will be skipped.",
                     fileName));


                return true;
            }



            fileName 
                = fileName.TrimEnd
                (fileExtension.ToCharArray());
        
            
            return false;
    
        }
    
    
    
    
    }


}
