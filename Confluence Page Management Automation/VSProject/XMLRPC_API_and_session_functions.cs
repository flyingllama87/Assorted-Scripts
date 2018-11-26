//Stuff Specific to the XML-RPC API (XMLRPC.NET) used & login/logout functions are implemented here.  In this file (bottom) there is an interface called confluenceproxy.  Here, we implement the functions that the remote API should have implemented.  Refer to official confluence API documentation for more. 

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
        //*************************LOGIN*****************

        private void txtLogin_Click(object sender, EventArgs e)
        {

            txtStatus.Text += "DEBUG: Creating confluece xml-rpc proxy obj now." + Environment.NewLine;
            confluenceProxy = XmlRpcProxyGen.Create<Iconfluence>();
            confluenceProxy.Url = txtXMLRPCURL.Text;

            txtStatus.Text += "Logging in..." + System.Environment.NewLine;
            try
            {
                token = confluenceProxy.login(txtUsername.Text, txtPassword.Text);
                txtStatus.Text += "Success! Security Token is: " + token + Environment.NewLine;
                btnLogin.Enabled = false;
                btnLogout.Enabled = true;
                btnMovePages.Enabled = true;
                btnMoveDeptPages.Enabled = true;

            }
            catch (Exception Ex)
            {
                txtStatus.Text += "ERROR! Log in failed: " + Ex.Message + Environment.NewLine;
            }

        }


        // **************LOGOUT**************

        private void txtLogout_Click(object sender, EventArgs e)
        {
            Boolean methodSuccess = true;

            try
            {
                txtStatus.Text += "Logging out..." + Environment.NewLine;
                methodSuccess = confluenceProxy.logout(token);
                txtStatus.Text += "Logged out." + Environment.NewLine;

                //Disable user input for stuff that will break the program
                btnLogout.Enabled = false;
                btnMovePages.Enabled = false;
                btnMoveDeptPages.Enabled = false;
                btnLogin.Enabled = true;
            }
            catch
            {
                txtStatus.Text += "Log out failed." + Environment.NewLine;
            }
        }
    }



    //****DECLARE FUNCTIONS IMPLEMENTED ON REMOTE API****
    //REFER TO CONFLUENCE XML-RPC API DOCUMENTATION FOR MORE INFORMATION.


    public interface Iconfluence : IXmlRpcProxy
    {
        [XmlRpcMethod("confluence2.login")]
        string login(string username, string password);

        [XmlRpcMethod("confluence2.logout")]
        Boolean logout(string token);

        [XmlRpcMethod("confluence2.getPage")]
        XmlRpcStruct getPage(string token, string spaceKey, string pageTitle);

        [XmlRpcMethod("confluence2.getPage")]
        XmlRpcStruct getPage(string token, string pageId);

        [XmlRpcMethod("confluence2.getChildren")]
        Array getChildren(string token, string pageId);

        [XmlRpcMethod("confluence2.movePage")]
        void movePage(string token, string sourcePageId, string targetPageId, string position);

        [XmlRpcMethod("confluence2.updatePage")]
        XmlRpcStruct updatePage(string token, XmlRpcStruct page, XmlRpcStruct pageUpdateOptions);

        [XmlRpcMethod("confluence2.getAncestors")]
        Array getAncestors(string token, string pageId);

        [XmlRpcMethod("confluence2.getDescendents")]
        Array getDescendents(string token, string pageId);

        [XmlRpcMethod("confluence2.storePage")]
        XmlRpcStruct storePage(string token, XmlRpcStruct page);

        [XmlRpcMethod("confluence2.getContentPermissionSets")]
        Array getContentPermissionSets(string token, String contentID);

        [XmlRpcMethod("confluence2.getContentPermissionSet")]
        XmlRpcStruct getContentPermissionSet(string token, String contentID, String permissionType);

        [XmlRpcMethod("confluence2.setContentPermissions")]
        void setContentPermissions(String token, String contentId, String permissionType, Array permissions);

        [XmlRpcMethod("confluence2.removePage")]
        void removePage(String Token, String pageId);


    }
}
