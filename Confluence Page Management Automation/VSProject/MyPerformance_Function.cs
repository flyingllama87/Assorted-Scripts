// This part of the script goes through all the pages under 'My performance'.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using CookComputing.XmlRpc;
using System.Collections;

namespace SSAConfluenceHRPageCycle
{
    public partial class UI_Main : Form
    {

        //***************MOVEPAGES*******************
        private void txtMovePages_Click(object sender, EventArgs e)
        {

            //Disable user input
            btnLogout.Enabled = false;
            btnMovePages.Enabled = false;
            btnMoveDeptPages.Enabled = false;

            //***Initialize variables***
            string PageID, DestinationPageID;
            XmlRpcStruct ParentPage = new XmlRpcStruct();
            XmlRpcStruct DestinationPage = new XmlRpcStruct();
            Array Children = null;
            List<string> listPagesToMove = new List<string>();
            string ExclusionsString;
            string[] ExclusionsList;
            List<string> listPageTitlesToMove = new List<string>();

            txtStatus.Text += "\r\n\r\nRetrieving 'My Performance' page..." + Environment.NewLine;

            // Try to get the source & destination pages using the user supplied information.
            try
            {
                ParentPage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtMyPerformancePageName.Text);
                DestinationPage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtDestinationPageName.Text);
                txtStatus.Text += "Success!! " + Environment.NewLine; ;
            }
            catch (Exception ex)
            {
                txtStatus.Text += "ERROR! Could not retrieve page: " + ex.Message + Environment.NewLine;
            }

            //String conversion...
            PageID = Convert.ToString(ParentPage["id"]);
            DestinationPageID = Convert.ToString(DestinationPage["id"]);

            //Get All child pages of 'Parent Page' (my performance) and store in a 'Children Object Array'
            try
            {
                Children = confluenceProxy.getChildren(token, PageID);
                txtStatus.Text += "DEBUG: Successfully retrieved all children pages" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txtStatus.Text += "ERROR! Could not get children pages of 'my performance page': " + ex.Message + Environment.NewLine;
            }

            //Read Exclusions into a list. Textbox delimited by a new line.
            ExclusionsString = txtExclude.Text;
            ExclusionsList = ExclusionsString.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            //Compile a list of pages to move, which is all except those listed in the exclusions multiline textbox.
            foreach (XmlRpcStruct ChildPage in Children)
            {
                Application.DoEvents();
                Boolean ExcludePage = false;

                //*** List the child page under examinations
                txtStatus.Text += "Examining the following child page: " + ChildPage["title"] + Environment.NewLine;

                //For each exclusion...
                foreach (string Exclusion in ExclusionsList)
                {
                    Application.DoEvents();

                    //If the current exclusion is located in the child page's title exclude the page.
                    if (Convert.ToString(ChildPage["title"]).IndexOf(Exclusion) >= 0)
                    {
                        ExcludePage = true;
                    }

                }

                //Add pages to PageID & Page Title list if they haven't been excluded...
                if (ExcludePage == false)
                {
                    listPagesToMove.Add(Convert.ToString(ChildPage["id"]));
                    listPageTitlesToMove.Add(Convert.ToString(ChildPage["title"]));
                }

            }

            //**************PROMPT USER FOR CONFIRMATION******************

            string ConfirmBoxCaption = "Confirm pages to move";
            string ConfirmBoxMessage = "The following pages will be moved:\r\n\r\n";

            foreach (string ChildTitle in listPageTitlesToMove)
            {
                Application.DoEvents();
                ConfirmBoxMessage += ChildTitle + Environment.NewLine;
            }

            var ConfirmBoxResult = MessageBox.Show(ConfirmBoxMessage, ConfirmBoxCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            //*******************EXECUTION CONFIRMATION SWITCH**************
            switch (ConfirmBoxResult)
            {
                case DialogResult.OK:

                    //**************************IMPORT NEW CONTENT************************

                    //For each child page in the list...
                    foreach (string ChildId in listPagesToMove)
                    {
                        Application.DoEvents();  //screen refresh

                        // Init variables

                        XmlRpcStruct ReturnedPage = new XmlRpcStruct();
                        XmlRpcStruct PageToEdit = new XmlRpcStruct();
                        XmlRpcStruct PageUpdateOptions = new XmlRpcStruct();
                        XmlRpcStruct TemplatePage = new XmlRpcStruct();
                        XmlRpcStruct NewPage = new XmlRpcStruct();

                        try
                        {

                            //Get details of page to modify it's content.  Store the details exactly and remove/add the details as needed.
                            PageToEdit = confluenceProxy.getPage(token, ChildId);
                            txtStatus.Text += "DEBUG: Title of page to edit: " + PageToEdit["title"] + Environment.NewLine ;

                            TemplatePage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtTemplateLocation.Text);

                            PageToEdit.Remove("content");
                            PageToEdit.Add("content", (string)TemplatePage["content"]); 


                            //This is meta information regarding the edit 
                            PageUpdateOptions.Add("versionComment", "Add prefix as part of HR confluence script");
                            PageUpdateOptions.Add("minorEdit", true);

                            //UPDATE THE PAGE!!
                            ReturnedPage = confluenceProxy.updatePage(token, PageToEdit, PageUpdateOptions);
                            txtStatus.Text += "The page has been successfully updated." + Environment.NewLine;

                        }
                        catch (Exception ex)
                        {
                            txtStatus.Text += "ERROR! Could not update page: " + ex.Message + Environment.NewLine;
                        }

                
                    }


                    btnLogout.Enabled = true;
                    btnMovePages.Enabled = true;
                    btnMoveDeptPages.Enabled = true;

                    MessageBox.Show("Process Complete.");
                    txtStatus.Text += "\r\nProcess Complete!" + Environment.NewLine;

                    //STOP ALL PROCESSING.
                    break;

                // USER DID NOT CLICK OK... DO NOTHING.
                default:
                    txtStatus.Text += "User canceled page move at confirmation message box." + Environment.NewLine;

                    btnLogout.Enabled = true;
                    btnMovePages.Enabled = true;
                    btnMoveDeptPages.Enabled = true;
                    break;
            }
        }
    }
}
