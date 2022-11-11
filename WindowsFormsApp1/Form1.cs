using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string token = "a37f9cac15fc2c19c764e14d42ec93614b079e959759a7285b61bec92ccdb976";//Genrated from  https://gorest.co.in/ NeverExpir

        public Form1()
        {
            InitializeComponent();
        }
        //https://gorest.co.in/
        //https://apipheny.io/free-api/#apis-without-key
        //https://8gwifi.org/jwsgen.jsp
        private void Process_Click(object sender, EventArgs e)
        {
            var fromAPI = GetEntriesData();
            var statusCode = fromAPI.StatusCode;
            var dataFromAPI = fromAPI.Data.entries.ToList();
            gridView1.Columns.Clear();
            gridControl1.DataSource = dataFromAPI;
        }

        private void GetUsers_Click(object sender, EventArgs e)
        {
            var fromAPI = GetUsersData();
            var statusCode = fromAPI.StatusCode;
            var dataFromAPI = fromAPI.Content;
            var valueSet = JsonConvert.DeserializeObject<List<User>>(dataFromAPI);
            gridView1.Columns.Clear();
            gridControl1.DataSource = valueSet.ToList();
        }

        private void InsertUser_Click(object sender, EventArgs e)
        {
            var response = SetUserData(new User
            {
                name = "Hadyo",
                email = "Hady@Dom.Cocoo",
                gender = "male",
                status = "inactive"
            });

            User user = response.Data;
            txt_Id.Text = user.id.ToString();
        }

        private void GetuserByID_Click(object sender, EventArgs e)
        {
            if (txt_Id.Text.Trim() == string.Empty)
            {
                MessageBox.Show("ادخل رقم المستخدم");
                txt_Id.Focus();
                return;
            }

            var response = GetUserDataById(txt_Id.Text.Trim());
            var dataFromAPI = response.Content;
            var valueSet = JsonConvert.DeserializeObject<List<User>>(dataFromAPI);
            gridView1.Columns.Clear();
            gridControl1.DataSource = valueSet.ToList();
        }

        IRestResponse<Root> GetEntriesData()
        {
            var client = new RestClient("https://api.publicapis.org/entries");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse<Root> response = client.Execute<Root>(request);

            IRestResponse response1 = client.Execute(request);
            return response;
        }
        IRestResponse<User> GetUsersData()
        {
            var client = new RestClient("https://gorest.co.in/public/v2/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse<User> response = client.Execute<User>(request);

            return response;
        }
        IRestResponse<User> GetUserDataById(string id)
        {
            var client = new RestClient("https://gorest.co.in/public/v2/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("id", id);

            IRestResponse<User> response = client.Execute<User>(request);

            return response;
        }
        IRestResponse<User> SetUserData(User inUser)
        {
            var client = new RestClient("https://gorest.co.in/public/v2/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("name", inUser.name);
            request.AddParameter("email", inUser.email);
            request.AddParameter("gender", inUser.gender);
            request.AddParameter("status", inUser.status);
            IRestResponse<User> response = client.Execute<User>(request);
            return response;
        }
    }
}
