using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sms.Test.DesktopApp;

public class Variable:INotifyPropertyChanged
{
    private string field;

    public string Field
    {
        get => field;
        set
        {
            this.field = value;
            OnPropertyChanged(nameof(Field));
        }
    }

    private string value;

    public string Value
    {
        get => value;
        set
        {
            this.value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    
    public void OnPropertyChanged([CallerMemberName]string prop = "")
    {
        if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
    }
}