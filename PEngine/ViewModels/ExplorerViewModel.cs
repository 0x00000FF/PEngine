using Microsoft.AspNetCore.Components;
using PEngine.ComponentModels;
using PEngine.Services;
using PEngine.Shared;

namespace PEngine.ViewModels;

public class ExplorerViewModel : IViewModel<ExplorerView>
{
    private PostService? _service;
    private ExplorerView? _explorer;
    
    public void Init(ExplorerView view)
    {
        _explorer = view;
    }
}