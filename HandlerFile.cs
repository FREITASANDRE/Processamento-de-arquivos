using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Processor
{
    public static class HandlerFile
    {

        public static void ProcessFile(string[] pTextContent, string pPathToSave, string pNameFile)
        {
            //percorrer linha por linha do arquivo e salva em um novo
            // TO DO - Salvar no banco
            foreach (string lTextLine in pTextContent)
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(pPathToSave, pNameFile), true))
                {
                    writer.WriteLine(lTextLine);
                }
            }

        }
    }
}
