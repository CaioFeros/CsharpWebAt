// Services/LogService.cs
using System;
using System.IO;
using System.Collections.Generic;

namespace CsharpWebAt.Services
{
    public class LogService
    {
        private readonly string _logFilePath;
        public List<string> MemoryLogs { get; private set; }

        public LogService()
        {
            // Define o caminho do arquivo de log no diretório raiz do projeto ou em um local acessível
            _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "AppLogs.txt");
            MemoryLogs = new List<string>();
        }

        /// <summary>
        /// Registra a mensagem no console.
        /// </summary>
        /// <param name="message">A mensagem a ser logada.</param>
        public void LogToConsole(string message)
        {
            Console.WriteLine($"[CONSOLE] {DateTime.Now}: {message}");
        }

        /// <summary>
        /// Registra a mensagem em um arquivo de texto.
        /// </summary>
        /// <param name="message">A mensagem a ser logada.</param>
        public void LogToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, $"[FILE] {DateTime.Now}: {message}{Environment.NewLine}");
                Console.WriteLine($"[INFO] Log gravado em arquivo: {_logFilePath}"); // Para feedback visual
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Erro ao gravar log em arquivo: {ex.Message}");
            }
        }

        /// <summary>
        /// Registra a mensagem em uma lista em memória.
        /// </summary>
        /// <param name="message">A mensagem a ser logada.</param>
        public void LogToMemory(string message)
        {
            MemoryLogs.Add($"[MEMORY] {DateTime.Now}: {message}");
            Console.WriteLine($"[INFO] Log gravado em memória. Total: {MemoryLogs.Count}");
        }
    }
}