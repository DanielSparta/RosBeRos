using System.Diagnostics;
using System.IO;
namespace WinFormsApp5
{
    public partial class Form1 : Form
    {
        string loc;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            int i = 0;
            found.Visible = false;
            this.BeginInvoke((MethodInvoker)(() =>
            {
                name.Visible = false;
                name.Text = "תוצאות עבור";
                button1.Enabled = false;
                string stringi = search.Text;
                search.Enabled = false;
                searching.Visible = true;
                listView1.Items.Clear();
                for (int number = 1; number <= 99999; number++)
                {
                    if (stringi.StartsWith("'") || stringi.StartsWith('"') || stringi == "")
                    {
                        listView1.Items.Add("מה אתה לעזאזל מנסה לעשות");
                        break;
                    }
                    Process powershell = new Process();
                    powershell.StartInfo.FileName = "powershell.exe";
                    powershell.StartInfo.CreateNoWindow = true;
                    powershell.StartInfo.UseShellExecute = false;
                    powershell.StartInfo.RedirectStandardOutput = true;

                    powershell.StartInfo.Arguments = @"
    $testing = Test-path "+loc+@"\'" + search.Text + @"*';
    if($testing)
    {
    Get-Content "+loc+@"\'" + search.Text + "*' | Select -First " + number + @"| Select -Last 1
    }
else
{
Get-Content "+loc+@"\'notfound.txt'
}
    ";
                    powershell.Start();
                    string output = powershell.StandardOutput.ReadToEnd();
                    if (output.StartsWith("end") || output.StartsWith("notfound"))
                    {
                        if(output.StartsWith("notfound"))
                            listView1.Items.Add("הפריט אינו נמצא!");
                        break;
                    }
                    powershell.WaitForExit();
                    if (number != 1)
                    {
                        listView1.Items.Add(output);
                        ++i;
                    }
                    else
                    {
                        name.Text += " "+output;
                        name.Visible = true;
                    }
                    output = "";
                }
                button1.Enabled = true;
                search.Enabled = true;
                searching.Visible = false;
                found.Text = "נמצאו " + i.ToString() + " תוצאות";
                found.Visible = true;
            }));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loc = location.Text;
        }
    }
}
