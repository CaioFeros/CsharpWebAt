using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting; 
using System.Web; 
using CsharpWebAt.Services;

namespace CsharpWebAt.Pages
{
    public class ViewNotesModel : PageModel
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly LogService _logService; // Adicionado para logging detalhado
        private readonly string _notesFolderPath;

        [BindProperty]

        public string NoteContent { get; set; }

        // Propriedade para o nome do arquivo a ser visualizado (recebido via rota ou query string)
        [FromQuery(Name = "file")] // Permite receber via ?file=nome.txt
        public string SelectedFile { get; set; }

        // Lista de arquivos .txt disponíveis na pasta de notas
        public List<string> AvailableFiles { get; set; } = new List<string>();

        // Conteúdo do arquivo selecionado para exibição
        public string FileContent { get; set; }

        // Mensagem de status para o usuário
        public string StatusMessage { get; set; }

        public ViewNotesModel(IWebHostEnvironment hostingEnvironment, LogService logService) // LogService injetado
        {
            _hostingEnvironment = hostingEnvironment;
            _logService = logService; // Atribuição do LogService
            // Combina o caminho raiz da web (wwwroot) com a pasta 'files'
            _notesFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "files");

            // Garante que a pasta exista
            if (!Directory.Exists(_notesFolderPath))
            {
                Directory.CreateDirectory(_notesFolderPath);
            }
        }

        public async Task OnGet()
        {
            await LoadAvailableFiles(); // Carrega a lista de arquivos ao carregar a página

            if (!string.IsNullOrEmpty(SelectedFile))
            {
                // Limpeza do nome do arquivo para evitar Path Traversal (segurança crítica)
                string safeFileName = Path.GetFileName(SelectedFile);
                string filePath = Path.Combine(_notesFolderPath, safeFileName);

                if (System.IO.File.Exists(filePath))
                {
                    try
                    {
                        // Le o conteúdo do arquivo
                        FileContent = await System.IO.File.ReadAllTextAsync(filePath);
                        // Codifica o conteúdo para HTML para exibição segura, prevenindo XSS
                        FileContent = HttpUtility.HtmlEncode(FileContent);
                        StatusMessage = $"Conteúdo de '{safeFileName}' carregado.";
                    }
                    catch (Exception ex)
                    {
                        FileContent = "Erro ao ler o arquivo.";
                        StatusMessage = $"Erro: {ex.Message}";
                        _logService.LogToConsole($"[ERROR] Erro ao ler arquivo '{safeFileName}': {ex.Message}");
                        _logService.LogToFile($"[ERROR] Erro ao ler arquivo '{safeFileName}': {ex.Message}");
                        _logService.LogToMemory($"[ERROR] Erro ao ler arquivo '{safeFileName}': {ex.Message}");
                    }
                }
                else
                {
                    FileContent = "Arquivo não encontrado.";
                    StatusMessage = $"Erro: O arquivo '{safeFileName}' não existe.";
                    _logService.LogToConsole($"[ERROR] Tentativa de ler arquivo não existente: '{safeFileName}'");
                    _logService.LogToFile($"[ERROR] Tentativa de ler arquivo não existente: '{safeFileName}'");
                    _logService.LogToMemory($"[ERROR] Tentativa de ler arquivo não existente: '{safeFileName}'");
                }
            }
        }

        public async Task<IActionResult> OnPostCreateNote()
        {

            try
            {
                // Gera um nome de arquivo único para a anotação
                string fileName = $"note_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}.txt";
                string filePath = Path.Combine(_notesFolderPath, fileName);

                // Salva o conteúdo da anotação no arquivo
                await System.IO.File.WriteAllTextAsync(filePath, NoteContent);

                StatusMessage = $"Anotação '{fileName}' criada com sucesso!";
                NoteContent = string.Empty; // Limpa o campo de texto após o salvamento
                _logService.LogToConsole($"[INFO] Anotação '{fileName}' salva com sucesso.");
                _logService.LogToFile($"[INFO] Anotação '{fileName}' salva com sucesso.");
                _logService.LogToMemory($"[INFO] Anotação '{fileName}' salva com sucesso.");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Erro ao salvar anotação: {ex.Message}";
                _logService.LogToConsole($"[ERROR] Erro ao salvar anotação: {ex.Message}");
                _logService.LogToFile($"[ERROR] Erro ao salvar anotação: {ex.Message}");
                _logService.LogToMemory($"[ERROR] Erro ao salvar anotação: {ex.Message}");
            }

            await LoadAvailableFiles(); // Recarrega a lista de arquivos após criar uma nova nota
            return Page(); // Retorna a mesma página para exibir o status e a lista atualizada
        }

        private async Task LoadAvailableFiles()
        {
            // Obtém todos os arquivos .txt da pasta e extrai apenas os nomes dos arquivos
            AvailableFiles = Directory.GetFiles(_notesFolderPath, "*.txt")
                                    .Select(Path.GetFileName)
                                    .OrderByDescending(f => f) // Ordem decrescente para ver os mais recentes primeiro
                                    .ToList();
        }
    }
}
