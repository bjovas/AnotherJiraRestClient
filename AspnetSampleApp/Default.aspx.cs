using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using SampleClassLibrary;


namespace AspnetSampleApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ArgumentExceptionButton_Click(object sender, EventArgs e)
        {
            // lets emulate some bad input handling.
            throw new ArgumentException("This is a fake exception created at " + DateTime.Now.ToShortTimeString());
        }

        protected void OutOfMemoryExceptionButton_Click(object sender, EventArgs e)
        {
            var calculator = new HeavyCalculations();
            calculator.DoSomeHeavyStuff(123, 345, 431);       
        }

        protected void DatabaseExceptionButton_Click(object sender, EventArgs e)
        {
            var database = new DataBaseStuff();
            database.GetUserIdFromUsername("MyUser");
        }
    }
}