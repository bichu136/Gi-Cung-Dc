﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Do_An
{
    public partial class DoingThings : Form
    {
        TypeData typedata;
        ThingsToDoData ttdData;
        DailyData dData;
        EventData eData;
        ProjectData pData;
        ObjectiveData oData;
        TimeComponent timeComponent;
        ThingsToDoData cursor;
        DataTable Namedt;
        public DoingThings()
        {
            timeComponent = new TimeComponent() { Location= new Point(12,100)};
            typedata = new TypeData();
            ttdData = new ThingsToDoData();
            eData = new EventData();
            dData = new DailyData();
            pData = new ProjectData();
            oData = new ObjectiveData();
            InitializeComponent();
            
            TypeCbBox.DataSource = typedata.ReadDataTable();
            TypeCbBox.DisplayMember = "Name";
            TypeCbBox.ValueMember = "ID";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecordData RData = new RecordData();
            Record package = new Record((int)NameCbBox.SelectedValue, DateTime.Now.Date, getCurrent());
            RData.Insert(package);
            ttdData.UpdateWhenDoingSomethingByID((int)NameCbBox.SelectedValue);
            this.Close();
        }

        private int getCurrent()
        {
            if (flag = true)
            {
                return Convert.ToInt32(CurrentTxtBox);
            }
            return 0;
        }

        bool flag = false;
        private void NameCbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox x = (ComboBox)sender;
            if (x.Items.Count != 0)
            {
                switch ((long)TypeCbBox.SelectedValue)
                {
                    case (long)ThingsToDo.types.Objective:
                        HasPlanChkBox.Checked = pData.isCheck();
                        break;
                    case (long)ThingsToDo.types.Project:
                        UnitLbl.Text = oData.Unit(NameCbBox.SelectedValue.ToString());
                        break;

                }
                
            }

            
        }

        private void TypeCbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox input = (ComboBox)sender;
            try
            {
                if (input.SelectedValue == null)
                {
                    MessageBox.Show("Chưa có công việc loại này");
                }
                HideAll();
                switch ((long)input.SelectedValue)
                {

                    case (long)ThingsToDo.types.Daily:
                        cursor = dData;
                        break;
                    case (long)ThingsToDo.types.Event:
                        cursor = eData;
                        break;
                    case (long)ThingsToDo.types.Objective:
                        cursor = oData;
                        LoadOpjective();
                        break;
                    case (long)ThingsToDo.types.Project:
                        cursor = pData;
                        LoadProject();
                        break;
                }
                Namedt = cursor.ReadDataTableForDoing();
                NameCbBox.DataSource = Namedt;
                NameCbBox.DisplayMember = "Name";
                NameCbBox.ValueMember = "ID";            
            }
            catch(Exception ex)
            {
                HideAll();
            }
        }
        private void LockAll()
        {

        }
        private void LoadProject()
        {
            HasPlanChkBox.Show();
        }
        private void LoadOpjective()
        {
            
            UnitLbl.Show();
            CurrentTxtBox.Show();
        }
        private void HideAll()
        {
            HasPlanChkBox.Hide();
            UnitLbl.Hide();
            CurrentTxtBox.Hide();

        }

        private void DoneBtn_Click(object sender, EventArgs e)
        {
            cursor.UpdateByDoing(NameCbBox.SelectedValue.ToString());
            RecordData Rdata = new RecordData();
            Rdata.Insert(new Record() { TTD_ID=(int)NameCbBox.SelectedValue , Date=DateTime.Now.Date,Current = getCurrent() });
        }
    }
}