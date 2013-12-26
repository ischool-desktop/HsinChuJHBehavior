using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using HsinChu.DailyLife.TransferStudent.DAL;
using HsinChu.DailyLife.TransferStudent.Data.SemesterHistory;
using HsinChu.DailyLife.TransferStudent.Data;
using HsinChu.DailyLife.TransferStudent.Data.DailyBehavior;
using HsinChu.DailyLife.TransferStudent.Data.GroupActivity;
using HsinChu.DailyLife.TransferStudent;

namespace HsinChu.DailyLife
{
    public partial class TransferStudentForm : FISCA.Presentation.Controls.BaseForm
    {
        private List<IStep> _steps;
        private int _currentStepIndex;

        public string CurrentStudentID { get; set; }
        public TransferStudentData TransferStudentData { get; set; }

        public TransferStudentForm(string studentid)
        {
            CurrentStudentID = studentid;

            InitializeComponent();
            _steps = new List<IStep>();

            this.TransferStudentData = TransferStudentDAL.GetStudentTransferData(studentid);

            _steps.Add(new Step1(this));
            _steps.Add(new Step2(this));
            _steps.Add(new Step3(this));

            _currentStepIndex = 0;
            btnPrevious.Enabled = false;
            btnSave.Enabled = false;
            LoadCurrentStep();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_currentStepIndex == _steps.Count - 1) return;

            IStep step = _steps[_currentStepIndex];

            if (!step.Valid())
            {
                MessageBoxEx.Show(step.ErrorMessage, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _currentStepIndex++;

            LoadCurrentStep();

            if (_currentStepIndex > 0)
                btnPrevious.Enabled = true;

            if (_currentStepIndex == _steps.Count - 1)
            {
                btnNext.Enabled = false;
                btnSave.Enabled = true;
            }
            else
            {
                btnNext.Enabled = true;
                btnSave.Enabled = false;
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (_currentStepIndex == 0) return;

            IStep step = _steps[_currentStepIndex];

            if (!step.Valid())
            {
                MessageBoxEx.Show(step.ErrorMessage, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _currentStepIndex--;

            LoadCurrentStep();

            if (_currentStepIndex < _steps.Count - 1)
                btnNext.Enabled = true;

            if (_currentStepIndex == 0)
                btnPrevious.Enabled = false;
            else
                btnPrevious.Enabled = true;
        }

        private void LoadCurrentStep()
        {
            pnlContent.Controls.Clear();
            pnlContent.Controls.Add((UserControl)_steps[_currentStepIndex]);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (IStep step in _steps)
            {
                if (!step.Valid())
                {
                    MessageBoxEx.Show(step.ErrorMessage, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                step.OnChangeStep();
            }
            try
            {
                TransferStudentDAL.Save(this.TransferStudentData, CurrentStudentID);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxEx.Show("儲存過程中發生錯誤", "儲存失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
