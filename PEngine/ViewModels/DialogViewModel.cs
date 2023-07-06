using PEngine.Shared.Dialogs;

namespace PEngine.ViewModels;

public class DialogViewModel
{
    private Queue<IDialog> DialogQueue { get; set; }

    public bool IsDialogShowing => DialogQueue.Count > 0;
    public IDialog? CurrentDialog => DialogQueue.FirstOrDefault();
    
    public DialogViewModel()
    {
        DialogQueue = new();
    }

    public void AddDialog(IDialog dialog)
    {
        DialogQueue.Enqueue(dialog);
    }

    public void CloseDialog()
    {
        if (IsDialogShowing)
        {
            DialogQueue.Dequeue();
        }
    }
}