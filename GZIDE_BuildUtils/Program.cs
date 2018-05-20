using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZIDE_BuildUtils {
    class Program {
        static readonly string [] HelpText = {
            "Available commands:",
            "  ZipFolder <input> <output>",
            "    Compresses a folder to a zip file.",
            "  UnzipFolder <zip file> <output folder>",
            "    Decompresses a zip file into a folder.",
            "  ZipFolderOverwrite <input> <output>",
            "    Compresses a folder to a zip file, overwriting the output file if it already exists.",
            "  UnzipFolderOverwrite <zip file> <output folder>",
            "    Decompresses a zip file into a folder, overwriting files.",
        };

        static void DisplayHelp () {
            foreach (string line in HelpText)
                Console.WriteLine (line);
        }

        static int Main (string [] args) {
            if (args.Length < 1) {
                Console.WriteLine ("No arguments passed, displaying help text.\n");
                DisplayHelp ();
            }
            switch (args [0].ToLower ()) {
                case "help":
                    DisplayHelp ();
                    return 0;

                case "zipfolder":
                    return ZipFolder (args, false);
                case "unzipfolder":
                    return UnzipFolder (args, false);
                case "zipfolderoverwrite":
                    return ZipFolder (args, true);
                case "unzipfolderoverwrite":
                    return UnzipFolder (args, true);

                default:
                    Console.WriteLine ("Invalid command \"{0}\", displaying help text.", args [0]);
                    DisplayHelp ();
                    return 255;
            }
        }

        static int ZipFolder (string [] args, bool overwrite) {
            if (args.Length != 3) {
                Console.WriteLine ("Incorrect number of arguments passed to command (expected 2), displaying help text.\n");
                DisplayHelp ();
                return 255;
            }

            string inputFolder = args [1];
            string outputZip = args [2];

            if (!Directory.Exists (inputFolder)) {
                Console.WriteLine ("Error: The input directory \"{0}\" does not exist!", inputFolder);
                return 1;
            } else if (!overwrite && File.Exists (outputZip)) {
                Console.WriteLine ("Error: The output file \"{0}\" already exists!", outputZip);
                return 1;
            }

            var a = new FastZip ();
            a.CreateEmptyDirectories = true;

            if (!overwrite)
                a.CreateZip (outputZip, inputFolder, true, "");
            else {
                string tmpFile = Path.Combine (Path.GetDirectoryName (outputZip), Path.GetTempFileName ());

                a.CreateZip (tmpFile, inputFolder, true, "");

                if (File.Exists (outputZip))
                    File.Delete (outputZip);

                File.Move (tmpFile, outputZip);
            }

            return 0;
        }

        static int UnzipFolder (string [] args, bool overwrite) {
            if (args.Length != 3) {
                Console.WriteLine ("Incorrect number of arguments passed to command (expected 2), displaying help text.\n");
                DisplayHelp ();
                return 255;
            }

            string inputZip = args [1];
            string outputFolder = args [2];

            if (!File.Exists (inputZip)) {
                Console.WriteLine ("Error: The input file \"{0}\" does not exist!", inputZip);
                return 1;
            } else if (!Directory.Exists (outputFolder)) {
                Console.WriteLine ("Error: The output folder \"{0}\" does not exist!", outputFolder);
                return 1;
            }

            var a = new FastZip ();
            a.CreateEmptyDirectories = true;
            a.ExtractZip (inputZip, outputFolder, (overwrite ? FastZip.Overwrite.Always : FastZip.Overwrite.Never), null, "", "", true);

            return 0;
        }
    }
}
