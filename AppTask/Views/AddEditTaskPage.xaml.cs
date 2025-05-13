using AppTask.Models;
using System;

namespace AppTask.Views;

public partial class AddEditTaskPage : ContentPage
{
    private TaskModel _task;
    public AddEditTaskPage()
    {
        InitializeComponent();

        _task = new TaskModel();
    }

    private void CloseModal(object sender, EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private void SaveData(System.Object sender, System.EventArgs e)
    {
        Navigation.PopModalAsync();
    }

    private async void AddStep(System.Object sender, System.EventArgs e)
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
}