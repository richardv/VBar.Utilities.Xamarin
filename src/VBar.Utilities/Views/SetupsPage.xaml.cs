namespace VBarUtilities.Views
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using Data;
    using Microsoft.AppCenter.Analytics;
    using Models;
    using Syncfusion.XlsIO;
    using VBar;
    using ViewModels;
    using Xamarin.Essentials;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetupsPage
    {
        public SetupsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var setupVms = new List<SetupViewModel>();

            var setups = await App.Database.Setups().OrderByDescending(b => b.DateAndTime).ToListAsync();

            foreach (var setup in setups)
            {
                setupVms.Add(new SetupViewModel
                {
                    Id = setup.Id,
                    Name = setup.Name,
                    Date = setup.DateAndTime
                });
            }

            SetupsFound.Text = $"{Fmt.No(setups.Count)} setup{Fmt.Plural(setups.Count)} found";

            Setups.ItemsSource = setupVms;
        }

        private async void AddSetup_Clicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync();

            if (file == null)
            {
                return;
            }

            using (var reader = new BinaryReader(await file.OpenReadAsync(), Encoding.Unicode))
            {
                if (reader.BaseStream.Length != 2400)
                {
                    await DisplayAlert("Invalid Setup", "Invalid Setup Format", "OK");

                    return;
                }

                byte[] buffer = new byte[reader.BaseStream.Length];

                reader.Read(buffer, 0, (int)reader.BaseStream.Length);

                var setup = new VuSetup()
                {
                    Name = file.FileName,
                    Type = "VBar",
                    ControllerId = "VControl",
                    DateAndTime = DateTime.Now,
                    FileData = Convert.ToBase64String(buffer),
                };

                await App.Database.InsertSetupAsync(setup);
            }

            OnAppearing();
        }

        private async void VStabiSetups_Clicked(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://www.vstabi.info/en/cloud?action=&sort=New+first&start=0&Aid=All+VBC-t&action=setuplist", BrowserLaunchMode.External);
        }

        private async void BuyMeACoffee_Clicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Coffee");

            await Browser.OpenAsync("https://www.buymeacoffee.com/3drchelipilot", BrowserLaunchMode.External);
        }

        private async void VStabiSetupView_Clicked(object sender, EventArgs e)
        {
            var id = ((Button)sender).BindingContext;

            await Shell.Current.GoToAsync(nameof(SetupPage) + "?SetupId=" + id);
        }

        private async void VStabiSetupExport_Clicked(object sender, EventArgs e)
        {
            var id = ((Button)sender).BindingContext;

            var setup = await App.Database.GetSetupById((int)id);

            var settingsBytes = Convert.FromBase64String(setup.FileData);

            var settingsStream = new MemoryStream(settingsBytes);

            var settings = new VBarSettings(setup.Name, settingsStream);

            if (settings.List.Count != 1200)
            {
                await DisplayAlert("Invalid Setup", "Invalid Setup Format", "OK");

                return;
            }

            var sb = new StringBuilder();

            foreach (var vBarSetting in settings.List)
            {
                sb.AppendLine(vBarSetting.Pos + " - " + vBarSetting.Value);
            }

            Debug.WriteLine(sb.ToString());

            using (var excelEngine = new ExcelEngine())
            {
                var application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;
                var workbook = application.Workbooks.Create(1);
                var worksheet = workbook.Worksheets[0];
                worksheet.Name = setup.Name;

                var row = 1;
                worksheet.Range[row, 2].Text = setup.Name;
                worksheet.Range[row, 2].CellStyle.Font.Bold = true;
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Save Date";
                worksheet.Range[row, 2].Text = setup.DateAndTime.ToString("d MMMM yyyy");
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Controller";
                worksheet.Range[row, 2].Text = setup.ControllerId;
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                row++;
                worksheet.Range[row, 2].Text = "Bank 1";
                worksheet.Range[row, 3].Text = "Bank 2";
                worksheet.Range[row, 4].Text = "Bank 3";
                if (settings.ArBank())
                    worksheet.Range[row, 5].Text = "AR bank";
                worksheet.Range[row, 1, row, 5].CellStyle.Font.Bold = true;

                row++;
                worksheet.Range[row, 1].Text = "Flight Parameter";

                row++;
                worksheet.Range[row, 1].Text = "Collective Curve";

                row++;
                worksheet.Range[row, 1].Text = "Max";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_100_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_100_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_100_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_100_4);

                row++;
                worksheet.Range[row, 1].Text = "P 50%";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_50_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_50_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_50_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_50_4);

                row++;
                worksheet.Range[row, 1].Text = "Center";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_0_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_0_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_0_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_0_4);

                row++;
                worksheet.Range[row, 1].Text = "P -50%";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_minus50_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_minus50_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_minus50_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_minus50_4);

                row++;
                worksheet.Range[row, 1].Text = "Min";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_minus100_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_minus100_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_minus100_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_minus100_4);

                var chart = worksheet.Charts.Add();
                chart.ChartTitle = "Collective Curve";
                chart.XPos = 350;
                chart.YPos = 150;

                var series1 = chart.Series.Add(ExcelChartType.Line);
                series1.Name = "Bank 1";

                var series2 = chart.Series.Add(ExcelChartType.Line);
                series2.Name = "Bank 2";

                var series3 = chart.Series.Add(ExcelChartType.Line);
                series3.Name = "Bank 3";


                var series4 = chart.Series.Add(ExcelChartType.Line);
                series4.Name = "AR Bank";

                var xValues = new object[] { "Min", "P -50%", "Center", "P 50%", "Max" };

                var yValues1 = new object[]
                {
                    settings.Value(VBarSettings.Settings.Collective_minus100_1),
                    settings.Value(VBarSettings.Settings.Collective_minus50_1),
                    settings.Value(VBarSettings.Settings.Collective_0_1),
                    settings.Value(VBarSettings.Settings.Collective_50_1),
                    settings.Value(VBarSettings.Settings.Collective_100_1)
                };

                var yValues2 = new object[]
                {
                    settings.Value(VBarSettings.Settings.Collective_minus100_2),
                    settings.Value(VBarSettings.Settings.Collective_minus50_2),
                    settings.Value(VBarSettings.Settings.Collective_0_2),
                    settings.Value(VBarSettings.Settings.Collective_50_2),
                    settings.Value(VBarSettings.Settings.Collective_100_2)
                };
                ;
                var yValues3 = new object[]
                {
                    settings.Value(VBarSettings.Settings.Collective_minus100_3),
                    settings.Value(VBarSettings.Settings.Collective_minus50_3),
                    settings.Value(VBarSettings.Settings.Collective_0_3),
                    settings.Value(VBarSettings.Settings.Collective_50_3),
                    settings.Value(VBarSettings.Settings.Collective_100_3)
                };

                var yValues4 = new object[]
                {
                    settings.Value(VBarSettings.Settings.Collective_minus100_4),
                    settings.Value(VBarSettings.Settings.Collective_minus50_4),
                    settings.Value(VBarSettings.Settings.Collective_0_4),
                    settings.Value(VBarSettings.Settings.Collective_50_4),
                    settings.Value(VBarSettings.Settings.Collective_100_4)
                };

                series1.EnteredDirectlyCategoryLabels = xValues;
                series1.EnteredDirectlyValues = yValues1;
                series2.EnteredDirectlyValues = yValues2;
                series3.EnteredDirectlyValues = yValues3;
                series4.EnteredDirectlyValues = yValues4;

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Main Rotor";

                row++;
                worksheet.Range[row, 1].Text = "Exponential";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Expo_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Expo_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Expo_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Expo_4);

                row++;
                worksheet.Range[row, 1].Text = "Agility";
                worksheet.Range[row, 2].Value2 = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_1);
                worksheet.Range[row, 3].Value2 = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_2);
                worksheet.Range[row, 4].Value2 = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.MainRotorAgility(VBarSettings.Settings.Mainrotor_Rate_4);

                row++;
                worksheet.Range[row, 1].Text = "Gain";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Gain_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Gain_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Gain_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Gain_4);

                row++;
                worksheet.Range[row, 1].Text = "Style";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Style_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Style_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Style_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Style_4);

                row++;
                worksheet.Range[row, 1].Text = "Lightness";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Lightness_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Lightness_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Lightness_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Lightness_4);

                row++;
                worksheet.Range[row, 1].Text = "Elev. Precomp";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Elev_Prec_4);

                row++;
                worksheet.Range[row, 1].Text = "Paddlesim";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Paddle_Sim_4);

                row++;
                worksheet.Range[row, 1].Text = "Integral";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Integral_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Integral_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Integral_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Integral_4);

                row++;
                worksheet.Range[row, 1].Text = "Pitch Pump";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Pitch_Pump_4);

                row++;
                worksheet.Range[row, 1].Text = "Coll. Balance";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Collective_Balance_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Collective_Balance_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Collective_Balance_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Collective_Balance_4);

                row++;
                worksheet.Range[row, 1].Text = "Optimizer Values";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer_4);

                row++;
                worksheet.Range[row, 1].Text = "Optimizer";
                worksheet.Range[row, 2].Text = settings.Value(VBarSettings.Settings.Mainrotor_Optimizer) == 1 ? "On" : "Off";

                row++;
                worksheet.Range[row, 1].Text = "General Response";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Heli_Size);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Tail Rotor";

                row++;
                worksheet.Range[row, 1].Text = "Exponential";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Expo_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Expo_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Expo_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Expo_4);

                row++;
                worksheet.Range[row, 1].Text = "Yaw Rate";
                worksheet.Range[row, 2].Value2 = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_1);
                worksheet.Range[row, 3].Value2 = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_2);
                worksheet.Range[row, 4].Value2 = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.TailRotorAgility(VBarSettings.Settings.Tailrotor_Rate_4);

                row++;
                worksheet.Range[row, 1].Text = "Gain";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Gain_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Gain_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Gain_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Gain_4);

                row++;
                worksheet.Range[row, 1].Text = "Proportional";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_P_Gain_4);

                row++;
                worksheet.Range[row, 1].Text = "Integral";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Gain_4);

                row++;
                worksheet.Range[row, 1].Text = "I Limiter";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Limit_4);

                row++;
                worksheet.Range[row, 1].Text = "I Discharge";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_I_Discharge_4);

                row++;
                worksheet.Range[row, 1].Text = "Differential";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_D_Gain_4);

                row++;
                worksheet.Range[row, 1].Text = "Coll. Precomp.";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Collective_4);

                row++;
                worksheet.Range[row, 1].Text = "Cycl. Precomp.";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Torque_Compensation_Cyclic_4);

                row++;
                worksheet.Range[row, 1].Text = "Stop Gain A";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_A_4);

                row++;
                worksheet.Range[row, 1].Text = "Stop Gain B";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Stop_Gain_B_4);

                row++;
                worksheet.Range[row, 1].Text = "Optimizer A";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_A_4);

                row++;
                worksheet.Range[row, 1].Text = "Optimizer B";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tailrotor_Optimize_Side_B_4);

                row++;
                worksheet.Range[row, 1].Text = "Wag Suppression";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_1);
                worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_2);
                worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_3);
                if (settings.ArBank())
                    worksheet.Range[row, 5].Value2 = settings.Value(VBarSettings.Settings.Tail_Wag_Suppression_4);

                row++;
                worksheet.Range[row, 1].Text = "Auto Opti";
                worksheet.Range[row, 2].Value2 = settings.OnOff(VBarSettings.Settings.Mainrotor_Autotrim_Tail);

                row++;
                worksheet.Range[row, 1].Text = "Tail Acceleration";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tail_Expert_Acceleration);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Governor";
                worksheet.Range[row, 2].Text = settings.GovernorType();
                worksheet.Range[row, 2, row, 4].Merge();

                if (settings.Value(VBarSettings.Settings.Governor_Mode) == GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Output";
                    worksheet.Range[row, 2].Value2 = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_1);
                    worksheet.Range[row, 3].Value2 = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_2);
                    worksheet.Range[row, 4].Value2 = settings.GovernorOutput(VBarSettings.Settings.Governor_ESC_Output_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Headspeed";
                    worksheet.Range[row, 2].Value2 = settings.Headspeed(VBarSettings.Settings.Headspeed_1);
                    worksheet.Range[row, 3].Value2 = settings.Headspeed(VBarSettings.Settings.Headspeed_2);
                    worksheet.Range[row, 4].Value2 = settings.Headspeed(VBarSettings.Settings.Headspeed_3);
                    worksheet.Range[row, 2, row, 4].NumberFormat = "#,##0";
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Gain";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Gain_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_Gain_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_Gain_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    if (settings.GovernorType() == "VBar Nitro Governor")
                    {
                        row++;
                        worksheet.Range[row, 1].Text = "Idle";
                        worksheet.Range[row, 2].Value2 = settings.Throttle(VBarSettings.Settings.Governor_ESC_Endpoint_Low) * 2;
                    }

                    row++;
                    worksheet.Range[row, 1].Text = "Collective Ad";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Add_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Add_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Add_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Cyclic Ad";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_Cyclic_Add_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Collective Dynamic";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_Collective_Dynamic_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Basic Throttle";
                    worksheet.Range[row, 2].Value2 = settings.Throttle(VBarSettings.Settings.Governor_Basic_Throttle_1);
                    worksheet.Range[row, 3].Value2 = settings.Throttle(VBarSettings.Settings.Governor_Basic_Throttle_2);
                    worksheet.Range[row, 4].Value2 = settings.Throttle(VBarSettings.Settings.Governor_Basic_Throttle_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Integral";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Integral_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_Integral_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_Integral_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Proportional Limit +";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_P_Lim_1);
                    worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_P_Lim_2);
                    worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_P_Lim_3);
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    if (settings.GovernorType() != "VBar Nitro Governor")
                    {
                        row++;
                        worksheet.Range[row, 1].Text = "Proportional Limit -";
                        worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_1);
                        worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_2);
                        worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_3);
                    }
                }

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    if (settings.GovernorType() == "VBar Nitro Governor")
                    {
                        row++;
                        worksheet.Range[row, 1].Text = "Differential";
                        worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_1);
                        worksheet.Range[row, 3].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_2);
                        worksheet.Range[row, 4].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Output_3);
                    }

                    row++;
                    worksheet.Range[row, 1].Text = "Min. Throttle";
                    worksheet.Range[row, 2].Value2 = settings.Throttle(VBarSettings.Settings.Governor_Min_Throttle);

                    if (settings.GovernorType() == "VBar Nitro Governor")
                    {
                        row++;
                        worksheet.Range[row, 1].Text = "Direct Throttle";
                        worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Nitro_Direct_Throttle);
                    }
                }

                row++;
                worksheet.Range[row, 1].Text = "AR Throttle";
                worksheet.Range[row, 2].Value2 = settings.Throttle(VBarSettings.Settings.Governor_Expert_Autorotation);

                if (settings.Value(VBarSettings.Settings.Governor_Mode) != GovernorTypes.External)
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Runup Limit";
                    worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Runup_Limit);
                }

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Autotrim";

                row++;
                worksheet.Range[row, 1].Text = "Elevator";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Elevator);

                row++;
                worksheet.Range[row, 1].Text = "Aileron";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Aileron);

                row++;
                worksheet.Range[row, 1].Text = "Tail";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim_Tail);

                row++;
                worksheet.Range[row, 1].Text = "Autotrim Flight";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Mainrotor_Autotrim) == 1 ? "On" : "Off";

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Device and Version";

                row++;
                worksheet.Range[row, 1].Text = "Device Type";
                worksheet.Range[row, 2].Text = setup.Type;

                row++;
                worksheet.Range[row, 1].Text = "Firmware Version";
                worksheet.Range[row, 2].Text = settings.Version();

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Model Setup";

                row++;
                worksheet.Range[row, 1].Text = "Basic Model Setup";

                row++;
                worksheet.Range[row, 1].Text = "Model Name";
                worksheet.Range[row, 2].Text = setup.Name.Replace("_", " ");
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Sensor Orientation";
                worksheet.Range[row, 2].Text = settings.SensorDirection();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "External Sensor";
                worksheet.Range[row, 2].Text = settings.ExternalSensor();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Rotation Direction";
                worksheet.Range[row, 2].Text = settings.MainRotorDirection();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Main Rotor Setup";

                row++;
                worksheet.Range[row, 1].Text = "Swashplate Type";
                worksheet.Range[row, 2].Text = settings.SwashplateType();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Elevator Top";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.End_Trim_Pos_Elevator);

                row++;
                worksheet.Range[row, 1].Text = "Aileron Top";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.End_Trim_Pos_Aileron);

                row++;
                worksheet.Range[row, 1].Text = "Elevator Bottom";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.End_Trim_Neg_Elevator);

                row++;
                worksheet.Range[row, 1].Text = "Aileron Bottom";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.End_Trim_Neg_Aileron);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Cyclic Ring";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Cyclic_Ring);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Geometry Correction";
                worksheet.Range[row, 2].Text = settings.OnOff(VBarSettings.Settings.Geometry_Correction);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Zero Collective";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Tail_Optimize_Zero_Collective);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Servo Direction Channel 1";
                worksheet.Range[row, 2].Value2 = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_1);

                row++;
                worksheet.Range[row, 1].Text = "Servo Direction Channel 2";
                worksheet.Range[row, 2].Value2 = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_2);

                row++;
                worksheet.Range[row, 1].Text = "Servo Direction Channel 3";
                worksheet.Range[row, 2].Value2 = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_3);

                row++;
                worksheet.Range[row, 1].Text = "Servo Direction Channel 4";
                worksheet.Range[row, 2].Value2 = settings.NormalReversed(VBarSettings.Settings.Servo_Direction_4);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Trim Servo Ch1";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Servo_1_Center_Trim);

                row++;
                worksheet.Range[row, 1].Text = "Trim Servo Ch2";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Servo_2_Center_Trim);

                row++;
                worksheet.Range[row, 1].Text = "Trim Servo Ch3";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Servo_3_Center_Trim);

                row++;
                worksheet.Range[row, 1].Text = "Trim Servo Ch4";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Servo_4_Center_Trim);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Collective Max";
                worksheet.Range[row, 2].Value2 = settings.CollectiveMax();

                row++;
                worksheet.Range[row, 1].Text = "Collective Min";
                worksheet.Range[row, 2].Value2 = settings.CollectiveMin();

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Cyclic Calibration";
                worksheet.Range[row, 2].Value2 = settings.CyclicCalibration();

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Tail Rotor Setup";

                row++;
                worksheet.Range[row, 1].Text = "Tail Servo Type";
                worksheet.Range[row, 2].Value2 = settings.TailServoType();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Tail Control";
                worksheet.Range[row, 2].Value2 = settings.TailControl();

                row++;
                worksheet.Range[row, 1].Text = "Tail Servo Direction";
                worksheet.Range[row, 2].Value2 = settings.NormalReversed(VBarSettings.Settings.Tailrotor_Servo_Reverse);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Tail Servo Left Limit";
                worksheet.Range[row, 2].Value2 = settings.TailLimit(VBarSettings.Settings.Tailrotor_Left_Limit);

                row++;
                worksheet.Range[row, 1].Text = "Tail Servo Right Limit";
                worksheet.Range[row, 2].Value2 = settings.TailLimit(VBarSettings.Settings.Tailrotor_Right_Limit);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Governor Setup";

                row++;
                worksheet.Range[row, 1].Text = "Governor Type";
                worksheet.Range[row, 2].Value2 = settings.GovernorType();
                worksheet.Range[row, 2, row, 4].Merge();

                row++;
                worksheet.Range[row, 1].Text = "Collective Control";
                worksheet.Range[row, 2].Value2 = "Off";

                row++;
                worksheet.Range[row, 1].Text = "Output Endpoint Full";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Endpoint_High);

                row++;
                worksheet.Range[row, 1].Text = "Output Endpoint Stop";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_ESC_Endpoint_Low);

                if (settings.GovernorType() == "VBar Nitro Governor")
                {
                    row++;
                    worksheet.Range[row, 1].Text = "Servo Reverse";
                    //worksheet.Range[row, 2].Value2 = settings.MainGearRatio();

                    row++;
                    worksheet.Range[row, 1].Text = "Cut Switch";
                    //worksheet.Range[row, 2].Value2 = settings.MainGearRatio();
                }

                row++;
                worksheet.Range[row, 1].Text = "Gear Ratio";
                worksheet.Range[row, 2].Value2 = settings.MainGearRatio();

                row++;
                worksheet.Range[row, 1].Text = "Motor Poles";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Governor_Sensor_Config);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Timer Setup";

                row++;
                worksheet.Range[row, 1].Text = "Time";
                worksheet.Range[row, 2].Value2 = settings.Value(VBarSettings.Settings.Timer_Duration);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "Manage Banks";

                row++;
                worksheet.Range[row, 1].Text = "Bank 4 as AR bank";
                worksheet.Range[row, 2].Value2 = settings.OnOff(VBarSettings.Settings.Autorotation_Bank);

                row++;
                row++;
                worksheet.Range[row, 1].Text = "RC Voltage Monitor";

                row++;
                worksheet.Range[row, 1].Text = "VBar Voltage Threshold";
                worksheet.Range[row, 2].Value2 = settings.RxVoltageWarning();
                worksheet.Range[row, 2].CellStyle.NumberFormat = "0.0";

                worksheet.Range[1, 1, row, 1].CellStyle.Font.Bold = true;

                worksheet.AutofitColumn(1);
                worksheet.AutofitColumn(2);
                worksheet.AutofitColumn(3);
                worksheet.AutofitColumn(4);

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    await DependencyService.Get<ISave>().SaveAndView(
                             $"{setup.Name}.xlsx",
                             "application/msexcel",
                             stream);
                }
            }
        }
    }
}