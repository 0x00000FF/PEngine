using Microsoft.AspNetCore.Components;

namespace PEngine.ViewModels;

public interface IViewModel<in T> where T: IComponent
{
    public void Init(T view);
}