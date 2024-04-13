using System.Windows.Forms;

namespace SnapNote
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        bool isTextMode = false;
        string cur_RTB_text = "";
        string commandClause = "";
        string fileSavePath = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            statusText.Text = "...";
            cur_RTB_text = richTextBox1.Text;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isTextMode)
            {
                richTextBox1.Text = cur_RTB_text;
            }
            else
            {
                cur_RTB_text = richTextBox1.Text;
            }
        }
        // Declare a flag to indicate if the 'F' key has been pressed
        private bool isFKeyPressed = false;

        private async void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Keys modifiers = Control.ModifierKeys;

            // Check if Ctrl key is pressed
            if ((modifiers & Keys.Control) == Keys.Control)
            {
                // Ctrl key is pressed
            }

            // Check if Alt key is pressed
            if ((modifiers & Keys.Alt) == Keys.Alt)
            {
                // Alt key is pressed
            }

            // Check if Shift key is pressed
            if ((modifiers & Keys.Shift) == Keys.Shift)
            {
                // Shift key is pressed
            }
            MessageBox.Show($"{char.ToUpper(e.KeyChar)}");
            if (isTextMode)
            {
                // Handle text mode key presses
                if (char.ToUpper(e.KeyChar) == '\u001b') // ESC = '\u001b'
                {
                    isTextMode = false;
                    await statusUpdate("Text mode deactivated.");
                }
            }
            else
            {
                await statusUpdate(e.KeyChar.ToString(), 0.25);

                // Check for 'I' or 'T' key presses to activate text mode
                if (char.ToUpper(e.KeyChar) == 'I' || char.ToUpper(e.KeyChar) == 'T')
                {
                    isTextMode = true;
                    await statusUpdate("Text mode activated.");
                }
                
                if (isFKeyPressed)
                {
                    if (char.ToUpper(e.KeyChar) == 'Q') //force exit
                    {
                        Environment.Exit(0);
                    }
                    if (char.ToUpper(e.KeyChar) == 'O')
                    {
                        await statusUpdate("Opening files...");
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.ShowDialog();

                        if (File.Exists(openFileDialog.FileName))
                        {
                            richTextBox1.Text = File.ReadAllText(openFileDialog.FileName);
                            cur_RTB_text = File.ReadAllText(openFileDialog.FileName);
                            fileSavePath = openFileDialog.FileName;
                            await statusUpdate(openFileDialog.FileName);
                        }
                        isFKeyPressed = false;
                    }
                    else if (char.ToUpper(e.KeyChar) == 'S')
                    {
                        await statusUpdate("Saving files...");
                        SaveFileDialog saveFileDialog = new SaveFileDialog();

                        if (fileSavePath == "")
                        {
                            saveFileDialog.ShowDialog();
                        }

                        if (File.Exists(saveFileDialog.FileName))
                        {
                            fileSavePath = saveFileDialog.FileName;
                        }

                        File.WriteAllText(fileSavePath, richTextBox1.Text);
                        await statusUpdate(saveFileDialog.FileName);
                        isFKeyPressed = false;
                    }
                    else
                    {
                        isFKeyPressed = false;
                    }
                }

                if (char.ToUpper(e.KeyChar) == 'F')
                {
                    isFKeyPressed = true;
                }
            }
        }


        async Task statusUpdate(string text, double waitTime = 1)
        {
            statusText.Text = text;
            Task.Delay(Convert.ToInt32(waitTime * 1000)).Wait();
            statusText.Text = "";
        }
    }
}
