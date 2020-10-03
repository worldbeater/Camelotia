using System.ComponentModel;
using System.Reactive;
using ReactiveUI;

namespace Camelotia.Presentation.Interfaces
{
    public interface ICreateFolderViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        bool IsLoading { get; }
        
        bool IsVisible { get; set; }
        
        string Name { get; set; }
        
        string Path { get; }
        
        bool HasErrorMessage { get; }
        
        string ErrorMessage { get; }
        
        ReactiveCommand<Unit, Unit> Create { get; }
        
        ReactiveCommand<Unit, Unit> Close { get; }
        
        ReactiveCommand<Unit, Unit> Open { get; }
    }
}