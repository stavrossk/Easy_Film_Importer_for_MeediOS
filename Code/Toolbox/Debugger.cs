//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                                               ''
//''    Copyright (C) 2008-2012  Stavros Skamagkis                               ''
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





// ReSharper disable CheckNamespace
namespace EMA
// ReSharper restore CheckNamespace
{
    using System;
    using System.IO;
    using Microsoft.Win32;


    /// <summary>
    /// The debugger.
    /// </summary>
    public class Debugger
    {

        /// <summary>
        /// The get temp path.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetTempPath()
        {

            //IMeedioSystem msystem = null;
            //IMeedioItem pluginDir = msystem.GetSettings("plugin-path");

            //string path = pluginDir["plugin-path"].ToString();
            //string path = @"D:\";


            RegistryKey key = Registry.LocalMachine;

            // DirectoryInfo di;
            key = key.OpenSubKey("SOFTWARE\\Meedio\\Meedio Essentials", false);
            if (key != null)
            {
                string path = key.GetValue("data").ToString();

                if (!path.EndsWith("\\")) path += "\\";

                // path += "logs\\";
                return path;
            }


            return String.Empty;
        }


        /// <summary>
        /// The get plugin path.
        /// </summary>
        /// <returns>
        /// The System.String.
        /// </returns>
        public static string GetPluginPath()
        {

           #if USE_MEEDIO
            RegistryKey key = Registry.LocalMachine;
            //DirectoryInfo di;

            key = key.OpenSubKey("SOFTWARE\\Meedio\\Meedio Essentials", false);
            path = key.GetValue("plugins").ToString();
            if (!path.EndsWith("\\"))
                path += "\\";

            path += @"import\Easy Film Importer\";
            return path;
            #elif USE_MEEDIOS


            if (MeediOS.Configuration.
                ConfigurationManager.
                GetPluginsDirectory
                ("import", @"Easy Film Importer\") != null)
            {

                string path 
                    = MeediOS.Configuration.
                    ConfigurationManager.
                    GetPluginsDirectory
                    ("import", @"Easy Film Importer\");

                return path;
            }


            return string.Empty;

            #endif
        }


                /// <summary>
                /// The get meedio root.
                /// </summary>
                /// <returns>
                /// The System.String.
                /// </returns>
                public static string GetMeedioRoot()
                {


                    #if USE_MEEDIO
                    RegistryKey key = Registry.LocalMachine;
                    //DirectoryInfo di;

                    key = key.OpenSubKey("SOFTWARE\\Meedio\\Meedio Essentials", false);
                    path = key.GetValue("root").ToString();
                    if (!path.EndsWith("\\"))
                        path += "\\";
                    return path;
                    #elif USE_MEEDIOS


                    if (MeediOS.Configuration.
                        ConfigurationManager.
                        GetRootDirectory
                        (null) != null)
                    {

                        string path
                            = MeediOS.Configuration.
                            ConfigurationManager.
                            GetRootDirectory(null);

                        return path;
                    }

                    return string.Empty;

                    #endif
                }





        /// <summary>
        /// The log message to file.
        /// </summary>
        /// <param name="msg">
        /// The msg.
        /// </param>
        public static void LogMessageToFile(string msg)
        {
            if (!Settings.WriteDebugLog)
                return;


            StreamWriter sw = File.AppendText(
                GetPluginPath() + "Debug.log");


            try
            {
                string logLine = string.Format(
                    "{0:G}: {1}", DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }// endof class
}

// endof namespace
