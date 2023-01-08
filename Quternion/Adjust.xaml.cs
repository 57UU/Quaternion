namespace Quternion;

public partial class Adjust : ContentView
{
	public Adjust()
	{
		InitializeComponent();
	
        slider.ValueChanged += Slider_ValueChanged;
	
	}

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
		lable.Text = $"{e.NewValue:F2}";
    }
}