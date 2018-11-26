// This part of the script goes through all the pages under 'My Departments performance'.

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

        private void btnMoveDeptPages_Click(object sender, EventArgs e)
        {
            //Disable user input
            btnLogout.Enabled = false;
            btnMovePages.Enabled = false;
            btnMoveDeptPages.Enabled = false;


            //*********************************************IMPLEMENT MY DEPARTMENTS PERFORMANCE FUNCTIONS HERE************************************

            txtStatus.Text += "\r\n\r\n Starting to process My Departments performance pages... " + Environment.NewLine;

            Array DeptPerfDescendantPages = null;
            XmlRpcStruct DeptPerfPage = new XmlRpcStruct();
            List<String> listPageIDsToMove = new List<String>();
            List<String> listPageTitlesToMove = new List<String>();

            string ExclusionsString;
            string[] ExclusionsList;


            try
            {
				
				//Get root 'My departments performance' page.
                DeptPerfPage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtMyDeptPerformancePageName.Text);
                DeptPerfDescendantPages = confluenceProxy.getDescendents(token, (string)DeptPerfPage["id"]);

                //Read Exclusions into a list. Textbox delimited by a new line.
                ExclusionsString = txtExclude.Text;
                ExclusionsList = ExclusionsString.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (XmlRpcStruct DescendantPage in DeptPerfDescendantPages)
                {
                    Application.DoEvents();
                    Boolean ExcludePage = false;

                    //*** List the child page under examinations
                    txtStatus.Text += "Examining the following descendant page: " + DescendantPage["title"] + Environment.NewLine;

                    //For each exclusion...
                    foreach (string Exclusion in ExclusionsList)
                    {
                        Application.DoEvents();
                        //If the current exclusion is located in the child page's title exclude the page.
                        if ((Convert.ToString(DescendantPage["title"]).IndexOf(Exclusion) >= 0) || (Convert.ToString(DescendantPage["title"]).IndexOf("Performance Appraisal") == -1))
                        {
                            ExcludePage = true;
                        }
                    }

                    //Add pages to PageID & Page Title list if they haven't been excluded...
                    if (ExcludePage == false)
                    {
                        listPageIDsToMove.Add(Convert.ToString(DescendantPage["id"]));
                        listPageTitlesToMove.Add(Convert.ToString(DescendantPage["title"]));
                    }

                }

                foreach (string PageTitle in listPageTitlesToMove)
                {
                    txtStatus.Text += "Page marked to move: " + PageTitle + Environment.NewLine;
                    Application.DoEvents();
                }

                txtStatus.Text += "Successfully retrieved descendant Pages. " + Environment.NewLine;
            }
            catch (Exception ex)
            {
                txtStatus.Text += "ERROR! Could not get descendant pages: " + ex.Message + Environment.NewLine;
            }


            //**************PROMPT USER FOR CONFIRMATION******************

            string ConfirmBoxCaption = "Confirm employees to process";
            string ConfirmBoxMessage = "The following employee pages & their child pages will be processed:\r\n\r\n";

            foreach (string ChildTitle in listPageTitlesToMove)
            {
                ConfirmBoxMessage += ChildTitle + Environment.NewLine;
                Application.DoEvents();
            }

            //REFRESH STATUS BOX
            txtStatus.SelectionStart = txtStatus.Text.Length;
            txtStatus.ScrollToCaret();
            txtStatus.Refresh();

            var ConfirmBoxResult = MessageBox.Show(ConfirmBoxMessage, ConfirmBoxCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            //*******************EXECUTION CONFIRMATION SWITCH**************
            switch (ConfirmBoxResult)
            {
                case DialogResult.OK:

                    txtStatus.Text += "User Clicked OK!" + Environment.NewLine;


                    //Starting Making changes.  This should be double checked at some stage.  Specifically that page titles and IDs match up.
                    foreach (string PageId in listPageIDsToMove)
                    {
                        Application.DoEvents();
                        
                        //get current page
                        XmlRpcStruct CurrentPage = new XmlRpcStruct();
                        XmlRpcStruct ArchivePage = new XmlRpcStruct();
                        CurrentPage = confluenceProxy.getPage(token, PageId);

                        //declare vars for entire function.
                        XmlRpcStruct NewArchivePageSetup = new XmlRpcStruct();
                        XmlRpcStruct NewArchivePage = new XmlRpcStruct();
                        XmlRpcStruct TemplatePage = new XmlRpcStruct();
                        XmlRpcStruct PageTemplate = new XmlRpcStruct();
                        XmlRpcStruct PageUpdateOptions = new XmlRpcStruct();
                        XmlRpcStruct ReturnedPage = new XmlRpcStruct();
                        object[] NewPageViewPermissions = null;

                        bool PageExists = true;

                        //CHECK TO SEE IF ARCHIVE PAGE ALREADY EXISTS

                        try
                        {
                            NewArchivePage = confluenceProxy.getPage(token, txtSpaceKey.Text, (string)CurrentPage["title"] + " Archive");
                        }
                        catch (Exception ex)
                        {
                            PageExists = false;
                            txtStatus.Text += "Verbose: Could not find existing archive page for " + (string)CurrentPage["title"] + ". Have to create new page." + Environment.NewLine;
                        }


                        //CREATE AN ARCHIVE PAGE FOR EACH EMPLOYEE PAGE UNDER DEPT PERF IF ARCHIVE PAGE COULD NOT BE FOUND.

                        if (PageExists == false)
                        {
                            NewArchivePageSetup.Add("space", txtSpaceKey.Text);
                            NewArchivePageSetup.Add("content", " ");

                            try
                            {

                                NewArchivePageSetup.Add("title", (string)CurrentPage["title"] + " Archive");
                                NewArchivePage = confluenceProxy.storePage(token, NewArchivePageSetup);

                                //confluenceProxy.movePage(token, (string)NewArchivePage["id"], PageId, "append");

                                txtStatus.Text += "Successfully created and moved archive page. " + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "Could not create archive pages " + ex.Message + Environment.NewLine;
                            }
                        }


                        //CREATE AN ARCHIVE PAGE FOR THE PERFORMANCE APPRAISAL AND MOVE THE CONTENT INTO IT.
                        //-------- TUESDAY CHANGES--------------- 2013 MARCH ---- FIXING MY ERROR
                        if (chkProcessPerformanceAppraisals.Checked == true)
                        {
                            
                            try
                            {
                                txtStatus.Text += "DEBUG: Creating a page and moving the content from : " + PageId + Environment.NewLine;

                                NewArchivePageSetup = new XmlRpcStruct();

                                NewArchivePageSetup.Add("space", txtSpaceKey.Text);
                                NewArchivePageSetup.Add("content", "placeholder"); //Needs permissions applied to it first before content is imported.
                                NewArchivePageSetup.Add("title", txtArchivePrefix.Text + CurrentPage["title"]);

                                //ArchivePage = confluenceProxy.storePage(token, NewArchivePageSetup);
                                txtStatus.Text += "Page created successfully." + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "Could not create archive page " + ex.Message + Environment.NewLine;
                            }

                            //************************SET PERMISSIONS*********************
                            NewPageViewPermissions = null;  // Used to assemble permissions for freshly created page
                            XmlRpcStruct PageViewPermissionSets = new XmlRpcStruct(); // The API sends back SETS of permissions as opposed to just the permission structure.
                            Array PageViewPermissions = null; // Array of permissions held in above set
                            XmlRpcStruct Permission = new XmlRpcStruct(); // Individual permission
                            int loopCounter = 0; // Used to keep track of array position in NewPageViewPermissions object array.

                            txtStatus.Text += "Trying permissions..." + Environment.NewLine;

                            try
                            {

                                PageViewPermissionSets = confluenceProxy.getContentPermissionSet(token, PageId, "View");  //Get permission sets from employee old page.
                                txtStatus.Text += "Acquired view permissions from the employee's old page..." + Environment.NewLine;

                                PageViewPermissions = (Array)PageViewPermissionSets["contentPermissions"];  //Move returned structure into an array.

                                NewPageViewPermissions = new object[PageViewPermissions.Length]; //define an object array as long as the length of the permissions received.

                                foreach (XmlRpcStruct PagePermission in PageViewPermissions)  //go through each permission and pass that along to the NewPageViewPermissions obj array to be applied to the employee's new page.
                                {
                                    Application.DoEvents();
                                    txtStatus.Text += "DEBUG: Currently applying permissions for the user: " + PagePermission["userName"] + Environment.NewLine;  //Username currently processed
                                    string userName = (string)PagePermission["userName"];  //hack to work around c# cutting off the command to add this value to the permission xmlrpcstruct.

                                    Permission = new XmlRpcStruct();

                                    Permission.Add("type", "View");  //We always modify view permissions.  Edit permissions enables all to view but some to edit.  View permission locks down edit.
                                    Permission.Add("userName", userName);
                                    Permission.Add("GroupName", "");

                                    NewPageViewPermissions[loopCounter] = Permission; //Move this freshly constructed permission into the large array to be sent.
                                    loopCounter++;
                                }

                                txtStatus.Text += "DEBUG: New permissions successfully Constructed!" + Environment.NewLine;

                                confluenceProxy.setContentPermissions(token, (string)ArchivePage["id"], "View", NewPageViewPermissions);
                                confluenceProxy.setContentPermissions(token, (string)NewArchivePage["id"], "View", NewPageViewPermissions);

                                txtStatus.Text += "New permissions successfully set!" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not set permissions: " + ex.Message + Environment.NewLine;

                            }

                            //******************COPY CONTENT FROM MAIN APPRAISAL PAGE AND PLACE INTO ARCHIVE AND BRING IN NEW CONTENT FROM TEMPLATE.

                            PageTemplate = new XmlRpcStruct();
                            PageUpdateOptions = new XmlRpcStruct();

                            try
                            {
                                //This is meta information regarding the edit 
                                PageUpdateOptions.Add("versionComment", "Move content over from performance appraisal now that permissions have been set.");
                                PageUpdateOptions.Add("minorEdit", true);

                                PageTemplate.Add("id", (string)ArchivePage["id"]);
                                PageTemplate.Add("content", (string)CurrentPage["content"]);
                                PageTemplate.Add("version", ArchivePage["version"]);
                                PageTemplate.Add("space", txtSpaceKey.Text);
                                PageTemplate.Add("title", (string)ArchivePage["title"]);
                                //PageTemplate.Add("parentId", (string)NewArchivePage["parentId"]);

                                ArchivePage = confluenceProxy.updatePage(token, PageTemplate, PageUpdateOptions);

                                //Wait for page to be added.
                                System.Threading.Thread.Sleep(250);

                                confluenceProxy.movePage(token, (string)ArchivePage["id"], (string)NewArchivePage["id"], "append");

                                txtStatus.Text += "Page content successfully updated for Archive Page." + Environment.NewLine;

                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not update archive performance appraisal page with content: " + ex.Message + Environment.NewLine;
                            }
                            

                            //*************************REPLACE CURRENTPAGE CONTENT WITH THAT OF TEMPLATE**********

                            PageTemplate = new XmlRpcStruct(); //Used for constructing 'pages' to send to the API
                            PageUpdateOptions = new XmlRpcStruct(); //Needed for the above
                            TemplatePage = new XmlRpcStruct(); //This is a template page with which you use to replace other pages content with

                            try
                            {
                                TemplatePage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtMyDeptPerfTemplateLocation.Text);
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not get template." + ex.Message + Environment.NewLine;
                            }

                            try
                            {
                                //This is meta information regarding the edit 
                                PageUpdateOptions.Add("versionComment", "Replace content from that of the template for this PM cycle.");
                                PageUpdateOptions.Add("minorEdit", true);

                                PageTemplate.Add("id", (string)CurrentPage["id"]);
                                PageTemplate.Add("content", (string)TemplatePage["content"]);
                                PageTemplate.Add("version", CurrentPage["version"]);
                                PageTemplate.Add("space", txtSpaceKey.Text);
                                PageTemplate.Add("title", (string)CurrentPage["title"]);
                                PageTemplate.Add("parentId", (string)CurrentPage["parentId"]);


                                CurrentPage = confluenceProxy.updatePage(token, PageTemplate, PageUpdateOptions);

                                txtStatus.Text += "Page content successfully updated for " + (string)CurrentPage["title"] + " performance appraisal page." + Environment.NewLine;

                                //txtStatus.Text += "Page Moved Successfully." + Environment.NewLine;

                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not update performance appraisal page with template content: " + ex.Message + Environment.NewLine;
                            }

                            //*************************MOVE THE PAGE JUST CREATED************************

                            try
                            {
                                //When updating page content, confluence appears to reset the pages position in hierachy to the top of the space.  This works around that.
                                confluenceProxy.movePage(token, (string)ArchivePage["id"], (string)NewArchivePage["id"], "append");
                                confluenceProxy.movePage(token, (string)NewArchivePage["id"], (string)CurrentPage["id"], "append");

                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "Could not move pages to work around confluence resetting page position.  Error: " + ex.Message + Environment.NewLine;
                            }
                        }
                        else
                        {
                            txtStatus.Text += "Not processing performance appraisals... Skipping." + Environment.NewLine;
                        }




                        //*******PROCESS PDP - archive***********

                        if (chkArchivePDP.Checked == Enabled)
                        {

                            try
                            {
                                Array ChildPages;
                                ReturnedPage = new XmlRpcStruct();
                                XmlRpcStruct PDPPage = new XmlRpcStruct();
                                PageUpdateOptions = new XmlRpcStruct();
                                string CurrentTitle;

                                //Get all children pages of current page being processed and look for one with "Professional Development Plan" in the title
                                ChildPages = confluenceProxy.getChildren(token, (string)CurrentPage["id"]);

                                foreach (XmlRpcStruct ChildPage in ChildPages)
                                {
                                    if (Convert.ToString(ChildPage["title"]).IndexOf("Professional Development Plan") >= 0)
                                    {
                                        //************************HERE HERE HERE**************
                                        PDPPage = confluenceProxy.getPage(token, (string)ChildPage["id"]);


                                        CurrentTitle = (string)PDPPage["title"];

                                        PDPPage.Remove("title");
                                        PDPPage.Add("title", "FY 2013 - " + CurrentTitle);

                                        //This is meta information regarding the edit 
                                        PageUpdateOptions.Add("versionComment", "Add archive prefix to PDP Page.");
                                        PageUpdateOptions.Add("minorEdit", true);

                                        ReturnedPage = confluenceProxy.updatePage(token, PDPPage, PageUpdateOptions);

                                        confluenceProxy.movePage(token, (string)ReturnedPage["id"], (string)NewArchivePage["id"], "append");

                                        txtStatus.Text += "Sucessfully archived PDP." + Environment.NewLine;

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "Could not archive PDP. Skipping.  Error msg: " + ex.Message + Environment.NewLine;
                            }
                        }



                        //*************************FIND CHILD BONUS % PAGE & CYCLE******************************

                        if (chkProcessBonus.Checked == true)
                        {
                            Array ChildPages;

                            //Get bonus % template page
                            TemplatePage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtBonusTemplatePageName.Text);
                            ReturnedPage = new XmlRpcStruct();
                            PageUpdateOptions = new XmlRpcStruct();

                            //Get all children pages of current page being processed and look for one with "bonus" in the title
                            ChildPages = confluenceProxy.getChildren(token, (string)CurrentPage["id"]);
                            foreach (XmlRpcStruct ChildPage in ChildPages)
                            {
                                PageTemplate = new XmlRpcStruct();

                                if (Convert.ToString(ChildPage["title"]).IndexOf("Bonus") >= 0)
                                {
                                    try
                                    {
                                        //This is meta information regarding the edit 
                                        PageUpdateOptions.Add("versionComment", "Replace content from that of the Bonus template for this PM cycle.");
                                        PageUpdateOptions.Add("minorEdit", true);

                                        //Construct page structure
                                        PageTemplate.Add("id", (string)ChildPage["id"]);
                                        PageTemplate.Add("content", (string)TemplatePage["content"]);
                                        PageTemplate.Add("version", ChildPage["version"]);
                                        PageTemplate.Add("space", txtSpaceKey.Text);
                                        PageTemplate.Add("title", (string)ChildPage["title"]);
                                        PageTemplate.Add("parentId", (string)ChildPage["parentId"]);

                                        //Update the page
                                        ReturnedPage = confluenceProxy.updatePage(token, PageTemplate, PageUpdateOptions);

                                        txtStatus.Text += "Page content successfully updated for '" + (string)ChildPage["title"] + "' page." + Environment.NewLine;

                                    }
                                    catch (Exception ex)
                                    {
                                        txtStatus.Text += "ERROR! Could not update bonus % page page with template content: " + ex.Message + Environment.NewLine;
                                    }
                                }
                            }
                        }
                        else
                        {
                            txtStatus.Text += "Skipped bonus % template as checkbox isn't enabled." + Environment.NewLine;
                        }


                        //**************Objectives template Import***********
                        //There should be logic here to create the page if it doesn't exist (& set permissions) or use an existing page if not.

                        if (chkProcessObjectives.Checked == true)
                        {

                            //INIT VARs                                
                            PageTemplate = new XmlRpcStruct();
                            ReturnedPage = new XmlRpcStruct();
                            TemplatePage = new XmlRpcStruct();
                            PageUpdateOptions = new XmlRpcStruct();
                            Array ChildPages;
                            
                            //Get all children pages of current page being processed and look for one with "Objectives" in the title
                            ChildPages = confluenceProxy.getChildren(token, (string)CurrentPage["id"]);
                            foreach (XmlRpcStruct ChildPage in ChildPages)
                            {
                                PageTemplate = new XmlRpcStruct();

                                if (Convert.ToString(ChildPage["title"]).IndexOf("Objectives") >= 0)
                                {
                                    try
                                    {

                                        //Get objectives template page
                                        TemplatePage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtObjectives.Text);


                                        //This is meta information regarding the edit 
                                        PageUpdateOptions.Add("versionComment", "Replace content from that of the Objectives template for this PM cycle.");
                                        PageUpdateOptions.Add("minorEdit", true);

                                        //Construct page structure
                                        PageTemplate.Add("id", (string)ChildPage["id"]);
                                        PageTemplate.Add("content", (string)TemplatePage["content"]);
                                        PageTemplate.Add("version", ChildPage["version"]);
                                        PageTemplate.Add("space", txtSpaceKey.Text);
                                        PageTemplate.Add("title", (string)ChildPage["title"]);
                                        PageTemplate.Add("parentId", (string)ChildPage["parentId"]);


                                        //Store the new objectives page
                                        ReturnedPage = confluenceProxy.storePage(token, PageTemplate);

                                        txtStatus.Text += "New Objectives page successfully created!" + Environment.NewLine;
                                    }
                                    catch (Exception ex)
                                    {
                                        txtStatus.Text += "ERROR! Could not update objectives page from template." + ex.Message + Environment.NewLine;
                                    }
                                }
                            }
                        }

                        if (chkCreateObjectives.Checked == true)
                        {
                            //******************This block of code is for the initial seeding of the objectives page.
                            try
                            {
                                //INIT VARs                                
                                PageTemplate = new XmlRpcStruct();
                                ReturnedPage = new XmlRpcStruct();
                                TemplatePage = new XmlRpcStruct();

                                TemplatePage = confluenceProxy.getPage(token, txtSpaceKey.Text, txtObjectives.Text);

                                string ObjectivesPageTitle = (string)CurrentPage["title"];
                                ObjectivesPageTitle = ObjectivesPageTitle.Replace("Performance Appraisal", "Objectives");

                                //Construct page structure
                                PageTemplate.Add("content", (string)TemplatePage["content"]);
                                PageTemplate.Add("space", txtSpaceKey.Text);
                                PageTemplate.Add("title", ObjectivesPageTitle);
                                PageTemplate.Add("parentId", PageId);

                                //Store the new objectives page
                                ReturnedPage = confluenceProxy.storePage(token, PageTemplate);

                                txtStatus.Text += "New Objectives page successfully created!" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not create objectives page from template." + ex.Message + Environment.NewLine;
                            }

                            //************************SET PERMISSIONS ON OBJECTIVES PAGE*********************
                            txtStatus.Text += "Trying permissions..." + Environment.NewLine;

                            try
                            {
                                txtStatus.Text += "Break..." + Environment.NewLine;
                                confluenceProxy.setContentPermissions(token, (string)ReturnedPage["id"], "View", NewPageViewPermissions);

                                txtStatus.Text += "New permissions successfully set on objectives page!" + Environment.NewLine;
                            }
                            catch (Exception ex)
                            {
                                txtStatus.Text += "ERROR! Could not set permissions: " + ex.Message + Environment.NewLine;
                            }
                        }

                    } //PAGE PROCESSING LOOP END

                    //Enable user input/buttons
                    btnLogout.Enabled = true;
                    btnMovePages.Enabled = true;
                    btnMoveDeptPages.Enabled = true;

                    txtStatus.Text += "\r\nProcess Complete!" + Environment.NewLine;
                    MessageBox.Show("Process Complete.");


                    //STOP ALL PROCESSING.
                    break;

                // USER DID NOT CLICK OK... DO NOTHING.
                default:

                    //Enable user input/buttons

                    btnLogout.Enabled = true;
                    btnMovePages.Enabled = true;
                    btnMoveDeptPages.Enabled = true;

                    txtStatus.Text += "User canceled page move at confirmation message box." + Environment.NewLine;
                    break;
            }

        }


    }
}
