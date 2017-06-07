using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;
using System;

namespace mengedu
{
    public partial class Form1 : Form
    {
        string str_con = "Server=127.0.0.1;User Id=root;Password=Stop2run!;Database=test";
        MySqlConnection mycon = null;//实例化链接
        MySqlDataAdapter ada = null;
        DataSet ds = null;
        DataSet ds1 = null;
        DataSet ds2 = null;
        DataSet ds3 = null;
        Data_Changed data_t;

        public Form1(){
            mycon = new MySqlConnection();
            data_t = new Data_Changed();
            data_t.datao = null;
            mycon.ConnectionString = str_con;
            mycon.Open();
            InitializeComponent();
        }
        ~Form1(){
            mycon.Close();
        }
        
        private void Form1_Load(object sender, System.EventArgs e){
          string str_com="select year_ from scorelevel group by year_ order by year_ desc;";
          try{
            ada = new MySqlDataAdapter(str_com, str_con);
            ds = new DataSet();
            ada.Fill(ds);

            foreach (DataRow mDr in ds.Tables[0].Rows){
              comboBox1.Items.Add(mDr["year_"].ToString());
              comboBox2.Items.Add(mDr["year_"].ToString());
            }
            comboBox1.Text = comboBox1.Items[0].ToString();
            comboBox2.Text = comboBox1.Items[1].ToString();
          }catch (Exception ex){
            MessageBox.Show("连接失败，原因是1：\n" + ex.Message);
            mycon.Close();
          }
        }

        private void button1_Click(object sender, System.EventArgs e){
          string str_cmd;
          textBox1.Text += comboBox1.Text + "  " + score.Text + "\r\n";
          str_cmd = "select * from scorelevel where year_=" + comboBox1.Text + " and score=" + score.Text;
          try{
            ada = new MySqlDataAdapter(str_cmd, str_con);
            ds = new DataSet();
            ada.Fill(ds);
            
            if(ds.Tables[0].Rows.Count>0){
              textBox1.Text +=ds.Tables[0].Rows[0]["pnum"].ToString()
                +" "+ds.Tables[0].Rows[0]["llevel"].ToString()
                +" "+ds.Tables[0].Rows[0]["hlevel"].ToString()
                +" "+ds.Tables[0].Rows[0]["alevel"].ToString()
                +"\r\n";
             textBox2.Text =ds.Tables[0].Rows[0]["llevel"].ToString();
             textBox4.Text =ds.Tables[0].Rows[0]["hlevel"].ToString();
            }
            ds.Clear();
            str_cmd = "select * from scorelevel where year_=" + comboBox2.Text
              +" and hlevel<=" +textBox2.Text +" and " +textBox2.Text +"<=llevel;";
            ada.SelectCommand.CommandText = str_cmd;
            ada.Fill(ds);
            textBox3.Text =ds.Tables[0].Rows[0]["score"].ToString();

            ds.Clear();
            str_cmd = "select * from scorelevel where year_=" + comboBox2.Text
              +" and hlevel<=" +textBox4.Text +" and " +textBox4.Text +"<=llevel;";
            ada.SelectCommand.CommandText = str_cmd;
            ada.Fill(ds);
            textBox5.Text =ds.Tables[0].Rows[0]["score"].ToString();
            
            ds.Reset();
            str_cmd = "select edu,prof,pnum,lscore,hscore from eduproscore where lscore<="+textBox3.Text+" and year_="+comboBox2.Text+";";
            ada.SelectCommand.CommandText = str_cmd;
            //ds = new DataSet();
            ada.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
//            dataGridView1.AutoResizeColumnHeadersHeight();
//                dataGridView1.AutoResizeColumns();

            //ds1.Reset();
            str_cmd = "select edu,prof,pnum,lscore,hscore from eduproscore where lscore<="+textBox5.Text+" and year_="+comboBox2.Text+";";
            ada.SelectCommand.CommandText = str_cmd;
            ds1 = new DataSet();
            ada.Fill(ds1);
            dataGridView2.DataSource = ds1.Tables[0];

            tabControl1.TabPages[2].Text = comboBox2.Text + "可选学校";


            str_cmd = "select score, pnum,score, COALESCE(pnum1,0) as pnum1, COALESCE(pnum2,0) as pnum2,"
              +"COALESCE(pnum3,0) as pnum3,COALESCE(pnum4,0) as pnum4,COALESCE(pnum5,0) as pnum5 from"
              +"((((select score,pnum from scorelevel where year_=2016) as At "
              +"left join "
              +"(select score as score1,pnum as pnum1 from scorelevel where year_=2015) as Bt "
              +"on At.score=Bt.score1) "
              +"left join "
              +"(select score as score2,pnum as pnum2 from scorelevel where year_=2014) as Ct "
              +"on At.score=Ct.score2) "
              +"left join "
              +"(select score as score3,pnum as pnum3 from scorelevel where year_=2013) as Dt "
              +"on At.score=Dt.score3) "
              +"left join "
              +"(select score as score4,pnum as pnum4 from scorelevel where year_=2012) as Et "
              +"on At.score=Et.score4 "
              +"left join "
              +"(select score as score5,pnum as pnum5 from scorelevel where year_=2011) as Ft "
              +"on At.score=Ft.score5 "
              +"order by score DESC;";
            ada.SelectCommand.CommandText = str_cmd;
            ds2 = new DataSet();
            ada.Fill(ds2);
            //chart1.DataBindTable(ds2.Tables[0].Columns.,"score");
            chart1.DataSource = ds2.Tables[0];
            chart1.Series[0].Name = "2016";
            chart1.Series[0].XValueMember = "score";
            chart1.Series[0].YValueMembers = "pnum";
            chart1.Series[1].Name = "2015";
            chart1.Series[1].XValueMember = "score";
            chart1.Series[1].YValueMembers = "pnum1";
            chart1.Series[2].Name = "2014";
            chart1.Series[2].XValueMember = "score";
            chart1.Series[2].YValueMembers = "pnum2";
            chart1.Series[3].Name = "2013";
            chart1.Series[3].XValueMember = "score";
            chart1.Series[3].YValueMembers = "pnum3";
            chart1.Series[4].Name = "2012";
            chart1.Series[4].XValueMember = "score";
            chart1.Series[4].YValueMembers = "pnum4";
            chart1.Series[5].Name = "2011";
            chart1.Series[5].XValueMember = "score";
            chart1.Series[5].YValueMembers = "pnum5";

            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.DataBind();
          }
            catch (Exception ex){
            MessageBox.Show("连接失败，原因是1：\n" + ex.Message);
            mycon.Close();
          }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e){
          dGView_DoubleClick(sender, e);
        }

        private void dataGridView2_DoubleClick(object sender, EventArgs e){
          //dGView_DoubleClick(sender, e);
        }

        private void dGView_DoubleClick(object sender, EventArgs e)
        {
          string str_cmd;
          //button1.Text = tabControl1.SelectedIndex.ToString();
          //textBox1.Text += ((DataGridView)sender).Location.X.ToString() +" "+ ((DataGridView)sender).Parent+ "\r\n";
          //textBox1.Text += ((DataGridView)sender).CurrentCell.RowIndex
//          textBox1.Text += ((DataGridView)sender)[0, ((DataGridView)sender).CurrentCell.RowIndex].Value
//            +" " +((DataGridView)sender)[1, ((DataGridView)sender).CurrentCell.RowIndex].Value + "\r\n";
          if (tabControl1.SelectedIndex != 1)
          {
            data_t.datao = sender;
            data_t.tageindex = tabControl1.SelectedIndex;
            data_t.X = ((DataGridView)sender).Top;
            data_t.Y = ((DataGridView)sender).Left;
            tabControl1.SelectTab(1);
            ((DataGridView)sender).Parent = tabControl1.TabPages[1];
            //dataGridView1.Top = 2;
            ((DataGridView)sender).Top = 3;
            ((DataGridView)sender).Left = 6;
            ((DataGridView)sender).BringToFront();
          }

          try{
            str_cmd = "select * from eduscore_level where edu =\""
              +((DataGridView)sender)[0, ((DataGridView)sender).CurrentCell.RowIndex].Value
              +"\" and prof=\""
              +((DataGridView)sender)[1, ((DataGridView)sender).CurrentCell.RowIndex].Value
              +"\" order by year_";
            
            ada.SelectCommand.CommandText = str_cmd;
            ds3 = new DataSet();
            ada.Fill(ds3);
            foreach(var series in chart2.Series){
              series.Points.Clear();
              series.IsValueShownAsLabel = true;
              series.XValueMember = "year_";
            }
//            chart2.Series[0].Points.Clear();
//            chart2.Series[1].Points.Clear();
//            chart2.Series[2].Points.Clear();
            chart2.DataSource = ds3.Tables[0];
            chart2.Series[0].Name = "lllevel";
//            chart2.Series[0].XValueMember = "year_";
            chart2.Series[0].YValueMembers = "lllevel";
            chart2.Series[1].Name = "lhlevel";
//            chart2.Series[1].XValueMember = "year_";
            chart2.Series[1].YValueMembers = "lhlevel";
            chart2.Series[2].Name = "lalevel";
//            chart2.Series[2].XValueMember = "year_";
            chart2.Series[2].YValueMembers = "lalevel";
            chart2.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart2.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
//            textBox1.Text +=ds3.Tables[0].Rows[0]["lllevel"].ToString() +"\r\n";
          }
            catch (Exception ex){
            MessageBox.Show("连接失败，原因是1：\n" + ex.Message);
            mycon.Close();
          }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e){
          //button1.Text = tabControl1.SelectedIndex.ToString();
          if (data_t.datao != null){
            ((DataGridView)(data_t.datao)).Parent = tabControl1.TabPages[data_t.tageindex];
            ((DataGridView)(data_t.datao)).Top = data_t.X;
            ((DataGridView)(data_t.datao)).Left = data_t.Y;
            textBox1.Text += "X="+data_t.X.ToString() +" " + data_t.X.ToString() + "\r\n";
          }
        }

//        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
//        {
//            textBox1.Text += ((DataGridView)sender).CurrentCell.Value+ " column Index" + e.ColumnIndex.ToString() + " " + e.RowIndex.ToString() + "\r\n";
//            textBox1.Text += ((DataGridView)sender)[0, e.RowIndex].Value+" " +((DataGridView)sender)[1, e.RowIndex].Value + "\r\n";
//        }
    }

    public class Data_Changed{
        public object datao;
        public int tageindex, X, Y;
    }
}
