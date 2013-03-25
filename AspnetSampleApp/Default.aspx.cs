using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;


namespace AspnetSampleApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ArgumentExceptionButton_Click(object sender, EventArgs e)
        {
            throw new ArgumentException("This is a fake exception created at " + DateTime.Now.ToShortTimeString());
        }

        protected void OutOfMemoryExceptionButton_Click(object sender, EventArgs e)
        {
            throw new OutOfMemoryException("This is a fake exception created at " + DateTime.Now.ToShortTimeString());
        }

        protected void DatabaseExceptionButton_Click(object sender, EventArgs e)
        {
            var database = new DataBaseStuff();

            database.GetUserIdFromUsername("MyUser");
        }
    }
}