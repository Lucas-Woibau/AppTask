using AppTask.Models;
using AppTask.Repositories;
using System;

namespace AppTask.Views;

public partial class AddEditTaskPage : ContentPage
{
    private ITaskModelRepository _taskModelRepository;
    private TaskModel _task;
    public AddEditTaskPage()
    {
        InitializeComponent();

        _taskModelRepository = new TaskModelRepository();
        _task = new TaskModel();

        BindableLayout.SetItemsSource(BindableLayout_Steps, _task.SubTasks);
    }
    public AddEditTaskPage(TaskModel task)
    {
        InitializeComponent();

        _taskModelRepository = new TaskModelRepository();
        _task = task;

        FillFields();

        BindableLayout.SetItemsSource(BindableLayout_Steps, _task.SubTasks);
    }

    private void FillFields()
    {
        Entry_TaskName.Text = _task.Name;
        Editor_TaskDescription.Text = _task.Description;
        DatePicker_TaskDate.Date = _task.PrevisionDate;
    }

    private void CloseModal(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void SaveData(object sender, EventArgs e)
    {
        GetDataFromForm();

        bool valid = ValidateData();

        if (valid)
        {
            SaveInDatabase();
            UpdateListInStartPage();
            Navigation.PopModalAsync();
        }
    }

    private void SaveInDatabase()
    {
        if (_task.Id == 0)
        {
            _taskModelRepository.Add(_task);
        }
        else
        {
            _taskModelRepository.Update(_task);
        }
    }

    private bool ValidateData()
    {
        bool validResult = true;
        Label_TaskNameRequired.IsVisible = false;

        if (string.IsNullOrWhiteSpace(_task.Name))
        {
            Label_TaskNameRequired.IsVisible = true;
            validResult = false;
        }

        return validResult;
    }

    private void GetDataFromForm()
    {
        _task.Name = Entry_TaskName.Text;
        _task.Description = Editor_TaskDescription.Text;
        _task.PrevisionDate = DatePicker_TaskDate.Date;
        _task.PrevisionDate = DatePicker_TaskDate.Date.Add(new TimeSpan(23, 59, 59));

        _task.Created = DateTime.Now;
        _task.IsCompleted = false;
    }

    private async void AddStep(object sender, EventArgs e)
    {
        var stepName = await DisplayPromptAsync("Etapa(subtarefa)", "Digite o nome da etapa(subtarefa):", "Adicionar", "Cancelar");

        if (!string.IsNullOrWhiteSpace(stepName))
        {
            _task.SubTasks.Add(new SubTaskModel { Name = stepName, IsCompleted = false });
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        DatePicker_TaskDate.WidthRequest = width - 30;
    }

    private async void OnTapToDelete(object sender, TappedEventArgs e)
    {

        var task = (SubTaskModel)e.Parameter;
        var confirm = await DisplayAlert("Deletar", $"Tem certeza que excluir a tarefa: {task.Name}?", "Sim", "Não");

        if (confirm)
        {
            _task.SubTasks.Remove((SubTaskModel)e.Parameter);
        }

    }
    private void UpdateListInStartPage()
    {
        var navPage = (NavigationPage)App.Current.MainPage;
        var startPage = (StartPage)navPage.CurrentPage;
        startPage.LoadData();
    }
}