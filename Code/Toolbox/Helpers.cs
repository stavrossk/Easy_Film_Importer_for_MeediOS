



using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using EMA.Code.RegEx_Matchers;
using EMA.ImportingEngine;

#if USE_MEEDIO
#elif USE_MEEDIOS
using MeediOS;
#endif


namespace EMA
{

    public static class Helpers
    {




                public static void UpdateProgress
                    (string generalStatus,
                    string specialStatus)
                {

                    Application.DoEvents();

                    string importerText
                        = String.IsNullOrEmpty(specialStatus)
                        ? generalStatus : specialStatus;


                    if (MainImportingEngine
                        .ThisProgress == null)
                        return;


                    MainImportingEngine.ThisProgress.Progress
                        (MainImportingEngine
                        .CurrentProgress, importerText);


                    Application.DoEvents();


                }
 



        public static void FileIsInUse
            (string location, 
            bool fileServerIsOnline,
            IMLItem item)
        {


            if (!fileServerIsOnline) 
                return;
            
            
            try
            {
                Debugger.LogMessageToFile("Checking if file is in use...");
                var filestream = File.OpenRead(location);
                filestream.Close();
                Settings.FileInUse = false;
            }
            catch (Exception)
            {
                Debugger.LogMessageToFile(String.Format
                    ("The file {0} is in use by another program.", location));

                UpdateProgress
                    ("Performing diagnostics", 
                    String.Format
                    ("This item's media file {0} " +
                     "is in use by another program." +
                     " MediFairy can not perform" +
                     " this operation.", location));

                
                Thread.Sleep(1500);
                
                Settings.FileInUse = true;
            
            }
        
        
        }



        public static bool UserCancels
            (string specialStatus)
        {


            Application.DoEvents();


            if (MainImportingEngine.ThisProgress.Progress
                (MainImportingEngine.CurrentProgress, specialStatus) 
                && !Settings.UserCancels)
                return false;


                Application.DoEvents();


                MainImportingEngine.CurrentProgress = 100;
                UpdateProgress("All operations were cancelled." +
                " Completed tasks were saved to library.",
                String.Empty);


                Application.DoEvents();

                MainImportingEngine.ThisProgress = null;

                return true;

        }








                public static string NormalizePath
                    (string path)
               {

                   try
                   {
                       path = path.Replace(':', '-');
                       path = path.Replace('/', '-');
                       //Path = Path.Replace('\\', '-');
                       path = path.Replace('*', '\'');
                       path = path.Replace('?', ';');
                       path = path.Replace('"', '\'');
                       path = path.Replace('<', '[');
                       path = path.Replace('>', ']');
                       path = path.Replace('|', '-');
                   }
                   catch (Exception e)
                   {
                       Debugger.LogMessageToFile
                           ("An unexpected error occurred" +
                            " in the String Normalizer. " +
                            "The error was: " + e );

                   }



                 return path;
               }




               public static string GetTagValueFromItem
                   (IMLItem item, string tagName)
               {

                   string tagValue = string.Empty;
                   try
                   {
                       tagValue = (string)
                           item.Tags[tagName];
                   }
                 
                   // ReSharper disable EmptyGeneralCatchClause
                   catch
                   // ReSharper restore EmptyGeneralCatchClause
                   { }

                   return tagValue;
               }





    }




}
