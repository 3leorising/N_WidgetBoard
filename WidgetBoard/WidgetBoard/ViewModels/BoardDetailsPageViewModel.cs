namespace WidgetBoard.ViewModels;

public class BoardDetailsPageViewModel : BaseViewModel
{
    private string boardName;
    public string BoardName
    {
        get => boardName;
        set => SetProperty(ref boardName, value);
    }
}
