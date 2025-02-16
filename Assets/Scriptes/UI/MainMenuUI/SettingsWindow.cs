using System;

public class SettingsWindow : AnimatedWindow
{
    public Action OnHide;
    public void OnOKClicked()
    {
        Hide();
        OnHide?.Invoke();
    }
}
