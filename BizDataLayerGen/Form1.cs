﻿using BizDataLayerGen.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizDataLayerGen
{
    public partial class CodeGenratorForm : Form
    {

        enum enDataBaseUploadType { enDirect = 1, enUpload = 2}

        public CodeGenratorForm()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddDataBaseNames()
        {
            string[] databaseNames;
            databaseNames = clsGeneralData.GetAllApplications();

            cbDatabaseName.Items.Clear();

            cbDatabaseName.Items.AddRange(databaseNames);

        }

        private void CodeGenratorForm_Load(object sender, EventArgs e)
        {
            AddDataBaseNames();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Database Files (*.db;*.bak;*.mdf;*.ldf;*.sdf)|*.db;*.bak;*.mdf;*.ldf;*.sdf";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // مسار الملف المختار
                string sourceFilePath = openFileDialog1.FileName;

                // تحديد مسار الوجهة لنسخ الملف (يمكنك تغييره)
                //string destinationFilePath = Path.Combine(@"C:\Programation Level 2\BizDataLayerGen\DataBaseUpload\", Path.GetFileName(sourceFilePath));

                try
                {
                    // نسخ الملف إلى الوجهة
                    clsGeneralData.AddDataBaseToSQL(sourceFilePath, Path.GetFileNameWithoutExtension(sourceFilePath));

                    MessageBox.Show($"The Upload Is Successfully {Path.GetFileName(sourceFilePath)}", "Upload Done", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cbDatabaseName.Items.Add(Path.GetFileNameWithoutExtension(sourceFilePath));
                    clsGlobal.DataBaseName = Path.GetFileNameWithoutExtension(sourceFilePath);

                    int index = cbDatabaseName.FindStringExact(Path.GetFileNameWithoutExtension(sourceFilePath));
                    cbDatabaseName.SelectedIndex = index;

                }
                catch (Exception ex)
                {
                    // التعامل مع الأخطاء
                    MessageBox.Show("Have Error: " + ex.Message, "Upload Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbPathFilesToGenrate.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            if (cbDatabaseName.SelectedItem == null || tbPathFilesToGenrate.Text == "")
            {
                MessageBox.Show("Choose the Path and DataBaseName After You Load", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            

        }

        private void cbDatabaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsGlobal.DataBaseName = cbDatabaseName.Text;

        }
    }
}
