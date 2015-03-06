//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
//''    Easy Film Importer for Meedio/MeediOS                                    ''
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





using System;
using MeediOS;


namespace EMA
{


    internal class GetImporterProperties
    {



        internal static bool GetDiagnosticsProperties
            (int index,
             IMeedioPluginProperty prop,
             ref int counter)
        {




            // ReSharper disable RedundantAssignment
            if (index == counter++)
            // ReSharper restore RedundantAssignment
            {
                prop.Name = "DebugLogProp";
                prop.Caption = "Write debug log file";

                prop.GroupCaption = "Diagnostics:";


                prop.HelpText = "If enabled, E.F.I. will write" +
                                " a debug log containing" + Environment.NewLine +
                                "important information from the last " +
                                "importing session for debugging purposes.";

                prop.DefaultValue = Settings.WriteDebugLog;
                prop.DataType = "bool";
                return true;
            }




            return false;
        }









    } //endof class




} //endof namespace