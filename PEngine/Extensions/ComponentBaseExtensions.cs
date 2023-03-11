using Microsoft.AspNetCore.Components;
using PEngine.Utilities;

namespace PEngine.Extensions
{
    public static class ComponentBaseExtensions
    {
        public delegate void SimpleDelegate();
        public delegate bool ShouldRenderDelegate();
        public delegate Task InvokeAsyncDelegate(Action action);

        public static void RequestUpdate(this ComponentBase component)
        {
            var shouldRender = (bool?) component.GetPrivateMethod<ShouldRenderDelegate>("ShouldRender").DynamicInvoke();

            if (shouldRender == true)
            {
                var invokeAsync = component.GetPrivateMethod<InvokeAsyncDelegate>("InvokeAsync");
                var stateHasChanged = component.GetPrivateMethod<Action>("StateHasChanged");

                invokeAsync.Invoke(stateHasChanged).Wait();
            }
        }
    }
}
