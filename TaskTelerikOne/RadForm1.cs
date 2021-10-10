using System;
using System.ComponentModel;
using System.Drawing;
using Telerik.WinControls.Export;
using Telerik.WinControls.UI;
using CellFormattingEventArgs = Telerik.WinControls.UI.CellFormattingEventArgs;

namespace TaskTelerikOne
{
    public partial class RadForm1 : Telerik.WinControls.UI.RadForm
    {
        BindingList<Issue> issues = new BindingList<Issue>();
        public RadForm1()
        {
            InitializeComponent();
            issues.Add(new Issue(1, "Application crashes while running", "New"));
            issues.Add(new Issue(2, "Incorrect title of the user's page", "In progress"));
            issues.Add(new Issue(3, "Changes are not stored to the database", "Completed"));
            this.radGridView1.DataSource = issues;

            this.radGridView1.BestFitColumns();
        }

        public static class StatusEnum
        {
            //Enum
            public const string NEW = "New";
            public const string IN_PROGRESS = "In progress";
            public const string COMPLETED = "Completed";
        }


        public class Issue : System.ComponentModel.INotifyPropertyChanged
        {
            int _id;
            string _description;
            string _status;

            public int Id
            {
                get
                {
                    return _id;
                }
                set
                {
                    if (this._id != value)
                    {
                        this._id = value;
                        OnPropertyChanged("Id");
                    }
                }
            }

            public string Description
            {
                get
                {
                    return _description;
                }
                set
                {
                    if (this._description != value)
                    {
                        this._description = value;
                        OnPropertyChanged("Description");
                    }
                }
            }

            public string Status
            {
                get
                {
                    return _status;
                }
                set
                {
                    if (this._status != value)
                    {
                        this._status = value;
                        OnPropertyChanged("Status");
                    }
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            public Issue(int id, string description, string status)
            {
                this.Id = id;
                this.Description = description;
                this.Status = status;
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

        }

        private void radGridView1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //Updating the colour of the background of the cell 
            if (e.CellElement.ColumnInfo.Name == "Status")
            {
                switch (e.CellElement.Text)
                {
                    case StatusEnum.NEW:
                        e.CellElement.BackColor = Color.Red;
                        break;
                    case StatusEnum.IN_PROGRESS:
                        e.CellElement.BackColor = Color.Yellow;
                        break;
                    case StatusEnum.COMPLETED:
                        e.CellElement.BackColor = Color.Green;
                        break;
                }
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            //Change of the status
            issues[0].Status = StatusEnum.IN_PROGRESS;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            //Adding a button that displays an issue when clicked
            issues.Add(new Issue(4, "Update fails", "New"));
        }
        private void radGridView1_CellValidating(object sender, CellValidatingEventArgs e)
        {
            //Making the user unable to write anything different from the key words New/Completed/In progress
            var column = e.Column as GridViewDataColumn;
            if (e.Row is GridViewDataRowInfo && column != null && column.FieldName == "Status")
            {
                Console.WriteLine((string)e.Value);
                var value = (string)e.Value;
                var row = (GridViewDataRowInfo)e.Row;
                if (string.IsNullOrEmpty(value))
                {
                    e.Cancel = true;
                    ((GridViewDataRowInfo)e.Row).ErrorText = "Validation error!";
                }
                else if (!value.Equals("New") && !value.Equals("Completed") && !value.Equals("In progress"))
                {
                    e.Cancel = true;
                    row.ErrorText = "Status must be New, Completed or In progress!";
                }
                else
                {
                    row.ErrorText = string.Empty;
                }
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            string exportFile = @"..\..\exportedData.xlsx";
       
            GridViewSpreadExport exporter = new GridViewSpreadExport(this.radGridView1);
            SpreadExportRenderer renderer = new SpreadExportRenderer();
            exporter.ExportVisualSettings = true;
            exporter.RunExport(exportFile, renderer);
            
        }
    }
}
