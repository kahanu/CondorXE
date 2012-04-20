using Condor.Core.Interfaces;

namespace Condor.Core
{
    public class ProgressDialogWrapper
    {
        protected Dnp.Utils.ProgressDialog _progressDialog;
        public ScriptSettings script;

        public ProgressDialogWrapper(Dnp.Utils.ProgressDialog progressDialog)
        {
            _progressDialog = progressDialog;
            script = ScriptSettings.GetInstance();
        }

        // Use this method to iterate the processing of code
        // over the number of database tables.
        public void InitDialog()
        {
            _progressDialog.Show();
            _progressDialog.ProgressBar.Minimum = 0;
            _progressDialog.ProgressBar.Maximum = script.Database.DefaultDatabase.Tables.Count;
            _progressDialog.ProgressBar.Value = 0;
        }

        // Not in use at the moment.
        //public void InitDialog(bool useSelectedTables)
        //{
        //    _progressDialog.Show();
        //    _progressDialog.ProgressBar.Minimum = 0;
        //    //_progressDialog.ProgressBar.Maximum = length;
        //    _progressDialog.ProgressBar.Value = 0;
        //}

        // Use this method if you want to iterate over a number of methods
        // rather than a number of database tables being processed.
        public void InitDialog(int length)
        {
            _progressDialog.Show();
            _progressDialog.ProgressBar.Minimum = 0;
            _progressDialog.ProgressBar.Maximum = length;
            _progressDialog.ProgressBar.Value = 0;
        }



        // Display the progress message on the dialog.
        public void Display(string message)
        {
            _progressDialog.Text = message;
            _progressDialog.ProgressBar.Value++;
        }

        public void HideDialog()
        {
            _progressDialog.Hide();
        }
    }
}
