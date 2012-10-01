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

        /// <summary>
        /// This method iterates over the selected number of database tables.
        /// </summary>
        public void InitDialog()
        {
            _progressDialog.Show();
            _progressDialog.ProgressBar.Minimum = 0;
            _progressDialog.ProgressBar.Maximum = script.Database.DefaultDatabase.Tables.Count;
            //_progressDialog.ProgressBar.Maximum = script.Tables.Count;
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

        /// <summary>
        /// This method iterates based on the argument value.  
        /// This is used if you want to iterate a different number of times
        /// than the number of database tables.
        /// </summary>
        /// <param name="length"></param>
        public void InitDialog(int length)
        {
            _progressDialog.Show();
            _progressDialog.ProgressBar.Minimum = 0;
            _progressDialog.ProgressBar.Maximum = length;
            _progressDialog.ProgressBar.Value = 0;
        }



        /// <summary>
        /// Display the progress message on the dialog.
        /// </summary>
        /// <param name="message"></param>
        public void Display(string message)
        {
            _progressDialog.Text = message;
            _progressDialog.ProgressBar.Value++;
        }

        /// <summary>
        /// You can forcefully hide the dialog.
        /// </summary>
        public void HideDialog()
        {
            _progressDialog.Hide();
        }
    }
}
