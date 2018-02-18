using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;


namespace plcdemo
{
    public partial class Form1 : Form
    {
        IntPtr h = IntPtr.Zero;
        public Form1()
        {
            InitializeComponent();
            
        }

        [DllImport("plcommpro.dll", EntryPoint = "Connect")]
        public static extern IntPtr Connect(string Parameters);
        [DllImport("plcommpro.dll", EntryPoint ="PullLastError")]
        public static extern int PullLastError();
        [DllImport("plcommpro.dll", EntryPoint = "GetPullSDKVersion")]
        public static extern IntPtr GetPullSDKVersion(string Parameters, int size);


//4.1 调用连接设备函数
        private void btnconnect_Click_1(object sender, EventArgs e)
        {
            string str = "";
            int ret = 0;        //用于接收错误ID号
            Cursor = Cursors.WaitCursor;

            if (this.radbtntcp.Checked)         //获取连接参数
                str = this.txttcp.Text;
            else if (this.radbtn485.Checked)
                str = this.txt485.Text;
            if (str == "")
                MessageBox.Show("Please select the connect mode:");

            if (IntPtr.Zero == h)
            {
                h = Connect(str);
                Cursor = Cursors.Default;
                if(h != IntPtr.Zero)
                {
                    MessageBox.Show("Connect device successful!");
                    btndisconncet.Enabled = true;
                    btnconnect.Enabled = false;

                    //device param tab页
                    btnselparam.Enabled = true;        //连接成功之后，选择参数项可用才有意义
                    btncleparam.Enabled = true;
                    this.btngetparam.Enabled = false;
                    this.btnsetparam.Enabled = false;
                    cmbparam.Enabled = false;
                    txtchgparam.Enabled = false;
                    btnchgparam.Enabled = false;

                    //Control device tab页
                    btndevcontrol.Enabled = true;
                    cmboperid.Enabled = true;
                    cmbdoorid.Enabled = false;
                    cmboutadd.Enabled = false;
                    txtdoorstate.Enabled = false;

                    //device data Tabe项
                    cmbdevtable.Enabled = true;
                    btngetdat.Enabled = false;
                    btnsetdat.Enabled = false;
                    btndeldata.Enabled = false;
                    btndatcount.Enabled = false;
                    btnfil.Enabled = false;
                    btnclefil.Enabled = false;
                    this.txtdevdata.Text = "\r\n \r\n Please select tablename above the frame!";

                    //RTLog Tab项
                    btnrtlogstart.Enabled = true;
                    btnrtlogstop.Enabled = true;
                }
                else
                {
                    ret = PullLastError();
                    MessageBox.Show("Connect device Failed! The error id is: " + ret);
                    btnselparam.Enabled = false;         //断开连接之后，选择参数项不可用

                }

               /* if (h == IntPtr.Zero)
                {
                    MessageBox.Show("Connect device Failed!");
                    PullLastError();
                    btnselparam.Enabled = false;         //断开连接之后，选择参数项不可用
                    return;
                }
                else
                {
                    MessageBox.Show("Connect device successful!");
                    btndisconncet.Enabled = true;
                    btnconnect.Enabled = false;
                    
                    //device param tab页
                    btnselparam.Enabled = true;        //连接成功之后，选择参数项可用才有意义
                    btncleparam.Enabled = true;
                    this.btngetparam.Enabled = false;
                    this.btnsetparam.Enabled = false;
                    cmbparam.Enabled = false;
                    txtchgparam.Enabled = false;
                    btnchgparam.Enabled = false;

                    //Control device tab页
                    btndevcontrol.Enabled = true;
                    cmboperid.Enabled = true;
                    cmbdoorid.Enabled = false;
                    cmboutadd.Enabled = false;
                    txtdoorstate.Enabled = false;

                    //device data Tabe项
                    cmbdevtable.Enabled = true;
                    btngetdat.Enabled = false;
                    btnsetdat.Enabled = false;
                    btndeldata.Enabled = false;
                    btndatcount.Enabled = false;
                    btnfil.Enabled = false;
                    btnclefil.Enabled = false;
                    this.txtdevdata.Text = "\r\n \r\n Please select tablename above the frame!";

                    //RTLog Tab项
                    btnrtlogstart.Enabled = true;
                    btnrtlogstop.Enabled = true;
                }*/
            }
        }


        [DllImport("plcommpro.dll", EntryPoint = "Disconnect")]
        public static extern void Disconnect(IntPtr h);


//4.2 调用断开连接函数
        private void btndisconncet_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero != h)
            {
                Disconnect(h);
                h = IntPtr.Zero;

                btndisconncet.Enabled = false;
                btnconnect.Enabled = true;

                //device param  tab页
                btncleparam.Enabled = false;
                btnselparam.Enabled = false;
                btngetparam.Enabled = false;
                btnsetparam.Enabled = false;
                cmbparam.Enabled = false;
                txtchgparam.Enabled = false;
                btnchgparam.Enabled = false;
                lsvselparam.Items.Clear();

                //Control device tab页
                btndevcontrol.Enabled = false;
                cmboperid.Enabled = false;
                cmbdoorid.Enabled = false;
                cmboutadd.Enabled = false;
                txtdoorstate.Enabled = false;

                //device data Tabe项
                cmbdevtable.Enabled = false;
                btngetdat.Enabled = false;
                btnsetdat.Enabled = false;
                btndeldata.Enabled = false;
                btndatcount.Enabled = false;
                btnfil.Enabled = false;
                btnclefil.Enabled = false;

                //RTLog Tab项
                btnrtlogstart.Enabled = false;
                btnrtlogstop.Enabled = false;
            }
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.btndisconncet.Enabled = false;
            this.cmboperid.SelectedIndex = 1;       //设置设备控制页面操作id的默认值
            //this.cmbdevtable.SelectedIndex = 0;
            this.radbtntcp.Checked = true;

            //device param  tab页
            this.btngetparam.Enabled = false;   //获取设备参数默认为不可用状态，选择要哪个参数后，该状态就自动变为可用
            btnsetparam.Enabled = false;
            btnselparam.Enabled = false;         //断开连接之后，选择参数项不可用
            btncleparam.Enabled = false;
            cmbparam.Enabled = false;
            txtchgparam.Enabled = false;
            btnchgparam.Enabled = false;

            //Control device tab页
            btndevcontrol.Enabled = false;
            cmboperid.Enabled = false;
            cmbdoorid.Enabled = false;
            cmboutadd.Enabled = false;
            txtdoorstate.Enabled = false;

            //device data Tabe项
            cmbdevtable.Enabled = false;
            btngetdat.Enabled = false;
            btnsetdat.Enabled = false;
            btndeldata.Enabled = false;
            btndatcount.Enabled = false;
            btnfil.Enabled = false;
            btnclefil.Enabled = false;

            //RTLog Tab项
            btnrtlogstart.Enabled = false;
            btnrtlogstop.Enabled = false;

            this.cmbseardev.Enabled = false;
            this.txtnewip.Enabled = false;
            this.txtdevpwd.Enabled = false;
            this.btnmodip.Enabled = false;
        }


        //选择要读取值的参数
        private void btnselparam_Click(object sender, EventArgs e)
        {
            string prestr = "";
            string selstr = "";
            bool tag = true;
            if (this.lsvpreparam.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select the params!");
                return;
            }
            else
            {
                for (int i = 0; i < this.lsvpreparam.SelectedItems.Count; i++)
                {
                    prestr = this.lsvpreparam.SelectedItems[i].Text;
                    for (int j = 0; j < this.lsvselparam.Items.Count; j++)
                    {
                        selstr = this.lsvselparam.Items[j].Text;
                        if (string.Equals(prestr, selstr))        //判断选择的参数与列表中的参数没有重复项
                        {
                            tag = false;
                        }
                    }
                    if (tag)
                    {
                        this.lsvselparam.Items.Add(prestr);
                        this.lsvselparam.Items[i].SubItems.Add("");
                    }
                    tag = true;
                }
                this.btngetparam.Enabled = true;
                this.btnsetparam.Enabled = true;
            }
        }


        //将选择的参数清空
        private void btncleparam_Click(object sender, EventArgs e)
        {
            if (this.lsvselparam.Items.Count == 0)
            {
                MessageBox.Show("The listview is no param!");
                return;
            }
            else
            {
                this.lsvselparam.Items.Clear();
                this.cmbparam.Items.Clear();
            }
            btngetparam.Enabled = false;
            btnsetparam.Enabled = false;
            cmbparam.Enabled = false;
            txtchgparam.Enabled = false;
            btnchgparam.Enabled = false;
        }


        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceParam")]
        public static extern int GetDeviceParam(IntPtr h, ref byte buffer,int buffersize, string itemvalues);

//4.4 调用读取设备参数函数
        private void btngetparam_Click(object sender, EventArgs e)
        {
            if (IntPtr.Zero != h)
            {
                int ret = 0, i = 0;
                int BUFFERSIZE = 10 * 1024 * 1024;
                byte[] buffer = new byte[BUFFERSIZE];
                int lv_sel_count = lsvselparam.Items.Count;
                string str = null;
                string tmp = null;
                string[] value = null;
                do
                {
                    str = str + this.lsvselparam.Items[i].Text;
                    if (i < lv_sel_count - 1)
                    {
                        str = str + ',';
                    }
                    i++;
                } while (i < lv_sel_count);
                //MessageBox.Show(str);
                ret = GetDeviceParam(h, ref buffer[0], BUFFERSIZE, str);       //获取设备参数值
                if (ret >= 0)
                {
                    tmp = Encoding.Default.GetString(buffer);
                    //MessageBox.Show(tmp);
                    value = tmp.Split(',');
                    for (int k = 0; k < lv_sel_count; k++)
                    {
                        //string sub_buf = value[k].ToString();
                        //string[] sub_str = sub_buf.Split('=');
                        string[] sub_str = value[k].Split('=');
                        this.lsvselparam.Items[k].SubItems[1].Text = sub_str[1].ToString();    //将读取到的值赋给对应参数的列表项

                    }
                    //对获取参数修改相应的值
                    cmbparam.Enabled = true;
                    txtchgparam.Enabled = true;
                    btnchgparam.Enabled = true;
                }
                else
                {
                    MessageBox.Show("GetDeviceParam function failed");
                    PullLastError();
                }
            }
            else
            {
                MessageBox.Show("Connect device failed!");
                return;
            }
        }



 //在向设备做SetDeviceParam操作之前先要把参数值修改了
        private void btnchgparam_Click(object sender, EventArgs e)
        {
            for (int selcount = 0; selcount < lsvselparam.Items.Count; selcount++)
            {
                if (string.Equals(lsvselparam.Items[selcount].Text, cmbparam.SelectedItem))
                {
                    //MessageBox.Show(txtchgparam.Text);
                    this.lsvselparam.Items[selcount].SubItems[1].Text = txtchgparam.Text;     //将文本框修改的的赋给以选中对应参数中显示出来
                }
            }
        }
        

        //下拉列表显示哪些参数项
        private void cmbparam_DropDown(object sender, EventArgs e)
        {
            this.cmbparam.Items.Clear();
            for (int selcount = 0; selcount < lsvselparam.Items.Count; selcount++)
            {
                string selparam = lsvselparam.Items[selcount].Text;
                if (!(string.Equals(selparam, "LockCount") || string.Equals(selparam, "ReaderCount") || string.Equals(selparam, "AuxInCount") || string.Equals(selparam, "AuxOutCount") || string.Equals(selparam, "Door1CancelKeepOpenDay") || string.Equals(selparam, "Door2CancelKeepOpenDay") || string.Equals(selparam, "Door3CancelKeepOpenDay") || string.Equals(selparam, "Door4CancelKeepOpenDay")))
                {
                    this.cmbparam.Items.Add(lsvselparam.Items[selcount].Text);
                }
            } 
        }


        [DllImport("plcommpro.dll", EntryPoint = "SetDeviceParam")]
        public static extern int SetDeviceParam(IntPtr h,string itemvalues);

 
//4.3 调用设置设备参数函数
        private void btnsetparam_Click(object sender, EventArgs e)
        {
            int ret = 0, i = 0, tt = 0;
            string str = "";
            DateTime dt;
            do             //将设置的参数连接成PullSDK规定接受的字符串
            {
                str = str + this.lsvselparam.Items[i].Text + "=" + this.lsvselparam.Items[i].SubItems[1].Text;
                if (lsvselparam.Items[i].Text == "DateTime")    //设置同步时间的值
                {
                    dt = DateTime.Now;
                    MessageBox.Show("Now datetime is:" + dt);
                    tt = ((dt.Year - 2000) * 12 * 31 + (dt.Month - 1) * 31 + (dt.Day - 1)) * (24 * 60 * 60) + dt.Hour * 60 * 60 + dt.Minute * 60 + dt.Second;
                    MessageBox.Show("Conver now datetime is:" + tt);
                    this.lsvselparam.Items[i].SubItems[1].Text = tt.ToString();
                }
                if (i < lsvselparam.Items.Count - 1)
                {
                    str = str + ',';
                }
                i++;
            } while (i < lsvselparam.Items.Count);
            //MessageBox.Show(str);
                ret = SetDeviceParam(h, str);    //将已选择的参数值设置到设备当中
            if (ret >= 0)
                MessageBox.Show("SetDeviceParam successfu!");
            else
                PullLastError();
        }



        [DllImport("plcommpro.dll", EntryPoint = "ControlDevice")]
        public static extern int ControlDevice(IntPtr h, int operationid, int param1, int param2, int param3, int param4, string options);

//控制设备动作函数
        private void btndevcontrol_Click(object sender, EventArgs e)
        {
            int ret = 0;
            int operid = Convert.ToInt32(this.cmboperid.SelectedItem.ToString());
            int doorid = 0;
            int outputadr = 0;
            int doorstate = 0;

            if (operid == 1)
            {
                doorid = Convert.ToInt32(this.cmbdoorid.SelectedItem.ToString());
                outputadr = Convert.ToInt32(this.cmboutadd.SelectedItem.ToString());
                doorstate = Convert.ToInt32(this.txtdoorstate.Text);
            }
            if (IntPtr.Zero != h)
                ret = ControlDevice(h, operid, doorid, outputadr, doorstate, 0, "");     //引用PullSDK控制设备操作函数
            else
            {
                MessageBox.Show("Connect device failed!");
                PullLastError();
                return;
            }
            if (ret >= 0)
            {
                MessageBox.Show("The operation is successfully!");
                return;
            }
        }

        private void comoperid_SelectedIndexChanged(object sender, EventArgs e)
        {
            int operid = Convert.ToInt32(this.cmboperid.SelectedItem.ToString());
            if (operid == 1)
            {
                this.cmbdoorid.Enabled = true;
                this.cmboutadd.Enabled = true;
                this.txtdoorstate.Enabled = true;
            }
            else if (operid == 2)
            {
                //this.comdoorid.Items.Clear();
                //this.comdoorid.Text = "";
                //this.comoutadd.Items.Clear();
                //this.comoutadd.Text = "";
                this.cmbdoorid.Enabled = false;
                this.cmboutadd.Enabled = false;
                this.txtdoorstate.Enabled = false;
            }
        }

        private void comoperid_DropDown(object sender, EventArgs e)
        {
            this.cmbdoorid.Items.Clear();
            this.cmboutadd.Items.Clear();
            this.cmbdoorid.Items.Add("1");
            this.cmbdoorid.Items.Add("2");
            this.cmbdoorid.Items.Add("3");
            this.cmbdoorid.Items.Add("4");
            this.cmbdoorid.Items.Add("5");
            this.cmbdoorid.Items.Add("6");
            this.cmboutadd.Items.Add("1");
            this.cmboutadd.Items.Add("2");
            this.cmbdoorid.SelectedIndex = 0;
            this.cmboutadd.SelectedIndex = 0;
        }



        public string devtablename = "";
        private void cmbdevtable_DropDownClosed(object sender, EventArgs e)
        {
            //string devtablename = "";
            if (devtablename == "")
            {
                devtablename = this.cmbdevtable.Items[0].ToString();
            }
            else
            {
                devtablename = this.cmbdevtable.SelectedItem.ToString();
            }
            if (string.Equals(devtablename, "user"))
            {
                this.txtdevdata.Text = "CardNo\tPin\tPassword\tGroup\tStartTime\tEndTime";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("CardNo");
                this.cmbfil.Items.Add("Pin");
                this.cmbfil.Items.Add("Password");
                this.cmbfil.Items.Add("Group");
                this.cmbfil.Items.Add("StartTime");
                this.cmbfil.Items.Add("EndTime");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "userauthorize"))
            {
                this.txtdevdata.Text = "Pin\tAuthorizeTimezoneId\tAuthorizeDoorId";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Pin");
                this.cmbfil.Items.Add("AuthorizeTimezoneId");
                this.cmbfil.Items.Add("AuthorizeDoorId");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "holiday"))
            {
                this.txtdevdata.Text = "Holiday\tHolidayType\tLoop";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Holiday");
                this.cmbfil.Items.Add("HolidayType");
                this.cmbfil.Items.Add("Loop");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "20110101";
            }
            else if (string.Equals(devtablename, "timezone"))
            {
                this.txtdevdata.Text = "TimezoneId\tSunTime1\tSunTime2\tSunTime3\tMonTime1\tMonTime2\tMonTime3\tTueTime1\tTueTime2\tTueTime3\tWedTime1\tWedTime2\tWedTime3\tThuTime1\tThuTime2\tThuTime3\tFriTime1\tFriTime2\tFriTime3\tSatTime1\tSatTime2\tSatTime3\tHol1Time1\tHol1Time2\tHol1Time3\tHol2Time1\tHol2Time2\tHol2Time3\tHol3Time1\tHol3Time2\tHol3Time3";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("TimezoneId");
                this.cmbfil.Items.Add("SunTime1");
                this.cmbfil.Items.Add("SunTime2");
                this.cmbfil.Items.Add("SunTime3");
                this.cmbfil.Items.Add("MonTime1");
                this.cmbfil.Items.Add("MonTime2");
                this.cmbfil.Items.Add("MonTime3");
                this.cmbfil.Items.Add("TueTime1");
                this.cmbfil.Items.Add("TueTime2");
                this.cmbfil.Items.Add("TueTime3");
                this.cmbfil.Items.Add("WedTime1");
                this.cmbfil.Items.Add("WedTime2");
                this.cmbfil.Items.Add("WedTime3");
                this.cmbfil.Items.Add("ThuTime1");
                this.cmbfil.Items.Add("ThuTime2");
                this.cmbfil.Items.Add("ThuTime3");
                this.cmbfil.Items.Add("FriTime1");
                this.cmbfil.Items.Add("FriTime2");
                this.cmbfil.Items.Add("FriTime3");
                this.cmbfil.Items.Add("SatTime1");
                this.cmbfil.Items.Add("SatTime2");
                this.cmbfil.Items.Add("SatTime3");
                this.cmbfil.Items.Add("Hol1Time1");
                this.cmbfil.Items.Add("Hol1Time2");
                this.cmbfil.Items.Add("Hol1Time3");
                this.cmbfil.Items.Add("Hol2Time1");
                this.cmbfil.Items.Add("Hol2Time2");
                this.cmbfil.Items.Add("Hol2Time3");
                this.cmbfil.Items.Add("Hol3Time1");
                this.cmbfil.Items.Add("Hol3Time2");
                this.cmbfil.Items.Add("Hol3Time3");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "transaction"))
            {
                this.txtdevdata.Text = "Cardno\tPin\tVerified\tDoorID\tEventType\tInOutState\tTime_second";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Cardno");
                this.cmbfil.Items.Add("Pin");
                this.cmbfil.Items.Add("Verified");
                this.cmbfil.Items.Add("DoorID");
                this.cmbfil.Items.Add("EventType");
                this.cmbfil.Items.Add("InOutState");
                this.cmbfil.Items.Add("Time_second");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "firstcard"))
            {
                this.txtdevdata.Text = "Pin\tDoorID\tTimezoneID";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Pin");
                this.cmbfil.Items.Add("DoorID");
                this.cmbfil.Items.Add("TimezoneID");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "multimcard"))
            {
                this.txtdevdata.Text = "Index\tDoorId\tGroup1\tGroup2\tGroup3\tGroup4\tGroup5";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Index");
                this.cmbfil.Items.Add("DoorId");
                this.cmbfil.Items.Add("Group1");
                this.cmbfil.Items.Add("Group2");
                this.cmbfil.Items.Add("Group3");
                this.cmbfil.Items.Add("Group4");
                this.cmbfil.Items.Add("Group5");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "inoutfun"))
            {
                this.txtdevdata.Text = "Index\tEventType\tInAddr\tOutType\tOutAddr\tOutTime\tReserved";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Index");
                this.cmbfil.Items.Add("EventType");
                this.cmbfil.Items.Add("InAddr");
                this.cmbfil.Items.Add("OutType");
                this.cmbfil.Items.Add("OutAddr");
                this.cmbfil.Items.Add("OutTime");
                this.cmbfil.Items.Add("Reserved");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }
            else if (string.Equals(devtablename, "templatev10"))
            {
                this.txtdevdata.Text = "Size\tUID\tPIN\tFingerID\tValid\tTemplate\tResverd\tEndTag";
                this.cmbfil.Items.Clear();
                this.cmbfil.Items.Add("Size");
                this.cmbfil.Items.Add("UID");
                this.cmbfil.Items.Add("PIN");
                this.cmbfil.Items.Add("FingerID");
                this.cmbfil.Items.Add("Valid");
                this.cmbfil.Items.Add("Template");
                this.cmbfil.Items.Add("Resverd");
                this.cmbfil.Items.Add("EndTag");
                this.cmbfil.SelectedIndex = 0;
                this.txtfilval.Text = "1";
            }

            //device data Tabe项
            cmbdevtable.Enabled = true;
            btngetdat.Enabled = true;
            btnsetdat.Enabled = true;
            btndeldata.Enabled = true;
            btndatcount.Enabled = true;
            btnfil.Enabled = true;
            btnclefil.Enabled = true;
        }

        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceDataCount")]
        public static extern int GetDeviceDataCount(IntPtr h,string tablename,string filter,string options);

// 4.8  调用GetDeviceDataCount函数
        private void btndatcount_Click(object sender, EventArgs e)
        {
            int ret =0;
            string options = "";
            string tablenamestr = this.cmbdevtable.SelectedItem.ToString();
            string[] count = new string[20];
            if(IntPtr.Zero != h)
            {
                ret = GetDeviceDataCount(h, devtablename, devdatfilter, options);
                if (ret >= 0)
                {
                    //MessageBox.Show("ret=" + ret);
                    this.txtgetdata.Text = "\r\n \r\nThe " +  tablenamestr + " table is : " + ret + "\r\n" ;
                }                   
            }
            else
            {
                MessageBox.Show("Connect device failed!");
                return;
            }
        }

        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceData")]
        public static extern int GetDeviceData(IntPtr h, ref byte buffer, int buffersize, string tablename, string filename, string filter, string options);

        string strcount = "";      //定义用于记录总数的字符串
 //4.7 GetDeviceData调用获取设备数据
        private void btngetdat_Click(object sender, EventArgs e)
        {
            int ret = 0;
            string str = this.txtdevdata.Text;
            int BUFFERSIZE = 10 * 1024 * 1024;
            byte[] buffer = new byte[BUFFERSIZE];
            string options = "";
            bool opt = this.chkdatopt.Checked;
            if(str == "")
                this.txtdevdata.Text = "\r\n \r\n Please select tablename above the frame!";
            if(opt)
                options = "NewRecord";
            if (IntPtr.Zero != h)
            {
                ret = GetDeviceData(h, ref buffer[0], BUFFERSIZE, devtablename, str, devdatfilter, options);
            }
            else
            {
                MessageBox.Show("Connect device failed!");
                return;
            }
            //MessageBox.Show(str);
            MessageBox.Show("ret=" + ret);
            if (ret >= 0)
            {
                this.txtgetdata.Text = Encoding.Default.GetString(buffer);
                strcount = Encoding.Default.GetString(buffer);
            }
            this.txtdevdata.Clear();
        }

        private void device_data_Click(object sender, EventArgs e)
        {

        }

        public string devdatfilter = "";
        private void btnfil_Click(object sender, EventArgs e)
        {
            if(devdatfilter == "")
            {
                this.txtfil.Clear();
                devdatfilter = this.cmbfil.SelectedItem.ToString() + "=" + this.txtfilval.Text;
                this.txtfil.Text = devdatfilter;
            }
            else
            {
                this.txtfil.Clear();
                devdatfilter = devdatfilter + "," + this.cmbfil.SelectedItem.ToString() + "=" + this.txtfilval.Text;
                this.txtfil.Text = devdatfilter;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            this.txtfil.Clear();
            devdatfilter = "";
        }



        [DllImport("plcommpro.dll", EntryPoint = "SetDeviceData")]
        public static extern int SetDeviceData(IntPtr h, string tablename, string data, string options);

//4.6 调用SetDeviceData函数
        private void btnsetdat_Click(object sender, EventArgs e)
        {
            int ret = 0;
            string data = this.txtdevdata.Text;
            string options = "";
            if (data == "")
            {
                this.txtdevdata.Text = "\r\n \r\n Please input set data in here!";
                this.txtgetdata.Clear();
            }
            else
            {
                if (IntPtr.Zero != h)
                {
                    ret = SetDeviceData(h, devtablename, data, options);
                    if (ret >= 0)
                    {
                        MessageBox.Show("SetDeviceData operation is successful!");
                        return;
                    }
                    else
                        MessageBox.Show("SetDeviceData operation is failed!");
                }
                else
                {
                    MessageBox.Show("Connect device failed!");
                    return;
                }
            }
        }




        [DllImport("plcommpro.dll", EntryPoint = "GetRTLog")]
        public static extern int GetRTLog(IntPtr h, ref byte buffer, int buffersize);

        //public Timer t = new Timer(10000);
        private void button11_Click(object sender, EventArgs e)
        {
            trglog.Enabled = true;
            MessageBox.Show("Start to RTLog");
        }

 //4.10 调用GetRTLog接口函数
        private void timer1_Tick(object sender, EventArgs e)        //使用定时器出发实时监控
        {
            int ret = 0, i = 0, buffersize = 256;
            string str = "";
            string[] tmp = null;
            byte[] buffer = new byte[256];
            i = this.lsvrtlog.Items.Count;          //当前列表个数赋给 i

            if (IntPtr.Zero != h)
            {

               ret = GetRTLog(h, ref buffer[0], buffersize);
               if (ret >= 0)
               {
                    str = Encoding.Default.GetString(buffer);
                    tmp = str.Split(',');
                    //MessageBox.Show(tmp[0]);
                    this.lsvrtlog.Items.Add(tmp[0]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[1]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[2]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[3]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[4]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[5]);
                    this.lsvrtlog.Items[i].SubItems.Add(tmp[6]);
                }
                i++;
            }
            else
            {
                MessageBox.Show("Connect device failed!");
                return;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            trglog.Enabled = false;
            MessageBox.Show("Stop to RTLog");
        }

        [DllImport("plcommpro.dll", EntryPoint = "SearchDevice")]
        //public static extern int SearchDevice( ref byte commtype, ref byte address, ref byte buffer);
        public static extern int SearchDevice(string commtype, string address, ref byte buffer);

//4.11 调用SearchDevice函数
        private void btnseardev_Click(object sender, EventArgs e)
        {
            int ret = 0, i = 0,j = 0,k=0;
            byte[] buffer = new byte[64 * 1024];
            string str = "";
            string[] filed = null;
            string[] tmp = null;
            string udp = "UDP";
            string adr = "255.255.255.255";

            MessageBox.Show("Start to SearchDevice!");
            this.labsearchdev.Text = "searching ......";

            ret = SearchDevice(udp,adr, ref buffer[0]);
            if (ret >= 0)
            {
                int count = this.lsvseardev.Items.Count;
                if(count>0)
                {
                    this.lsvseardev.Items.Clear();
                }
                str = Encoding.Default.GetString(buffer);
                str = str.Replace("\r\n", "\t");
                tmp = str.Split('\t');                    //将多条语句分开

                //int p = this.lsvseardev.Items.Count;
                while (j < tmp.Length-1)
                {
                    k = 0;
                    string[] sub_str = tmp[j].Split(',');

                    filed = sub_str[k++].Split('=');            //去 “=” 右边的数值赋给列表框里
                    this.lsvseardev.Items.Add(filed[1]);

                    filed = sub_str[k++].Split('=');
                    this.lsvseardev.Items[i].SubItems.Add(filed[1]);

                    filed = sub_str[k++].Split('=');
                    this.lsvseardev.Items[i].SubItems.Add(filed[1]);

                    filed = sub_str[k++].Split('=');
                    this.lsvseardev.Items[i].SubItems.Add(filed[1]);

                    filed = sub_str[k++].Split('=');
                    this.lsvseardev.Items[i].SubItems.Add(filed[1]);

                    i++;        //列表框的下一行
                    j++;        //每一行的下一列
                }
                this.labsearchdev.Text = "";
                this.cmbseardev.Enabled = true;
                this.txtnewip.Enabled = true;
                this.txtdevpwd.Enabled = true;
                this.btnmodip.Enabled = true;
            }
            else
            {
                MessageBox.Show("SearchDevice operation is failed!");
                return;
            }
        }

        //实现下拉框可以选择搜索到的设备
        private void cmbseardev_DropDown(object sender, EventArgs e)
        {
            string str = "";
            this.cmbseardev.Items.Clear();
            for (int i = 0; i < lsvseardev.Items.Count; i++)
            {
                str = this.lsvseardev.Items[i].SubItems[1].Text;
                this.cmbseardev.Items.Add(str);
            }
        }

        //实现下拉选择设备完成时，对应的IP地址要在文本框里显示出来
        private void cmbseardev_DropDownClosed(object sender, EventArgs e)
        {
            string cmbstr = "";
            string lsvstr = "";
            //if (this.cmbseardev.SelectedItem.ToString() == "")
                //cmbstr =this.cmbseardev.SelectedItem.ToString();
            
            if (lsvstr == "")
            {
                lsvstr = this.lsvseardev.Items[0].SubItems[1].Text;
            }
            else
            {
                lsvstr = this.cmbseardev.SelectedItem.ToString();
            }

            //MessageBox.Show(cmbstr);
            for (int i = 0; i < lsvseardev.Items.Count; i++)
            {
                lsvstr = this.lsvseardev.Items[i].SubItems[1].Text;
                if (string.Equals(cmbstr, lsvstr))
                {
                    this.txtnewip.Text = this.lsvseardev.Items[i].SubItems[1].Text;
                    break;
                }
            }
        }

        [DllImport("plcommpro.dll", EntryPoint = "ModifyIPAddress")]
        public static extern int ModifyIPAddress(string commtype,string address,string buffer);

//4.12 修改ModifyIPAddress地址
        private void btnmodip_Click(object sender, EventArgs e)         
        {
            int ret = 0, row = 0;
            string udp = "UDP";
            string address = "255.255.255.255";
            string buffer = "";
            string selstr = this.cmbseardev.SelectedItem.ToString();
            string itemstr = "";
            //MessageBox.Show(selstr);
            for (int i = 0; i < this.lsvseardev.Items.Count; i++)       //判断是哪一台设备要修改IP地址
            {

                itemstr = this.lsvseardev.Items[i].SubItems[1].Text;
                //MessageBox.Show(itemstr);
                if (string.Equals(itemstr, selstr))
                {
                    //buffer = "MAC=" + lsvseardev.Items[i].SubItems[0].Text + "," + "IPAddress=" + txtnewip.Text + "," + "ComPwd=" + txtdevpwd.Text;
                    buffer = "MAC=" + lsvseardev.Items[i].SubItems[0].Text + "," + "IPAddress=" + txtnewip.Text ;
                    row = i;    //将修改IP的行i号赋给row
                    //return;
                    ret = ModifyIPAddress(udp, address, buffer);              //调用修改IP地址接口函数
                    if (ret >= 0)
                    {
                        //this.lsvseardev.Items[row].SubItems[1].Text = this.txtdevpwd.Text;
                        MessageBox.Show("ModifyIPAddress operation is successful!");
                        return;
                    }
                }
            }

        }

        [DllImport("plcommpro.dll", EntryPoint = "GetDeviceFileData")]
        public static extern int GetDeviceFileData(IntPtr h, ref byte buffer, ref int buffersize, string filename, string options);

 //4.15 调用GetDeviceFileData函数
        private void btngetdevfile_Click(object sender, EventArgs e)
        {
            int ret = 0;
            int buffersize = 4 * 1024 * 1024;
            //int[] buffersize = new int[BUFFERSIZE];
            byte[] buffer = new byte[buffersize];
            string filename = this.txtgetdevfile.Text;
            string options = "";
            string filepath = "";
            if (IntPtr.Zero != h)
            {
                if (filename == "")
                {
                    MessageBox.Show("Please input the file name!");
                }
                else
                {
                    ret = GetDeviceFileData(h, ref buffer[0], ref buffersize, filename, options);
                    if (ret >= 0)
                    {
                        SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                        saveFileDialog1.Filter = "txt files(*.txt)|*.txt|All files(*.*)|*.*";
                        saveFileDialog1.FilterIndex = 2;
                        saveFileDialog1.RestoreDirectory = true;
                        saveFileDialog1.FileName = System.IO.Path.GetFileName(filename);

                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            filepath = saveFileDialog1.FileName;
                            FileStream fsFile = new FileStream(filepath, FileMode.Create);
                            fsFile.Seek(0, SeekOrigin.Begin);
                            fsFile.Write(buffer, 0, buffersize);
                            fsFile.Close();
                        }
                        else
                        {
                            MessageBox.Show(filename +" is not exist!");
                        }
                        MessageBox.Show("Successful download file!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Handle has disconnect!");
                return;
            }
        }

        private void btntarfile_Click(object sender, EventArgs e)
        {
            string filename;
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = System.IO.Path.GetFileName(this.openFileDialog1.FileName);
                if (filename != "")
                {
                    this.txttarfile.Text = filename;
                }
            }
        }

        [DllImport("plcommpro.dll", EntryPoint = "SetDeviceFileData")]
        public static extern int SetDeviceFileData(IntPtr h, string filename, ref byte buffer, int buffersize, string options);

 //调用SetDeviceFileData函数
        private void btnsetdevfile_Click(object sender, EventArgs e)
        {
            string filename = "";
            int buffersize = 0,ret = 0;
            //byte[] buffer = new byter[buffersize];
            string options = "";

            if(this.openFileDialog1.FileName == "")
            {
                MessageBox.Show("Please select file!");
                return;
            }

            FileStream fsFile = File.OpenRead(this.openFileDialog1.FileName);
            buffersize = (int)fsFile.Length;
            byte[] buffer = new byte[buffersize];

            if (fsFile.Read(buffer, 0, buffersize) != buffersize)
            {
                MessageBox.Show("Read file error!");
            }
            else
            {
                filename = this.txttarfile.Text;

                if (IntPtr.Zero != h)
                {
                    ret = SetDeviceFileData(h, filename, ref buffer[0], buffersize, options);
                    if (ret >= 0)
                    {
                        MessageBox.Show("Uploaded file success!");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Uploaded file failed!");
                        //ret = PullLastError();
                        return;
                    }
                    fsFile.Close();
                }
                else
                {
                    MessageBox.Show("Handle has disconnect!");
                    return;
                }
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            this.txtdoorstate.Text = "0";
            //this.cmbdevtable.SelectedIndex = 0;
            //this.txtdevdata.Text = "CardNo\tPin\tPassword\tGroup\tStartTime\tEndTime";
            //this.cmbfil.Items.Clear();
            //this.cmbfil.Items.Add("CardNo");
            //this.cmbfil.SelectedIndex = 0;
        }

        [DllImport("plcommpro.dll", EntryPoint = "DeleteDeviceData")]
        public static extern int DeleteDeviceData(IntPtr h, string tablename, string data, string options);

//4.9  调用DeleteDeviceData函数
        private void btndeldata_Click(object sender, EventArgs e)
        {
            int ret = 0;
            string tablename = this.cmbdevtable.SelectedItem.ToString();
            string data = this.txtdevdata.Text;
            string options = "";
            if (data == "")
                this.txtdevdata.Text = "\r\n \r\n Please input delete data in here!";
            else
            {
                if (IntPtr.Zero != h)
                {
                    ret = DeleteDeviceData(h, tablename, data, options);
                    if (ret >= 0)
                        MessageBox.Show("The deleted operation successfu!");
                    else
                        MessageBox.Show("The deleted operation failed!");
                }
            }
        }

        private void btndelall_Click(object sender, EventArgs e)
        {
            int ret = 0;
            string tablename = "";
            string data = txtdevdata.Text;
            string options = "";
            for (int i = 0; i < cmbdevtable.Items.Count; i++)
            {
                tablename = cmbdevtable.Items[i].ToString();
                if (IntPtr.Zero != h)
                {
                    ret = DeleteDeviceData(h, tablename, data, options);
                    if (ret >= 0)
                        MessageBox.Show(tablename + " Delete all operation successful!");
                    else
                    {
                        MessageBox.Show("Delete all operation failed! " + ret);
                    }
                }
                else
                    MessageBox.Show("Please connect device");
            }
        }
	}
}
