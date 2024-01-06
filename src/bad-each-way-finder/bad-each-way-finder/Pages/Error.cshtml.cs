using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Composition;
using System.Diagnostics;

namespace bad_each_way_finder.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string ExceptionMessage { get; set; }

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
            ExceptionMessage = string.Empty;
        }

        public void OnGet()
        {
            try
            {
                var ex = TempData["Exception"]?.ToString();
                Exception? exception = new Exception();
                if (ex != null)
                {
                    exception = JsonConvert.DeserializeObject<Exception>(ex);
                    ExceptionMessage = exception!.Message;
                }
            }
            catch
            {
                ExceptionMessage = $"Unknown Exception";
            }

            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}