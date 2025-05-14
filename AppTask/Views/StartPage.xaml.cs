using AppTask.Models;
using AppTask.Repositories;

namespace AppTask.Views;

public partial class StartPage : ContentPage
{
    private ITaskModelRepository _taskModelRepository;
    public StartPage()
    {
        _taskModelRepository = new TaskModelRepository();

        InitializeComponent();

        LoadData();
    }

    public void LoadData()
    {
        var tasks = _taskModelRepository.GetAll();
        CollectionViewTasks.ItemsSource = tasks;
        LblEmptyText.IsVisible = tasks.Count <= 0;
    }

    private void OnButtonClickedToAdd(object sender, EventArgs e)
    {
        Navigation.PushModalAsync(new AddEditTaskPage());
    }

    private void OnBorderClickedToFocusEntry(object sender, TappedEventArgs e)
    {
        Entry_Search.Focus();
    }

    private async void OnImageClickedToDelete(object sender, TappedEventArgs e)
    {
        var task = (TaskModel)e.Parameter;
        var confirm = await DisplayAlert("Deletar", $"Tem certeza que excluir a tarefa: {task.Name}?", "Sim", "Não");

        if (confirm)
        {
            _taskModelRepository.Delete(task);
            LoadData();
        }
    }

    private void OnCheckboxClickedToComplete(object sender, TappedEventArgs e)
    {
        var task = (TaskModel)e.Parameter;

        task.IsCompleted = ((CheckBox)sender).IsChecked;
        _taskModelRepository.Update(task);
    }

    private void OnTapToEdit(object sender, TappedEventArgs e)
    {
        var task = (TaskModel)e.Parameter;
        Navigation.PushModalAsync(new AddEditTaskPage(_taskModelRepository.GetById(task.Id)));
    }
}