﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace Do_An
{
    class ThingsToDoData : SQLiteManage
    {
        public ThingsToDoData():base()
        {
            cmd.Parameters.Add("$ID",DbType.String);
            cmd.Parameters.Add("$Name",DbType.String);
            cmd.Parameters.Add("$Status",DbType.Int32);
            cmd.Parameters.Add("$lastupdate",DbType.String);
        }
        public override void Close()
        {
            base.Close();
        }

        public override long Insert(object values)
        {
            cnn.Open();
            ThingsToDo Input = (ThingsToDo)values;
            cmd.Parameters["$Name"].Value = Input.name;
            cmd.Parameters["$Status"].Value =  Input.status;
            cmd.Parameters["$lastupdate"].Value = Input.lastUpDate.ToString("yyyy'-'MM'-'dd HH:mm:ss");
            cmd.ExecuteNonQuery();
            long x = GetLastInsertID();
            cnn.Close();
            return x;
            //Program.manager.InsertTo();
        }
        public virtual void UpdateStatusByID(string ID, long statuses)
        {
            cnn.Open();
            cmd.CommandText = "update ThingToDo set Status = $Status where ID = $ID;";
            cmd.Parameters["$Status"].Value = statuses;
            cmd.Parameters["$ID"].Value = ID;
            cmd.ExecuteNonQuery();
            cnn.Close();
        }
        public void UpdateDropStatus()
        {
            cnn.Open();
            cmd.CommandText = "Update ThingsToDo set status = $status where date(lastupdate,'+1 month') < date('now')";
            cmd.Parameters["$Status"].Value =  ThingsToDo.statuses.Dropped;
            cnn.Close();
        }
        public virtual void UpdateByDoing(string ID, long statuses)
        {
            cmd.CommandText = "update ThingToDo set Status = $Status, lastupdate = datetime('now') where ID = $ID";
            cmd.Parameters["$ID"].Value = ID;
            cmd.Parameters["$Status"].Value = statuses;
            cnn.Open();
            long x = cmd.ExecuteNonQuery();
            cnn.Close();
        }

        public virtual DataTable ReadDataTableForDoing()
        {
            return null;
        }
        public long GetLastInsertID()
        {
            long x =0;
            cmd.CommandText = "select last_insert_rowid() from ThingToDo";
            x =(long)cmd.ExecuteScalar();
            return x;
        }
    }

}