using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Processor
{
    public class Process : BackgroundService
    {
        private FileSystemWatcher _watcher;
        private static IConfiguration _configuration;
        private string _PathToProcess { get; set; }
        private string _PathProcessed { get; set; }
        public Process(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Initializer();
            return Task.CompletedTask;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                //ler todo o arquivo anexado txt linha por linha
                string[] lContent = File.ReadAllLines(e.FullPath);

                //processar o arquivo
                HandlerFile.ProcessFile(lContent, _PathProcessed, e.Name);

                if (File.Exists(e.FullPath))//deleta o arquivo que foi processado
                    File.Delete(e.FullPath);
            }
            catch (Exception lEx)
            {
                throw lEx;
            }

        }

        private void Initializer()
        {
            //inicializa as variáveis
            // seta o watcher para monitorar o caminho com os arquivos que serão inseridos
            try
            {
                _PathToProcess = _configuration.GetSection("Configuration")["PathToProcess"];
                _PathProcessed = _configuration.GetSection("Configuration")["PathProcessed"];
                _watcher = new FileSystemWatcher();
                _watcher.Path = _PathToProcess;
                _watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
                _watcher.Filter = "*.txt";
                _watcher.Created += new FileSystemEventHandler(OnChanged); //monitorar os arquivos que foi criado.
                _watcher.EnableRaisingEvents = true;

            }
            catch (Exception lEx)
            {

                throw lEx;
            }

        }

    }
}
