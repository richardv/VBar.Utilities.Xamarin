namespace VBarUtilities.Views
{
    using System;
    using System.IO;
    using VBar;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [QueryProperty("SetupId", "SetupId")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupPage
    {
        public int SetupId { get; set; }

        public SetupPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var setup = await App.Database.Setups().FirstOrDefaultAsync(f => f.Id == SetupId);

            if (setup == null)
            {
                return;
            }

            var settingsBytes = Convert.FromBase64String(setup.FileData);

            var settingsStream = new MemoryStream(settingsBytes);

            var settings = new VBarSettings(setup.Name, settingsStream);

            if (settings.List.Count != 1200)
            {
                await DisplayAlert("Invalid Setup", "Invalid Setup Format", "OK");

                return;
            }

            Name.Text = settings.Name;

            Collective_100_1.Text = settings.Value(VBarSettings.Settings.Collective_100_1).ToString();
            Collective_100_2.Text = settings.Value(VBarSettings.Settings.Collective_100_2).ToString();
            Collective_100_3.Text = settings.Value(VBarSettings.Settings.Collective_100_3).ToString();
            Collective_100_4.Text = settings.Value(VBarSettings.Settings.Collective_100_4).ToString();

            Collective_50_1.Text = settings.Value(VBarSettings.Settings.Collective_50_1).ToString();
            Collective_50_2.Text = settings.Value(VBarSettings.Settings.Collective_50_2).ToString();
            Collective_50_3.Text = settings.Value(VBarSettings.Settings.Collective_50_3).ToString();
            Collective_50_4.Text = settings.Value(VBarSettings.Settings.Collective_50_4).ToString();

            Collective_0_1.Text = settings.Value(VBarSettings.Settings.Collective_0_1).ToString();
            Collective_0_2.Text = settings.Value(VBarSettings.Settings.Collective_0_2).ToString();
            Collective_0_3.Text = settings.Value(VBarSettings.Settings.Collective_0_3).ToString();
            Collective_0_4.Text = settings.Value(VBarSettings.Settings.Collective_0_4).ToString();

            Collective_minus50_1.Text = settings.Value(VBarSettings.Settings.Collective_minus50_1).ToString();
            Collective_minus50_2.Text = settings.Value(VBarSettings.Settings.Collective_minus50_2).ToString();
            Collective_minus50_3.Text = settings.Value(VBarSettings.Settings.Collective_minus50_3).ToString();
            Collective_minus50_4.Text = settings.Value(VBarSettings.Settings.Collective_minus50_4).ToString();

            Collective_minus100_1.Text = settings.Value(VBarSettings.Settings.Collective_minus100_1).ToString();
            Collective_minus100_2.Text = settings.Value(VBarSettings.Settings.Collective_minus100_2).ToString();
            Collective_minus100_3.Text = settings.Value(VBarSettings.Settings.Collective_minus100_3).ToString();
            Collective_minus100_4.Text = settings.Value(VBarSettings.Settings.Collective_minus100_4).ToString();

            Mainrotor_Expo_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Expo_1).ToString();
            Mainrotor_Expo_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Expo_2).ToString();
            Mainrotor_Expo_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Expo_3).ToString();
            Mainrotor_Expo_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Expo_4).ToString();

            Mainrotor_Rate_1.Text = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_1).ToString();
            Mainrotor_Rate_2.Text = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_2).ToString();
            Mainrotor_Rate_3.Text = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_3).ToString();
            Mainrotor_Rate_4.Text = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_4).ToString();

            Mainrotor_Gain_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Gain_1).ToString();
            Mainrotor_Gain_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Gain_2).ToString();
            Mainrotor_Gain_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Gain_3).ToString();
            Mainrotor_Gain_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Gain_4).ToString();

            Mainrotor_Style_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Style_1).ToString();
            Mainrotor_Style_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Style_2).ToString();
            Mainrotor_Style_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Style_3).ToString();
            Mainrotor_Style_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Style_4).ToString();

            Mainrotor_Lightness_1.Text = settings.Value(VBarSettings.Settings.Lightness_1).ToString();
            Mainrotor_Lightness_2.Text = settings.Value(VBarSettings.Settings.Lightness_2).ToString();
            Mainrotor_Lightness_3.Text = settings.Value(VBarSettings.Settings.Lightness_3).ToString();
            Mainrotor_Lightness_4.Text = settings.Value(VBarSettings.Settings.Lightness_4).ToString();

            Mainrotor_Elev_Prec_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_1).ToString();
            Mainrotor_Elev_Prec_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_2).ToString();
            Mainrotor_Elev_Prec_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_3).ToString();
            Mainrotor_Elev_Prec_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_4).ToString();

            Mainrotor_Paddle_Sim_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_1).ToString();
            Mainrotor_Paddle_Sim_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_2).ToString();
            Mainrotor_Paddle_Sim_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_3).ToString();
            Mainrotor_Paddle_Sim_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_4).ToString();

            Mainrotor_Integral_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Integral_1).ToString();
            Mainrotor_Integral_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Integral_2).ToString();
            Mainrotor_Integral_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Integral_3).ToString();
            Mainrotor_Integral_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Integral_4).ToString();

            Mainrotor_Pitch_Pump_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_1).ToString();
            Mainrotor_Pitch_Pump_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_2).ToString();
            Mainrotor_Pitch_Pump_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_3).ToString();
            Mainrotor_Pitch_Pump_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_4).ToString();

            Collective_Balance_1.Text = settings.Value(VBarSettings.Settings.Collective_Balance_1).ToString();
            Collective_Balance_2.Text = settings.Value(VBarSettings.Settings.Collective_Balance_2).ToString();
            Collective_Balance_3.Text = settings.Value(VBarSettings.Settings.Collective_Balance_3).ToString();
            Collective_Balance_4.Text = settings.Value(VBarSettings.Settings.Collective_Balance_4).ToString();

            Mainrotor_Optimizer_1.Text = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_1).ToString();
            Mainrotor_Optimizer_2.Text = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_2).ToString();
            Mainrotor_Optimizer_3.Text = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_3).ToString();
            Mainrotor_Optimizer_4.Text = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_4).ToString();

            Mainrotor_Optimizer.Text = settings.OnOff(VBarSettings.Settings.Mainrotor_Optimizer);

            Heli_Size.Text = settings.Value(VBarSettings.Settings.Heli_Size).ToString();

            Tailrotor_Expo_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Expo_1).ToString();
            Tailrotor_Expo_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Expo_2).ToString();
            Tailrotor_Expo_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Expo_3).ToString();
            Tailrotor_Expo_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Expo_4).ToString();

            Tailrotor_Rate_1.Text = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_1).ToString();
            Tailrotor_Rate_2.Text = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_2).ToString();
            Tailrotor_Rate_3.Text = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_3).ToString();
            Tailrotor_Rate_4.Text = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_4).ToString();

            Tailrotor_Gain_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Gain_1).ToString();
            Tailrotor_Gain_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Gain_2).ToString();
            Tailrotor_Gain_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Gain_3).ToString();
            Tailrotor_Gain_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Gain_4).ToString();

            Tailrotor_P_Gain_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_1).ToString();
            Tailrotor_P_Gain_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_2).ToString();
            Tailrotor_P_Gain_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_3).ToString();
            Tailrotor_P_Gain_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_4).ToString();

            Tailrotor_I_Gain_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_1).ToString();
            Tailrotor_I_Gain_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_2).ToString();
            Tailrotor_I_Gain_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_3).ToString();
            Tailrotor_I_Gain_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_4).ToString();

            Tailrotor_I_Limit_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_1).ToString();
            Tailrotor_I_Limit_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_2).ToString();
            Tailrotor_I_Limit_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_3).ToString();
            Tailrotor_I_Limit_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_4).ToString();

            Tailrotor_I_Discharge_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_1).ToString();
            Tailrotor_I_Discharge_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_2).ToString();
            Tailrotor_I_Discharge_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_3).ToString();
            Tailrotor_I_Discharge_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_4).ToString();

            Tailrotor_D_Gain_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_1).ToString();
            Tailrotor_D_Gain_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_2).ToString();
            Tailrotor_D_Gain_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_3).ToString();
            Tailrotor_D_Gain_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_4).ToString();

            Tailrotor_Torque_Compensation_Collective_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_1).ToString();
            Tailrotor_Torque_Compensation_Collective_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_2).ToString();
            Tailrotor_Torque_Compensation_Collective_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_3).ToString();
            Tailrotor_Torque_Compensation_Collective_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_4).ToString();

            Tailrotor_Torque_Compensation_Cyclic_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_1).ToString();
            Tailrotor_Torque_Compensation_Cyclic_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_2).ToString();
            Tailrotor_Torque_Compensation_Cyclic_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_3).ToString();
            Tailrotor_Torque_Compensation_Cyclic_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_4).ToString();

            Tailrotor_Stop_Gain_A_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_1).ToString();
            Tailrotor_Stop_Gain_A_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_2).ToString();
            Tailrotor_Stop_Gain_A_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_3).ToString();
            Tailrotor_Stop_Gain_A_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_4).ToString();

            Tailrotor_Stop_Gain_B_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_1).ToString();
            Tailrotor_Stop_Gain_B_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_2).ToString();
            Tailrotor_Stop_Gain_B_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_3).ToString();
            Tailrotor_Stop_Gain_B_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_4).ToString();

            Tailrotor_Optimize_Side_A_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_1).ToString();
            Tailrotor_Optimize_Side_A_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_2).ToString();
            Tailrotor_Optimize_Side_A_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_3).ToString();
            Tailrotor_Optimize_Side_A_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_4).ToString();

            Tailrotor_Optimize_Side_B_1.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_1).ToString();
            Tailrotor_Optimize_Side_B_2.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_2).ToString();
            Tailrotor_Optimize_Side_B_3.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_3).ToString();
            Tailrotor_Optimize_Side_B_4.Text = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_4).ToString();

            Tailrotor_Wag_Suppression_1.Text = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_1).ToString();
            Tailrotor_Wag_Suppression_2.Text = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_2).ToString();
            Tailrotor_Wag_Suppression_3.Text = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_3).ToString();
            Tailrotor_Wag_Suppression_4.Text = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_4).ToString();

            Mainrotor_Autotrim_Tail.Text = settings.OnOff(VBarSettings.Settings.Mainrotor_Autotrim_Tail);

            Tail_Expert_Acceleration.Text = settings.Value(VBarSettings.Settings.Tail_Expert_Acceleration).ToString();

            GovernorType.Text = settings.GovernorType();

            if (settings.Value(VBarSettings.Settings.Governor_Mode) == GovernorTypes.External)
            {
                Governor_ESC_Output_1.Text = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_1).ToString();
                Governor_ESC_Output_2.Text = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_2).ToString();
                Governor_ESC_Output_3.Text = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_3).ToString();
            }

            if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
            {
                Headspeed_1.Text = settings.Headspeed(VBarSettings.Settings.Headspeed_1).ToString();
                Headspeed_2.Text = settings.Headspeed(VBarSettings.Settings.Headspeed_2).ToString();
                Headspeed_3.Text = settings.Headspeed(VBarSettings.Settings.Headspeed_3).ToString();

                Governor_Gain_1.Text = settings.Value(VBarSettings.Settings.Governor_Gain_1).ToString();
                Governor_Gain_2.Text = settings.Value(VBarSettings.Settings.Governor_Gain_2).ToString();
                Governor_Gain_3.Text = settings.Value(VBarSettings.Settings.Governor_Gain_3).ToString();

                Governor_Collective_Add_1.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Add_1).ToString();
                Governor_Collective_Add_2.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Add_2).ToString();
                Governor_Collective_Add_3.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Add_3).ToString();

                Governor_Cyclic_Add_1.Text = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_1).ToString();
                Governor_Cyclic_Add_2.Text = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_2).ToString();
                Governor_Cyclic_Add_3.Text = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_3).ToString();

                Governor_Collective_Dynamic_1.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_1).ToString();
                Governor_Collective_Dynamic_2.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_2).ToString();
                Governor_Collective_Dynamic_3.Text = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_3).ToString();

                Governor_Basic_Throttle_1.Text = settings.Value(VBarSettings.Settings.Governor_Basic_Throttle_1).ToString();
                Governor_Basic_Throttle_2.Text = settings.Value(VBarSettings.Settings.Governor_Basic_Throttle_2).ToString();
                Governor_Basic_Throttle_3.Text = settings.Value(VBarSettings.Settings.Governor_Basic_Throttle_3).ToString();

                Governor_Integral_1.Text = settings.Value(VBarSettings.Settings.Governor_Integral_1).ToString();
                Governor_Integral_2.Text = settings.Value(VBarSettings.Settings.Governor_Integral_2).ToString();
                Governor_Integral_3.Text = settings.Value(VBarSettings.Settings.Governor_Integral_3).ToString();

                Governor_P_Limit_1.Text = settings.Value(VBarSettings.Settings.Governor_P_Lim_1).ToString();
                Governor_P_Limit_2.Text = settings.Value(VBarSettings.Settings.Governor_P_Lim_2).ToString();
                Governor_P_Limit_3.Text = settings.Value(VBarSettings.Settings.Governor_P_Lim_3).ToString();

                Governor_P_Limit_Minus_1.Text = settings.Value(VBarSettings.Settings.Governor_ESC_Output_1).ToString();
                Governor_P_Limit_Minus_2.Text = settings.Value(VBarSettings.Settings.Governor_ESC_Output_2).ToString();
                Governor_P_Limit_Minus_3.Text = settings.Value(VBarSettings.Settings.Governor_ESC_Output_3).ToString();

                Governor_Min_Throttle.Text = settings.Value(VBarSettings.Settings.Governor_Min_Throttle).ToString();
            }

            Governor_Expert_Autorotation.Text = settings.Throttle(VBarSettings.Settings.Governor_Expert_Autorotation).ToString();

            Governor_Runup_Limit.Text = settings.Value(VBarSettings.Settings.Governor_Runup_Limit).ToString();

            Mainrotor_Autotrim_Aileron.Text = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Aileron).ToString();
            Mainrotor_Autotrim_Elevator.Text = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Elevator).ToString();
            Autotrim_Tail.Text = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Tail).ToString();
            Mainrotor_Autotrim.Text = settings.OnOff(VBarSettings.Settings.Mainrotor_Autotrim);

            Version.Text = settings.Version();

            Name2.Text = settings.Name.Replace("_", " ");

            SensorDirection.Text = settings.SensorDirection();

            ExternalSensor.Text = settings.ExternalSensor();

            MainRotorDirection.Text = settings.MainRotorDirection();

            SwashplateType.Text = settings.SwashplateType();

            End_Trim_Pos_Elevator.Text = settings.Value(VBarSettings.Settings.End_Trim_Pos_Elevator).ToString();
            End_Trim_Pos_Aileron.Text = settings.Value(VBarSettings.Settings.End_Trim_Pos_Aileron).ToString();
            End_Trim_Neg_Elevator.Text = settings.Value(VBarSettings.Settings.End_Trim_Neg_Elevator).ToString();
            End_Trim_Neg_Aileron.Text = settings.Value(VBarSettings.Settings.End_Trim_Neg_Aileron).ToString();

            Cyclic_Ring.Text = settings.Value(VBarSettings.Settings.Cyclic_Ring).ToString();

            Geometry_Correction.Text = settings.OnOff(VBarSettings.Settings.Geometry_Correction);

            Tail_Optimize_Zero_Collective.Text = settings.Value(VBarSettings.Settings.Tail_Optimize_Zero_Collective).ToString();

            Servo_Direction_1.Text = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_1);
            Servo_Direction_2.Text = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_2);
            Servo_Direction_3.Text = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_3);
            Servo_Direction_4.Text = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_4);

            Servo_1_Center_Trim.Text = settings.Value(VBarSettings.Settings.Servo_1_Center_Trim).ToString();
            Servo_2_Center_Trim.Text = settings.Value(VBarSettings.Settings.Servo_2_Center_Trim).ToString();
            Servo_3_Center_Trim.Text = settings.Value(VBarSettings.Settings.Servo_3_Center_Trim).ToString();
            Servo_4_Center_Trim.Text = settings.Value(VBarSettings.Settings.Servo_4_Center_Trim).ToString();

            CollectiveMax.Text = settings.CollectiveMax().ToString();
            CollectiveMin.Text = settings.CollectiveMin().ToString();

            CyclicCalibration.Text = settings.CyclicCalibration().ToString();

            TailServoType.Text = settings.TailServoType();
            TailControl.Text = settings.TailControl();
            Tailrotor_Servo_Reverse.Text = settings.NormalReversed(VBarSettings.Settings.Tailrotor_Servo_Reverse);

            Tailrotor_Servo_Left_Limit.Text = settings.TailLimit(VBarSettings.Settings.Tailrotor_Left_Limit).ToString();
            Tailrotor_Servo_Right_Limit.Text = settings.TailLimit(VBarSettings.Settings.Tailrotor_Right_Limit).ToString();

            GovernorType2.Text = settings.GovernorType();

            Governor_ESC_Endpoint_High.Text = settings.Value(VBarSettings.Settings.Governor_ESC_Endpoint_High).ToString();
            Governor_ESC_Endpoint_Low.Text = settings.Value(VBarSettings.Settings.Governor_ESC_Endpoint_Low).ToString();

            MainGearRatio.Text = settings.MainGearRatio();

            Governor_Sensor_Config.Text = settings.Value(VBarSettings.Settings.Governor_Sensor_Config).ToString();

            AR_Bank.Text = settings.OnOff(VBarSettings.Settings.Autorotation_Bank);

            Timer_Duration.Text = settings.Value(VBarSettings.Settings.Timer_Duration).ToString();

            RxVoltageWarning.Text = settings.RxVoltageWarning().ToString();
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }
    }
}