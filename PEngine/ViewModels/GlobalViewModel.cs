using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace PEngine.ViewModels
{
    public partial class GlobalViewModel : PageModel
    {
        public bool IsLoading { get; private set; }

        private CancellationTokenSource LoadingTokenSource;

        public GlobalViewModel() 
        {
            LoadingTokenSource = new CancellationTokenSource();
        }

        public async Task RequestTask(Func<CancellationToken, Task> action)
        {
            IsLoading = true;

            var token = LoadingTokenSource.Token;

            token.ThrowIfCancellationRequested();
            await action(token);

            IsLoading = false;
        }
    }
}
