using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using CookComputing.XmlRpc;
using System.Collections.Generic;


namespace V6APIRecordContactActivity
{

    public class MainForm : System.Windows.Forms.Form
    {

        class clsDB_ID_NAME
        {
            private int _ID;
            private string _name;

            public int ID
            {
                get { return _ID; }
                set { _ID = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public clsDB_ID_NAME(string name, int value)
            {
                _name = name;
                _ID = value;
            }

            public override string ToString()
            {
                return _name;
            }

        }

        private System.Windows.Forms.Button login_button;

        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ComboBox DatabaseList;
        private Button btnGetEmailsSentToContact;
        private Button btnLogout;
        private Label DatabaseListLabel;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public MainForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.login_button = new System.Windows.Forms.Button();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.status_bar = new System.Windows.Forms.TextBox();
            this.DatabaseList = new System.Windows.Forms.ComboBox();
            this.btnGetEmailsSentToContact = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.DatabaseListLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // login_button
            // 
            this.login_button.Location = new System.Drawing.Point(17, 64);
            this.login_button.Name = "login_button";
            this.login_button.Size = new System.Drawing.Size(192, 24);
            this.login_button.TabIndex = 0;
            this.login_button.Text = "Retrieve Database List";
            this.login_button.Click += new System.EventHandler(this.login_button_Click);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(94, 8);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(115, 20);
            this.username.TabIndex = 2;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(94, 38);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(115, 20);
            this.password.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 24);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // status_bar
            // 
            this.status_bar.AcceptsReturn = true;
            this.status_bar.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status_bar.Location = new System.Drawing.Point(226, 8);
            this.status_bar.Multiline = true;
            this.status_bar.Name = "status_bar";
            this.status_bar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.status_bar.Size = new System.Drawing.Size(387, 392);
            this.status_bar.TabIndex = 10;
            // 
            // DatabaseList
            // 
            this.DatabaseList.Enabled = false;
            this.DatabaseList.FormattingEnabled = true;
            this.DatabaseList.Location = new System.Drawing.Point(17, 178);
            this.DatabaseList.Name = "DatabaseList";
            this.DatabaseList.Size = new System.Drawing.Size(193, 21);
            this.DatabaseList.TabIndex = 11;
            this.DatabaseList.SelectedIndexChanged += new System.EventHandler(this.DatabaseList_SelectedIndexChanged);
            // 
            // btnGetEmailsSentToContact
            // 
            this.btnGetEmailsSentToContact.Enabled = false;
            this.btnGetEmailsSentToContact.Location = new System.Drawing.Point(17, 205);
            this.btnGetEmailsSentToContact.Name = "btnGetEmailsSentToContact";
            this.btnGetEmailsSentToContact.Size = new System.Drawing.Size(193, 24);
            this.btnGetEmailsSentToContact.TabIndex = 12;
            this.btnGetEmailsSentToContact.Text = "Record Contact Activity";
            this.btnGetEmailsSentToContact.UseVisualStyleBackColor = true;
            this.btnGetEmailsSentToContact.Click += new System.EventHandler(this.btnGetEmailsSentToContact_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Enabled = false;
            this.btnLogout.Location = new System.Drawing.Point(18, 372);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 28);
            this.btnLogout.TabIndex = 13;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // DatabaseListLabel
            // 
            this.DatabaseListLabel.AutoSize = true;
            this.DatabaseListLabel.Location = new System.Drawing.Point(15, 158);
            this.DatabaseListLabel.Name = "DatabaseListLabel";
            this.DatabaseListLabel.Size = new System.Drawing.Size(75, 13);
            this.DatabaseListLabel.TabIndex = 14;
            this.DatabaseListLabel.Text = "Database List:";
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(625, 412);
            this.Controls.Add(this.DatabaseListLabel);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.btnGetEmailsSentToContact);
            this.Controls.Add(this.DatabaseList);
            this.Controls.Add(this.status_bar);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.login_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vision6 Support VeMail API tool - Record Contact Email Open Activity";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.TextBox status_bar;
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new MainForm());
        }


        //DECLARE GLOBAL VARIABLES
        
        //Class that holds a database id and name property.  Used to add a name to a drop down, but retrieve the ID.
        clsDB_ID_NAME SelectedDB;

        //VeMail API object
        API api = null;
        bool boolDBSelected = false;
        bool boolIsLoggedIn = false;

        private void login_button_Click(object sender, System.EventArgs e)
        {

            // Create new API object
            if (api == null) {
                api = new API();
            }

            // Login
            try
            {
                api.login(this.username.Text, this.password.Text);
                this.status_bar.Text = "Logged in.";

                //Other user options are now available.
                btnLogout.Enabled = true;
                btnGetEmailsSentToContact.Enabled = true;
                DatabaseList.Enabled = true;

            }
            catch (Exception ex)
            {
                this.status_bar.Text = "Login Failed: " + ex.Message;
                return;
            }

            //Ensure the user is logged in before continuing
            boolIsLoggedIn = (bool)api.isLoggedIn();
            if (boolIsLoggedIn == false)
            {
                this.status_bar.Text = "Login Failed. Ensure your username and password is correct and try again.";
                return;
            }

            try
            {
                // Declare varaiables
                XmlRpcStruct objDatabases = new XmlRpcStruct();
                XmlRpcStruct database = new XmlRpcStruct();

                // Search databasses
                objDatabases = (XmlRpcStruct)api.searchDatabases();

                //Loop through each database in the response and add their information to the database list.
                foreach (DictionaryEntry keyDatabase in objDatabases)
                {
                    database = (XmlRpcStruct)objDatabases[keyDatabase.Key];

                    //Populate the database list with an item/object that stores a name and id property.
                    this.DatabaseList.Items.Add(new clsDB_ID_NAME(database["name"].ToString(), Convert.ToInt32(database["id"].ToString())));
                }
                this.status_bar.Text += "\r\nSuccessfully populated database list.  Choose a database and click 'Record Contact Activity'.";
                btnGetEmailsSentToContact.Enabled = true;
            }
            catch (Exception ex)
            {
                this.status_bar.Text = "\r\nRetrieving databases failed:" + ex.Message;
                return;
            }
        }

        private void DatabaseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the user selects an option from the list, change the value of SelectDB accordingly.
            SelectedDB = (clsDB_ID_NAME)DatabaseList.SelectedItem;
            boolDBSelected = true;
        }

        private void btnGetEmailsSentToContact_Click(object sender, EventArgs e)
        {
            //Declare variables that need to exist for the entire event
            int intEmailCounter = 0;
            int intContactCounter = 0;
            string strFieldData = "";
            object[] objContactsToBeEdited;
            XmlRpcStruct xmlDBContacts = new XmlRpcStruct();
            XmlRpcStruct xmlContact = new XmlRpcStruct();

            if (boolDBSelected == false)
            {
                this.status_bar.Text += "\r\nNo database selected.  Select a database from the drop down and try again.";
                return;
            }

            //Handle exceptions for any miscellaneous errors.
            try
            {
                //Add new field to record eDM activity
                try
                {
                    XmlRpcStruct field = new XmlRpcStruct();
                    field["name"] = "eDM Activity";
                    field["type"] = "text";
                    field["id"] = api.addField(System.Convert.ToInt32(SelectedDB.ID), field);

                    this.status_bar.Text += "\r\nNew 'eDM Activity' text field added into selected database.";
                }
                catch (Exception ex)
                {
                    this.status_bar.Text += "\r\nUnable to add field: " + ex.Message;
                    this.status_bar.Text += "\r\nIf the above error is 'Server returned a fault exception.'; this will occur when the field already exists.  Nothing to really worry about.";
                }

                //Searching for all contacts in the selected DB 
                try
                {
                    this.status_bar.Text += "\r\nSearching for contacts in selected DB.";
                    xmlDBContacts = (XmlRpcStruct)api.searchContacts(SelectedDB.ID);
                    objContactsToBeEdited = new object[xmlDBContacts.Count];
                }
                catch (Exception ex)
                {
                    this.status_bar.Text += "\r\nFailed seraching for contacts: " + ex.Message;
                    this.status_bar.Text += "\r\nDatabase may have no contacts";
                    return;
                }

                XmlRpcStruct xmlEmailForContact = new XmlRpcStruct();

                //loop through each contact and retrieve the emails sent to the contact
                foreach (DictionaryEntry keyContacts in xmlDBContacts)
                {
                    strFieldData = "";
                    XmlRpcStruct xmlEmailsForContact = new XmlRpcStruct();
                    xmlEmailForContact = new XmlRpcStruct();

                    // Variable to hold data about one specific contact.
                    xmlContact = (XmlRpcStruct)xmlDBContacts[keyContacts.Key];

                    //Get the emails sent to the contact
                    xmlEmailsForContact = (XmlRpcStruct)api.getEmailsSentToContact(SelectedDB.ID, Convert.ToInt32(xmlContact["id"].ToString()));

                    // If the contact has been sent an e-mail:
                    if (xmlEmailsForContact.ContainsKey("0"))
                    {
              
                        //Use a sorted list to sort the emails by send time
                        SortedList<int, int> SortedByTimeEmailList = new SortedList<int, int>();

                        // Loop through each sent email and add the send time as a key and the key (from the UNSORTED response from the server) as the value in to a sorted list

                        foreach (DictionaryEntry email in xmlEmailsForContact)
                        {
                            xmlEmailForContact = (XmlRpcStruct)xmlEmailsForContact[email.Key];
                            SortedByTimeEmailList.Add(Convert.ToInt32(xmlEmailForContact["send_time"]), Convert.ToInt32(email.Key.ToString()));
                            intEmailCounter++;
                        } 


                        //Go through the sorted list, output a responses to user and contruct an 'activity string'.
                        foreach (KeyValuePair<int, int> kvpEmail in SortedByTimeEmailList)
                        {
                            xmlEmailForContact = (XmlRpcStruct)xmlEmailsForContact[kvpEmail.Value.ToString()];

                            // Look at the 'open type' for each email.  Modify the value of strFieldData depending on if the email was opened or not.
                            if ((string)xmlEmailForContact["open_type"] == "0")
                            {
                                strFieldData += 0;
                            }
                            else if ((string)xmlEmailForContact["open_type"] == "1" || (string)xmlEmailForContact["open_type"] == "2") //If email was opened in html of plaintext
                            {
                                strFieldData += 1;
                            }
                            else
                            {
                                this.status_bar.Text += "\r\nSomething went wrong with looking for an 'open type'";
                            }
                        }
                    }

                    // Editing the 'eDM activity' field with the activity string.  Should be modified to construct one call to editContacts as opposed to for each contact.
                    try
                    {
                        XmlRpcStruct xmlContactToBeEdited = new XmlRpcStruct();
                        
                        //Edit the 'eDm activity' field' for the contact selected in the loop
                        xmlContactToBeEdited["id"] = xmlContact["id"];
                        xmlContactToBeEdited["eDM activity"] = strFieldData;

                        //Compile the array of contacts
                        objContactsToBeEdited[intContactCounter] = xmlContactToBeEdited;

                        this.status_bar.Text += "\r\nContact successfully analysed.";
                        intContactCounter++;
                    }
                    catch (Exception ex)
                    {
                        this.status_bar.Text += "\r\nFailed editing contacts: " + ex.Message;
                        return;
                    }

                }


                //Send one editContacts request to the API.  Pass an object with the details of all contacts in DB.
                try
                {
                    //Setup appropriate variables.
                    bool trigger_campaign = false;
                    object editContactsResult = new object();

                    // Send editContacts request to api.
                    editContactsResult = api.editContacts(System.Convert.ToInt32(SelectedDB.ID), objContactsToBeEdited, trigger_campaign);
                    this.status_bar.Text += "\r\nContacts edited.";
                }
                catch (Exception ex)
               {
                    this.status_bar.Text += "\r\nFailed editing contacts: " + ex.Message;
                    return;
               }

               this.status_bar.Text += "\r\nSuccess! Please logout via the 'Logout' button before quitting."; //YAY!
            }
            catch (Exception ex)
            {
                this.status_bar.Text += "\r\nUnknown error in main loop: " + ex.Message;
                return;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {


            try
            {
                // Logout
                api.logout();
                this.status_bar.Text += "\r\nLogged out.";

                btnLogout.Enabled = false;
                btnGetEmailsSentToContact.Enabled = false;
                DatabaseList.Enabled = false;
            }
            catch (Exception ex)
            {
                this.status_bar.Text += "\r\nFailed logging out: " + ex.Message;
                return;
            }
           
        }

    }
}
