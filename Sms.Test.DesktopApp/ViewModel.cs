using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sms.Test.DesktopApp;

public class ViewModel : INotifyPropertyChanged
{
    private readonly ILogger<ViewModel> _logger;
    private readonly IHostApplicationLifetime _lifetime;
    public ObservableCollection<Variable>  Variables { get; set; }
    /*
     * Комментарий 2: Так как в задаче явно не указанно с какими переменными средами требуется работать, было принято
     * решение разместить ключи и дефолтные значения в appsettings.json. Приложение будет взаимодействовать только с
     * данными переменными среды. 
     */
    public Dictionary<string,string> VariableDefalutValues { get; set; }

    public void GetEnvironmentVariables()
    {
        Variables.Clear();
        foreach (var name in VariableDefalutValues.Keys)
        {  
            var variable = new Variable();
            var envVar =  Environment.GetEnvironmentVariable(name,EnvironmentVariableTarget.User);
            variable.Field = name;
            if (envVar == null)
                variable.Value = VariableDefalutValues[name];
            else
                variable.Value = envVar;
            Variables.Add(variable);   
        }
        SetBinding();
    }

    public void SetBinding()
    {
        foreach (Variable item in Variables)
        {
            item.PropertyChanged += Variable_PropertyChanged;
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;
    
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if(PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }

    private void Variable_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        try
        {
            if (e.PropertyName == nameof(Variable.Value) && sender != null)
            {
                var variable = (Variable)sender;
                Environment.SetEnvironmentVariable(
                    variable.Field,
                    variable.Value,
                    EnvironmentVariableTarget.User);
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    private void OnApplicationStopping()
    {
        foreach (Variable item in Variables)
        {
            item.PropertyChanged -= Variable_PropertyChanged;
        }
        Variables.Clear();
    }
    /*
     * Комментарий 3: Если я правильно понял задачу, то требуется взаимодействия с переменными среды в ОС. Но там нет
     * такого поля под названием "Комментарий", поэтому данное требования проигнорированно до прояснения деталей задачи.
     */
    public ViewModel(IConfiguration config, ILogger<ViewModel> logger,IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _lifetime = lifetime;
        VariableDefalutValues = new Dictionary<string,string>();
        Variables = new ObservableCollection<Variable>();
        try
        {
            var defaults = config.GetSection("VariablesDefaultValues");
            foreach (var item in defaults.GetChildren())
            {
                VariableDefalutValues.TryAdd(item.Key, item.Value);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
        GetEnvironmentVariables();
        lifetime.ApplicationStopping.Register(OnApplicationStopping);
    }
}