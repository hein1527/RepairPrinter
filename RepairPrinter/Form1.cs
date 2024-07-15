using System.Diagnostics;

namespace RepairPrinter
{
    public partial class Form1 : Form
    {
        const string batName = @"repairPrinter.bat";
        string runningPath;
        public Form1()
        {
            InitializeComponent();
            setTranslations();
            this.runningPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string wholePath = Path.Combine(runningPath, batName);
            if (!File.Exists(wholePath))
            {
                var res = MessageBox.Show(Resources.warningIncompleteInstallation, Resources.warningCommon, MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    repairInstallation();
                }
                return;
            }
            RunBatFileAsAdmin(wholePath);
        }
        private void RunBatFileAsAdmin(string filePath)
        {
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.FileName = filePath;
                processInfo.Verb = "runas";
                processInfo.UseShellExecute = true;

                Process process = new Process();
                process.StartInfo = processInfo;
                process.Start();
                process.WaitForExit();
                MessageBox.Show(Resources.successMessage, Resources.successCommon);

            }
            catch (Exception)
            {
                MessageBox.Show(Resources.warningError, Resources.warningCommon);
            }
        }

        private void repairInstallation()
        {
            string batContent = $"""
                @echo off
                echo {Resources.batStopping}
                net stop spooler
                echo {Resources.batDeleting}
                del /Q /F "%systemroot%\System32\spool\PRINTERS\*.*"
                echo {Resources.batStarting}
                net start spooler
                echo ´{Resources.batFinished}
                """;
            File.WriteAllText(Path.Combine(runningPath, batName), batContent);
            MessageBox.Show(Resources.successCreating, Resources.successCommon);
        }
        private void setTranslations()
        {
            Text = Resources.guiTitle;
            button1.Text = Resources.guiCleanBtn;
            label1.Text = Resources.guiInfoLbl;
        }
    }
}
