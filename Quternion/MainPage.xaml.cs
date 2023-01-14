using System.Diagnostics;
using System.Numerics;

namespace Quternion;

public partial class MainPage : ContentPage
{
	
	public MainPage()
	{
		InitializeComponent();
		horizon.slider.Maximum =360;
        horizon.slider.Value = 180;
		vertical.slider.Maximum = 90;
	}
    float initValueHorizon;
    float initValueVertical;
    private async void ToggleOrientation()
    {
        if (OrientationSensor.Default.IsSupported)
        {
            if (!OrientationSensor.Default.IsMonitoring)
            {
                // Turn on compass
                isinit=true;
                initValueHorizon=(float)horizon.slider.Value;
                initValueVertical=(float)vertical.slider.Value;

                OrientationSensor.Default.ReadingChanged += Orientation_ReadingChanged;
                OrientationSensor.Default.Start(SensorSpeed.UI);
                horizon.slider.IsEnabled = false;
                vertical.slider.IsEnabled = false;
                
            }
            else
            {
                // Turn off compass
                OrientationSensor.Default.Stop();
                OrientationSensor.Default.ReadingChanged -= Orientation_ReadingChanged;
                horizon.slider.IsEnabled = true;
                vertical.slider.IsEnabled = true;
            }
        }
        else
        {
            await DisplayAlert("Error", "unsupported","OK");
        }
    }
    private Quaternion q0;
    bool isinit = true;

    private void Orientation_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
    {
        var q = e.Reading.Orientation;
        if(isinit)
        {
            q0=Quaternion.Conjugate(q);
            isinit = false;
            return;
        }
        q= q0*q;
        
        
        Quaternion p0=new Quaternion(0,1,0,0);
        Quaternion p1=q*p0*Quaternion.Conjugate(q);

        status.Text = $"{{X:{p1.X} Y:{p1.Y} Z:{p1.Z} W:{p1.W}}}";.

        float x, y,z;
        x=p1.X; y=p1.Y;z=p1.Z;



        var horizonRotation = MathF.Acos(y/magnitude(x,y));
        if (x > 0)
        {
            horizonRotation = -horizonRotation;
        }

        var horizonLength=magnitude(x,y);
        var verticalRotation=MathF.Acos(horizonLength) ;//this is a unit quaternion
        if(z < 0) {
            verticalRotation = -verticalRotation;
        }

        point.Text = $"horizon {horizonRotation},vertical {verticalRotation}";

        horizon.slider.Value = initValueHorizon+horizonRotation / (MathF.PI )*180;
        vertical.slider.Value=initValueVertical+verticalRotation / (MathF.PI )* 180;
        if (isDebug)
        {

            isDebug = false;
        }

    }
    bool isDebug = false;
    private static float magnitude(float x,float y)
    {
        return MathF.Pow(x*x + y*y,0.5f);
    } 

    private void Button_Clicked(object sender, EventArgs e)
    {
        ToggleOrientation();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        horizon.slider.Value = 180;
        vertical.slider.Value = 0;
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        isDebug = true;  
    }
}

